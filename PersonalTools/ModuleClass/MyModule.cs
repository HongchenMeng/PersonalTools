using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Reflection;//反射

namespace PersonalTools.ModuleClass
{
    class MyModule
    {
        #region  公共变量
        DataClass.MyMeans MyDataClass = new PersonalTools.DataClass.MyMeans();   //声明MyMeans类的一个对象，以调用其方法
        Encrypt.Encrypt encrypt = new Encrypt.Encrypt();
        //public static string FindValue = "";  //存储查询条件

        #endregion
        #region  自动编号
        /// <summary>
        /// 在添加信息时自动计算编号.
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="ID">字段名</param>
        /// <returns>返回String对象</returns>
        public string GetAutocoding(string TableName, string ID)
        {
            string str = MyDataClass.ReturnFirstRowCols("select max(" + ID + ") NID from " + TableName);
            if (str != "")
            {
                int i = Convert.ToInt32(str.ToString());  //将当前找到的最大编号转换成整数
                ++i;
                string s = i.ToString();
                return s;
            }
            return "1";
        }
        #endregion
        #region  清空控件集上的控件信息
        /// <summary>
        ///将GroupBox控件集上控件的Text属性设置为空
        /// </summary>
        /// <param name="GroupBox"></param>
        public void ClearGropBox(Control.ControlCollection GroupBox)
        {
            foreach (Control C in GroupBox)
            {
                if (C.GetType().Name == "TextBox" | C.GetType().Name == "MaskedTextBox" | C.GetType().Name == "ComboBox")
                    C.Text = "";
            }
        }
        #endregion
        public void ControlEnabled(Control.ControlCollection GroupBox,bool Enabled)
        {
            foreach(Control C in GroupBox)
            {
                if(C.GetType().Name!="Label")
                {
                    C.Enabled = Enabled;
                }
            }
        }
        #region  保存添加或修改的信息
        /// <summary>
        /// 历遍GroupBox内的控件，生成Insert语句用的字段和值
        /// </summary>
        /// <param name="GroupBox"></param>
        /// <returns>返回数组</returns>
        public string[] SqlInsert(Control.ControlCollection GroupBox)
        {
            string temName = "";
            string[] temF;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string s0 = "";
            string s1 = "";
            string cText = "";
            string[] f = new string[] { "PassWord", "PasswordSecurity", "Email", "Answer1", "Answer2", "Answer3", "Note", "Phone" };
            foreach (Control C in GroupBox)
            {
                if (C.GetType().Name == "TextBox" | C.GetType().Name == "MaskedTextBox" | C.GetType().Name == "ComboBox")
                {
                    temName = C.Name;
                    temF = temName.Split(Convert.ToChar('_'));
                    cText = C.Text.Trim().ToString();
                    //部分字段进行加密处理
                    foreach (string s in f)
                    {
                        if(s==temF[1])
                        {
                            cText = encrypt.EncryptDES(cText);//加密
                        }
                    }
                        dic.Add(temF[1], cText);
                }
            }
            foreach (KeyValuePair<string, string> d in dic)
            {
                if (s0 == "")
                {
                    s0 = d.Key;
                    s1 = "'" + d.Value + "'";
                    continue;
                }
                s0 = s0 + "," + d.Key;
                if (d.Value == null | d.Value == "")
                {
                    s1 = s1 + "," + "NULL";
                }
                else
                {
                    s1 = s1 + "," + "'" + d.Value + "'";
                }
            }
            string[] sqlInsert = new string[2] { s0, s1 };
            return sqlInsert;
        }
        /// <summary>
        /// 历遍GroupBox内的控件，生成Update【修改】语句用的字段和值
        /// </summary>
        /// <param name="GroupBox"></param>
        /// <returns>字符串</returns>
        public string SqlUpdate(Control.ControlCollection GroupBox)
        {
            string temName = "";
            string[] temF;
            string sqlUpdate = null;
            string[] f = new string[] { "PassWord", "PasswordSecurity", "Email", "Answer1", "Answer2", "Answer3", "Note", "Phone" };
            foreach (Control C in GroupBox)
            {
                if (C.GetType().Name == "TextBox" | C.GetType().Name == "MaskedTextBox" | C.GetType().Name == "ComboBox")
                {
                    temName = C.Name;
                    temF = temName.Split(Convert.ToChar('_'));
                    foreach (string s in f)
                    {
                        if (s == temF[1])
                        {
                            C.Text = encrypt.EncryptDES(C.Text.Trim().ToString());//加密
                        }
                    }
                    if (sqlUpdate == null)
                    {
                        if (C.Text == "")
                        {
                            sqlUpdate = temF[1] + "=NULL";
                        }
                        else
                        {
                            sqlUpdate = temF[1] + "=" + "'" + C.Text.Trim().ToString() + "'";
                        }
                        continue;
                    }
                    if (C.Text == "")
                    {
                        sqlUpdate = sqlUpdate + "," + temF[1] + "=NULL";
                    }
                    else
                    {
                        sqlUpdate = sqlUpdate + "," + temF[1] + "=" + "'" + C.Text.Trim().ToString() + "'";
                    }
                }
            }

            return sqlUpdate;
        }

        #endregion
        #region  用按钮控制数据记录移动时，改变按钮的可用状态
        /// <summary>
        /// 用按钮控制数据记录移动时，改变按钮的可用状态
        /// </summary>
        /// <param name="btn1">第1个按钮</param>
        /// <param name="b1">第1个按钮状态ture为启用，false为禁用</param>
        /// <param name="btn2">按钮2（可选）</param>
        /// <param name="b2"></param>
        /// <param name="btn3">按钮3（可选）</param>
        /// <param name="b3"></param>
        /// <param name="btn4">按钮4（可选）</param>
        /// <param name="b4"></param>
        /// <param name="btn5">按钮5（可选）</param>
        /// <param name="b5"></param>
        /// <param name="btn6">按钮6（可选）</param>
        /// <param name="b6"></param>
        public void EnabledButton(Button btn1, bool b1, Button btn2 = null, bool b2 = false, Button btn3 = null, bool b3 = false, Button btn4 = null, bool b4 = false, Button btn5 = null, bool b5 = false, Button btn6 = null, bool b6 = false)
        {
            btn1.Enabled = b1;
            if (btn2 != null) { btn2.Enabled = b2; }
            if (btn3 != null) { btn3.Enabled = b3; }
            if (btn4 != null) { btn4.Enabled = b4; }
            if (btn5 != null) { btn5.Enabled = b5; }
            if (btn6 != null) { btn6.Enabled = b6; }
        }
        #endregion
        #region  向comboBox控件传递数据表中的数据
        /// <summary>
        /// 动态向comboBox控件的下拉列表添加数据.
        /// </summary>
        /// <param name="comboBox">comboBox控件</param>
        /// <param name="SQLstr">SQL语句</param>
        public void comboBoxAddData(ComboBox comboBox, string SQLstr,bool b=false)
        {
            comboBox.Items.Clear();
            DataClass.MyMeans MyDataClsaa = new PersonalTools.DataClass.MyMeans();
            SQLiteDataReader MyDR = MyDataClsaa.ExecuteReader(SQLstr);
            if (MyDR.HasRows)
            {
                while (MyDR.Read())
                {
                    if (MyDR[0].ToString() != "" && MyDR[0].ToString() != null)
                    {
                        string mydr0 = MyDR[0].ToString();
                        if (b == true)
                            mydr0 = encrypt.DecryptDES(mydr0);
                        comboBox.Items.Add(mydr0);
                    }
                        
                }
            }
            MyDR.Close();

        }
        #endregion
        #region 更新ataGridView标号和按钮状态
        /// <summary>
        /// 点击【第1个，前1个， 后1个，最后1个】其中一个后，改变他们的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="dataGridView"></param>
        /// <param name="btn_N_First"></param>
        /// <param name="btn_N_Previous"></param>
        /// <param name="btn_N_Next"></param>
        /// <param name="btn_N_End"></param>
        public void DataGridViewButton(object sender , EventArgs e , DataGridView dataGridView,Button btn_N_First,Button btn_N_Previous,Button btn_N_Next,Button btn_N_End)
        {
            int maxRow = dataGridView.RowCount-1;
            int ColInd = 0;
            if (dataGridView.CurrentCell.ColumnIndex == -1 || dataGridView.CurrentCell.ColumnIndex > 1)
            {
                ColInd = 0;
            }
            else
            {
                ColInd = dataGridView.CurrentCell.ColumnIndex;//活动单元格列标
            }

            if ((((Button)sender).Name) == btn_N_First.Name)
            {
                dataGridView.CurrentCell = dataGridView[ColInd, 0];//第一条数据
            }
            if ((((Button)sender).Name) == btn_N_Previous.Name)
            {
                if (dataGridView.CurrentCell.RowIndex >0)
                {
                    dataGridView.CurrentCell = dataGridView[ColInd, dataGridView.CurrentCell.RowIndex-1];//上一条数据
                }
            }
            if ((((Button)sender).Name) == btn_N_Next.Name)
            {
                if (dataGridView.CurrentCell.RowIndex < maxRow)
                {
                    dataGridView.CurrentCell = dataGridView[ColInd, dataGridView.CurrentCell.RowIndex + 1];//下一条数据
                }
            }
            if ((((Button)sender).Name) == btn_N_End.Name)
            {
                dataGridView.CurrentCell = dataGridView[ColInd, maxRow];//最后一条数据
            }

            if(dataGridView.RowCount==0)//dataGridView没有数据
            {
                this.EnabledButton(btn_N_First, false, btn_N_Previous, false, btn_N_Next, false, btn_N_End, false);
            }
            else if(dataGridView.CurrentCell.RowIndex == 0)//当前在第1条数据
            {
                this.EnabledButton(btn_N_First, false, btn_N_Previous, false, btn_N_Next, true, btn_N_End, true);
            }
            else if(dataGridView.CurrentCell.RowIndex == maxRow)//当前在最后1条数据
            {
                this.EnabledButton(btn_N_First, true, btn_N_Previous, true, btn_N_Next, false, btn_N_End, false);
            }
            else
            {
                this.EnabledButton(btn_N_First, true, btn_N_Previous, true, btn_N_Next, true, btn_N_End, true);
            }
            
        }
        /// <summary>
        /// 设置DataGridView对应的【第1个，前1个， 后1个，最后1个】按钮的状态
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="btn_N_First"></param>
        /// <param name="btn_N_Previous"></param>
        /// <param name="btn_N_Next"></param>
        /// <param name="btn_N_End"></param>
        public void DataGridViewButton(DataGridView dataGridView, Button btn_N_First, Button btn_N_Previous, Button btn_N_Next, Button btn_N_End)
        {
            if (dataGridView.RowCount == 0)
            {
                this.EnabledButton(btn_N_First, false, btn_N_Previous, false, btn_N_Next, false, btn_N_End, false);
            }
            else if (dataGridView.CurrentCell.RowIndex == 0)
            {
                this.EnabledButton(btn_N_First, false, btn_N_Previous, false, btn_N_Next, true, btn_N_End, true);
            }
            else if (dataGridView.CurrentCell.RowIndex == dataGridView.RowCount)
            {
                this.EnabledButton(btn_N_First, true, btn_N_Previous, true, btn_N_Next, false, btn_N_End, false);
            }
            else
            {
                this.EnabledButton(btn_N_First, true, btn_N_Previous, true, btn_N_Next, true, btn_N_End, true);
            }
        }
        /// <summary>
        /// 设置DataGridView的游标，当前标号/总标号 1/99
        /// 需要设置dataGridView.AllowUserToAddRows = false，屏蔽空行
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="control">一个控件，一般为textbox</param>
        public void DataGridViewButton(DataGridView dataGridView, Control control)
        {
            if((dataGridView.RowCount != 0))
                {
                int s = dataGridView.CurrentCell.RowIndex + 1;
                int n = dataGridView.RowCount;
                string str = s + "/" + n;
                control.Text = str;
            }
            else
            {
                control.Text = "0/0";
            }
        }
        #endregion



        #region  添加用户权限
        /// <summary>
        /// 在添加用户时，将权限模版中的信息添加到用户权限表中.
        /// </summary>
        /// <param name="ID">用户编号</param>
        /// <param name="n">权限值</param>
        /// <>
        public void ADD_Pope(string ID, int n)
        {
            DataSet DSet;
            DSet = MyDataClass.getDataSet("select PopeName from tb_PopeModel", "tb_PopeModel");
            for (int i = 0; i < DSet.Tables[0].Rows.Count; i++)
            {
                MyDataClass.getsqlcom("insert into tb_UserPope (ID,PopeName,Pope) values('" + ID + "','" + Convert.ToString(DSet.Tables[0].Rows[i][0]) + "'," + n + ")");
            }
        }
        #endregion
        #region  显示用户权限
        /// <summary>
        /// 显示指定用户的权限.
        /// </summary>
        /// <param name="GBox">GroupBox控件的数据集</param>
        /// <param name="TName">获取用户编号</param>
        public void Show_Pope(Control.ControlCollection GBox, string TID)
        {
            string sID = "";
            string CheckName = "";
            bool t = false;
            DataSet DSet = MyDataClass.getDataSet("select ID,PopeName,Pope from tb_UserPope where ID='" + TID + "'", "tb_UserPope");
            for (int i = 0; i < DSet.Tables[0].Rows.Count; i++)
            {
                sID = Convert.ToString(DSet.Tables[0].Rows[i][1]);
                if ((int)(DSet.Tables[0].Rows[i][2]) == 1)
                    t = true;
                else
                    t = false;
                foreach (Control C in GBox)
                {
                    if (C.GetType().Name == "CheckBox")
                    {
                        CheckName = C.Name;
                        if (CheckName.IndexOf(sID) > -1)
                        {
                            ((CheckBox)C).Checked = t;
                        }
                    }
                }
            }
        }
        #endregion
        #region  修改指定用户权限
        /// <summary>
        /// 修改指定用户的权限.
        /// </summary>
        /// <param name="GBox">GroupBox控件的数据集</param>
        /// <param name="TName">获取用户编号</param>
        public void Amend_Pope(Control.ControlCollection GBox, string TID)
        {
            string CheckName = "";
            int tt = 0;
            foreach (Control C in GBox)
            {
                if (C.GetType().Name == "CheckBox")
                {
                    if (((CheckBox)C).Checked)
                        tt = 1;
                    else
                        tt = 0;
                    CheckName = C.Name;
                    string[] Astr = CheckName.Split(Convert.ToChar('_'));
                    MyDataClass.getsqlcom("update tb_UserPope set Pope=" + tt + " where (ID='" + TID + "') and (PopeName='" + Astr[1].Trim() + "')");
                }
            }

        }
        #endregion
        #region  根据用户权限设置主窗体菜单
        /// <summary>
        /// 根据用户权限设置菜单是否可用.
        /// </summary>
        /// <param name="MenuS">MenuStrip控件</param>
        /// <param name="UName">当前登录用户名</param>
        public void MainPope(MenuStrip MenuS, String UName)
        {
            string Str = "";
            string MenuName = "";
            DataSet DSet = MyDataClass.getDataSet("select ID from tb_Login where Name='" + UName + "'", "tb_Login");    //获取当前登录用户的信息
            string UID = Convert.ToString(DSet.Tables[0].Rows[0][0]);   //获取当前用户编号
            DSet = MyDataClass.getDataSet("select ID,PopeName,Pope from tb_UserPope where ID='" + UID + "'", "tb_UserPope");    //获取当前用户的权限信息
            bool bo = false;
            for (int k = 0; k < DSet.Tables[0].Rows.Count; k++) //遍历当前用户的权限名称
            {
                Str = Convert.ToString(DSet.Tables[0].Rows[k][1]);  //获取权限名称
                if (Convert.ToInt32(DSet.Tables[0].Rows[k][2]) == 1)    //判断权限是否可用
                    bo = true;
                else
                    bo = false;
                for (int i = 0; i < MenuS.Items.Count; i++) //遍历菜单栏中的一级菜单项
                {
                    ToolStripDropDownItem newmenu = (ToolStripDropDownItem)MenuS.Items[i];  //记录当前菜单项下的所有信息
                    if (newmenu.HasDropDownItems && newmenu.DropDownItems.Count > 0)    //如果当前菜单项有子级菜单项
                        for (int j = 0; j < newmenu.DropDownItems.Count; j++)    //遍历二级菜单项
                        {
                            MenuName = newmenu.DropDownItems[j].Name;   //获取当前菜单项的名称
                            if (MenuName.IndexOf(Str) > -1) //如果包含权限名称
                                newmenu.DropDownItems[j].Enabled = bo;  //根据权限设置可用状态
                            ToolStripDropDownItem newmenu2 = (ToolStripDropDownItem)newmenu.DropDownItems[j];   //记录当前菜单项的所有信息
                            if (newmenu2.HasDropDownItems && newmenu2.DropDownItems.Count > 0)  //如果当前菜单项有子级菜单项
                                for (int p = 0; p < newmenu2.DropDownItems.Count; p++)  //遍历三级菜单项
                                {
                                    MenuName = newmenu2.DropDownItems[p].Name;  //获取当前菜单项的名称
                                    if (MenuName.IndexOf(Str) > -1) //如果包含权限名称
                                        newmenu2.DropDownItems[p].Enabled = bo;  //根据权限设置可用状态
                                }
                        }
                }
            }
        }
        #endregion

        #region  将日期转换成指定的格式
        /// <summary>
        /// 将日期转换成yyyy-mm-dd格式.
        /// </summary>
        /// <param name="NDate">日期</param>
        /// <returns>返回String对象</returns>
        public string Date_Format(string NDate)
        {
            string sm, sd;
            int y, m, d;
            try
            {
                y = Convert.ToDateTime(NDate).Year;
                m = Convert.ToDateTime(NDate).Month;
                d = Convert.ToDateTime(NDate).Day;
            }
            catch
            {
                return "";
            }
            if (y == 1900)
                return "";
            if (m < 10)
                sm = "0" + Convert.ToString(m);
            else
                sm = Convert.ToString(m);
            if (d < 10)
                sd = "0" + Convert.ToString(d);
            else
                sd = Convert.ToString(d);
            return Convert.ToString(y) + "-" + sm + "-" + sd;
        }
        #endregion

        #region  将时间转换成指定的格式
        /// <summary>
        /// 将时间转换成yyyy-mm-dd格式.
        /// </summary>
        /// <param name="NDate">日期</param>
        /// <returns>返回String对象</returns>
        public string Time_Format(string NDate)
        {
            string sh, sm, se;
            int hh, mm, ss;
            try
            {
                hh = Convert.ToDateTime(NDate).Hour;
                mm = Convert.ToDateTime(NDate).Minute;
                ss = Convert.ToDateTime(NDate).Second;

            }
            catch
            {
                return "";
            }
            sh = Convert.ToString(hh);
            if (sh.Length < 2)
                sh = "0" + sh;
            sm = Convert.ToString(mm);
            if (sm.Length < 2)
                sm = "0" + sm;
            se = Convert.ToString(ss);
            if (se.Length < 2)
                se = "0" + se;
            return sh + sm + se;
        }
        #endregion

        #region  遍历清空指定的控件
        /// <summary>
        /// 清空所有控件下的所有控件.
        /// </summary>
        /// <param name="Con">可视化控件</param>
        public void Clear_Control(Control.ControlCollection Con)
        {
            foreach (Control C in Con)
            { //遍历可视化组件中的所有控件
                if (C.GetType().Name == "TextBox")  //判断是否为TextBox控件
                    if (((TextBox)C).Visible == true)   //判断当前控件是否为显示状态
                        ((TextBox)C).Clear();   //清空当前控件
                if (C.GetType().Name == "MaskedTextBox")  //判断是否为MaskedTextBox控件
                    if (((MaskedTextBox)C).Visible == true)   //判断当前控件是否为显示状态
                        ((MaskedTextBox)C).Clear();   //清空当前控件
                if (C.GetType().Name == "ComboBox")  //判断是否为ComboBox控件
                    if (((ComboBox)C).Visible == true)   //判断当前控件是否为显示状态
                        ((ComboBox)C).Text = "";   //清空当前控件的Text属性值
                if (C.GetType().Name == "PictureBox")  //判断是否为PictureBox控件
                    if (((PictureBox)C).Visible == true)   //判断当前控件是否为显示状态
                        ((PictureBox)C).Image = null;   //清空当前控件的Image属性
            }
        }
        #endregion

        #region  将当前表的数据信息显示在指定的控件上
        /// <summary>
        /// 将DataGridView控件的当前记录显示在其它控件上.
        /// </summary>
        /// <param name="DGrid">DataGridView控件</param>
        /// <param name="GBox">GroupBox控件的数据集</param>
        /// <param name="TName">获取信息控件的部份名称</param>
        public void Show_DGrid(DataGridView DGrid, Control.ControlCollection GBox, string TName)
        {
            string sID = "";
            if (DGrid.RowCount > 0)
            {
                for (int i = 2; i < DGrid.ColumnCount; i++)
                {
                    sID = TName + i.ToString();
                    foreach (Control C in GBox)
                    {
                        if (C.GetType().Name == "TextBox" | C.GetType().Name == "MaskedTextBox" | C.GetType().Name == "ComboBox")
                            if (C.Name == sID)
                            {
                                if (C.GetType().Name != "MaskedTextBox")
                                    C.Text = DGrid[i, DGrid.CurrentCell.RowIndex].Value.ToString();
                                else
                                    C.Text = Date_Format(Convert.ToString(DGrid[i, DGrid.CurrentCell.RowIndex].Value).Trim());
                            }
                    }
                }
            }

        }
        #endregion

        #region  组合查询条件
        /// <summary>
        /// 根据控件是否为空组合查询条件.
        /// </summary>
        /// <param name="GBox">GroupBox控件的数据集</param>
        /// <param name="TName">获取信息控件的部份名称</param>
        /// <param name="TName">查询关系</param>
        public string Find_Grids(Control.ControlCollection GBox, string TName, string ANDSign, string FindValue = "")
        {
            string sID = "";    //定义局部变量
            if (FindValue.Length > 0)
                FindValue = FindValue + ANDSign;
            foreach (Control C in GBox)
            { //遍历控件集上的所有控件
                if (C.GetType().Name == "TextBox" | C.GetType().Name == "ComboBox"|C.GetType().Name=="DateTimePicker")
                { //判断是否要遍历的控件
                    if (C.GetType().Name == "ComboBox" && C.Text != "")
                    {   //当指定控件不为空时
                        sID = C.Name;
                        if (sID.IndexOf(TName) > -1)
                        {    //当TName参数是当前控件名中的部分信息时
                            string[] Astr = sID.Split(Convert.ToChar('_')); //用“_”符号分隔当前控件的名称,获取相应的字段名
                            FindValue = FindValue + "(" + Astr[1] + " = '" + C.Text + "')" + ANDSign;   //生成查询条件
                        }
                    }
                    if (C.GetType().Name == "TextBox" && C.Text != "")
                    {
                        sID = C.Name;
                        if (sID.IndexOf(TName) > -1)
                        {    //当TName参数是当前控件名中的部分信息时
                            string[] Astr = sID.Split(Convert.ToChar('_')); //用“_”符号分隔当前控件的名称,获取相应的字段名
                            FindValue = FindValue + "(" + Astr[1] + " like '%" + C.Text + "%')" + ANDSign;   //生成查询条件
                        }
                    }
                        if (C.GetType().Name == "DateTimePicker" && C.Text!= "")  //如果当前为DateTimePicker控件，并且控件不为空
                    {
                        sID = C.Name;   //获取当前控件的名称
                        if (sID.IndexOf(TName) > -1)    //判断TName参数值是否为当前控件名的子字符串
                        {
                            string[] Astr = sID.Split(Convert.ToChar('_')); //以“_”为分隔符，将控件名存入到一维数组中
                            string m_Sgin = ""; //用于记录逻辑运算符
                            string mID = "";    //用于记录字段名
                             mID = Astr[1];  //获取当前条件所对应的字段名称
                            foreach (Control C1 in GBox)    //遍历控件集
                            {
                                if (C1.GetType().Name == "ComboBox")    //判断是否为ComboBox组件
                                    if ((C1.Name).IndexOf(mID) > -1)    //判断当前组件名是否包含条件组件的部分文件名
                                    {
                                        if (C1.Text == "")  //当查询条件为空时
                                            break;  //退出本次循环
                                        else
                                        {
                                            m_Sgin = C1.Text;   //将条件值存储到m_Sgin变量中
                                            break;
                                        }
                                    }
                            }
                            if (m_Sgin != "")   //当该务件不为空时
                                FindValue = FindValue + "(" + mID + m_Sgin + "datetime('"+C.Text + "'))" + ANDSign;    //组合SQL语句的查询条件
                        }
                    }
                }
            }
            if (FindValue.Length > 0)   //当存储查询条的变量不为空时，删除逻辑运算符AND和OR
            {
                if (FindValue.IndexOf("AND") > -1)  //判断是否用AND连接条件
                    FindValue = FindValue.Substring(0, FindValue.Length - 4);
                if (FindValue.IndexOf("OR") > -1)  //判断是否用OR连接条件
                    FindValue = FindValue.Substring(0, FindValue.Length - 3);
            }
            else
                FindValue = "";

            return FindValue;
        }
        #endregion

        /// <summary>
        /// 添加子菜单
        /// </summary>
        /// <param name="toolStripItemCollection">要添加到的子菜单集合</param>
        /// <param name="Menutext">菜单按钮显示的文字</param>
        /// <param name="menuClicked">点击时触发的事件</param>
        /// <param name="MenuTag">按钮的Tag属性值，格式：窗体/字段/中文字段名/表名/权限值 </param>
        /// <returns>生成的子菜单，如果为分隔条则返回null</returns>
        public ToolStripMenuItem AddContextMenu(ToolStripItemCollection toolStripItemCollection, string Menutext, EventHandler menuClicked=null, string MenuTag = null)
        {
            if (!string.IsNullOrEmpty(Menutext))
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem(Menutext);
                if (menuClicked != null)
                {
                    tsmi.Click += menuClicked;
                    tsmi.Enabled = false;//设置不可点击
                }
                toolStripItemCollection.Add(tsmi);
                tsmi.Tag = MenuTag;

                return tsmi;
            }
            return null;
        }
       /// <summary>
       /// 点击事件
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
       public void MenuClicked(object sender, EventArgs e)//,string AssemblyName, string ClassName
        {
            ToolStripMenuItem tsm = (ToolStripMenuItem)sender;
            string tag = (string)tsm.Tag;
            string[] s= tag.Split(Convert.ToChar('/'));

            if(s[0]== "F_BasicSet")
            {
                F_BasicSet.basicFied = s[1];
                F_BasicSet.basicFiedzh = s[2];
                F_BasicSet.basicTable =s[3];
                F_BasicSet fbasicset = new F_BasicSet();
                fbasicset.Text = sender.ToString();
                fbasicset.ShowDialog();
            }
           if(s[0]== "passwordForm.F_PasswordManager")
            {
                passwordForm.F_PasswordManager.AdvancedCheck = s[1];
                passwordForm.F_PasswordManager f = new passwordForm.F_PasswordManager();
                f.ShowDialog();
            }
            //反射应用
            //Type t = Assembly.GetAssembly(this.GetType()).GetType(AssemblyName + "." + s[0]);//PersonalTools.F_BasicSet
            //Form ff = (Form)Activator.CreateInstance(t);
            if(s[0]=="F_Category")
            {
                F_Category fcategory = new F_Category();
                fcategory.ShowDialog();
            }
            if (s[0]=="F_UserSet")
            {
                F_UserSet fuserset = new F_UserSet();
                fuserset.ShowDialog();
            }
            
            if(s[0]=="F_Help")
            {
                F_Help fhelp = new F_Help();
                fhelp.ShowDialog();
            }
        }

        /// <summary>
        /// 根据权限初始化权限
        /// </summary>
        /// <param name="MenuS"></param>
        /// <param name="qx">权限值</param>
        public void MaiPope(MenuStrip MenuS, int qx)
        {
            for (int i = 0; i < MenuS.Items.Count; i++) //遍历菜单栏中的一级菜单项
            {

                ToolStripDropDownItem newmenu = (ToolStripDropDownItem)MenuS.Items[i];  //记录当前菜单项下的所有信息
                if (newmenu.Tag.ToString() != "")
                {
                    int iTag = this.iTag(newmenu);
                    if (iTag >= qx)
                        newmenu.Enabled = true;
                }
                if (newmenu.HasDropDownItems && newmenu.DropDownItems.Count > 0)    //如果当前菜单项有子级菜单项
                    for (int j = 0; j < newmenu.DropDownItems.Count; j++)    //遍历二级菜单项
                    {
                        ToolStripDropDownItem newmenuA = (ToolStripDropDownItem)newmenu.DropDownItems[j];
                        if (newmenuA.Tag.ToString()!="")
                        {
                            int iTag = this.iTag(newmenuA);
                            if (iTag >= qx)
                                newmenuA.Enabled = true;
                        }
                        ToolStripDropDownItem newmenu2 = (ToolStripDropDownItem)newmenu.DropDownItems[j];   //记录当前菜单项的所有信息
                        if (newmenu2.HasDropDownItems && newmenu2.DropDownItems.Count > 0)  //如果当前菜单项有子级菜单项
                            for (int p = 0; p < newmenu2.DropDownItems.Count; p++)  //遍历三级菜单项
                            {
                                if (newmenu2.DropDownItems[j].Tag.ToString() != "")
                                {
                                    int iTag = this.iTag(newmenu);
                                    if (iTag >= qx)
                                        newmenu2.DropDownItems[j].Enabled = true;
                                }
                            }
                    }
            }
        }
        private int iTag(ToolStripDropDownItem tsm)
        {
            string tag = (string)tsm.Tag;
            string[] s = tag.Split(Convert.ToChar('/'));
            int j = s.Length - 1;//最后一位
            int i = Convert.ToInt32(s[j]);
            return i;
        }

    }
}
