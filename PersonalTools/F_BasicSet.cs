using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PersonalTools
{
    public partial class F_BasicSet : Form
    {
        public F_BasicSet()
        {
            InitializeComponent();
        }
        ModuleClass.MyModule MyMC = new PersonalTools.ModuleClass.MyModule();
        DataClass.MyMeans MyDataClass = new PersonalTools.DataClass.MyMeans();
        static DataSet basicDataSet;
        public static string basicFied = "";
        public static string basicFiedzh = "";
        public static string basicTable = "";

        string basicSQL= "select ID as 编号," + basicFied + " as " + basicFiedzh + " from "+basicTable;

        string BasictemID = "";
        /// <summary>
        /// 0为初始状态，1为添加，2为修改
        /// </summary>
        int BasicisAdd = 0;


        private void F_BasicSet_Load(object sender, EventArgs e)
        {
            //根据SQL语句进行查询
            basicDataSet = MyDataClass.getDataSet(basicSQL, basicTable);
            dataGridView1.DataSource = basicDataSet.Tables[0];
            dataGridView1.AllowUserToAddRows = false;//dataGridView会默认在最后添加一个空行，这句话会让空行消失。

            dataGridView1.AutoGenerateColumns = true; //是否自动创建列
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;//第2列自动调整列宽
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)//禁用排序功能
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.ShowDataGridViewToTextbox(dataGridView1);
            
            MyMC.DataGridViewButton(dataGridView1, btn_N_First, btn_N_Previous, btn_N_Next, btn_N_End);//设置按钮状态
            MyMC.DataGridViewButton(dataGridView1, txt_N_cursor);//设置游标

        }
        private void ShowDataGridViewToTextbox(DataGridView DGrid)
        {
            //当DataGridView控件的记录>1时，将当前行中信息显示在相应的控件上,dataGridView列标号由SQL语句决定
            if (DGrid.RowCount > 1)
            {
                lbl_Basic.Text = "当前值：";
                txt_Basic.Text= DGrid[1, DGrid.CurrentCell.RowIndex].Value.ToString();
            }
        }
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            ShowDataGridViewToTextbox(this.dataGridView1);

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
            MyMC.DataGridViewButton(sender, e, dataGridView1, btn_N_First, btn_N_Previous, btn_N_Next, btn_N_End);//设置按钮状态
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

        private void btn_Save_Click(object sender, EventArgs e)
        {
            string sql = "";
            if (txt_Basic.Text.Trim().ToString() != "")
            {
                if (BasicisAdd == 1)//添加
                {

                    string ID = MyMC.GetAutocoding(basicTable, "ID");//自动编号

                    sql = "Insert into " + basicTable + "(ID," + basicFied + ")values(" + "'" + ID + "'" + "," + "'" + txt_Basic.Text.Trim().ToString() + "'" + ")";
                    MyDataClass.getsqlcom(sql);

                    btn_Cancel_Click(sender, e);//调用取消按钮
                    int r = dataGridView1.RowCount-1;
                    dataGridView1.CurrentCell = dataGridView1[0, r];

                }
                if (BasicisAdd == 2)//修改
                {
                    sql = " update " + basicTable + " set " + basicFied + "='" + txt_Basic.Text.Trim().ToString() + "'" + " where ID='" + BasictemID + "'";
                    MyDataClass.getsqlcom(sql);

                    btn_Cancel_Click(sender, e);//调用取消按钮
                }

            }
            else
            {
                MessageBox.Show("不能为空！");
            }

        }


        private void btn_Add_Click(object sender, EventArgs e)
        {
            BasicisAdd = 1;
            MyMC.EnabledButton(this.btn_Add, false, this.btn_Edit, false, btn_Delete, false, btn_Save, true, btn_Cancel, true);

            txt_Basic.Text = "";
            lbl_Basic.Text = "请在输入新值";
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            BasicisAdd = 0;
            MyMC.EnabledButton(btn_Add, true, btn_Edit, true, btn_Delete, true, btn_Save, false, btn_Cancel, false);
            basicDataSet = MyDataClass.getDataSet(basicSQL, basicTable);
            dataGridView1.DataSource = basicDataSet.Tables[0];
            this.ShowDataGridViewToTextbox(dataGridView1);
        }


        private void btn_Edit_Click(object sender, EventArgs e)
        {
            BasicisAdd = 2;
            MyMC.EnabledButton(btn_Add, false, btn_Edit, false, btn_Delete, false, btn_Save, true, btn_Cancel, true);

            txt_Basic.Text = "";
            lbl_Basic.Text = "请在输入修改后的新值";
        }
    }
}
