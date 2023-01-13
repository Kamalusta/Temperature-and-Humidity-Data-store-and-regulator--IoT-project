using System;
using System.Windows.Forms;


namespace arduino_project
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //PrintDialog MyPrintDialog = new PrintDialog();

            //if (MyPrintDialog.ShowDialog() == DialogResult.OK)
            //{
            //    System.Drawing.Printing.PrinterSettings values;
            //    values = MyPrintDialog.PrinterSettings;
            //    MyPrintDialog.Document = printDocument1;
            //    printDocument1.PrintController = new System.Drawing.Printing.StandardPrintController();
            //    printDocument1.Print();
            //}

            //printDocument1.Dispose();

            chartF.Printing.PrintDocument.DefaultPageSettings.Landscape = true;
            chartF.Printing.PrintDocument.DefaultPageSettings.Margins.Left = 0;
            chartF.Printing.PrintDocument.DefaultPageSettings.Margins.Right = 0;
            chartF.Printing.PrintDocument.DefaultPageSettings.Margins.Top = 0;
            chartF.Printing.PrintDocument.DefaultPageSettings.Margins.Bottom = 0;


            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = this.chartF.Printing.PrintDocument;

            ppd.ShowDialog();

            //  chartF.Printing.PrintDocument.DocumentName = "printDocument1";


        }
    }
}
