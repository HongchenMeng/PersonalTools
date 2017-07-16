using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace PersonalTools
{
    public partial class F_UserSet : Form
    {
        ModuleClass.MyModule MyMC = new PersonalTools.ModuleClass.MyModule();
        DataClass.MyMeans MyDataClass = new PersonalTools.DataClass.MyMeans();
        Encrypt.Encrypt encrypt = new Encrypt.Encrypt();
        static int isAdd = 0;//1是添加 2是修改 3是删除
        //string SQL = "";
        //static DataSet MyDS_Grid;
        private void btn_Save_Click(object sender, EventArgs e)
        {
            if(cob_Name.Text=="")
            {
                MessageBox.Show("用户名不能为空");
            }
            string nametxt = encrypt.EncryptDES(cob_Name.Text.Trim());
            if (isAdd == 3)//删除
            {
                int limit = Int32.Parse(MyDataClass.ReturnFirstRowCols("select GropPope from t_Login where Name='" + nametxt + "'"));
                if (F_Main.Login_Limit >= limit)
                {
                    MessageBox.Show("不能删除权限高于当前用户的用户");
                    return;
                }
                string sqlsc = "DELETE FROM t_Login WHERE  Name='" + nametxt + "'";
                MyDataClass.getsqlcom(sqlsc);
                MessageBox.Show("删除成功");
                return;
            }

            if(mtxt_PassWord1.Text=="")
            {
                return;
            }
            if(mtxt_PassWord2.Text=="")
            {
                return;
            }

            if(mtxt_PassWord1.Text!=mtxt_PassWord2.Text)
            {
                MessageBox.Show("两次输入的密码不相同，请重新输入");
                return;
            }
            
            string passwordtxt = encrypt.EncryptMD5(mtxt_PassWord2.Text.Trim());
            if(isAdd==1)//添加
            {
                if (mtxt_GropPope.Text == "")
                {
                    MessageBox.Show("添加改的权限不能为空");
                    return;
                }
                if (Int32.Parse(mtxt_GropPope.Text) < F_Main.Login_Limit)
                {
                    MessageBox.Show("添加改的权限必须小于当前用户权限");
                    return;
                }
                string sqljc = "SELECT count(*) FROM t_Login where Name='"+nametxt+"'";
                string s = MyDataClass.ReturnFirstRowCols(sqljc);
                if(s=="0")//不存在这个账号
                {
                    string ID = MyMC.GetAutocoding("t_Login", "ID");//自动编号
                    sqljc = "Insert into t_Login(ID,Name,PassWord,GropPope)values('" + ID +"','"+ nametxt +"','"+ passwordtxt +"','" + mtxt_GropPope.Text + "')";
                    MyDataClass.getsqlcom(sqljc);
                    MessageBox.Show("添加成功！");
                    return;
                }
                else
                {
                    MessageBox.Show("该用户已存在，请重新输入用户名");
                    return;
                }
            }
           if (isAdd==2)//修改
            {
                string sqlxg;
                if (F_Main.Login_Name == cob_Name.Text.Trim()|| mtxt_GropPope.Text.Trim() == "")
                {
                    sqlxg = "update t_Login set PassWord='" + passwordtxt + "' where Name='" + nametxt + "'";
                }
                else
                {
                    int limit = Int32.Parse(MyDataClass.ReturnFirstRowCols("select GropPope from t_Login where Name='" + nametxt + "'"));
                    if (F_Main.Login_Limit >= limit)
                    {
                        MessageBox.Show("修改的权限不能高于当前用户权限");
                        return;
                    }
                    if (Int32.Parse(mtxt_GropPope.Text.Trim()) < F_Main.Login_Limit)
                    {
                        MessageBox.Show("添加改的权限必须小于当前用户权限");
                        return;
                    }
                    sqlxg = "update t_Login set PassWord='" + passwordtxt + "',GropPope='" + mtxt_GropPope.Text.Trim() + "' where Name='" + nametxt + "'";
                }
                    MyDataClass.getsqlcom(sqlxg);
                    MessageBox.Show("修改成功！");
                    return;
            }

        }
        public F_UserSet()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cobAdd();
            MyMC.EnabledButton(btn_Add, true, btn_Edit, true, btn_Hide, true, btn_Save, false, btn_Cancel, false);
            //string theName = encrypt.DecryptDES(F_Main.Login_Name);
            // SQL = "select Name,GropPope from t_Login where Name='"+theName+"'";
            //MyDS_Grid = MyDataClass.getDataSet(SQL, "t_Login");
        }
        /// <summary>
        /// 加载下拉菜单
        /// </summary>
        private void cobAdd()
        {
            cob_Name.Items.Clear();
            cob_Name.Items.Add(F_Main.Login_Name);
            string s = "select Name from t_Login Where GropPope >'"+F_Main.Login_Limit+"'";
            DataClass.MyMeans MyDataClsaa = new PersonalTools.DataClass.MyMeans();
            SQLiteDataReader MyDR = MyDataClsaa.ExecuteReader(s);
            if (MyDR.HasRows)
            {
                while (MyDR.Read())
                {
                    if (MyDR[0].ToString() != "" && MyDR[0].ToString() != null)
                        cob_Name.Items.Add(encrypt.DecryptDES( MyDR[0].ToString()));
                }
            }
            MyDR.Close();
            cob_Name.Text = F_Main.Login_Name;
        }
        private void ClealText()
        {
            cob_Name.Text = F_Main.Login_Name;
            mtxt_PassWord1.Text = "";
            mtxt_PassWord2.Text = "";
            mtxt_GropPope.Text = "";
        }
        private void btn_Edit_Click(object sender, EventArgs e)
        {
            isAdd = 2;
            MyMC.EnabledButton(btn_Add, false, btn_Edit, false, btn_Hide, false, btn_Save, true, btn_Cancel, true);
        }

        private void btn_Hide_Click(object sender, EventArgs e)
        {
            isAdd = 3;
            MyMC.EnabledButton(btn_Add, false, btn_Edit, false, btn_Hide, false, btn_Save, true, btn_Cancel, true);
        }


        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            isAdd = 0;
            MyMC.EnabledButton(btn_Add, true, btn_Edit, true, btn_Hide, true, btn_Save, false, btn_Cancel, false);
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            isAdd = 1;
            ClealText();//初始化
            MyMC.EnabledButton(btn_Add, false, btn_Edit, false, btn_Hide, false, btn_Save, true, btn_Cancel, true);
        }


    }
}
