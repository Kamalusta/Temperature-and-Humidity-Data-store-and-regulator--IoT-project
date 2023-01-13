using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace arduino_project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SQLiteConnection connsql = new SQLiteConnection(@"Data Source = C:\Anbar app\myproyekt.db; Version=3");

        SqlConnection baglanti = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Baza.mdf;Integrated Security=True");
        string[] temprut1;
        string netice;
        int i = 1;
        int value1 = 0;
        int value2 = -600;
        DateTime minValue, maxValue, mdlValue;
        int saniye = 0;
        Boolean stop;

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'bazaDataSet.Temprut' table. You can move, or remove it, as needed.
            //  this.temprutTableAdapter.Fill(this.bazaDataSet.Temprut);
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);

            chart1.Series[0].XValueType = ChartValueType.Time;
            chart1.Series[1].XValueType = ChartValueType.Time;

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            chart1.ChartAreas[0].CursorX.IntervalType = DateTimeIntervalType.Seconds;

            chart1.ChartAreas[0].CursorX.AutoScroll = true;
            chart1.ChartAreas[0].CursorY.AutoScroll = true;

            chart1.ChartAreas[0].AxisX.ScrollBar.Size = 15;
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false;
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;

            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY2.ScaleView.Zoomable = true;

            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;

            chart1.ChartAreas[0].AxisY.Interval = 5;

            chart1.ChartAreas[0].AxisX.ScaleView.Zoom(chart1.ChartAreas[0].AxisX.Minimum, chart1.ChartAreas[0].AxisX.Maximum);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            value2 = -3600;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                serialPort1.DataReceived -= serialPort1_DataReceived;
                serialPort1.Close();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.ToString();

            if (netice != null)
            {
                baglanti.Open();
                SqlCommand emr = new SqlCommand("insert into Temprut (Temperatur,Rutubet,Tarix,Saat) values (@temp,@rut,@tarix,@tarix2) ", baglanti);
                emr.Parameters.AddWithValue("@temp", temprut1[1]);
                emr.Parameters.AddWithValue("@rut", temprut1[0]);
                emr.Parameters.AddWithValue("@tarix", DateTime.Now);
                emr.Parameters.AddWithValue("@tarix2", DateTime.Now);
                emr.ExecuteNonQuery();
                baglanti.Close();
                i++;

                connsql.Open();
                SQLiteCommand emr2 = new SQLiteCommand("insert into TempRut (Temperator,Rutubet,Tarix) values (@temp,@rut,@tarix)", connsql);
                emr2.Parameters.AddWithValue("@temp", temprut1[1]);
                emr2.Parameters.AddWithValue("@rut", temprut1[0]);
                emr2.Parameters.AddWithValue("@tarix", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                emr2.ExecuteNonQuery();
                connsql.Close();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
            f2.chartF.Series[0].Points.Clear();
            f2.chartF.Series[1].Points.Clear();

            //baglanti.Open();
            //SqlCommand emr = new SqlCommand("SELECT *from Temprut WHERE Tarix >= @baslangic and Tarix <= @bitis", baglanti);
            //emr.Parameters.AddWithValue("@baslangic", dateTimePicker1.Value.ToLongTimeString());
            //emr.Parameters.AddWithValue("@bitis", dateTimePicker2.Value.Date);
            //SqlDataReader oxu = emr.ExecuteReader();
            //SqlDataAdapter adapter = new SqlDataAdapter(emr);
            //while (oxu.Read())
            //    f2.chartF.DataSource = adapter;
            //baglanti.Close();

            int min = dateTimePicker1.Value.Day;
            int max = dateTimePicker2.Value.Day;


            connsql.Open();
            SQLiteCommand emr2 = new SQLiteCommand("SELECT Temperator, Rutubet, Tarix from TempRut WHERE Tarix BETWEEN @baslangic and @bitis", connsql);
            emr2.Parameters.AddWithValue("@baslangic", dateTimePicker1.Value);
            emr2.Parameters.AddWithValue("@bitis", dateTimePicker2.Value);
            SQLiteDataReader oxu2 = emr2.ExecuteReader();
            // DataSet ds = new DataSet();
            DataTable dt = new DataTable("TempRut");
            dt.Load(oxu2);
            // ds.Tables.Add(dt);
            //  SQLiteDataAdapter adapter2 = new SQLiteDataAdapter(emr2);
            //  adapter2.Fill(ds);
            // while (oxu2.Read())

            connsql.Close();



            f2.chartF.DataSource = dt;
            f2.chartF.Series[0].XValueMember = "Tarix";
            f2.chartF.Series[0].YValueMembers = "Temperator";
            f2.chartF.Series[1].XValueMember = "Tarix";
            f2.chartF.Series[1].YValueMembers = "Rutubet";
            f2.chartF.DataBind();




            f2.chartF.Series[0].XValueType = ChartValueType.DateTime;
            f2.chartF.Series[1].XValueType = ChartValueType.DateTime;

            f2.chartF.ChartAreas[0].AxisX.LabelStyle.Format = "dd.MM.yyyy  HH:mm:ss";
            //  f2.chartF.ChartAreas[0].CursorX.IntervalType = DateTimeIntervalType.Seconds;
            //  f2.chartF.ChartAreas[0].CursorX.Interval = 60;
            //  f2.chartF.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Minutes;
            //  f2.chartF.ChartAreas[0].AxisX.Interval = 1;

            f2.chartF.ChartAreas[0].CursorX.AutoScroll = true;
            f2.chartF.ChartAreas[0].CursorY.AutoScroll = true;

            f2.chartF.ChartAreas[0].AxisX.ScrollBar.Size = 15;
            f2.chartF.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            f2.chartF.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false;
            f2.chartF.ChartAreas[0].AxisX.ScrollBar.Enabled = true;

            f2.chartF.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            f2.chartF.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            f2.chartF.ChartAreas[0].AxisY2.ScaleView.Zoomable = true;

            f2.chartF.ChartAreas[0].CursorX.IsUserEnabled = true;
            f2.chartF.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;

            f2.chartF.ChartAreas[0].AxisY.Interval = 5;

            f2.chartF.ChartAreas[0].AxisX.ScaleView.Zoom(f2.chartF.ChartAreas[0].AxisX.Minimum, f2.chartF.ChartAreas[0].AxisX.Maximum);

            //        f2.chartF.ChartAreas[0].AxisX.Minimum = dateTimePicker1.Value.ToOADate();
            //          f2.chartF.ChartAreas[0].AxisX.Maximum = dateTimePicker2.Value.ToOADate();




            // f2.chartF.Series[0].Points.InsertXY(min, max);  
            // f2.chartF.Series[1].Points.Clear();
            // f2.chartF.Series[1].Points.AddY(min, max);


        }

        private void button4_Click(object sender, EventArgs e)
        {
            value2 = -600;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        Boolean siqnal = false;

        public void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            netice = serialPort1.ReadLine();
            temprut1 = netice.Split('/');

            minValue = DateTime.Now.AddSeconds(value2);
            maxValue = DateTime.Now.AddSeconds(value1);
            mdlValue = DateTime.Now;

            chart1.ChartAreas[0].AxisX.Minimum = minValue.ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = maxValue.ToOADate();

            this.chart1.Series[0].Points.AddXY(mdlValue.ToOADate(), temprut1[1]);
            this.chart1.Series[1].Points.AddXY(mdlValue.ToOADate(), temprut1[0]);

            int yuksektemeratur = Convert.ToInt32(temprut1[1]);
            if (yuksektemeratur > 23 && stop == false && siqnal == false)
            {
                label1.Text = "YUKSEK TEMPERATUR";
                label1.ForeColor = Color.Firebrick;
                serialPort1.Write("1");
                label2.ForeColor = Color.Firebrick;
                saniye = saniye + 1;
                if (saniye > 10)
                {
                    // button3.BackColor = Color.Green;
                    serialPort1.Write("3");
                    //  button3.Text = "START";
                    siqnal = true;
                    //  label1.Visible = false;
                }

            }


            if (yuksektemeratur <= 23 && stop != true)
            {
                label1.Text = "NORMAL TEMPERATUR";
                label1.ForeColor = Color.Green;
                serialPort1.Write("2");
                label2.ForeColor = Color.Green;
                saniye = 0;
                siqnal = false;
            }
            if (yuksektemeratur > 23)
                label4.ForeColor = Color.Firebrick;
            else
                label4.ForeColor = Color.Green;

            serialPort1.DiscardInBuffer();

            label2.Text = temprut1[2];
            label4.Text = temprut1[1] + " C";
            label6.Text = temprut1[0] + " %";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.BackColor == Color.Green)
            {
                if (serialPort1.IsOpen != true)
                {
                    serialPort1.Open();
                    timer1.Start();
                }
                button3.BackColor = Color.Firebrick;
                serialPort1.Write("4");
                button3.Text = "STOP";
                stop = false;
                label1.Visible = true;
                saniye = 0;
                siqnal = false;
            }
            else if (button3.BackColor == Color.Firebrick)
            {
                button3.BackColor = Color.Green;
                serialPort1.Write("3");
                button3.Text = "START";
                stop = true;
                label1.Visible = false;
                saniye = 0;
                siqnal = false;
            }
        }
    }
}
