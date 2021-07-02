using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Otomasyon.Connection;

namespace Otomasyon
{
    public partial class formRegister : Form
    {
        public formRegister()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            clearControls();
            usernameTextBox.Select();
        }

        private void clearControls()
        {
            foreach(TextBox tb in this.Controls.OfType<TextBox>())
            {
                tb.Text = string.Empty;
            }
        }

        private void formRegister_Load(object sender, EventArgs e)
        {
            loadUserData();
            usernameTextBox.Select();
        }

        private void loadUserData()
        {
            DataTable userData = ServerConnection.executeSQL("SELECT username, password, mission, role  from LoginTbl");
            dataGridView1.DataSource = userData;

            dataGridView1.Columns[0].HeaderText = "Kullanıcı Adı";
            dataGridView1.Columns[0].Width = 100;

            dataGridView1.Columns[1].HeaderText = "Şifre";
            dataGridView1.Columns[1].Width = 100;

            dataGridView1.Columns[2].HeaderText = "Görev";
            dataGridView1.Columns[2].Width = 100;

            dataGridView1.Columns[3].HeaderText = "Yetki";
            dataGridView1.Columns[3].Width = 100;


        }

        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Silmek İstediğine Emin misin?", "Kayıt Sil",
                  MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                  MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                {
                    ServerConnection.executeSQL("DELETE FROM LoginTbl WHERE username = '" + dataGridView1.CurrentRow.Cells[0].Value + "'");
                    loadUserData();

                    MessageBox.Show("Kayıt Silindi", "Silme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            MessageBoxButtons btn = MessageBoxButtons.OK;
            MessageBoxIcon ico = MessageBoxIcon.Information;
            string caption = "Veri Kaydı";

            if (string.IsNullOrEmpty(usernameTextBox.Text))
            {
                MessageBox.Show("Kullanıcı Adı Alanı Boş Geçilemez", caption, btn, ico);
                usernameTextBox.Select();
                return;
            }

            if (string.IsNullOrEmpty(passwordTextBox.Text))
            {
                MessageBox.Show("Şifre Alanı Boş Geçilemez", caption, btn, ico);
                passwordTextBox.Select();
                return;
            }
            if (string.IsNullOrEmpty(confirmPasswordTextBox.Text))
            {
                MessageBox.Show("Şifreni Tekrar Gir", caption, btn, ico);
                confirmPasswordTextBox.Select();
                return;
            }
            if (string.IsNullOrEmpty(gorevTextBox.Text))
            {
                MessageBox.Show("Görev Alanı Boş Geçilemez", caption, btn, ico);
                gorevTextBox.Select();
                return;
            }
            if (string.IsNullOrEmpty(yetkiTextBox.Text))
            {
                MessageBox.Show("Yetki Alanı Boş Geçilemez", caption, btn, ico);
                yetkiTextBox.Select();
                return;
            }

            if (passwordTextBox.Text != confirmPasswordTextBox.Text)
            {
                MessageBox.Show("Şifreler Uyumsuz; Lütfen Tekrar Dene", caption, btn, ico);
                confirmPasswordTextBox.Select();
                return;
            }
            string yourSQL = "SELECT username FROM LoginTbl WHERE username = '"+usernameTextBox.Text+"'";
            DataTable checkDuplicates = Otomasyon.Connection.ServerConnection.executeSQL(yourSQL);

            if(checkDuplicates.Rows.Count > 0)
            {
                MessageBox.Show("Bu Kullanıcı Adı Zaten Alınmış. Lütfen Başka Bir Kullanıcı Adı Oluştur", 
                    "Tekrar Eden Kullanıcı Adı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                usernameTextBox.SelectAll();
                return;
            }
            DialogResult result;
            result = MessageBox.Show("Veriyi Kaydetmek İstiyor musun?", "Veri Kaydı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                string mySQL = string.Empty;

                mySQL += "INSERT INTO LoginTbl(username, password, mission, role)";
                mySQL += "VALUES ('"+usernameTextBox.Text+"','"+passwordTextBox.Text+"', '"+gorevTextBox.Text+"', '"+int.Parse(yetkiTextBox.Text)+"' )";

                Otomasyon.Connection.ServerConnection.executeSQL(mySQL);

                MessageBox.Show("Veri Kaydı Başarılı Bir Şekilde Tamamlandı",
                                "Veri Kaydı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                loadUserData();
                clearControls();
            }
        }
    }
}
