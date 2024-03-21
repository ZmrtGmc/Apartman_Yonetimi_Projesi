using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; //sql bağlantısı için

namespace apart_yönetici
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static SqlConnection con = new SqlConnection("Data Source=DESKTOP-8O3I53L;Initial Catalog=ZG_Apart;Integrated Security=True"); //tablomuzun yolu
        public static SqlCommand cmd;
        public static SqlDataReader dr;
        public static string who;
        public static string name;

        private void button1_Click(object sender, EventArgs e)
        {
            //yönetici ve kiracı ev sahibi olmak üzere üç şekilde giriş yapılabilir
            //1. yönetici giriş sadedce zmrt 1
            con.Close();
            cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText="Select * From users1 where user_ad='"+textBox1.Text+"' AND password='"+textBox2.Text+"'";
            dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                who = "a";
            } else
            {
                con.Close();
                MessageBox.Show("Hatalı Giriş");
                textBox1.Text = textBox2.Text = "";
            }
            if(who=="a")
            {
                if(textBox1.Text=="zmrt"&&textBox2.Text=="1")
                {
                    //yönetici
                    con.Close();
                    Form2 form2sec = new Form2();
                    form2sec.Show();
                    this.Hide();
                }
                else
                {
                    //apartman sakini
                    con.Close();
                    name = textBox1.Text;
                    Form3 frm3 = new Form3();
                    frm3.Show();
                    this.Hide();
                }
            }
        }
    }
}
