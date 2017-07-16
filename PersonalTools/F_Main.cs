using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace PersonalTools
{
    public partial class F_Main : Form
    {
        private static ChineseLunisolarCalendar chineseDate = new ChineseLunisolarCalendar();

        public static int Login_Limit = 999; //定义全局变量，登陆权限
        public static string Login_Name = "";  //定义全局变量，记录当前登录的用户名
        public static int Login_n = 0;  //用户登录与重新登录的标识

        DataClass.MyMeans MyClass = new PersonalTools.DataClass.MyMeans();
        ModuleClass.MyModule MyMenu = new PersonalTools.ModuleClass.MyModule();

        #region 日历相关变量
        private int x = 10;//初始x坐标
        private int y = 10;//初始y坐标
        private int size = 1;//初始百分比比例
        private int width = 420;//初始宽度
        private int height = 440;//初始高度

        private string xq = "日一二三四五六";
        private DateTime date = DateTime.Now;
        private int selectYear;//初始化选择年份
        private int selectMonth;   //初始化选择月份
        private DateTime selectDay = DateTime.Now;
        private bool flag = true; //标记是否重绘panel
        private DateTime[,] dateArray = new DateTime[7, 6]; //new string[7, 6];   //记录日期信息

        Button btnPervYear = new Button();
        Button btnNextYear = new Button();
        ComboBox cmbSelectYear = new ComboBox();//年份选择
        ComboBox cmbSelectMonth = new ComboBox();//月份选择
        Button btnPervMonth = new Button();
        Button btnNextMonth = new Button();
        #endregion

        public F_Main()
        {
            InitializeComponent();
            DrawControls();
        }
        private void F_Main_Load(object sender, EventArgs e)
        {
            SetMenu();//加载菜单

            F_Login FrmLogin = new F_Login();   //声时登录窗体，进行调用
            FrmLogin.Tag = 1;   //将登录窗体的Tag属性设为1，表示调用的是登录窗体
            FrmLogin.ShowDialog();//加载登陆窗体
            FrmLogin.Dispose();
            MyMenu.MaiPope(mS_Main, Login_Limit);//初始化权限
            toolStripStatusLabel1.Text = "当前用户：";
            toolStripStatusLabel2.Text = Login_Name;
        }
        #region 日期相关
        /// <summary>
        /// 绘制按钮控件
        /// </summary>
        private void DrawControls()
        {
            #region 年份选择下拉控件
            
            btnPervYear.Location = new System.Drawing.Point(x + 10, y + 12 * size);
            btnPervYear.Name = "btnPervYear";
            btnPervYear.Size = new System.Drawing.Size(23 * size, 23 * size);
            btnPervYear.TabIndex = 0;
            btnPervYear.Text = "<";
            btnPervYear.UseVisualStyleBackColor = true;
            btnPervYear.Click += new System.EventHandler(btnPervYear_Click);

            //var cmbSelectYear = new ComboBox();//年份选择
            cmbSelectYear.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSelectYear.FormattingEnabled = true;
            cmbSelectYear.Location = new Point(x + 35 * size, y + 12 * size);
            cmbSelectYear.Name = "cmbSelectYear";
            cmbSelectYear.AutoSize = false;
            cmbSelectYear.DropDownHeight = 100;
            cmbSelectYear.Size = new Size(80 * size, 23 * size);
            cmbSelectYear.TabIndex = 0;
            cmbSelectYear.SelectionChangeCommitted += new EventHandler(cmbSelectYear_SelectionChangeCommitted);

           // var btn2 = new Button();
            btnNextYear.Location = new System.Drawing.Point(x + 117, y + 12 * size);
            btnNextYear.Name = "btnNextYear";
            btnNextYear.Size = new System.Drawing.Size(23 * size, 23 * size);
            btnNextYear.TabIndex = 0;
            btnNextYear.Text = ">";
            btnNextYear.UseVisualStyleBackColor = true;
            btnNextYear.Click += new System.EventHandler(btnNextYear_Click);

            for (int i = 1900; i <= 2049; i++)
            {
                cmbSelectYear.Items.Add(i + "年");
                if (i == date.Year)
                {
                    cmbSelectYear.SelectedItem = i + "年";
                    selectYear = i;
                }
            }
            #endregion

            #region 月份选择下拉控件

            //var btn3 = new Button();
            btnPervMonth.Location = new System.Drawing.Point(x + 150, y + 12 * size);
            btnPervMonth.Name = "btnPervMonth";
            btnPervMonth.Size = new System.Drawing.Size(23 * size, 23 * size);
            btnPervMonth.TabIndex = 0;
            btnPervMonth.Text = "<";
            btnPervMonth.UseVisualStyleBackColor = true;
            btnPervMonth.Click += new System.EventHandler(btnPervMonth_Click);

            //var cmbSelectMonth = new ComboBox();//月份选择
            cmbSelectMonth.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSelectMonth.FormattingEnabled = true;
            cmbSelectMonth.Location = new Point(x + 175, y + 12 * size);//相对于容器左上角的坐标
            cmbSelectMonth.Name = "cmbSelectMonth";
            cmbSelectMonth.AutoSize = false;
            cmbSelectMonth.DropDownHeight = 100;
            cmbSelectMonth.Size = new Size(60 * size, 23 * size);
            cmbSelectMonth.TabIndex = 2;
            cmbSelectMonth.SelectionChangeCommitted += new EventHandler(cmbSelectMonth_SelectionChangeCommitted);

            //var btn4 = new Button();
            btnNextMonth.Location = new System.Drawing.Point(x + 237, y + 12 * size);
            btnNextMonth.Name = "btnNextMonth";
            btnNextMonth.Size = new System.Drawing.Size(23 * size, 23 * size);
            btnNextMonth.TabIndex = 0;
            btnNextMonth.Text = ">";
            btnNextMonth.UseVisualStyleBackColor = true;
            btnNextMonth.Click += new System.EventHandler(this.btnNextMonth_Click);

            for (int i = 1; i <= 12; i++)
            {
                cmbSelectMonth.Items.Add(i + "月");
                if (i == date.Month)
                {
                    cmbSelectMonth.SelectedItem = i + "月";
                    selectMonth = i;
                }
            }
            #endregion

            var btnToday = new Button();
            btnToday.Location = new System.Drawing.Point(x + 300, y + 12 * size);
            btnToday.Name = "btnToday";
            btnToday.Size = new System.Drawing.Size(100 * size, 23 * size);
            btnToday.TabIndex = 0;
            btnToday.Text = "返回今天";
            btnToday.UseVisualStyleBackColor = true;
            btnToday.Click += new System.EventHandler(this.btnToday_Click);

            splitC.Panel2.Controls.Add(cmbSelectYear);
            splitC.Panel2.Controls.Add(cmbSelectMonth);
            splitC.Panel2.Controls.Add(btnToday);
            splitC.Panel2.Controls.Add(btnPervYear);
            splitC.Panel2.Controls.Add(btnNextYear);
            splitC.Panel2.Controls.Add(btnPervMonth);
            splitC.Panel2.Controls.Add(btnNextMonth);
        }
        /// <summary>
        /// 绘制日期数字区域
        /// </summary>
        /// <param name="datetime"></param>
        private void DrawDateNum(DateTime datetime)
        {
            ModuleClass.Calendar CalendarDate = new ModuleClass.Calendar();
            DateTime dtNow = DateTime.Parse(DateTime.Now.ToShortDateString());
            int firstDayofWeek = CalendarDate.GetWeekOfFirstDay(selectYear, selectMonth);
            int endMonthDay = CalendarDate.GetMonthDays(selectYear, selectMonth);

            var fontNum = new Font("", 16);
            var fontHoliday = new Font("", 10);
            var solidBrushWeekdays = new SolidBrush(Color.Gray);
            var solidBrushWeekend = new SolidBrush(Color.Chocolate);
            var solidBrushHoliday = new SolidBrush(Color.BurlyWood);
            Graphics g = splitC.Panel2.CreateGraphics();
            int num = 1;

            for (int j = 0; j < 6; j++)
            {
                for (int i = 0; i < 7; i++)
                {
                    #region 处理空白日期为空
                    if (j == 0 && i < firstDayofWeek) //定义当月第一天的星期的位置
                    {
                        continue;
                    }
                    if (num > endMonthDay) //定义当月最后一天的位置
                    {
                        break;
                    }
                    #endregion

                    DateTime dt = DateTime.Parse(datetime.Year + "-" + datetime.Month + "-" + num);
                    TimeSpan ts = dt - dtNow;
                    dateArray[i, j] = dt;
                    if(ts.Days==0)//今日绘制椭圆标识
                    {
                        g.DrawEllipse(new Pen(Color.Chocolate, 3), (15 + 60 * i * size), (92 + 60 * j * size), 50, 35);
                    }

                    string rqNum = num.ToString(CultureInfo.InvariantCulture);
                    int Nx = 28 + 60 * i * size;//日期数字x坐标
                    int Ny = 95 + 60 * j * size;

                    int Hx = 20 + 60 * i * size;//农历、假日x坐标
                    int Hy = 125 + 60 * j * size;
                    if(num>9)
                    {
                        Nx = Nx - 8;
                    }
                    if (i == 0 || i == 6)//周末日期
                    {
                        g.DrawString(rqNum, fontNum, solidBrushWeekend, Nx, Ny);//绘制日期
                        DateHoliday(g, dt, solidBrushWeekend, Hx, Hy);//绘制农历、节假日
                    }
                    else
                    {
                        g.DrawString(rqNum, fontNum, solidBrushWeekdays, Nx, Ny);
                        DateHoliday(g, dt, solidBrushWeekdays, Hx, Hy);
                    }
                    num++;
                }
            }
        }
        private void HolidaySize(Graphics g,string str,float fontsize, Brush brush, int dhX, int dhY)
        {
            Font f = new Font("", fontsize);
            SizeF sf = g.MeasureString(str, f); //测量文字
            SizeF sf1 = g.MeasureString(str, f, 80);
            SizeF sf2 = g.MeasureString(str, f, 60);
            RectangleF rf;

            int wordWidth = Convert.ToInt32(sf.Width);  //获取文字长度  一个中文10号汉字长度为20

            if (wordWidth < 45)//两个汉字
            {
                g.DrawString(str, new Font("", fontsize), brush, dhX, dhY);
                return;
            }
            if ( wordWidth < 65)//三个汉字
            {
                g.DrawString(str, new Font("", fontsize), brush, dhX-10, dhY);
                return;
            }
            if (wordWidth < 95)//四个汉字
            {
                 rf = new RectangleF(dhX , dhY, sf2.Width, sf2.Height);
                g.DrawString(str, new Font("", fontsize - 2), brush, rf);
                return;
             }
            if (wordWidth < 105)//五个汉字
            {
                 rf = new RectangleF(dhX - 6, dhY, sf1.Width, sf1.Height);
                g.DrawString(str, new Font("", fontsize - 2), brush, rf);
                return;
            }
            //六个汉字以上
             rf = new RectangleF(dhX - 15, dhY, sf1.Width, sf1.Height);
            g.DrawString(str, new Font("", fontsize - 5), brush, rf);
           // g.DrawString(str, new Font("", fontsize - 4), brush, dhX - 15, dhY);
        }
        private void DateHoliday(Graphics g,DateTime dt,Brush brush, int dhX,int dhY)
        {
            
            if (dt < new DateTime(1900, 1, 30)||dt>new DateTime(2049,12,31))//超出农历转化情况
            {
                ModuleClass.Calendar CCD = new ModuleClass.Calendar(dt,false);
                if (CCD.DateHoliday != "")//是否有公历节日
                {
                    Font f = new Font("", 10);
                    string str = CCD.DateHoliday;
                    HolidaySize(g, str, 10, brush, dhX, dhY);
                }
                else
                {
                    if (CCD.WeekDayHoliday != "")//是否有以周计算的节日
                    {
                        Font f = new Font("", 10);
                        string str = CCD.WeekDayHoliday;
                        HolidaySize(g, str, 10, brush, dhX, dhY);
                    }
                }
                }
            else
            {
                //农历
                ModuleClass.Calendar CCD = new ModuleClass.Calendar(dt);
                if (CCD.ChineseCalendarHoliday != "")//是否有农历节日
                {
                    g.DrawString(CCD.ChineseCalendarHoliday, new Font("", 10), brush, dhX, dhY);//28 + 60 * i * size, 125 + 60 * j * size
                }
                else
                {
                    if (CCD.DateHoliday != "")//是否有公历节日
                    {
                        Font f = new Font("", 10);
                        string str = CCD.DateHoliday;
                        HolidaySize(g, str, 10, brush, dhX, dhY);
                    }
                    else
                    {
                        if (CCD.WeekDayHoliday != "")//是否有以周计算的节日
                        {
                            Font f = new Font("", 10);
                            string str = CCD.WeekDayHoliday;
                            HolidaySize(g, str, 10, brush, dhX, dhY);
                        }
                        else//没有节日就显示农历
                        {
                            if (CCD.ChineseDayString == "初一")
                            {
                                g.DrawString(CCD.ChineseMonthString, new Font("", 10), brush, dhX, dhY);
                                return;
                            }
                            g.DrawString(CCD.ChineseDayString, new Font("", 10), brush, dhX, dhY);
                        }
                    }
                }
            }
        }
        private void splitC_Panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            ModuleClass.Calendar CCD = new ModuleClass.Calendar(selectDay,false);

            string dateStr = CCD.DateString;//公历日期中文表示法 如一九九七年七月一日
            string WeekDayStr = CCD.WeekDayStr;//星期几
            string DateHoliday = CCD.DateHoliday;//公历节日
            string WeekDayHoliday = CCD.WeekDayHoliday;//星期节日


            g.DrawString(dateStr, new Font("微软雅黑", 12), new SolidBrush(Color.Black), 60, 20);//公历日期
            g.FillRectangle(new SolidBrush(Color.Green), 60, 50, 180, 180);//底框
            if(selectDay.Day<10)//一位数日历
            {
                g.DrawString(selectDay.Day.ToString(), new Font("微软雅黑", 72), new SolidBrush(Color.Black), 90, 55);//日历天数
            }
            else//两位数日历
            {
                g.DrawString(selectDay.Day.ToString(), new Font("微软雅黑", 72), new SolidBrush(Color.Black), 60, 55);//日历天数
            }
            if(DateHoliday!="")//有公历节日
            {
                g.DrawString(DateHoliday, new Font("微软雅黑", 20), new SolidBrush(Color.Red), 80, 175);//公历节日
            }



           if(WeekDayHoliday!="")//有星期节日
            {
                g.DrawString(WeekDayStr + "(" + WeekDayHoliday + ")", new Font("微软雅黑", 20), new SolidBrush(Color.Red), 45, 270);//星期
            }
           else
            {
                g.DrawString(WeekDayStr, new Font("微软雅黑", 20), new SolidBrush(Color.Black), 80, 270);//星期
            }
            if (selectDay>=new DateTime(1990,1,30)&&selectDay<=new DateTime(2049,12,31))
            {
                ModuleClass.Calendar CCD2 = new ModuleClass.Calendar(selectDay);
                string ChineseMonthString = CCD2.ChineseMonthString;//农历月份
                string ChineseDayString = CCD2.ChineseDayString;//农历日期
                string ChineseCalendarHoliday = CCD2.ChineseCalendarHoliday;//农历节日
                string GanZhiAnimalYearString = CCD2.GanZhiAnimalYearString;//取农历年的干支表示法如 乙丑年
                string GanZhiMonthString = CCD2.GanZhiMonthString;
                string GanZhiDayString = CCD2.GanZhiDayString;
                // string AnimalString = CCD.AnimalString;//属相 如猴
                if (ChineseCalendarHoliday != "")//有农历节日
                {
                    g.DrawString(ChineseMonthString + ChineseDayString + "(" + ChineseCalendarHoliday + ")", new Font("微软雅黑", 20), new SolidBrush(Color.Red), 50, 230);
                }
                else
                {
                    g.DrawString(ChineseMonthString + ChineseDayString, new Font("微软雅黑", 20), new SolidBrush(Color.Black), 80, 230);
                }
                g.DrawString(GanZhiAnimalYearString, new Font("微软雅黑", 20), new SolidBrush(Color.Black), 60, 310);
                g.DrawString(GanZhiMonthString, new Font("微软雅黑", 18), new SolidBrush(Color.Black), 60, 350);
                g.DrawString(GanZhiDayString, new Font("微软雅黑", 18), new SolidBrush(Color.Black), 160, 350);

                g.DrawString("当前用户：" + Login_Name, new Font("微软雅黑", 14), new SolidBrush(Color.Black), 60, 550);
            }
            }
        private void splitC_Panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            #region 画内外框线条
            Pen PenOutline = new Pen(Color.Blue, 2);//创建外框线条，蓝色，宽度3
            Pen PenInnerline = new Pen(Color.Blue, 1);
            Pen PenInnerline1 = new Pen(Color.Gray, 1);
            g.DrawRectangle(PenOutline, x, y, width * size , height * size );//创建矩形外框
            g.DrawLine(PenInnerline, x+5, y+50*size, x+width * size-2, y + 50 * size);//画横线,分割按钮行
            for (int i = 0; i < 6; i++)//循环画横线
            {
                g.DrawLine(PenInnerline1, x + 5, y + 80 * size +60*i * size , x+width * size  - 2, y + 80 * size  + 60 * i * size );//画横线,分割星期行
            }
            g.DrawString("猴年猴赛雷,愿小猴子健康成长！" , new Font("微软雅黑", 14), new SolidBrush(Color.Red), x+5,y+80*size+60*6*size);
            g.DrawString("丙申年 【猴】丁酉月 壬子日！", new Font("微软雅黑", 12), new SolidBrush(Color.Blue), x + 5, y + 80 * size + 60 * 6 * size+30);
            #endregion
            #region 填充 “日一二三四五六”标识
            var solidBrushWeekday = new SolidBrush(Color.Black);
            var solidBrushWeekend = new SolidBrush(Color.Red);
            for (int i=0;i<7;i++)
            {
                if(i==0||i==6)
                {
                    g.DrawString(xq[i].ToString(), new Font("微软雅黑", 12), solidBrushWeekend, 28 +60* i*size, 60);
                }
                else
                {
                    g.DrawString(xq[i].ToString(), new Font("微软雅黑", 12), solidBrushWeekday, 28 + 60 * i*size, 60);
                }
            }
            #endregion

            if(flag)
            {
                DrawDateNum(date);
            }
            
        }
        #region 代码生成控件事件
        /// <summary>
        /// 年份选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSelectYear_SelectionChangeCommitted(object sender, EventArgs e)
        {
            flag = false;
            //var cmbSelectYear = sender as ComboBox;
            string s = cmbSelectYear.SelectedItem.ToString();
            selectYear = Int32.Parse(s.Substring(0, s.Length - 1));
            date = new DateTime(selectYear, selectMonth, 1);
            splitC.Panel2.Refresh();
            DrawDateNum(date);

        }

        private void cmbSelectMonth_SelectionChangeCommitted(object sender, EventArgs e)
        {
            flag = false;
           // var cmbSelectMonth = sender as ComboBox;
            string s = cmbSelectMonth.SelectedItem.ToString();
            selectMonth =Int32.Parse(s.Substring(0, s.Length - 1));
            date = new DateTime(selectYear, selectMonth, 1);
            splitC.Panel2.Refresh();
            DrawDateNum(date);
        }
        private void btnToday_Click(object sender, EventArgs e)
        {
            flag = false;
            splitC.Panel2.Refresh();
            date = DateTime.Now;
            selectYear = date.Year;
            selectMonth = date.Month;
            selectDay = DateTime.Now;
            DrawDateNum(date);

            cmbSelectMonth.SelectedItem = selectMonth + "月";
            cmbSelectYear.SelectedItem = selectYear + "年";
            splitC.Panel1.Refresh();

        }

        private void splitC_Panel2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.X < x || (e.X > (x + width * size)))
                {
                    return;
                }
                if ((e.Y < (y + 80 * size)) || (e.Y > (y + height * size)))
                {
                    return;
                }
                int x1 = (e.X - x) / 60;
                int y1 = (e.Y - y - 80 * size) / 60;
                Graphics g = splitC.Panel2.CreateGraphics();
                splitC.Panel2.Refresh();
                g.FillRectangle(new SolidBrush(Color.Green), x1 * 60 + 10 * size, y1 * 60 + 90 * size, 60, 60);
                DrawDateNum(date);
                if (dateArray[x1, y1]== new DateTime(1,1,1))
                {
                    return;
                }
                selectDay = dateArray[x1, y1];
                splitC.Panel1.Refresh();
            }
        }

        private void splitC_Panel2_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void btnPervYear_Click(object sender, EventArgs e)
        {
            if (date.Year == 1901)
            {
                return;
            }
            date = date.AddYears(-1);
           // var cmbSelectYear = sender as ComboBox;

            RefreshPanel2();
        }
        private void btnNextYear_Click(object sender, EventArgs e)
        {
            if (date.Year == 2049)
            {
                return;
            }
            date = date.AddYears(1);
            RefreshPanel2();
        }
        private void btnPervMonth_Click(object sender, EventArgs e)
        {
            if (date.Year == 1900 & date.Month == 2)
            {
                return;
            }
            date = date.AddMonths(-1);
            RefreshPanel2();

        }
        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            if (date.Year == 2049 & date.Month == 12)
            {
                return;
            }
            date = date.AddMonths(1);
            RefreshPanel2();

        }
        private void RefreshPanel2()
        {
            flag = false;
            splitC.Panel2.Refresh();
            selectYear = date.Year;
            selectMonth = date.Month;
            DrawDateNum(date);

            cmbSelectMonth.SelectedItem = selectMonth + "月";
            cmbSelectYear.SelectedItem = selectYear + "年";


        }
        #endregion
        #endregion

        private void SetMenu()
        {
            //手动绘制MenuStrip 命名为 mS_Main
           // ToolStripMenuItem PWM = MyMenu.AddContextMenu(mS_Main.Items, "密码管理");
            //MyMenu.AddContextMenu(PWM.DropDownItems, "查询", new EventHandler(MyMenu.MenuClicked), "passwordForm.F_PasswordManager/2");
            //MyMenu.AddContextMenu(PWM.DropDownItems, "邮箱设置", new EventHandler(MyMenu.MenuClicked), "F_BasicSet/Email/邮箱/t_Email/2");
            //MyMenu.AddContextMenu(PWM.DropDownItems, "问题设置", new EventHandler(MyMenu.MenuClicked), "F_BasicSet/Questions/密保问题/t_Questions/2");
            MyMenu.AddContextMenu(mS_Main.Items, "普通查询", new EventHandler(MyMenu.MenuClicked), "passwordForm.F_PasswordManager/0/2");
            MyMenu.AddContextMenu(mS_Main.Items, "高级查询", new EventHandler(MyMenu.MenuClicked), "passwordForm.F_PasswordManager/1/2");
           // MyMenu.AddContextMenu(mS_Main.Items, "分类管理", new EventHandler(MyMenu.MenuClicked), "F_Category/2");
           // MyMenu.AddContextMenu(mS_Main.Items, "邮箱设置", new EventHandler(MyMenu.MenuClicked), "F_BasicSet/Email/邮箱/t_Email/2");
            MyMenu.AddContextMenu(mS_Main.Items, "问题设置", new EventHandler(MyMenu.MenuClicked), "F_BasicSet/Questions/密保问题/t_Questions/2");
            MyMenu.AddContextMenu(mS_Main.Items, "用户管理", new EventHandler(MyMenu.MenuClicked), "F_UserSet/999");
            MyMenu.AddContextMenu(mS_Main.Items, "工具说明", new EventHandler(MyMenu.MenuClicked), "F_Help/999");
        }

        #region  设置主窗体菜单不可用
        /// <summary>
        /// 设置主窗体菜单不可用.
        /// </summary>
        /// <param name="MenuS">MenuStrip控件</param>
        public void MainMenuF(MenuStrip MenuS)
        {
            string Men = "";
            for (int i = 0; i < MenuS.Items.Count; i++)
            {
                Men = ((ToolStripDropDownItem)MenuS.Items[i]).Name;
                if (Men.IndexOf("Menu") == -1)
                    ((ToolStripDropDownItem)MenuS.Items[i]).Enabled = false;
                ToolStripDropDownItem newmenu = (ToolStripDropDownItem)MenuS.Items[i];
                if (newmenu.HasDropDownItems && newmenu.DropDownItems.Count > 0)
                    for (int j = 0; j < newmenu.DropDownItems.Count; j++)
                    {
                        Men = newmenu.DropDownItems[j].Name;
                        if (Men.IndexOf("Menu") == -1)
                            newmenu.DropDownItems[j].Enabled = false;
                        ToolStripDropDownItem newmenu2 = (ToolStripDropDownItem)newmenu.DropDownItems[j];
                        if (newmenu2.HasDropDownItems && newmenu2.DropDownItems.Count > 0)
                            for (int p = 0; p < newmenu2.DropDownItems.Count; p++)
                                newmenu2.DropDownItems[p].Enabled = false;
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
            DataSet DSet = MyClass.getDataSet("select ID from tb_Login where Name='" + UName + "'", "tb_Login");    //获取当前登录用户的信息
            string UID = Convert.ToString(DSet.Tables[0].Rows[0][0]);   //获取当前用户编号
            DSet = MyClass.getDataSet("select ID,PopeName,Pope from tb_UserPope where ID='" + UID + "'", "tb_UserPope");    //获取当前用户的权限信息
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
        #region  通过权限对主窗体进行初始化
        /// <summary>
        /// 对主窗体初始化
        /// </summary>
        private void Preen_Main()
        {
            //statusStrip1.Items[2].Text = DataClass.MyMeans.Login_Name;  //在状态栏显示当前登录的用户名
            //treeView1.Nodes.Clear();
            //MyMenu.GetMenu(treeView1, menuStrip1);  //调用公共类MyModule下的GetMenu()方法，将menuStrip1控件的子菜单添加到treeView1控件中
            //MyMenu.MainMenuF(menuStrip1);   //将菜单栏中的各子菜单项设为不可用状态
            //MyMenu.MainPope(menuStrip1, DataClass.MyMeans.Login_Name);  //根据权限设置相应子菜单的可用状态
        }
        #endregion

    }
}
