using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; //library requirement for SQL connection

namespace apart_yönetici
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable();
        SqlDataAdapter da;
        int tutar, top, result;
        DateTime date;
        string today;
        void list(string srg) //the program comes to the list according to the resident of the apartment that you work in.
        {
            dt.Clear();
            Form1.con.Open();
            Form1.cmd = new SqlCommand("Select * From users1 where user_ad='"+srg+"'", Form1.con);
            da = new SqlDataAdapter(Form1.cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            Form1.con.Close();
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            Form1.con.Close();
            list(Form1.name);
            groupBox2.Enabled = false;
            groupBox3.Visible = false;
            DateTime date = DateTime.Now;
            string today = Convert.ToString(date.AddDays(5));
            button2.Enabled = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        { //To print data from a table to texboxes
            int satır = dataGridView1.CurrentRow.Index;
            textBox1.Text = dataGridView1.Rows[satır].Cells["tel"].Value.ToString();
            textBox2.Text = dataGridView1.Rows[satır].Cells["user_ad"].Value.ToString();
            textBox3.Text = dataGridView1.Rows[satır].Cells["password"].Value.ToString();
            textBox4.Text = dataGridView1.Rows[satır].Cells["hot_water"].Value.ToString();
            textBox5.Text = dataGridView1.Rows[satır].Cells["cold_water"].Value.ToString();
            textBox6.Text = dataGridView1.Rows[satır].Cells["park"].Value.ToString();
            textBox7.Text = dataGridView1.Rows[satır].Cells["cleaning"].Value.ToString();
            textBox8.Text = dataGridView1.Rows[satır].Cells["total_payout"].Value.ToString();
            textBox9.Text = dataGridView1.Rows[satır].Cells["son_tarıh"].Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int kontrol = Convert.ToInt32(textBox8.Text);
            if(kontrol==0)
            {
                MessageBox.Show("You Have No Payment.");
            }
            else
            {
                groupBox3.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //fiş oluşturulacak
            ppDialog.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //çıkış
            Form1 frm1 = new Form1();
            frm1.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //günceleme yapılacak 
            Form1.con.Open();
            Form1.cmd = new SqlCommand("Update users1 set tel=@tel,user_ad=@ad,password=@pass where user_ad=@namee", Form1.con);
            Form1.cmd.Parameters.AddWithValue("@tel", textBox1.Text);
            Form1.cmd.Parameters.AddWithValue("@ad", textBox2.Text);
            Form1.cmd.Parameters.AddWithValue("@pass", textBox3.Text);
            Form1.cmd.Parameters.AddWithValue("@namee", Form1.name);
            Form1.cmd.ExecuteNonQuery();
            Form1.con.Close();
            MessageBox.Show("Güncelleme Başarılı");
            list(Form1.name);
        }

        private void button5_Click(object sender, EventArgs e)
        {
             date = DateTime.Now;
             today = Convert.ToString(date.AddDays(5));
            top = Convert.ToInt32(textBox8.Text);
            //ödeme al
            if (textBox10.Text!="")
            {
                tutar = Convert.ToInt32(textBox10.Text);
                if (tutar<=top)
                {
                    result = top - tutar; //sıfır çıkarsa tüm borcu ödedi 
                    if(result==0)
                    {
                        Form1.con.Open();
                        Form1.cmd = new SqlCommand("Update users1 set total_payout=@toplam where user_ad=@namee", Form1.con);
                        Form1.cmd.Parameters.AddWithValue("@toplam", result.ToString());
                        Form1.cmd.Parameters.AddWithValue("@namee", Form1.name);
                        Form1.cmd.ExecuteNonQuery();
                        Form1.con.Close();
                        MessageBox.Show("Ödeme İşlemi Başarılı");
                        list(Form1.name);
                        groupBox3.Visible = false;
                        button2.Enabled = true;
                        textBox8.Text = result.ToString();
                    }
                    else
                    {
                        Form1.con.Open();
                        Form1.cmd = new SqlCommand("Update users1 set total_payout=@toplam,son_tarıh=@son where user_ad=@namee", Form1.con);
                        Form1.cmd.Parameters.AddWithValue("@toplam", result.ToString());
                        Form1.cmd.Parameters.AddWithValue("@son", today);
                        Form1.cmd.Parameters.AddWithValue("@namee", Form1.name);
                        Form1.cmd.ExecuteNonQuery();
                        Form1.con.Close();
                        MessageBox.Show("Ödeme İşlemi Başarılı");
                        list(Form1.name);
                        groupBox3.Visible = false;
                        button2.Enabled = true;
                        textBox8.Text = result.ToString();
                    }

                }
                else
                {
                    MessageBox.Show("You Can't Pay More Than The Total");
                }
                


            }
            else
            {
                MessageBox.Show("Enter The Amount Payable");
            }
        }

        Font Baslik = new Font("Verdana", 12, FontStyle.Bold);
        Font veri = new Font("Calibri", 12, FontStyle.Italic);//veri yazıları
        SolidBrush sb = new SolidBrush(Color.Black);
        private void pdYazici_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            StringFormat sFormat = new StringFormat();
            sFormat.Alignment = StringAlignment.Near;
            Font başlık = new Font("Times New Roman", 14, FontStyle.Underline);
            e.Graphics.DrawString(" Payment Receipt ", Baslik, sb, 200, 100);
            e.Graphics.DrawString("----------------------------------------------------------------------------------", Baslik, sb, 70, 170);
            ///////////////////////////////////////////////////////////
            e.Graphics.DrawString("NAME", başlık, Brushes.Red, 70, 150);
            e.Graphics.DrawString("SURNAME", başlık, Brushes.Red, 230, 150);
            e.Graphics.DrawString("Payment Total", başlık, Brushes.Red, 380, 150);
            e.Graphics.DrawString("Total", başlık, Brushes.Red, 550, 150);
            e.Graphics.DrawString("Islem_Tarıh", başlık, Brushes.Red, 650, 150);
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {

                e.Graphics.DrawString(dataGridView1.Rows[i].Cells["name"].Value.ToString(), veri, sb, 70, 190 + (i * 30));
                e.Graphics.DrawString(dataGridView1.Rows[i].Cells["surname"].Value.ToString(), veri, sb, 250, 190 + (i * 30));
                e.Graphics.DrawString(dataGridView1.Rows[i].Cells["total_payout"].Value.ToString(), veri, sb, 450, 190 + (i * 30));
                e.Graphics.DrawString(top.ToString(), veri, sb, 550, 190 + (i * 30));
                e.Graphics.DrawString(today, veri, sb, 650, 190 + (i * 30));
            }
            e.Graphics.DrawString("------------------------------------------------------------------------------------", Baslik, sb, 70, 190 + ((dataGridView1.Rows.Count-1) * 30));
            e.Graphics.DrawString("Remaining Debt : " +result.ToString(), Baslik, sb, 520, 190 + ((dataGridView1.Rows.Count - 1) * 30 + 30));
        }
    }
}
