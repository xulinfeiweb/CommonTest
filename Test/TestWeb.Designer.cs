namespace Test
{
    partial class TestWeb
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
            this.webBrowserTest = new System.Windows.Forms.WebBrowser();
            this.btn_click = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webBrowserTest
            // 
            this.webBrowserTest.Location = new System.Drawing.Point(0, 0);
            this.webBrowserTest.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserTest.Name = "webBrowserTest";
            this.webBrowserTest.Size = new System.Drawing.Size(939, 686);
            this.webBrowserTest.TabIndex = 0;
            this.webBrowserTest.Url = new System.Uri("http://localhost:4064/Home/AdminDefault", System.UriKind.Absolute);
            this.webBrowserTest.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowserTest_DocumentCompleted);
            // 
            // btn_click
            // 
            this.btn_click.Location = new System.Drawing.Point(63, 712);
            this.btn_click.Name = "btn_click";
            this.btn_click.Size = new System.Drawing.Size(75, 23);
            this.btn_click.TabIndex = 1;
            this.btn_click.Text = " 测试";
            this.btn_click.UseVisualStyleBackColor = true;
            this.btn_click.Click += new System.EventHandler(this.btn_click_Click);
            // 
            // TestWeb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 747);
            this.Controls.Add(this.btn_click);
            this.Controls.Add(this.webBrowserTest);
            this.Name = "TestWeb";
            this.Text = "TestWeb";
            this.Load += new System.EventHandler(this.TestWeb_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.WebBrowser webBrowserTest;
        private System.Windows.Forms.Button btn_click;
    }
}