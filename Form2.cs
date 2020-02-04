using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MLTcola桌面管理系统
{
    public partial class Form2 : Form
    {
        private bool music_state = false;
        private bool text_state = false;
        private string music_flag = "";//用来存放music路径
        private string text_flag = "";//用来存放text路径
        private string base_paths = "";//用来存放软件所在电脑的位置
        private string all_paths = "";//用来存放所有的位置
        private string user_name;
        private int i = 0;
        public Form2()
        {
            InitializeComponent();
            label1.Hide();
            label2.Hide();
            textBox2.Hide();
            textBox3.Hide();
            button4.Hide();
            button5.Hide();
            textBox1.Hide();


        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            //隐藏其余不是设置路径的界面
            textBox1.Hide();
            //显示设置路径的界面
            label1.Show();
            label2.Show();
            textBox2.Show();
            textBox3.Show();
            button4.Show();
            button5.Show();
            textBox2.Text = ((Form1)this.Owner).music_paths;
            textBox3.Text = ((Form1)this.Owner).text_paths;


            Read_data();
        }

       
        #region  帮助
        private void button3_Click(object sender, EventArgs e)
        {
            //隐藏其余不是设置路径的界面
            label1.Hide();
            label2.Hide();
            textBox2.Hide();
            textBox3.Hide();
            button4.Hide();
            button5.Hide();
            //显示设置路径的界面
            textBox1.Show();

            user_name = ((Form1)this.Owner).user_name;
            textBox1.Text = "尊敬的用户：\r\n您好！您现在使用的是由M公司研发的桌面智能化管理系统，主要功能是根据输入打开相应软件。这款软件主要是为了让电脑工作者可以能够方便的" +
                             "、快捷的使用电脑。刚接触此软件你可能不会对此软件产生好感，因为你觉得鼠标可能比键盘更加的方便快捷，当然在一些简单的工作量" +
                             "情况下，并不能显示出此款软件的优点，但是当你打开了较多的软件之后，你会发现当再想打开另一个软件的时候，你必须在回到桌面然后" +
                             "在去打开，非常的不方便，这时候就能够显示出此款软件的优越性。下面是此款软件的快捷键设置：\r\n"+
                             "Esc                         退出软件\r\n" +
                            "Tab                         自动补全(在输入界面可以根据已输入的进行相似补全)\r\n" +
                            "Ctrl+ '                     自动隐藏(让软件自动向左隐藏)\r\n" +
                            "Delete                      清楚当前所有输入内容\r\n" +
                            "Enter                       确认当前输入内容\r\n" +
                            "如果遇到不懂的问题可以联系本公司的工作人员:\r\nQQ:2904188898\r\n本公司正在统计软件的使用情况，如果您有时间和精力的话，麻烦您往本公司支付宝上转一毛钱，以支持我们能够继续做下去"+
                            "\r\n支付宝：18233768517";



            //需要编辑文档
        }

        #endregion


        #region 设置路径
        private void button1_Click(object sender, EventArgs e)
        {
            //隐藏其余不是设置路径的界面
            textBox1.Hide();
            //显示设置路径的界面
            label1.Show();
            label2.Show();
            textBox2.Show();
            textBox3.Show();
            button4.Show();
            button5.Show();
            textBox2.Text = ((Form1)this.Owner).music_paths;
            textBox3.Text = ((Form1)this.Owner).text_paths;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if(music_state==false)
            {
                textBox2.Enabled = true;
                button4.Text = "完成";
                music_flag = SelectPath();
                music_state = true;
            }
            else
            {
                textBox2.Enabled = false;
                button4.Text = "编辑";
                music_state = false;
                textBox2.Text = music_flag;
                ((Form1)this.Owner).music_paths = textBox2.Text.ToString();
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (text_state == false)
            {
                textBox3.Enabled = true;
                button5.Text = "完成";
                text_flag = SelectPath();
                text_state = true;
            }
            else
            {
                textBox3.Enabled = false;
                button5.Text = "编辑";
                text_state = false;
                textBox3.Text = text_flag;
                ((Form1)this.Owner).text_paths = textBox3.Text.ToString();
            }
        }
        #endregion

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((Form1)this.Owner).textBox2.Focus();
            Save_data();
        }

        #region  选择一个路径
        private string SelectPath() //弹出一个选择目录的对话框
        {
            OpenFileDialog file = new OpenFileDialog();
            file.ShowDialog();
            return file.SafeFileName;
        }
        #endregion

        #region   读取储存数据
        private void Save_data()
        {
            //*****添加********//
            music_flag = textBox2.Text.ToString();
            text_flag = textBox3.Text.ToString();
            all_paths = music_flag +"]" + text_flag +"]";
            base_paths = Application.StartupPath;
            StreamWriter sw = File.CreateText(base_paths + "\\1.txt");
            sw.WriteLine(all_paths);
            sw.Close();
        }

        private void Read_data()
        {
            base_paths = Application.StartupPath;
            StreamReader sr = new StreamReader(base_paths + "\\1.txt", false);
            all_paths = sr.ReadLine().ToString();
            sr.Close();
            while(all_paths.Substring(i,1)!="]")
            {
                textBox2.AppendText(all_paths.Substring(i,0).ToLower());
                i++;
            }
            i++;
            while (all_paths.Substring(i, 1) != "]")
            {
                textBox3.AppendText(all_paths.Substring(i,0).ToLower());
                i++;
            }
            i = 0;
        }
        #endregion
    }
}
