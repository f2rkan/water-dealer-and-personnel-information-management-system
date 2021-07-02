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
using System.Data.SqlClient;

namespace Otomasyon
{
    public partial class formLogin : Form
    {
        public formLogin()
        {
            InitializeComponent();
        }

        private void formLogin_Load(object sender, EventArgs e)
        {
            usernameTextBox.Select();

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openRegisterFromLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            formRegister register = new formRegister();
            register.ShowDialog();
        }

        private void showPasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(showPasswordCheckBox.Checked == true)
            {
                passwordTextBox.UseSystemPasswordChar = false;
            }
            else
            {
                passwordTextBox.UseSystemPasswordChar = true;
            }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(usernameTextBox.Text) &&
               !string.IsNullOrEmpty(passwordTextBox.Text))
            {
                string mySQL = string.Empty;

                mySQL += "SELECT * FROM LoginTbl ";
                mySQL += "WHERE username = '" + usernameTextBox.Text + "'";
                mySQL += "AND password = '" + passwordTextBox.Text + "'";


                DataTable userData = ServerConnection.executeSQL(mySQL);

                if(userData.Rows.Count > 0)
                {
                    usernameTextBox.Clear();
                    passwordTextBox.Clear();
                    showPasswordCheckBox.Checked = false;

                    this.Hide();
                    Form1 f1 = new Form1();
                    f1.ShowDialog();
                    f1 = null;

                    this.Show();
                    this.usernameTextBox.Select();
                }
                else
                {
                    MessageBox.Show("Kullanıcı Adı ya da Şifre Hatalı",
                        "Tekrar Dene", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    usernameTextBox.Focus();
                    usernameTextBox.SelectAll();
                }
                
            }
            else
            {
                MessageBox.Show("Lütfen Kullanıcı ve Şifre Girişi Yap", "Geçerli Bir Giriş Yap",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation
                    );
                usernameTextBox.Select();
            }
        }

        private void usernameTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
