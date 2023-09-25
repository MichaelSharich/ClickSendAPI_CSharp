using IO.ClickSend.ClickSend.Api;
using IO.ClickSend.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClickSendAPI
{
    public partial class CredentialsPopup : Form
    {

        MainForm mainForm;
        public CredentialsPopup()
        {
            InitializeComponent();
            this.Visible = true;
        }

        public void SetMainForm(MainForm InMain)
        {
            mainForm = InMain;
        }

        private void btnAPILogin_Click(object sender, EventArgs e)
        {
            Configuration config = new Configuration()
            {
                Username = TBUserName.Text,
                Password = TBAPIKey.Text
            };

            string response = "";
            try
            {
                ContactListApi APITest = new ContactListApi(config);
                response = APITest.ListsGet();
                
                if(response.Equals(""))
                {
                    //ErrorBox

                }

            }
            catch(Exception ex)
            {

            }

            string LoginCheck = response.Substring(13, 3);

            if (LoginCheck.Equals("200"))
            {
                mainForm.InitializeAPI(config);
            }

        }
    }
}
