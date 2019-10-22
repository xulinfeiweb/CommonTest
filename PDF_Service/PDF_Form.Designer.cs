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
            this.txtID.Location = new System.Drawing.Point(156, 94);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(311, 21);
            this.txtID.TabIndex = 10;
            // 
            // btn_id
            // 
            this.btn_id.Location = new System.Drawing.Point(55, 92);
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
            this.btn_query.Location = new System.Drawing.Point(88, 149);
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
            this.btn_adc.Location = new System.Drawing.Point(254, 149);
            this.btn_adc.Name = "btn_adc";
            this.btn_adc.Size = new System.Drawing.Size(109, 23);
            this.btn_adc.TabIndex = 21;
            this.btn_adc.Text = "测试";
            this.btn_adc.UseVisualStyleBackColor = true;
            this.btn_adc.Click += new System.EventHandler(this.btn_adc_Click);
            // 
            // PDF_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 730);
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
    }
}

