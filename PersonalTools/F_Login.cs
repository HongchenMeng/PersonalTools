using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SQLite;
using System.Security.Cryptography;


namespace PersonalTools
{
    public partial class F_Login : Form
    {
        DataClass.MyMeans MyClass = new PersonalTools.DataClass.MyMeans();
        Encrypt.Encrypt encrypt = new Encrypt.Encrypt();
        public F_Login()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if ((int)(this.Tag) == 1)
            {
                F_Main.Login_n = 3;
                Application.Exit();
            }
            else
               if ((int)(this.Tag) == 2)
                this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("用户名不能为空！");
            }
            else if (txtPassWord.Text.Trim() == "")
            {
                MessageBox.Show("密码不能为空！！！");
            }
            else
            {
              string ConnectionString = DataClass.MyMeans.DataSource;
                string pw = DataClass.MyMeans.PassWord;
                using (SQLiteConnection Conn = new SQLiteConnection(ConnectionString))
                {
                    Conn.SetPassword(pw);
                    Conn.Open();//打开数据库
                    using (SQLiteCommand Cmd = Conn.CreateCommand())
                    {
                        Cmd.CommandText = "select * from t_Login where Name =@Name and PassWord=@PassWord";
                        string NameString = encrypt.EncryptDES(txtName.Text.Trim());
                        string PWString = encrypt.EncryptMD5(txtPassWord.Text.Trim());
                        Cmd.Parameters.Add(new SQLiteParameter("Name", NameString));
                        Cmd.Parameters.Add(new SQLiteParameter("PassWord", PWString));
                        if (Convert.ToInt32(Cmd.ExecuteScalar()) > 0)//用户名、密码正确
                        {
                            F_Main.Login_Name = txtName.Text.Trim();
                            string sqlLimit = "select GropPope from t_Login where Name ='"+NameString+"' and PassWord ='"+ PWString+"'";
                            F_Main.Login_Limit = Convert.ToInt32(MyClass.ReturnFirstRowCols(sqlLimit));//取得权限值
                            //F_Main.Login_ID = temDR.GetString(0);
                            F_Main.Login_n = (int)(this.Tag);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("用户名或密码错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtName.Text = "";
                            txtPassWord.Text = "";
                        }
                    }
                }
            }
}
        private void F_Login_Load(object sender, EventArgs e)
        {
            try
            {
                //SQLiteConnection connection = new SQLiteConnection(DataClass.MyMeans.DataSource);
                //connection.Open();
                //connection.Close();
                MyClass.getsqlcom("select count(*)  from sqlite_master where type='table' and name = 'yourtablename'");
                txtName.Text = "admin";
                txtPassWord.Text = "admin";
            }
            catch
            {
                MessageBox.Show("数据库连接失败。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }
        #region 焦点事件
        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
                txtPassWord.Focus();
        }

        private void txtPassWord_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
                btnLogin.Focus();
        }

        private void F_Login_Activated(object sender, EventArgs e)
        {
            txtName.Focus();
        }
        #endregion
    }
}
