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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        SqlDataAdapter da;
        DataTable dt = new DataTable();
        int a, b, c, d, total, id;
        //burası yönetici ile giriş yapılığında açılacak ekran 
        void listele() //tüm tabloyu çeker 
        {
            dt.Clear();
            Form1.con.Open();
            Form1.cmd = new SqlCommand("Select * From users1", Form1.con);
            da = new SqlDataAdapter(Form1.cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            Form1.con.Close();
            label1.Text = "Total Users: " + (dataGridView1.Rows.Count - 1);
            label2.Text = DateTime.Now.ToLongDateString();
            groupBox2.Text = "List";
        }
        void update_list() //güncelleme işleminden sonra tekrar listele
        {
            dt.Clear();
            Form1.con.Open();
            Form1.cmd = new SqlCommand("Select * From users1", Form1.con);
            da = new SqlDataAdapter(Form1.cmd);
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }
        void kayıt2() //faize giren ödemeleri ayrı tabloda tutmak için 
        {
            int faiz = 0;
            int gun = 0;
            Form1.con.Open();
            Form1.cmd.Connection = Form1.con;
            Form1.cmd = new SqlCommand("insert into users11 values('" + id + "','" + textBox1.Text + "','" + textBox2.Text + "','" + total.ToString() + "','" + dateTimePicker1.Text.ToString() + "'," +
                "'" + textBox10.Text + "','" + textBox11.Text + "','" + textBox12.Text + "','" + textBox13.Text + "','" + faiz.ToString() + "','" + gun.ToString() + "')", Form1.con);
            Form1.cmd.ExecuteNonQuery();
            Form1.con.Close();

        }

        void KayıtSil(int numara) //delete
        {
            Form1.cmd = new SqlCommand("Delete From users1 Where apart_no=@numara", Form1.con);
            Form1.cmd.Parameters.AddWithValue("@numara", numara);
            Form1.con.Open();
            Form1.cmd.ExecuteNonQuery();
            Form1.con.Close();
        }
        void kayıtsil2(int numara2) //aynı şekilde ikinic tablodadan da sil
        {
            Form1.cmd = new SqlCommand("Delete From users11 Where id=@numara2", Form1.con);
            Form1.cmd.Parameters.AddWithValue("@numara2", numara2);
            Form1.con.Open();
            Form1.cmd.ExecuteNonQuery();
            Form1.con.Close();
        }
        void temizle() //temizlik 
        {
            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = textBox6.Text = textBox7.Text = textBox8.Text = textBox9.Text = textBox10.Text = textBox11.Text = textBox12.Text = textBox13.Text = "";
            dateTimePicker1.Text = DateTime.Now.ToShortDateString();
        }
        void kayıt() //verileri kaydetmek
        {
            a = Convert.ToInt32(textBox10.Text); b = Convert.ToInt32(textBox11.Text); c = Convert.ToInt32(textBox12.Text); d = Convert.ToInt32(textBox13.Text);
            total = a + b + c + d;
            string today = DateTime.Now.ToShortDateString();//2021-02-05  uzun yol
            id = (dataGridView1.Rows.Count - 1) + 1;
             Form1.con.Open();
             Form1.cmd.Connection = Form1.con;
             Form1.cmd = new SqlCommand("insert into users1(id,name,surname,tc,tel,apart_no,ev_drm,user_ad,password,total_payout,ıslem_tarıh,son_tarıh,status,hot_water,cold_water,park,cleaning) values('" + id.ToString() + "','" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','"
                 + textBox5.Text + "','" + textBox6.Text + "','" + textBox7.Text + "','" + textBox8.Text + "','" + total.ToString() + "','" + today + "','" + dateTimePicker1.Text.ToString() + "','" + textBox9.Text + "','" + a.ToString() + "','" + b.ToString() + "','" + c.ToString() + "','" + d.ToString() + "')", Form1.con);
             Form1.cmd.ExecuteNonQuery();
             MessageBox.Show("Ekleme işlemi Başarılı");
             Form1.con.Close();
            kayıt2();
            temizle();

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("ev_drm");
            comboBox1.Items.Add("total_payout");
            comboBox2.Items.Add("Host");
            comboBox2.Items.Add("Tenant");
            groupBox6.Visible = false;
            groupBox8.Visible = false;
            groupBox7.Visible = false;
            groupBox3.Visible = false;//groubbox_bir deki verileri görebilmek için kapalı tutuyoruz.
            listele();

        }
        public int varmı(string aranan) //aynı daireden oturan kişilerin farklı borçları olamayacağı için bir daireden bir kişi sorumlu
        {
            int sonuc;
            Form1.cmd = new SqlCommand("Select COUNT(apart_no) from users1 WHERE apart_no='" + aranan + "'", Form1.con);
            Form1.con.Open();
            sonuc = Convert.ToInt32(Form1.cmd.ExecuteScalar());
            Form1.con.Close();
            return sonuc;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //kayıt yapılacak 
            groupBox1.Enabled = false;
            groupBox3.Visible = true;
            button7.Enabled = false;
            listele();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //güncelleme yapılacak
            //apartman numarasına ve idisine göre güncelleme yapılacaktır
            groupBox6.Visible = true;
            dt.Clear();
            Form1.con.Open();
            Form1.cmd = new SqlCommand("Select * From users1", Form1.con);
            da = new SqlDataAdapter(Form1.cmd);
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            Form1.con.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //sil
            foreach (DataGridViewRow drow in dataGridView1.SelectedRows)
            {
                int numara = Convert.ToInt32(drow.Cells[5].Value);
                int numara2 = Convert.ToInt32(drow.Cells[0].Value);
                KayıtSil(numara);
                kayıtsil2(numara2);   
                MessageBox.Show("Silme İşlemi Başarılı");
            }
            listele();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox8.Visible = true;
            groupBox9.Visible = false;
            groupBox1.Enabled = false;
            button7.Enabled = false;
            dt.Clear();
            //Form1.con.Open();
            Form1.cmd = new SqlCommand("Select * From users1", Form1.con);
            da = new SqlDataAdapter(Form1.cmd);
            da.Fill(dt);
            dataGridView4.DataSource = dt;
            Form1.con.Close();
            label25.Text = "Result: " + (dataGridView4.Rows.Count - 1);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //yazdır 
            ppDiyalog.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            groupBox7.Visible = true;
            groupBox1.Enabled = false;
            button7.Enabled = false;
            dt.Clear();
            //Form1.con.Open();
            Form1.cmd = new SqlCommand("Select * From users1", Form1.con);
            da = new SqlDataAdapter(Form1.cmd);
            da.Fill(dt);
            dataGridView3.DataSource = dt;
            Form1.con.Close();
            //faiz güncellenmesi
            for (i = 0; i < dataGridView3.Rows.Count - 1; i++)
            {
                DateTime bit = Convert.ToDateTime(dataGridView3.Rows[i].Cells["son_tarıh"].Value.ToString());
                DateTime bas2 = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                TimeSpan result = bit - bas2;
                fark = result.TotalDays;
                if (fark < 0)
                {
                    id3 = Convert.ToInt32(dataGridView3.Rows[i].Cells["id"].Value.ToString());
                    a1 = Convert.ToInt32(dataGridView3.Rows[i].Cells["hot_water"].Value.ToString());
                    b1 = Convert.ToInt32(dataGridView3.Rows[i].Cells["cold_water"].Value.ToString());
                    c1 = Convert.ToInt32(dataGridView3.Rows[i].Cells["park"].Value.ToString());
                    d1 = Convert.ToInt32(dataGridView3.Rows[i].Cells["cleaning"].Value.ToString());
                }
                faiz(a1, b1, c1, d1,id3);

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //çıkış From1 dönüş
            Form1 bay = new Form1();
            bay.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //tabloya kaydet
            //eğer girilen apartman numarası daha önceden eklenmişse kayıt yapılmamalı
            if (varmı(textBox5.Text) != 0)
            {
                MessageBox.Show("Someone else lives in this apartment.");
                textBox5.Text = "";
            }
            else
            {
                kayıt();
                listele();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //form2 geri dön
            groupBox3.Visible = false;
            groupBox1.Enabled = true;
            button7.Enabled = true;
            listele();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //update
            groupBox6.Visible = true;
            groupBox1.Enabled = false;
            button7.Enabled = false;
            update_list();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //Form2 dön
            groupBox7.Visible = false;
            groupBox1.Enabled = true;
            button7.Enabled = true;
            listele();
        }
        double fark, faizlen;
        int i=0;
        int a1, b1, c1, d1, id3;
        Font veri = new Font("Calibri", 12, FontStyle.Italic);//veri yazıları
        SolidBrush sb = new SolidBrush(Color.Black);

        private void button13_Click(object sender, EventArgs e)
        {
            groupBox8.Visible = false;
            groupBox1.Enabled = true;
            button7.Enabled = true;
            listele();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //seçilene göre işlem
            //1. ev durumuna göre ev sahipleri veya kiracılar
            //2.ödemesi olanları listele
            if(comboBox1.Text=="ev_drm")
            {
                groupBox9.Visible = true;
                evdurumuna(comboBox2.Text);
            }
            else if(comboBox1.Text=="total_payout")
            {
                dt.Clear();
                Form1.con.Open();
                Form1.cmd = new SqlCommand("Select * From users1 where total_payout>'" + 0 + "'", Form1.con);
                da = new SqlDataAdapter(Form1.cmd);
                da.Fill(dt);
                dataGridView4.DataSource = dt;
                Form1.con.Close();
            }
            label25.Text = "Result: " + (dataGridView4.Rows.Count - 1);

        }
        void evdurumuna(string srg)
        {
            dt.Clear();
            Form1.con.Open();
            Form1.cmd = new SqlCommand("Select * From users1 where ev_drm='"+srg+"'", Form1.con);
            da = new SqlDataAdapter(Form1.cmd);
            da.Fill(dt);
            dataGridView4.DataSource = dt;
            Form1.con.Close();
        }

        private void groupBox9_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            evdurumuna(comboBox2.Text);
            label25.Text = "Result: " + (dataGridView4.Rows.Count - 1);
        }

        private void pdYazici_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            StringFormat sFormat = new StringFormat();
            sFormat.Alignment = StringAlignment.Near;
            Font başlık = new Font("Times New Roman", 14, FontStyle.Underline);
            e.Graphics.DrawString(" Apartment Residents ", Baslik, sb, 200, 100);
            ///////////////////////////////////////////////////////////
            e.Graphics.DrawString("NAME", başlık, Brushes.Red, 70, 150);
            e.Graphics.DrawString("SURNAME", başlık, Brushes.Red, 250, 150);
            e.Graphics.DrawString("TOTAL", başlık, Brushes.Red, 450, 150);
            e.Graphics.DrawString("SON_TARIH", başlık, Brushes.Red, 550, 150);
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {

                e.Graphics.DrawString(dataGridView1.Rows[i].Cells["name"].Value.ToString(), veri, sb, 70, 190 + (i * 30));
                e.Graphics.DrawString(dataGridView1.Rows[i].Cells["surname"].Value.ToString(), veri, sb, 250, 190 + (i * 30));
                e.Graphics.DrawString(dataGridView1.Rows[i].Cells["total_payout"].Value.ToString(), veri, sb, 450, 190 + (i * 30));
                e.Graphics.DrawString(dataGridView1.Rows[i].Cells["son_tarıh"].Value.ToString(), veri, sb, 550, 190 + (i * 30));
            }
        }

        Font Baslik = new Font("Verdana", 12, FontStyle.Bold);

        private void dataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //ödeme gününü gçeiktirenler kırmız renk oluyor
            for (i=0;i<dataGridView3.Rows.Count-1;i++)
            {
                DateTime bit = Convert.ToDateTime(dataGridView3.Rows[i].Cells["son_tarıh"].Value.ToString());
                DateTime bas2 = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                TimeSpan result = bit - bas2;
                 fark = result.TotalDays;
                if (fark<0)
                {
                    dataGridView3.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
                else
                {
                    dataGridView3.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }

            }
        }
        void faiz(int a1,int b1,int c1,int d1,int id3)
        {
            faizlen = ((a1 + b1 + c1 + d1) * 15 / 100) * fark + (a1 + b1 + c1 + d1); //faiz tutarım yani yeni ödenmesi gereken toplam tutar
            Form1.cmd = new SqlCommand("Update users1 set total_payout=@top2 where id=@idd", Form1.con);
            Form1.cmd.Parameters.AddWithValue("@top2", faizlen.ToString());
            Form1.cmd.Parameters.AddWithValue("@idd", id3);
            Form1.con.Open();
            Form1.cmd.ExecuteNonQuery();
            Form1.con.Close();
            Form1.cmd = new SqlCommand("Update users11 set total_payout=@top2 where id=@idd", Form1.con);
            Form1.cmd.Parameters.AddWithValue("@top2", faizlen.ToString());
            Form1.cmd.Parameters.AddWithValue("@idd", id3);
            Form1.con.Open();
            Form1.cmd.ExecuteNonQuery();
            Form1.con.Close();
            listele();
        }
        int satır;
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            satır = dataGridView2.CurrentRow.Index;
            MessageBox.Show(satır.ToString());
            textBox14.Text = dataGridView2.Rows[satır].Cells["status"].Value.ToString();
            textBox15.Text = dataGridView2.Rows[satır].Cells["hot_water"].Value.ToString();
            textBox16.Text = dataGridView2.Rows[satır].Cells["cold_water"].Value.ToString();
            textBox17.Text = dataGridView2.Rows[satır].Cells["park"].Value.ToString();
            textBox18.Text = dataGridView2.Rows[satır].Cells["cleaning"].Value.ToString();
            satır = satır + 1;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //form2 dön
            groupBox1.Enabled = true;
            button7.Enabled = true;
            groupBox6.Visible = false;
        }
        void guncel2()
        {
            int top2 = Convert.ToInt32(textBox15.Text) + Convert.ToInt32(textBox16.Text) + Convert.ToInt32(textBox17.Text) + Convert.ToInt32(textBox18.Text);
            Form1.cmd = new SqlCommand("Update users11 set total_payout=@top2,son_tarıh=@tarıh,hot=@hot,cold=@cold,park=@park,cleaning=@clean where id=@idd", Form1.con);
            Form1.cmd.Parameters.AddWithValue("@hot", textBox15.Text);
            Form1.cmd.Parameters.AddWithValue("@cold", textBox16.Text);
            Form1.cmd.Parameters.AddWithValue("@park", textBox17.Text);
            Form1.cmd.Parameters.AddWithValue("@clean", textBox18.Text);
            Form1.cmd.Parameters.AddWithValue("@tarıh", dateTimePicker2.Value.ToString());
            Form1.cmd.Parameters.AddWithValue("@top2", top2.ToString());
            Form1.cmd.Parameters.AddWithValue("@idd", satır);
            Form1.con.Open();
            Form1.cmd.ExecuteNonQuery();
            Form1.con.Close();
        }
        private void button10_Click(object sender, EventArgs e)
        {
            //güncelle bilgileri
            //toplam tutarda değişecek 
            int top = Convert.ToInt32(textBox15.Text) + Convert.ToInt32(textBox16.Text) + Convert.ToInt32(textBox17.Text) + Convert.ToInt32(textBox18.Text);
            //Form1.con.Open();
            Form1.cmd = new SqlCommand("Update users1 set status=@statu,hot_water=@hot,cold_water=@cold,park=@park,cleaning=@clean,son_tarıh=@tarıh,total_payout=@top where id=@idd",Form1.con);
            Form1.cmd.Parameters.AddWithValue("@statu", textBox14.Text);
            Form1.cmd.Parameters.AddWithValue("@hot", textBox15.Text);
            Form1.cmd.Parameters.AddWithValue("@cold", textBox16.Text);
            Form1.cmd.Parameters.AddWithValue("@park", textBox17.Text);
            Form1.cmd.Parameters.AddWithValue("@clean", textBox18.Text);
            Form1.cmd.Parameters.AddWithValue("@tarıh", dateTimePicker2.Text.ToString());
            Form1.cmd.Parameters.AddWithValue("@top",top.ToString());
            Form1.cmd.Parameters.AddWithValue("@idd", satır);
            Form1.cmd.ExecuteNonQuery();
            Form1.con.Close();
            guncel2();
            MessageBox.Show("Güncelleme Başarılı");
            textBox14.Text = textBox15.Text = textBox16.Text = textBox17.Text = textBox18.Text = "";
            dateTimePicker2.Text = DateTime.Now.ToShortDateString();
            update_list();
        }
    }
}
