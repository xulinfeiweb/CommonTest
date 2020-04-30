namespace PDF_Service
{
    partial class PDF_Form
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
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btn_submit = new System.Windows.Forms.Button();
            this.txtID = new System.Windows.Forms.TextBox();
            this.btn_id = new System.Windows.Forms.Button();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.txt_pdf_value = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_img_path = new System.Windows.Forms.TextBox();
            this.btn_img = new System.Windows.Forms.Button();
            this.btn_query = new System.Windows.Forms.Button();
            this.txt_pdf_path = new System.Windows.Forms.TextBox();
            this.btn_pdf = new System.Windows.Forms.Button();
            this.btn_adc = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // btn_submit
            // 
            this.btn_submit.Location = new System.Drawing.Point(713, 166);
            this.btn_submit.Name = "btn_submit";
            this.btn_submit.Size = new System.Drawing.Size(75, 23);
            this.btn_submit.TabIndex = 11;
            this.btn_submit.Text = "生成PDF";
            this.btn_submit.UseVisualStyleBackColor = true;
            this.btn_submit.Click += new System.EventHandler(this.btn_submit_Click);
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(162, 128);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(311, 21);
            this.txtID.TabIndex = 10;
            // 
            // btn_id
            // 
            this.btn_id.Location = new System.Drawing.Point(61, 126);
            this.btn_id.Name = "btn_id";
            this.btn_id.Size = new System.Drawing.Size(75, 23);
            this.btn_id.TabIndex = 9;
            this.btn_id.Text = "选择文件";
            this.btn_id.UseVisualStyleBackColor = true;
            this.btn_id.Click += new System.EventHandler(this.btn_id_Click);
            // 
            // richTextBox
            // 
            this.richTextBox.Location = new System.Drawing.Point(39, 195);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(957, 523);
            this.richTextBox.TabIndex = 13;
            this.richTextBox.Text = "";
            // 
            // txt_pdf_value
            // 
            this.txt_pdf_value.Location = new System.Drawing.Point(675, 21);
            this.txt_pdf_value.Name = "txt_pdf_value";
            this.txt_pdf_value.Size = new System.Drawing.Size(311, 21);
            this.txt_pdf_value.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(587, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "填写数据";
            // 
            // txt_img_path
            // 
            this.txt_img_path.Location = new System.Drawing.Point(675, 76);
            this.txt_img_path.Name = "txt_img_path";
            this.txt_img_path.Size = new System.Drawing.Size(311, 21);
            this.txt_img_path.TabIndex = 17;
            // 
            // btn_img
            // 
            this.btn_img.Location = new System.Drawing.Point(574, 74);
            this.btn_img.Name = "btn_img";
            this.btn_img.Size = new System.Drawing.Size(75, 23);
            this.btn_img.TabIndex = 16;
            this.btn_img.Text = "选择图片";
            this.btn_img.UseVisualStyleBackColor = true;
            this.btn_img.Click += new System.EventHandler(this.btn_img_Click);
            // 
            // btn_query
            // 
            this.btn_query.Location = new System.Drawing.Point(223, 166);
            this.btn_query.Name = "btn_query";
            this.btn_query.Size = new System.Drawing.Size(109, 23);
            this.btn_query.TabIndex = 18;
            this.btn_query.Text = "获取PDF内容";
            this.btn_query.UseVisualStyleBackColor = true;
            this.btn_query.Click += new System.EventHandler(this.btn_query_Click);
            // 
            // txt_pdf_path
            // 
            this.txt_pdf_path.Location = new System.Drawing.Point(675, 130);
            this.txt_pdf_path.Name = "txt_pdf_path";
            this.txt_pdf_path.Size = new System.Drawing.Size(311, 21);
            this.txt_pdf_path.TabIndex = 20;
            // 
            // btn_pdf
            // 
            this.btn_pdf.Location = new System.Drawing.Point(554, 128);
            this.btn_pdf.Name = "btn_pdf";
            this.btn_pdf.Size = new System.Drawing.Size(95, 23);
            this.btn_pdf.TabIndex = 19;
            this.btn_pdf.Text = "选择PDF文件";
            this.btn_pdf.UseVisualStyleBackColor = true;
            this.btn_pdf.Click += new System.EventHandler(this.btn_pdf_Click);
            // 
            // btn_adc
            // 
            this.btn_adc.Location = new System.Drawing.Point(364, 76);
            this.btn_adc.Name = "btn_adc";
            this.btn_adc.Size = new System.Drawing.Size(109, 23);
            this.btn_adc.TabIndex = 21;
            this.btn_adc.Text = "测试";
            this.btn_adc.UseVisualStyleBackColor = true;
            this.btn_adc.Click += new System.EventHandler(this.btn_adc_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(66, 74);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(226, 21);
            this.textBox1.TabIndex = 22;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(349, 166);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 23);
            this.button1.TabIndex = 23;
            this.button1.Text = "PDF内容转XML";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(871, 166);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 24;
            this.button2.Text = "压缩";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(473, 166);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 25;
            this.button3.Text = "生成word";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(574, 166);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 26;
            this.button4.Text = "生成word2";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(333, 19);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 27;
            this.button5.Text = "word转PDF";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(86, 166);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(109, 23);
            this.button6.TabIndex = 28;
            this.button6.Text = "获取PDF局部内容";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // PDF_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 730);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btn_adc);
            this.Controls.Add(this.txt_pdf_path);
            this.Controls.Add(this.btn_pdf);
            this.Controls.Add(this.btn_query);
            this.Controls.Add(this.txt_img_path);
            this.Controls.Add(this.btn_img);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_pdf_value);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.btn_submit);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.btn_id);
            this.Name = "PDF_Form";
            this.Text = "PDF_Form";
            this.Load += new System.EventHandler(this.PDF_Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btn_submit;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Button btn_id;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.TextBox txt_pdf_value;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_img_path;
        private System.Windows.Forms.Button btn_img;
        private System.Windows.Forms.Button btn_query;
        private System.Windows.Forms.TextBox txt_pdf_path;
        private System.Windows.Forms.Button btn_pdf;
        private System.Windows.Forms.Button btn_adc;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
    }
}

