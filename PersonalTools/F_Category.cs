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
    public partial class F_Category : Form
    {
        ModuleClass.MyModule MyMC = new PersonalTools.ModuleClass.MyModule();
        DataClass.MyMeans MyDataClass = new PersonalTools.DataClass.MyMeans();
        public F_Category()
        {
            InitializeComponent();

        }

        private void btn_Add_Click(object sender, EventArgs e)
        {


            string ID = MyMC.GetAutocoding("t_Category", "ID");//自动编号
            string Category = find_Category.Text;
            string Type = find_Type.Text;
            string sqlin1 = "select * from t_Category where Category= '" + Category + "' and Type='" + Type + "'";
            string sqlin2 = "INSERT INTO t_Category(ID,Category,Type) VALUES('" + ID + "','" + Category + "','" + Type + "')";// +
                                                                                                                              // " WHERE not exists(select * from t_Category where Category= "+Category+" and Type="+Type +")";
            try
            {
              string s=  MyDataClass.ReturnFirstRowCols(sqlin1);
                MessageBox.Show("记录已存在");
            }
            catch
            {
                
                MyDataClass.getsqlcom(sqlin2);
                MessageBox.Show("添加成功");
            }
            }

        private void F_Category_Load(object sender, EventArgs e)
        {
            MyMC.comboBoxAddData(find_Category, "select distinct Category from t_Category");
        }

        private void find_Type_TextChanged(object sender, EventArgs e)
        {
            find_Type.Items.Clear();
            MyMC.comboBoxAddData(find_Type, "select Type from t_Category where Category='" + find_Category.Text.Trim() + "'");
        }
    }
}
