using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace Otomasyon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=FURKAN\SQLEXPRESS;Initial Catalog=Otomasyon;Integrated Security=True");

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            con.Open();
            String kayit = "insert into satislar(kullanici, urunAdi, urunFiyati, odemeYontemi, islemTarihi) values (@kullanici, @urunAdi, @urunFiyati, @odemeYontemi, @islemTarihi)";
            SqlCommand komut = new SqlCommand(kayit, con);
            
            komut.Parameters.AddWithValue("@kullanici", textBox2.Text);
            komut.Parameters.AddWithValue("@urunAdi", textBox6.Text);
            komut.Parameters.AddWithValue("@urunFiyati", textBox3.Text);
            komut.Parameters.AddWithValue("@odemeYontemi", textBox4.Text);
            komut.Parameters.AddWithValue("@islemTarihi", dateTimePicker1.Value.Date);

            komut.ExecuteNonQuery();
            MessageBox.Show("veri ekleme basarili");
            con.Close();
            BindData();
        }
        void BindData()
        {
            SqlCommand command = new SqlCommand("select * from satislar ", con);
            SqlDataAdapter sd = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            dataGridView1.DataSource = dt;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            con.Open();
            String kayit = "update satislar set kullanici = @kullanici, urunAdi = @urunAdi, urunFiyati = @urunFiyati, odemeYontemi = @odemeYontemi, islemTarihi = @islemTarihi where urunID = @urunID";
            SqlCommand komut = new SqlCommand(kayit, con);

            komut.Parameters.AddWithValue("urunID", int.Parse(textBox1.Text));
            komut.Parameters.AddWithValue("@kullanici", textBox2.Text);
            komut.Parameters.AddWithValue("@urunAdi", textBox6.Text);
            komut.Parameters.AddWithValue("@urunFiyati", textBox3.Text);
            komut.Parameters.AddWithValue("@odemeYontemi", textBox4.Text);
            komut.Parameters.AddWithValue("@islemTarihi", dateTimePicker1.Value.Date);
            komut.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("guncelleme islemi basarili");
            BindData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                MessageBox.Show("Geçerli bi ürün ID gir.");
            }
            else
            {
                con.Open();
                String kayit = "delete satislar where urunID = @urunID";
                SqlCommand komut = new SqlCommand(kayit, con);

                komut.Parameters.AddWithValue("urunID", int.Parse(textBox1.Text));
                komut.Parameters.AddWithValue("@kullanici", textBox2.Text);
                komut.Parameters.AddWithValue("@urunAdi", textBox6.Text);
                komut.Parameters.AddWithValue("@urunFiyati", textBox3.Text);
                komut.Parameters.AddWithValue("@odemeYontemi", textBox4.Text);
                komut.Parameters.AddWithValue("@islemTarihi", dateTimePicker1.Value.Date);
                komut.ExecuteNonQuery();
                BindData();
                MessageBox.Show("silme işlemi başarılı");
                con.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString))
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();
                    using(DataTable dt = new DataTable("satislar"))
                    {
                        using(SqlCommand cmd = new SqlCommand("select * from satislar where kullanici like @kullanici or urunAdi like @urunAdi or odemeYontemi like @odemeYontemi", cn))
                        {
                            
                            cmd.Parameters.AddWithValue("kullanici", string.Format("%{0}%", textBox5.Text));
                            cmd.Parameters.AddWithValue("urunAdi", string.Format("%{0}%", textBox5.Text));
                            cmd.Parameters.AddWithValue("odemeYontemi", string.Format("%{0}%", textBox5.Text));
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            adapter.Fill(dt);
                            dataGridView1.DataSource = dt;
                            label7.Text = $"Toplam Kayıt: {dataGridView1.RowCount - 1}";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13) //enter tusu
            {
                buttonSearch.PerformClick();
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.ReadOnly = true;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
