namespace MessengerClientWinForm
{
    partial class FMessenger
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
            this.rtbTextArea = new System.Windows.Forms.RichTextBox();
            this.bSend = new System.Windows.Forms.Button();
            this.tbSendText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // rtbTextArea
            // 
            this.rtbTextArea.Location = new System.Drawing.Point(13, 13);
            this.rtbTextArea.Name = "rtbTextArea";
            this.rtbTextArea.Size = new System.Drawing.Size(331, 323);
            this.rtbTextArea.TabIndex = 0;
            this.rtbTextArea.Text = "";
            // 
            // bSend
            // 
            this.bSend.BackColor = System.Drawing.Color.SkyBlue;
            this.bSend.Location = new System.Drawing.Point(269, 345);
            this.bSend.Name = "bSend";
            this.bSend.Size = new System.Drawing.Size(75, 23);
            this.bSend.TabIndex = 1;
            this.bSend.Text = "Отправить";
            this.bSend.UseVisualStyleBackColor = false;
            this.bSend.Click += new System.EventHandler(this.bSend_Click);
            // 
            // tbSendText
            // 
            this.tbSendText.BackColor = System.Drawing.SystemColors.Info;
            this.tbSendText.Location = new System.Drawing.Point(13, 347);
            this.tbSendText.Name = "tbSendText";
            this.tbSendText.Size = new System.Drawing.Size(250, 20);
            this.tbSendText.TabIndex = 2;
            // 
            // FMessenger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(356, 402);
            this.ControlBox = false;
            this.Controls.Add(this.tbSendText);
            this.Controls.Add(this.bSend);
            this.Controls.Add(this.rtbTextArea);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(372, 418);
            this.Name = "FMessenger";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Messenger";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbTextArea;
        private System.Windows.Forms.Button bSend;
        private System.Windows.Forms.TextBox tbSendText;
    }
}

