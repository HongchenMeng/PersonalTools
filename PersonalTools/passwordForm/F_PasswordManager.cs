using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PersonalTools.passwordForm
{
    public partial class F_PasswordManager : Form
    {

        public F_PasswordManager()
        {
            InitializeComponent();
        }
        ModuleClass.MyModule MyMC = new PersonalTools.ModuleClass.MyModule();
        DataClass.MyMeans MyDataClass = new PersonalTools.DataClass.MyMeans();
        Encrypt.Encrypt encrypt = new Encrypt.Encrypt();
        static DataSet MyDS_Grid;
        /// <summary>
        /// 0为普通查询，1为高级查询
        /// </summary>
      public static  string AdvancedCheck = "0";
        static string Sut_SQL = "";

        static string temID = "";
        string temsql = "select Category as 大类,Type as 小类,UserName as 用户名,ID as 编号," +
            "WebUrl as 网址,PassWord as 用户密码,PasswordSecurity as 安全密码,Email as 密保邮箱," +
            "Questions1 as 密保问题1,Answer1 as 密保答案1,Questions2 as 密保问题2,Answer2 as 密保答案2," +
            "Questions3 as 密保问题3,Answer3 as 密保答案3,Note as 备注,Phone as 手机,AdvancedCheck as 是否高级查询 " +
            "from t_PasswordManager";
        string ARsign = " AND ";
        /// <summary>
        /// 0为初始状态，1为添加，2为修改
        /// </summary>
        static int  isAdd = 0;
        private void getsql()
        {
            string sql1 =temsql+ " where Hide='0'and AdvancedCheck<='" + AdvancedCheck + "'";// and CreatedTime>datetime('2016-09-01') 
            if (F_Main.Login_Name == "admin" || F_Main.Login_Name == "administrator")
            {
                Sut_SQL = sql1 + " ORDER BY Category,Type";
            }
            else
            {
                //其他用户只能查询本人创建的
                Sut_SQL = sql1 + "and WhoCreated='" + F_Main.Login_Name + "' ORDER BY Category,Type";
            }

        }
        private void F_PasswordManager_Load(object sender, EventArgs e)
        {
            getsql();//生成sql语句
            //根据SQL语句进行查询
            MyDS_Grid = MyDataClass.getDataSet(Sut_SQL, "t_PasswordManager");
            dataGridView1.DataSource = MyDS_Grid.Tables[0];
            dataGridView1.AllowUserToAddRows = false;//dataGridView会默认在最后添加一个空行，这句话会让空行消失。
            dataGridView1.AutoGenerateColumns = true; //是否自动创建列
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;//禁用排序功能
                if (i >= 3)
                {
                    dataGridView1.Columns[i].Visible = false;//隐藏dataGridView1控件中不需要的列字段
                }
            }
            Grid_Inof(this.dataGridView1);

            MyMC.DataGridViewButton(dataGridView1, btn_N_First, btn_N_Previous, btn_N_Next, btn_N_End);//设置按钮状态
            MyMC.DataGridViewButton(dataGridView1, txt_N_cursor);//设置游标

            MyMC.comboBoxAddData(cobx_Email, "select distinct Email from t_PasswordManager  where Hide = '0'and AdvancedCheck <= '" + AdvancedCheck + "' ORDER BY Category",true);
            MyMC.comboBoxAddData(cobx_Questions1, "select Questions from  t_Questions");
            MyMC.comboBoxAddData(cobx_Questions2, "select Questions from t_Questions");
            MyMC.comboBoxAddData(cobx_Questions3, "select Questions from t_Questions");


            MyMC.comboBoxAddData(find_WhoChange, "select Name from t_Login where GropPope>'" + F_Main.Login_Limit + "'  ORDER BY Name", true);
            find_WhoChange.Items.Add(F_Main.Login_Name);
            //加载大类下拉信息,小类根据大类来变
            MyMC.comboBoxAddData(find_Category, "select distinct Category from t_PasswordManager  where Hide='0'and AdvancedCheck<='" + AdvancedCheck + "' ORDER BY Category");
            MyMC.comboBoxAddData(cobx_Category, "select distinct Category from t_PasswordManager  where Hide='0'and AdvancedCheck<='" + AdvancedCheck + "' ORDER BY Category");

            rbtnAnd.Checked = true;

            //设置数据控件不可用
            MyMC.ControlEnabled(grobShow.Controls, false);
            chb_ShowPassword.Enabled = true;
            MyMC.EnabledButton(btn_Add, true, btn_Edit, true, btn_Hide, true, btn_Save, false, btn_Cancel, false, btn_ExportToExcel, true);
        }
        
        #region  显示“密码表”表中的指定记录
        /// <summary>
        /// 动态读取指定的记录行，并进行显示.
        /// </summary>
        /// <param name="DGrid">DataGridView控件</param>
        /// <returns>返回string对象</returns> 
        public string Grid_Inof(DataGridView DGrid)
        {
            
            //当DataGridView控件的记录>1时，将当前行中信息显示在相应的控件上,dataGridView列标号由SQL语句决定
            if (DGrid.RowCount >= 1)
            {
                cobx_Category.Text= DGrid[0, DGrid.CurrentCell.RowIndex].Value.ToString();
                cobx_Type.Text= DGrid[1, DGrid.CurrentCell.RowIndex].Value.ToString();
                txt_UserName.Text= DGrid[2, DGrid.CurrentCell.RowIndex].Value.ToString();
                txt_WebUrl.Text= DGrid[4, DGrid.CurrentCell.RowIndex].Value.ToString();
                mtxt_PassWord.Text= encrypt.DecryptDES(DGrid[5, DGrid.CurrentCell.RowIndex].Value.ToString());//密码
                txt_PasswordSecurity.Text= encrypt.DecryptDES(DGrid[6, DGrid.CurrentCell.RowIndex].Value.ToString());//安全码
                cobx_Email.Text = encrypt.DecryptDES(DGrid[7, DGrid.CurrentCell.RowIndex].Value.ToString());//密保邮箱
                cobx_Questions1.Text = DGrid[8, DGrid.CurrentCell.RowIndex].Value.ToString();
                txt_Answer1.Text = encrypt.DecryptDES(DGrid[9, DGrid.CurrentCell.RowIndex].Value.ToString());//答案
                cobx_Questions2.Text = DGrid[10, DGrid.CurrentCell.RowIndex].Value.ToString();
                txt_Answer2.Text = encrypt.DecryptDES(DGrid[11, DGrid.CurrentCell.RowIndex].Value.ToString());//
                cobx_Questions3.Text = DGrid[12, DGrid.CurrentCell.RowIndex].Value.ToString();
                txt_Answer3.Text = encrypt.DecryptDES(DGrid[13, DGrid.CurrentCell.RowIndex].Value.ToString());//
                txt_Note.Text = encrypt.DecryptDES(DGrid[14, DGrid.CurrentCell.RowIndex].Value.ToString());//备注
                txt_Phone.Text = encrypt.DecryptDES(DGrid[15, DGrid.CurrentCell.RowIndex].Value.ToString());//手机


                string AdvancedCheckStr = DGrid[16, DGrid.CurrentCell.RowIndex].Value.ToString();
                if(AdvancedCheckStr=="0")
                {
                    chb_AdvancedCheck.Checked = false;
                }
                else
                {
                    chb_AdvancedCheck.Checked = true;
                }

                temID = DGrid[3, DGrid.CurrentCell.RowIndex].Value.ToString();
                return DGrid[0, DGrid.CurrentCell.RowIndex].Value.ToString();   //返回行标
            }
            else
            {
                ////使用MyMeans公共类中的Clear_Control()方法清空指定控件集中的相应控件
                //MyMC.Clear_Control(tabControl1.TabPages[0].Controls);
                //tem_ID = "";
                return "";
            }
        }
        #endregion
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            string s = Grid_Inof(this.dataGridView1);

        }
        #region 浏览数据，第一条记录/上一条记录 下一条记录/最后一条记录

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            MyMC.DataGridViewButton(dataGridView1, btn_N_First, btn_N_Previous, btn_N_Next, btn_N_End);//设置按钮状态
            MyMC.DataGridViewButton(dataGridView1, txt_N_cursor);//设置游标
        }

        #region 【第1个，前1个， 后1个，最后1个】按钮点击事件
        private void btn_N_First_Click(object sender, EventArgs e)
        {
            MyMC.DataGridViewButton(sender, e,dataGridView1, btn_N_First, btn_N_Previous, btn_N_Next, btn_N_End);//设置按钮状态
            MyMC.DataGridViewButton(dataGridView1, txt_N_cursor);//设置游标
        }

        private void btn_N_Previous_Click(object sender, EventArgs e)
        {
            MyMC.DataGridViewButton(sender, e, dataGridView1, btn_N_First, btn_N_Previous, btn_N_Next, btn_N_End);//设置按钮状态
            MyMC.DataGridViewButton(dataGridView1, txt_N_cursor);//设置游标
        }

        private void btn_N_Next_Click(object sender, EventArgs e)
        {
            MyMC.DataGridViewButton(sender, e, dataGridView1, btn_N_First, btn_N_Previous, btn_N_Next, btn_N_End);//设置按钮状态
            MyMC.DataGridViewButton(dataGridView1, txt_N_cursor);//设置游标
        }

        private void btn_N_End_Click(object sender, EventArgs e)
        {
            MyMC.DataGridViewButton(sender, e, dataGridView1, btn_N_First, btn_N_Previous, btn_N_Next, btn_N_End);//设置按钮状态
            MyMC.DataGridViewButton(dataGridView1, txt_N_cursor);//设置游标
        }
        #endregion
        #endregion
        #region 添加、修改、删除、保存、导出按钮
        private void btn_Add_Click(object sender, EventArgs e)
        {
            MyMC.ClearGropBox(grobShow.Controls);//清空控件值
            isAdd = 1;
            btn_Save.Text = "确认添加";
            btn_Save.ForeColor = Color.Green;
            MyMC.EnabledButton(btn_Add, false, btn_Edit, false, btn_Hide, false, btn_Save, true, btn_Cancel, true, btn_ExportToExcel, false);
            MyMC.ControlEnabled(grobShow.Controls, true);
            if (AdvancedCheck == "0")//在高级界面打开才可选择高级
            {
                chb_AdvancedCheck.Enabled = false;
            }
        }


        private void btn_Save_Click(object sender, EventArgs e)
        {
            string sql = "";
           string AdvancedCheck2 = "0";
            if (chb_AdvancedCheck.Checked==true)
            {
                AdvancedCheck2 = "1";
            }
            string WhoCreated = F_Main.Login_Name;
            string CreatedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string WhoChange = F_Main.Login_Name;
            string ChangeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (isAdd==1 )//添加
            {
                string[] temStr = MyMC.SqlInsert(grobShow.Controls);//历遍集合内的控件，生成添加语句的部分
                string ID = MyMC.GetAutocoding("t_PasswordManager", "ID");//自动编号

                temStr[0] = temStr[0] + ",ID,AdvancedCheck,WhoCreated,CreatedTime,WhoChange,ChangeTime,Hide";
                temStr[1] = temStr[1] + "," + "'" + ID + "'" + "," + "'" + AdvancedCheck2 + "'" + "," + "'" + WhoCreated + "'" + "," + "'" + CreatedTime+ "'" + "," + "'" + WhoChange + "'" + "," + "'" + ChangeTime + "'"+",'0'";
                sql = "Insert into t_PasswordManager(" + temStr[0] + ")values(" + temStr[1] + ")";
                MyDataClass.getsqlcom(sql);
            }
            if(isAdd==2)//修改
            {

                string s = MyMC.SqlUpdate(grobShow.Controls);
                string s1 = ",AdvancedCheck='"+ AdvancedCheck2 + "',WhoChange='" + WhoChange+ "',ChangeTime='" + ChangeTime + "',AdvancedCheck='" + AdvancedCheck2 + "'";
                sql = " update t_PasswordManager set " +s +s1+"where ID='" + temID + "'";
                MyDataClass.getsqlcom(sql);
            }
            if(isAdd==3)//删除，实际为隐藏
            {
                sql = " update t_PasswordManager set Hide='1', WhoChange='" + WhoChange + "',ChangeTime='" + ChangeTime + "where ID='" + temID + "'";
                MyDataClass.getsqlcom(sql);
            }
            btn_Cancel_Click(sender, e);//调用取消按钮
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            isAdd = 2;
            btn_Save.Text = "确认修改";
            btn_Save.ForeColor = Color.Blue;
            MyMC.EnabledButton(btn_Add, false, btn_Edit, false, btn_Hide, false, btn_Save, true, btn_Cancel, true, btn_ExportToExcel, false);
            MyMC.ControlEnabled(grobShow.Controls, true);
            if (AdvancedCheck == "0")//在高级界面打开才可选择高级
            {
                chb_AdvancedCheck.Enabled = false;
            }
        }
        private void btn_Hide_Click(object sender, EventArgs e)
        {
            isAdd = 3;//删除，实际为隐藏
            btn_Save.Text = "确认删除";
            btn_Save.ForeColor = Color.Red;
            MyMC.EnabledButton(btn_Add, false, btn_Edit, false, btn_Hide, false, btn_Save, true, btn_Cancel, true, btn_ExportToExcel, false);
            MyMC.ControlEnabled(grobShow.Controls, true);
            if (AdvancedCheck == "0")//在高级界面打开才可选择高级
            {
                chb_AdvancedCheck.Enabled = false;
            }
        }
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            isAdd = 0;
            btn_Save.Text = "保存";
            btn_Save.ForeColor = Color.Black;
            MyMC.EnabledButton(btn_Add, true, btn_Edit, true, btn_Hide, true, btn_Save, false, btn_Cancel, false, btn_ExportToExcel, true);
            MyDS_Grid = MyDataClass.getDataSet(Sut_SQL, "t_PasswordManager");
            dataGridView1.DataSource = MyDS_Grid.Tables[0];
            string s = Grid_Inof(this.dataGridView1);
            MyMC.ControlEnabled(grobShow.Controls, false);
            chb_ShowPassword.Enabled = true;
            if (AdvancedCheck == "0")//在高级界面打开才可选择高级
            {
                chb_AdvancedCheck.Enabled = false;
            }
        }

        #endregion

        private void cobx_Category_TextChanged(object sender, EventArgs e)
        {
            cobx_Type.Items.Clear();
            MyMC.comboBoxAddData(cobx_Type, "select Distinct Type from t_PasswordManager where Category='" + cobx_Category.Text.Trim() + "' ORDER BY Type");
        }

        private void find_Category_TextChanged(object sender, EventArgs e)
        {
            find_Type.Items.Clear();
            MyMC.comboBoxAddData(find_Type, "select Distinct Type from t_PasswordManager where Category='" + find_Category.Text.Trim() + "' ORDER BY Type");
        }

        private void chb_ShowPassword_CheckedChanged(object sender, EventArgs e)
        {
           if(chb_ShowPassword.Checked==true)
            {
                mtxt_PassWord.PasswordChar = new char();
            }
           else
            {
                mtxt_PassWord.PasswordChar = '*';
            }
        }

        private void rbtnOr_CheckedChanged(object sender, EventArgs e)
        {
            ARsign = " OR ";
        }

        private void rbtnAnd_CheckedChanged(object sender, EventArgs e)
        {
            ARsign = " AND ";
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            string Find_SQL = temsql;  //存储显示数据表中所有信息的SQL语句
            string FindValue = "";

            string aaa = CreatedTime_sign.Text;
            string bbb = find_CreatedTime.Text;
            FindValue = MyMC.Find_Grids(grobFind.Controls, "find", ARsign);

            if(FindValue!="")
            {
                Find_SQL = Find_SQL + " where Hide='0'and AdvancedCheck<='" + AdvancedCheck + "' AND" + FindValue;
                Find_SQL = Find_SQL + " ORDER BY Category,Type";
                MyDS_Grid = MyDataClass.getDataSet(Find_SQL, "t_PasswordManager");
            }
            else
            {
                MyDS_Grid = MyDataClass.getDataSet(Sut_SQL, "t_PasswordManager");
            }
            dataGridView1.DataSource = MyDS_Grid.Tables[0];
            //Grid_Inof(this.dataGridView1);

            MyMC.DataGridViewButton(dataGridView1, btn_N_First, btn_N_Previous, btn_N_Next, btn_N_End);//设置按钮状态
            MyMC.DataGridViewButton(dataGridView1, txt_N_cursor);//设置游标
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //find_Type.Text = "";
            //find_Category.Text = "";
            //find_WhoChange.Text = "";
            MyMC.comboBoxAddData(find_WhoChange, "select Name from t_Login where GropPope>'" + F_Main.Login_Limit + "'  ORDER BY Name", true);
            find_WhoChange.Items.Add(F_Main.Login_Name);
            //加载大类下拉信息,小类根据大类来变
            MyMC.comboBoxAddData(find_Category, "select distinct Category from t_PasswordManager  where Hide='0'and AdvancedCheck<='" + AdvancedCheck + "' ORDER BY Category");
            find_Type.Items.Clear();
            MyMC.comboBoxAddData(find_Type, "select Distinct Type from t_PasswordManager where Category='" + find_Category.Text.Trim() + "' ORDER BY Type");

            find_UserName.Text = "";
            ChangeTime_Sign.Text = "";
            CreatedTime_sign.Text = "";
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            MyDS_Grid = MyDataClass.getDataSet(Sut_SQL, "t_PasswordManager");
            dataGridView1.DataSource = MyDS_Grid.Tables[0];
        }

        private void btn_ExportToExcel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("功能暂未开放");
        }

        private void chb_AdvancedCheck_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.ToolTipTitle = "高级存储";
            this.toolTip1.IsBalloon = false;
            this.toolTip1.UseFading = true;
            this.toolTip1.Show("勾选后则将该账户进行高级存储", this.chb_AdvancedCheck);
        }

        private void chb_ShowPassword_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.ToolTipTitle = "显示密码";
            this.toolTip1.IsBalloon = false;
            this.toolTip1.UseFading = true;
            this.toolTip1.Show("勾选后将显示明文密码", this.chb_ShowPassword);
        }

    }
}
