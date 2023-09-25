namespace ClickSendAPI
{
    partial class CredentialsPopup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblUserName = new System.Windows.Forms.Label();
            this.TBUserName = new System.Windows.Forms.TextBox();
            this.lblAPIKey = new System.Windows.Forms.Label();
            this.TBAPIKey = new System.Windows.Forms.TextBox();
            this.btnAPILogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(523, 37);
            this.lblUserName.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(152, 28);
            this.lblUserName.TabIndex = 0;
            this.lblUserName.Text = "User Name:";
            // 
            // TBUserName
            // 
            this.TBUserName.Location = new System.Drawing.Point(269, 80);
            this.TBUserName.Name = "TBUserName";
            this.TBUserName.Size = new System.Drawing.Size(723, 39);
            this.TBUserName.TabIndex = 1;
            // 
            // lblAPIKey
            // 
            this.lblAPIKey.AutoSize = true;
            this.lblAPIKey.Location = new System.Drawing.Point(535, 146);
            this.lblAPIKey.Name = "lblAPIKey";
            this.lblAPIKey.Size = new System.Drawing.Size(124, 28);
            this.lblAPIKey.TabIndex = 2;
            this.lblAPIKey.Text = "API Key:";
            // 
            // TBAPIKey
            // 
            this.TBAPIKey.Location = new System.Drawing.Point(269, 188);
            this.TBAPIKey.Name = "TBAPIKey";
            this.TBAPIKey.Size = new System.Drawing.Size(723, 39);
            this.TBAPIKey.TabIndex = 3;
            // 
            // btnAPILogin
            // 
            this.btnAPILogin.Location = new System.Drawing.Point(516, 256);
            this.btnAPILogin.Name = "btnAPILogin";
            this.btnAPILogin.Size = new System.Drawing.Size(173, 55);
            this.btnAPILogin.TabIndex = 4;
            this.btnAPILogin.Text = "Login";
            this.btnAPILogin.UseVisualStyleBackColor = true;
            this.btnAPILogin.Click += new System.EventHandler(this.btnAPILogin_Click);
            // 
            // CredentialsPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.ClientSize = new System.Drawing.Size(1232, 332);
            this.Controls.Add(this.btnAPILogin);
            this.Controls.Add(this.TBAPIKey);
            this.Controls.Add(this.lblAPIKey);
            this.Controls.Add(this.TBUserName);
            this.Controls.Add(this.lblUserName);
            this.Font = new System.Drawing.Font("SimSun", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "CredentialsPopup";
            this.Text = "CredentialsPopup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox TBUserName;
        private System.Windows.Forms.Label lblAPIKey;
        private System.Windows.Forms.TextBox TBAPIKey;
        private System.Windows.Forms.Button btnAPILogin;
    }
}