//using PdfSharp.Drawing;
//using PdfSharp.Pdf;
//using PdfSharp.Pdf.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using java.io;
//using java.awt.geom;
//using java.awt.image;
//using java.io;
//using javax.imageio;
//using org.apache.pdfbox.io;
//using org.apache.pdfbox.pdfparser;
//using org.apache.pdfbox.pdmodel;
//using org.apache.pdfbox.pdmodel.edit;
//using org.apache.pdfbox.pdmodel.graphics.xobject;
//using org.apache.pdfbox.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //https://www.cnblogs.com/yjd_hycf_space/p/7942444.html
        //https://www.cnblogs.com/luozhongtao/p/9667190.html
        //https://oomake.com/question/2251600
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string file = GetPath();
        }

        private void btn_id_Click(object sender, EventArgs e)
        {
            string file = GetPath();
            //string str = OnCreated(file);

            //PDDocument document = null;
            //document = PDDocument.load(file);
            //if (document.isEncrypted())
            //{
            //    document.decrypt("");
            //}
            //int w = 120;
            //int h = 100;
            //if (!string.IsNullOrWhiteSpace(txt_w.Text))
            //{
            //    w = Convert.ToInt32(txt_w.Text);
            //}
            //if (!string.IsNullOrWhiteSpace(txt_h.Text))
            //{
            //    h = Convert.ToInt32(txt_h.Text);
            //}
            //PDFTextStripperByArea stripper = new PDFTextStripperByArea();
            //stripper.setSortByPosition(true);
            //Rectangle2D.Double d = new Rectangle2D.Double(0, 0, w, h);
            //stripper.addRegion("class1", d);
            //java.util.List allPages = document.getDocumentCatalog().getAllPages();
            //PDPage firstPage = (PDPage)allPages.get(0);
            //stripper.extractRegions(firstPage);
            //var str = stripper.getTextForRegion("class1");
            //str = stripper.getText(document);
            //richTextBox.Text = str;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string file = GetPath();
            WritePDF(file);
            //FileInputStream ins = new FileInputStream(file);
            //PDFParser parser = new PDFParser(ins);
            //parser.parse();
            //PDDocument pdDocument = parser.getPDDocument();
            //PDFTextStripper stripper = new PDFTextStripper();
            //string result = stripper.getText(pdDocument);
            //richTextBox.Text = result;

            //string path = "";
            //createPDFFromImage(file, "C:\\Users\\Administrator\\Desktop\\图片\\不认识.jpg", path);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string file = GetPath();
            PdfReader reader = new PdfReader(file);

            PdfReaderContentParser parser = new PdfReaderContentParser(reader);

            ITextExtractionStrategy strategy;
            strategy = parser.ProcessContent<SimpleTextExtractionStrategy>(1, new SimpleTextExtractionStrategy());
            richTextBox.Text = strategy.GetResultantText();
        }
        public void WritePDF(string path)
        {
            string filename = System.IO.Path.GetFileNameWithoutExtension(path) + "_new.pdf";
            string currpath = System.IO.Path.GetDirectoryName(path) + "\\" + filename;
            using (FileStream stream = new FileStream(currpath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                //string path2 = "C:\\Users\\Administrator\\Desktop\\pdf\\802984579.pdf";
                PdfReader pdfReader = new PdfReader(path);//读pdf
                using (PdfStamper pdfStamper = new PdfStamper(pdfReader, stream))
                {
                    BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//获取系统的字体
                    //BaseFont baseFontk = BaseFont.CreateFont("C:\\Windows\\Fonts\\simkai.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                    iTextSharp.text.Font font2 = new iTextSharp.text.Font(baseFont, 11);//字体样式
                    Phrase ActualName2 = new Phrase("编号：你是我的小苹果", font2);//姓名
                    PdfContentByte over2 = pdfStamper.GetOverContent(1);//pdf页数
                    ColumnText.ShowTextAligned(over2, Element.ALIGN_CENTER, ActualName2, 265, 800, 0);//姓名

                    MessageBox.Show("生成成功。");
                }
            }
        }
        public string GetPath()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*.*)|*.*";
            string file = "";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                file = fileDialog.FileName;
                this.txtID.Text = file;
            }
            return file;
        }


        /**
    * Add an image to an existing PDF document.
    *
    * @param inputFile The input PDF to add the image to.
    * @param image The filename of the image to put in the PDF.
    * @param outputFile The file to write to the pdf to.
    *
    * @throws IOException If there is an error writing the data.
    * @throws COSVisitorException If there is an error writing the PDF.
    */
        //public void createPDFFromImage(String inputFile, String image, String outputFile)
        //{
        //    // the document
        //    PDDocument doc = null;
        //    try
        //    {
        //        doc = PDDocument.load(inputFile);

        //        //we will add the image to the first page.
        //        PDPage page = (PDPage)doc.getDocumentCatalog().getAllPages().get(0);

        //        PDXObjectImage ximage = null;
        //        if (image.ToLower().EndsWith(".jpg") || image.ToLower().EndsWith(".png"))
        //        {
        //            InputStream stream = new FileInputStream(image);
        //            ximage = new PDJpeg(doc, stream);
        //        }
        //        else if (image.ToLower().EndsWith(".tif") || image.ToLower().EndsWith(".tiff"))
        //        {
        //            //ximage = new PDCcitt(doc, new java.io.RandomAccessFile(new java.io.File(image), "r"));
        //        }
        //        else
        //        {
        //            BufferedImage awtImage = ImageIO.read(new java.io.File(image));
        //            ximage = new PDPixelMap(doc, awtImage);
        //        }
        //        PDPageContentStream contentStream = new PDPageContentStream(doc, page, true, true);

        //        //contentStream.drawImage(ximage, 20, 20 );
        //        // better method inspired by http://stackoverflow.com/a/22318681/535646
        //        float scale = 0.5f; // reduce this value if the image is too large

        //        ximage.setHeight(ximage.getHeight() / 5);
        //        ximage.setWidth(ximage.getWidth() / 5);
        //        contentStream.drawXObject(ximage, 20, 200, ximage.getWidth() * scale, ximage.getHeight() * scale);

        //        contentStream.close();
        //        doc.save(outputFile);
        //    }
        //    finally
        //    {
        //        if (doc != null)
        //        {
        //            doc.close();
        //        }
        //    }
        //}

        //private string OnCreated(string filepath)
        //{
        //    try
        //    {
        //        string pdffilename = filepath;
        //        PdfReader pdfReader = new PdfReader(pdffilename);
        //        int numberOfPages = pdfReader.NumberOfPages;
        //        string text = string.Empty;

        //        for (int i = 1; i <= numberOfPages; ++i)
        //        {
        //            byte[] bufferOfPageContent = pdfReader.GetPageContent(i);
        //            text += System.Text.Encoding.UTF8.GetString(bufferOfPageContent);
        //        }
        //        pdfReader.Close();

        //        return text;
        //    }
        //    catch (Exception ex)
        //    {
        //        StreamWriter wlog = File.AppendText(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\mylog.log");
        //        wlog.WriteLine("原因：" + ex.ToString());
        //        wlog.Flush();
        //        wlog.Close(); return null;
        //    }


        //}

    }
}
