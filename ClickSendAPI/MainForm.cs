using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//ClickSend
using IO.ClickSend.ClickSend.Api;
using IO.ClickSend.Client;
using IO.ClickSend.ClickSend.Model;
using System.Security.Cryptography.X509Certificates;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.IO;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace ClickSendAPI
{
    public struct FContactListEntry
    {
        public int ListID;
        public string ListName;
        public int NumContacts;
    }

    public struct FMMSWithURL
    {
        public MmsMessage InMessage;
        public String InURL;
    }
    public partial class MainForm : Form
    {
        AccountApi AccAPI;
        SMSApi SMSAPI;
        MMSApi MMSAPI;
        VoiceApi VoiceAPI;
        TransactionalEmailApi TransAPI;
        ContactListApi ContactListAPI;
        ContactApi ContactAPI;
        SmsCampaignApi SmsCampaignAPI;
        CredentialsPopup CredPop;
        SubaccountApi SubaccountAPI;
        Configuration APIConfig;

        FContactListEntry ContactListEntry;
        List<FContactListEntry> AllContactLists;
        List<Contact> ListContacts;
        Stream CSV;
        Stream TestCSV;
        string StrAccNum;
        int intAccNum;
        bool AccNumIsValid = false;


        public MainForm()
        {
            InitializeComponent();
            this.Opacity = 0;
            this.Show();
            CredPop = new CredentialsPopup();
            CredPop.SetMainForm(this);
        }

        public void InitializeAPI(Configuration config)
        {
            this.Opacity = 100;
            CredPop.Opacity = 0;
            CredPop.Show();
            this.Show();
            APIConfig = config;
            AccAPI = new AccountApi(config);
            SMSAPI = new SMSApi(config);
            MMSAPI = new MMSApi(config);
            VoiceAPI = new VoiceApi(config);
            TransAPI = new TransactionalEmailApi(config);
            ContactListAPI = new ContactListApi(config);
            ContactAPI = new ContactApi(config);
            SmsCampaignAPI = new SmsCampaignApi(config);
            SubaccountAPI = new SubaccountApi(config);

            AllContactLists = new List<FContactListEntry>();
            ListContacts = new List<Contact>();

            lblSMSError.Visible = false;
            lblSMSSent.Visible = false;
            lblMMSSent.Visible = false;
            lblVoiceError.Visible = false;
            lblVoiceSent.Visible = false;
            lblTransError.Visible = false;
            lblTransSent.Visible = false;
            lblSMSCampError.Visible = false;
            lblSMSCampSent.Visible = false;
            lblCustMMSSent.Visible = false;
            lblCustMMSError.Visible = false;

            FillComboBoxes();
            PopulateNewLogin();
        }

        private void ValidateAccountNumber(string InAccNum)
        {
            bool CanConvert = int.TryParse(InAccNum, out intAccNum);
            if (CanConvert)
            {
                AccNumIsValid = true;
            }
            else
            {
                AccNumIsValid = false;
            }
        }

        private void FillComboBoxes()
        {
            CBVoiceLang.Items.Add("en-us");
            CBVoiceLang.Items.Add("es");
            CBVoiceLang.Items.Add("es-us");
            CBVoiceLang.Items.Add("fr");
        }

        private void PopulateNewLogin()
        {
            //Fill Account login results
            string response = AccAPI.AccountGet();
            string[] ResponseArr = response.Split(',');
            foreach (string str in ResponseArr)
            {
                TBResultsBox.Items.Add(str);
            }

            //Populate Contact information
            string ContactResponse = ContactListAPI.ListsGet();
            ParseResponse(ContactResponse, "ContactList");
            foreach (FContactListEntry Entry in AllContactLists) {
                LBAccContactLists.Items.Add(Entry.ListID);
                CBContactLists.Items.Add(Entry.ListID);
                CBSMSCampContactList.Items.Add(Entry.ListID);
                CBCustomMMSContacts.Items.Add(Entry.ListID);
                    }

        }

        private void btnSendSMS_Click(object sender, EventArgs e)
        {
            var listOfSMS = new List<SmsMessage>
            {
                new SmsMessage(
                    to: TBSMSTo.Text,
                    body: TBSMSBody.Text,
                    from: "",
                    source: "sdk",
                    schedule: 0
                    )
            };

            var SMSCollesction = new SmsMessageCollection(listOfSMS);
            string response = SMSAPI.SmsSendPost(SMSCollesction);

            string GoodResponseCheck = response.Substring(13, 3);

            if (GoodResponseCheck.Equals("200"))
            {
                lblSMSError.Visible = false;
                lblSMSSent.Visible = true;
            }
            else
            {
                lblSMSError.Visible = true;
                lblSMSSent.Visible = false;
            }
        }

        private void btnMMSSend_Click(object sender, EventArgs e)
        {
            var ListOfMessages = new List<MmsMessage>();
            MmsMessage CurMessage = new MmsMessage(
                TBMMSTo.Text,
                TBMMSBody.Text,
                TBMMSSubject.Text,
                TBMMSFrom.Text,
                TBMMSCountry.Text,
                "sdk",
                0,
                0,
                TBMMSCustString.Text,
                null
                );

            ListOfMessages.Add(CurMessage);

            var mmsCollection = new MmsMessageCollection("https://static.wixstatic.com/media/fa14b7_523f47bbf46942ea8084abb8ae8b8636~mv2.jpg/v1/fill/w_1600,h_900,al_c,q_90/file.jpg", ListOfMessages);
            string response = MMSAPI.MmsSendPost(mmsCollection);

            string GoodResponseCheck = response.Substring(13, 3);

            if (GoodResponseCheck.Equals("200"))
            {

                lblMMSSent.Visible = true;
            }
            else
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ListOfVoiceMessages = new List<VoiceMessage>();
            VoiceMessage CurVM = new VoiceMessage(

                to: TBVoiceTo.Text,
                body: TBVoiceBody.Text,
                voice: TBVoiceVoice.Text,
                customString: TBVoiceCustStr.Text,
                country: TBVoiceCountry.Text,
                source: "sdk",
                listId: 0,
                lang: CBVoiceLang.Text,
                schedule: 0,
                requireInput: 0,
                machineDetection: 0
                );

            if (CHBDetect.Checked)
            {
                CurVM.MachineDetection = 1;
            }
            if (CHBRequireInput.Checked)
            {
                CurVM.RequireInput = 1;
            }

            ListOfVoiceMessages.Add(CurVM);

            var VoiceMessageCollection = new VoiceMessageCollection(ListOfVoiceMessages);
            string response = VoiceAPI.VoiceSendPost(VoiceMessageCollection);

            string GoodResponseCheck = response.Substring(13, 3);

            if (GoodResponseCheck.Equals("200"))
            {
                lblVoiceError.Visible = false;
                lblVoiceSent.Visible = true;
            }
            else
            {
                lblVoiceError.Visible = true;
                lblVoiceSent.Visible = false;
            }
        }

        private void btnTransSend_Click(object sender, EventArgs e)
        {
            EmailRecipient Recipient = new EmailRecipient(
                email: TBTransTo.Text,
                name: "John Doe"
                );
            EmailRecipient RecipCC = new EmailRecipient(
                email: TBTransCC.Text,
                name: "John Doe"
                );
            EmailRecipient RecipBCC = new EmailRecipient(
                email: TBTransBBC.Text,
                name: "John Doe"
                );
            Attachment Attachment = new Attachment(
                content: "content",
                type: "type",
                filename: "filename",
                disposition: "disposition",
                contentId: "contentId"
                );
            EmailFrom emailFrom = new EmailFrom(
                emailAddressId: "27083",
                name: "Mike"
                );
            var ListRecipients = new List<EmailRecipient>();
            var ListCC = new List<EmailRecipient>();
            var ListBCC = new List<EmailRecipient>();
            ListRecipients.Add(Recipient);
            ListCC.Add(RecipCC);
            ListBCC.Add(RecipBCC);

            string response = TransAPI.EmailSendPost(new Email(
                to: ListRecipients,
                cc: ListCC,
                bcc: ListBCC,
                from: emailFrom,
                subject: TBTransSubject.Text,
                body: TBTransBody.Text,
                attachments: null,
                schedule: 0
                ));

            string GoodResponseCheck = response.Substring(13, 3);

            if (GoodResponseCheck.Equals("200"))
            {
                lblTransError.Visible = false;
                lblTransSent.Visible = true;
            }
            else
            {
                lblTransError.Visible = true;
                lblTransSent.Visible = false;
            }

        }

        private void ParseResponse(string response, string ParseType)
        {
            if (ParseType.Equals("ContactList"))
            {
                string[] InLines = response.Split(',');
                ContactListEntry = new FContactListEntry();
                ContactListEntry.ListID = 0;
                ContactListEntry.ListName = "Default";
                ContactListEntry.NumContacts = -1;

                foreach (string Line in InLines)
                {
                    //Object has all fields set
                    if (ContactListEntry.ListID != 0 && ContactListEntry.ListName != "Default" && ContactListEntry.NumContacts != -1)
                    {
                        AllContactLists.Add(ContactListEntry);
                        ContactListEntry = new FContactListEntry();
                        ContactListEntry.ListID = 0;
                        ContactListEntry.ListName = "Default";
                        ContactListEntry.NumContacts = -1;
                    }

                    if (Line.Contains("\"list_id\":") && !Line.Contains("\"data\":"))
                    {
                        string[] SplitSpace = Line.Split(':');
                        ContactListEntry.ListID = int.Parse(SplitSpace[1]);
                    }
                    if (Line.Contains("\"list_name\":"))
                    {
                        string[] SplitSpace = Line.Split(':');
                        ContactListEntry.ListName = SplitSpace[1];
                    }
                    if (Line.Contains("\"_contacts_count\":"))
                    {
                        string[] SplitSpace = Line.Split(':');
                        ContactListEntry.NumContacts = int.Parse(SplitSpace[1]);
                    }
                }
            }

            if (ParseType.Equals("Contact"))
            {
                string[] InLines = response.Split(',');
                Contact CurContact = new Contact(
                    phoneNumber: "Not Set",
                    custom1: "Not Set",
                    email: "Not Set",
                    faxNumber: "Not Set",
                    firstName: "Not Set",
                    addressLine1: "Not Set",
                    addressLine2: "Not Set",
                    addressCity: "Not Set",
                    addressState: "Not Set",
                    addressPostalCode: "Not Set",
                    addressCountry: "Not Set",
                    organizationName: "Not Set",
                    custom2: "Not Set",
                    custom3: "Not Set",
                    custom4: "Not Set",
                    lastName: "Not Set"
                    );
                bool FNameSet = false;
                bool LNameSet = false;
                bool PhoneSet = false;
                bool CustomSet = false;

                if(ListContacts.Count > 0)
                {
                    ListContacts.Clear();
                }

                foreach (string Line in InLines)
                {
                    if (FNameSet && LNameSet && PhoneSet && CustomSet)
                    {
                        ListContacts.Add(CurContact);
                        CurContact = new Contact(
                            phoneNumber: "Not Set",
                            custom1: "Not Set",
                            email: "Not Set",
                            faxNumber: "Not Set",
                            firstName: "Not Set",
                            addressLine1: "Not Set",
                            addressLine2: "Not Set",
                            addressCity: "Not Set",
                            addressState: "Not Set",
                            addressPostalCode: "Not Set",
                            addressCountry: "Not Set",
                            organizationName: "Not Set",
                            custom2: "Not Set",
                            custom3: "Not Set",
                            custom4: "Not Set",
                            lastName: "Not Set"
                            );
                        FNameSet = false;
                        LNameSet = false;
                        PhoneSet = false;
                        CustomSet = false;
                    }
                    if (Line.Contains("\"first_name\":") && !Line.Contains("\"data\":"))
                    {
                        string[] SplitSpace = Line.Split(':');
                        CurContact.FirstName = SplitSpace[1];
                        FNameSet = true;
                    }
                    if (Line.Contains("\"last_name\":"))
                    {
                        string[] SplitSpace = Line.Split(':');
                        CurContact.LastName = SplitSpace[1];
                        LNameSet = true;
                    }
                    if (Line.Contains("\"phone_number\":"))
                    {
                        string[] SplitSpace = Line.Split(':');
                        CurContact.PhoneNumber = SplitSpace[1];
                        PhoneSet = true;
                    }
                    if (Line.Contains("\"custom_1\":"))
                    {
                        string[] SplitSpace = Line.Split(':');
                        string FullURL = "";
                        for (int i = 1; i < SplitSpace.Length; i++)
                        {
                            FullURL += SplitSpace[i] + ":";
                        }
                        CurContact.Custom1 = FullURL;
                        CustomSet = true;
                    }
                }
            }
        }

        private int ParseContactListNumber(string response)
        {
            string[] InResponse = response.Split(',');
            string InNumber = InResponse[3];
            string[] SplitNumber = InNumber.Split(':');
            string OutNumber = SplitNumber[2];

            return int.Parse(OutNumber);
        }
        private void CBContactLists_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBContactLists.SelectedIndex == -1)
            {
                return;
            }

            string ContactListNumber = CBContactLists.Text;

            string response = ContactAPI.ListsContactsByListIdGet(int.Parse(ContactListNumber));

            ParseResponse(response, "Contact");

            if (ListContacts.Count > 0)
            {
                LBContacts.Items.Clear();
                foreach (Contact CurCon in ListContacts)
                {
                    LBContacts.Items.Add(" FName: " + CurCon.FirstName + "    LName: " + CurCon.LastName + "    Phone: " + CurCon.PhoneNumber);
                }
            }
        }

        private void btnSMSCampSend_Click(object sender, EventArgs e)
        {
            if(CBSMSCampContactList.Items.Count > 0 && CBSMSCampContactList.SelectedIndex != 0)
            {

            }
            if (TBSMSCampName.Text.Equals(""))
            {

            }
            if (TBSMSCampBody.Text.Equals(""))
            {

            }

            int Int_ListID = int.Parse(CBSMSCampContactList.Text);
            string response = SmsCampaignAPI.SmsCampaignsSendPost(new SmsCampaign(
                listId: Int_ListID,
                name: TBSMSCampName.Text,
                body: TBSMSCampBody.Text
                )) ;
        }

        private void btnSendCustomMMS_Click(object sender, EventArgs e)
        {
            string ContactListNumber = CBCustomMMSContacts.Text;
            int ListStrToInt = int.Parse(ContactListNumber);

            List<FMMSWithURL> CustArr = new List<FMMSWithURL>();
            var CustomersURLOne = new List<MmsMessage>();
            var CustomersURLTwo = new List<MmsMessage>();

            List<String> URLS = new List<String>();
            
            foreach (Contact CurCon in ListContacts)
            {
                FMMSWithURL NewEntry = new FMMSWithURL();
                MmsMessage CurMessage = new MmsMessage(
                   CurCon.PhoneNumber,
                   TBCustomMMSBody.Text,
                   "None",
                   "+18335071382",
                   "US",
                   "sdk",
                   0,
                   0,
                   "None",
                   "Michael.Sharich@clicksend.com"
               );

                NewEntry.InMessage = CurMessage;
                NewEntry.InURL = CurCon.Custom1;
                CustArr.Add(NewEntry);
            }

            foreach(FMMSWithURL CurSend in CustArr)
            {
                if (!URLS.Contains(CurSend.InURL))
                {
                    URLS.Add(CurSend.InURL);
                }
            }



            var UrlOneMMSCollection = new MmsMessageCollection(URLS[0], CustomersURLOne);
            var UrlOneResponse = MMSAPI.MmsSendPost(UrlOneMMSCollection);

            var UrlTwoMMSCollection = new MmsMessageCollection(URLS[1], CustomersURLTwo);
            var UrlTwoResponse = MMSAPI.MmsSendPost(UrlTwoMMSCollection);

            string UrlOneGoodResponseCheck = UrlOneResponse.Substring(13, 3);
            string UrlTwoGoodResponseCheck = UrlTwoResponse.Substring(13, 3);

            if (UrlOneGoodResponseCheck.Equals("200") && UrlTwoGoodResponseCheck.Equals("200"))
            {
                lblCustMMSSent.Visible = true;
            }


        }

        private void CBCustomMMSContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBCustomMMSContacts.SelectedIndex == -1)
            {
                return;
            }

            string ContactListNumber = CBCustomMMSContacts.Text;

            string response = ContactAPI.ListsContactsByListIdGet(int.Parse(ContactListNumber));

            ParseResponse(response, "Contact");

            if (ListContacts.Count > 0)
            {
                foreach (Contact CurCon in ListContacts)
                {
                    LBCustomContacts.Items.Add("FName: " + CurCon.FirstName + "LName: " + CurCon.LastName + "Phone: " + CurCon.PhoneNumber);
                }
            }
        }

        private void btnChangeCredentials_Click(object sender, EventArgs e)
        {
            TBResultsBox.Items.Clear();
            LBAccContactLists.Items.Clear();
            LBContacts.Items.Clear();
            CBContactLists.Items.Clear();
            CBSMSCampContactList.Items.Clear();
            CBCustomMMSContacts.Items.Clear();
            AllContactLists.Clear();
            ListContacts.Clear();
            this.Opacity = 0;
            this.Show();
            CredPop.Opacity = 100;
            CredPop.Show();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            lblSubFileName.Text = openFileDialog1.FileName;
            CSV = openFileDialog1.OpenFile();
            StreamReader SR = new StreamReader(CSV);

            string InRow = SR.ReadLine();
            string[] Columns = InRow.Split(new char[] { ',' });

            string TopDisplay = "";
            foreach(string str in Columns)
            {
                TopDisplay += str + " - ";
            }

            LBSubAccInfo.Items.Add(TopDisplay);

            while(!SR.EndOfStream )
            {
                string Row = SR.ReadLine();
                string[] Split = Row.Split(new char[] { ',' });

                string LineString = "";
                foreach(string str in Split)
                {
                    LineString += str + " - ";
                }

                LBSubAccInfo.Items.Add(LineString);
            }
        }

        private void btnSelectCSV_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void ParseCSV()
        {

        }

        private void btnSubUpload_Click(object sender, EventArgs e)
        {
            CSV = openFileDialog1.OpenFile();
            StreamReader SR = new StreamReader(CSV);

            string InRow = SR.ReadLine();

            while (!SR.EndOfStream)
            {
                string Row = SR.ReadLine();
                string[] Columns = Row.Split(',');

                Guid guid = Guid.NewGuid();
                string APIKey = guid.ToString();

                Subaccount CurSub = new Subaccount(
                    apiUsername: Columns[0],
                    password: Columns[1],
                    email: Columns[2],
                    phoneNumber: Columns[3],
                    firstName: Columns[4],
                    lastName: Columns[5],
                    accessBilling: 1,
                    accessContacts: 1,
                    accessUsers: 1,
                    accessReporting: 1,
                    accessSettings: 1
                    );
                var response = SubaccountAPI.SubaccountsPost(CurSub);
            }
        }

        private void CreateCSV_Click(object sender, EventArgs e)
        {
            StringBuilder SB = new StringBuilder();

            SB.AppendLine("FirstName, Last_Name, Mobile, Email");
            for(int i = 0; i <= 200000; i++)
            {
                SB.AppendLine("John, Doe, +1234567890, test@test.com");
            }

            File.WriteAllText(@"C:\\Users\\micsha\\Documents\\Test\\Test.csv", SB.ToString());
        }

        private void BuildMessageCollection_Click(object sender, EventArgs e)
        {
            TestCSV = File.OpenRead(@"C:\\Users\\micsha\\Documents\\Test\\Test.csv");
            StreamReader SR = new StreamReader(TestCSV);

            //Read header
            string InRow = SR.ReadLine();
            var listOfSMS = new List<SmsMessage>();

            while (!SR.EndOfStream)
            {
                string Row = SR.ReadLine();
                string[] Columns = Row.Split(',');

                // Column[0] = First Name / Column[1] = Last Name / Column[2] = Mobile / Column[3] = email
                SmsMessage CurrentMessage = new SmsMessage(
                                             to: Columns[2],
                                             body: "Hello, this is a test message!",
                                             from: "",
                                             source: "sdk",
                                             schedule: 0
                                             );
                listOfSMS.Add(CurrentMessage);
            }

            // Post Messages:

            //var SMSCollesction = new SmsMessageCollection(listOfSMS);
            //string response = SMSAPI.SmsSendPost(SMSCollesction);
        }

        private void ContactCSV_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void CreateContactList_Click(object sender, EventArgs e)
        {
            string ContactListName = TBContactListName.Text;
            ContactList contactList = new ContactList( listName: ContactListName);

            var response = ContactListAPI.ListsPost(contactList);

            var test = 1;
        }

        private void AutoContactLists_Click(object sender, EventArgs e)
        {
            CSV = openFileDialog1.OpenFile();
            if (!CSV.CanRead) { return; }
            StreamReader SR = new StreamReader(CSV);

            string InRow = SR.ReadLine();
            int ContactsCreated = 0;

            string FileName = openFileDialog1.FileName;

            string[] SplitFileName = FileName.Split('\\');
            FileName = SplitFileName[SplitFileName.Length - 1];
            string[] SplitPeriod = FileName.Split('.');
            string ContactListName = SplitPeriod[0];

            ContactList contactList = new ContactList(listName: ContactListName);

            var response = ContactListAPI.ListsPost(contactList);

            int ContactListNumber = ParseContactListNumber(response);

            var AllContacts = new List<Contact>();
          
            while (!SR.EndOfStream) {
                string InLine = SR.ReadLine();
                string[] Columns = InLine.Split(',');
                string[] RemoveHyperLink = Columns[5].Split('"');
                string HyperLink = RemoveHyperLink[3];
                HyperLink = HyperLink.Replace("\"", "");

                Columns[2] = Columns[2].Remove(0,1);
                Columns[2].Trim();
                Columns[3] = Columns[3].Remove(Columns[3].Length - 1);
                Columns[3].Replace(" ", "");

                Contact NewContact = new Contact(
                    phoneNumber: Columns[0],
                    organizationName: Columns[1],
                    addressLine1: Columns[2],
                    addressCity: Columns[3],
                    custom1: Columns[4],
                    custom2: HyperLink,
                    custom3: Columns[6]
                    );

                AllContacts.Add( NewContact );
                try
                {
                    var AddContactResponse = ContactAPI.ListsContactsByListIdPost(NewContact, ContactListNumber);
                    ContactsCreated++;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                if (ContactsCreated >= 20000)
                {
                    ContactsCreated = 0;
                    AllContacts.Clear();
                    //Create contact list for adding contacts
                    string NewContactListName = "";

                    ContactList NewContactList = new ContactList(listName: NewContactListName);

                    var ContactListResponse = ContactListAPI.ListsPost(NewContactList);
                }//20K Contacts
            } //SR.EOF
        }
    }
}
