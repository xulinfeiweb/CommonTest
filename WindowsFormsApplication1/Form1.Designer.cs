namespace WindowsFormsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btn_id = new System.Windows.Forms.Button();
            this.txtID = new System.Windows.Forms.TextBox();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.txt_w = new System.Windows.Forms.TextBox();
            this.txt_h = new System.Windows.Forms.TextBox();
            this.label_w = new System.Windows.Forms.Label();
            this.label_h = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // btn_id
            // 
            this.btn_id.Location = new System.Drawing.Point(51, 87);
            this.btn_id.Name = "btn_id";
            this.btn_id.Size = new System.Drawing.Size(75, 23);
            this.btn_id.TabIndex = 0;
            this.btn_id.Text = "选择文件";
            this.btn_id.UseVisualStyleBackColor = true;
            this.btn_id.Click += new System.EventHandler(this.btn_id_Click);
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(176, 87);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(311, 21);
            this.txtID.TabIndex = 1;
            // 
            // richTextBox
            // 
            this.richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox.Location = new System.Drawing.Point(12, 156);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(957, 547);
            this.richTextBox.TabIndex = 2;
            this.richTextBox.Text = "";
            // 
            // txt_w
            // 
            this.txt_w.Location = new System.Drawing.Point(145, 37);
            this.txt_w.Name = "txt_w";
            this.txt_w.Size = new System.Drawing.Size(100, 21);
            this.txt_w.TabIndex = 3;
            // 
            // txt_h
            // 
            this.txt_h.Location = new System.Drawing.Point(365, 37);
            this.txt_h.Name = "txt_h";
            this.txt_h.Size = new System.Drawing.Size(103, 21);
            this.txt_h.TabIndex = 4;
            // 
            // label_w
            // 
            this.label_w.AutoSize = true;
            this.label_w.Location = new System.Drawing.Point(49, 40);
            this.label_w.Name = "label_w";
            this.label_w.Size = new System.Drawing.Size(65, 12);
            this.label_w.TabIndex = 5;
            this.label_w.Text = "宽度（W）:";
            // 
            // label_h
            // 
            this.label_h.AutoSize = true;
            this.label_h.Location = new System.Drawing.Point(272, 40);
            this.label_h.Name = "label_h";
            this.label_h.Size = new System.Drawing.Size(77, 12);
            this.label_h.TabIndex = 6;
            this.label_h.Text = "高度度（H）:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(51, 127);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(193, 127);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 704);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label_h);
            this.Controls.Add(this.label_w);
            this.Controls.Add(this.txt_h);
            this.Controls.Add(this.txt_w);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.btn_id);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btn_id;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.TextBox txt_w;
        private System.Windows.Forms.TextBox txt_h;
        private System.Windows.Forms.Label label_w;
        private System.Windows.Forms.Label label_h;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

