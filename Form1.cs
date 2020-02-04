using CSharpKeyboardHook;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MLTcola桌面管理系统
{
    public partial class Form1 : Form
    {
        public string music_paths=@"D:\Download\网易云音乐\installing\CloudMusic\cloudmusic.exe";//音乐路径
        public string text_paths = @"D:\Download\notepad++\installing\Notepad++\notepad++.exe";//记事本路径
        private string[] softwares = {"CLOUDMUSIC","NOTEPAD++"};
        private decimal similarity;
        private decimal similarity_max = -10;
        private int similarity_maxindex = -10;
        public string user_name = "MLTcola";//用户名字
        private int hide_speed = 1;//窗口自动隐藏的速度，实际为窗口每运动一单位所用时间，越大时间越长，速度越慢
        private int form_location_x;//窗口自动隐藏前的x坐标
        private int num = 2;//用来判断窗体的是否隐藏
        //***************************************************************************************************
        private string textbox_content = null;
        private bool software_flag = false;//软件打开标志
        private bool music_flag = false;//音乐打开标志
        private bool add_flag = false;//TAB补充标志
        private bool exit_flag = false;//退出标志
        private bool delete_flag = false;//删除内容标志
        private bool gouzi_flag = false;//因为钩子读取键盘值要有两个，一个按下一个抬起，以此为标志
        private string gouzi_savedata = "";//存储钩子按键获取的内容
        private int i = 0;//在钩子中的gouzi_savedata有应用

        //勾子管理类
        private KeyboardHookLib _keyboardHook = null;
        public IntPtr morehandle;

        public Form1()
        {
            InitializeComponent();
            timer2.Interval = 50;
            timer2.Start();
            timer3.Interval = 50;
            timer3.Start();
            timer4.Interval = 50;
            timer4.Start();
            this.TopMost = true;
            //软件位置初始化
            #region  安装钩子
            //安装勾子

            _keyboardHook = new KeyboardHookLib();

            _keyboardHook.InstallHook(this.KeyPress);
            #endregion
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region 开机自启
            // 添加到 当前登陆用户的 注册表启动项
            //RegistryKey RKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            //RKey.SetValue("MLTcola桌面管理系统", @"D:\MY\C#程序\winform程序\MLTcola桌面管理系统\MLTcola桌面管理系统\MLTcola桌面管理系统\bin\Debug\MLTcola桌面管理系统.exe");

            // 添加到 所有用户的 注册表启动项
            //RegistryKey RKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            //RKey.SetValue("MLTcola桌面管理系统", @"D:\MY\C#程序\winform程序\MLTcola桌面管理系统\MLTcola桌面管理系统\MLTcola桌面管理系统\bin\Debug\MLTcola桌面管理系统.exe");
            #endregion

        }
        #region   钩子隐藏弹出窗体
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            //取消勾子

            if (_keyboardHook != null) _keyboardHook.UninstallHook();
        }
        public void KeyPress(KeyboardHookLib.HookStruct hookStruct, out bool handle)
        {

            handle = false; //预设不拦截任何键
            Keys key = (Keys)hookStruct.vkCode;
            if (hookStruct.vkCode >= (int)Keys.A && hookStruct.vkCode <= (int)Keys.Z)
                {
                    gouzi_savedata = gouzi_savedata + key.ToString().ToLower();
                    if(i%2-1==0)
                    {
                        gouzi_flag = !gouzi_flag;
                        if(gouzi_flag)
                            textBox1.AppendText(gouzi_savedata.Substring(i-1,1).ToLower());
                        else
                            textBox1.AppendText(gouzi_savedata.Substring(i, 1).ToLower());

                    }             
                        i++;
                    if(i>=100)
                    {
                        i = 0;
                    }
                }
            if (num % 2 == 0 && num % 4 != 0)
               {
                    if (key == Keys.Enter)
                    {
                        software_flag = true;
                        timer1.Start();
                    }
                    if (key == Keys.Escape)
                    {
                        exit_flag = true;
                    }
                    if ((hookStruct.vkCode >= (int)Keys.A && hookStruct.vkCode <= (int)Keys.Z) || hookStruct.vkCode == (int)Keys.Enter || hookStruct.vkCode == (int)Keys.Back)
                    {
                        handle = true;
                    }

               }
            if (key == Keys.Tab)
            {
                add_flag = true;
            }            
            if (key == Keys.Delete)
            {
                delete_flag = true;
            }
            if ((Control.ModifierKeys & Keys.Control) != 0 &&key == Keys.Oem7)
            {
                if (num % 4 == 0)       
                {
                      for (int i = 0; i < 360; i++)
                      {
                            this.Left = i - 360;
                            System.Threading.Thread.Sleep(hide_speed);
                       }                  
                       textBox1.Text = "";
                }
                
                if (num % 2 == 0 && num % 4 != 0)
                 {
                      form_location_x = this.Location.X;

                       for (int i = 0; i < this.Width + form_location_x - 20; i++)
                       {
                          this.Left = form_location_x - i;
                          System.Threading.Thread.Sleep(hide_speed);
                       }
                 }
                
                num++;
            }        
        }
        #endregion

        #region 打开软件
        private void timer1_Tick(object sender, EventArgs e)
        {

            textbox_content = textBox1.Text.ToString().ToUpper();
            try
            {
                    if (textbox_content == softwares[1]&&software_flag==true)//记事本打开
                {
                    Process text_program = new Process();
                    text_program.StartInfo.FileName = text_paths;
                    text_program.Start();
                    software_flag = false;
                }
                    if (textbox_content == softwares[0] && software_flag == true)//音乐打开
                {
                        Process music_program = new Process();
                        music_program.StartInfo.FileName = music_paths;
                        music_program.Start();
                        software_flag = false;
                 }                
              
            }
            catch (Exception)
            {
            }
            timer1.Stop();
        }


        private void timer3_Tick(object sender, EventArgs e)
        {
            if (add_flag == true)                                        //自动补全标志
            {
                add_flag = false;
                textbox_content = textBox1.Text.ToString().ToUpper();
                for (int i = 0; i < softwares.Length; i++)//寻找最大相似度的软件
                {
                    similarity = GetSimilarityWith(textbox_content, softwares[i]);
                    if (similarity > similarity_max)
                    {
                        similarity_max = similarity;
                        similarity_maxindex = i;
                    }
                }
                if(similarity>=0)
                {
                    textBox1.Text = softwares[similarity_maxindex].ToLower();
                    similarity_max = -10;
                    similarity_maxindex = -10;
                } 
                else
                {
                    MessageBox.Show("查无此软件", "MLTcola提醒您");
                    textBox1.Text 
                        = "";
                }
            }
            if (exit_flag == true)//esc退出
            {
                System.Environment.Exit(0);
                exit_flag = false;
            }
            if (delete_flag == true)
            {
                textBox1.Text = "";
                gouzi_savedata = "";
                i = 0;
                delete_flag = false;
            }

        }

        #endregion

        #region//模糊搜索，求解相似度
        public static decimal GetSimilarityWith(string sourceString, string str)
        {
            try
            {
                decimal Kq = 2;
                decimal Kr = 1;
                decimal Ks = 1;
                char[] ss = sourceString.ToCharArray();
                char[] st = str.ToCharArray();
                //获取交集数量
                int q = ss.Intersect(st).Count();
                int s = ss.Length - q;
                int r = st.Length - q;
                return Kq * q / (Kq * q + Kr * r + Ks * s);
            }
            catch (Exception)
            {               
                return -1;
                
            }
        }
        #endregion

        #region//帮助窗口
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Owner = this;
            f.MaximizeBox = false;
            f.MinimizeBox = false;
            f.Show();
        }
        #endregion


        #region   窗体透明度

        //private void textBox1_MouseEnter(object sender, EventArgs e)
        //{
        //    this.Opacity = 1;
        //}
        //private void Form1_MouseLeave(object sender, EventArgs e)
        //{
        //    this.Opacity = 0.5;//透明度设置....半透明
        //}
        #endregion

        #region  按键隐藏窗口与自动弹出窗口
        //private void timer2_Tick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        timer2.Stop();
        //        if (hide_flag==true)
        //        {
        //            form_location_x = this.Location.X;

        //            for (int i = 0; i < this.Width + form_location_x-20; i++)
        //            {
        //                this.Left = form_location_x - i;
        //                System.Threading.Thread.Sleep(hide_speed);
        //            }
        //            this.Focus();
        //            pop_flag = false;
        //        }
        //            if (hide_flag == false && this.Location.X < 1)
        //                {
        //                   for (int i = 0; i < 360; i++)
        //                      {
        //                           this.Left = i - 360;
        //                           System.Threading.Thread.Sleep(hide_speed);
        //                      }
        //                   this.textBox1.Focus();
        //                   textBox1.Text = "";
        //                   pop_flag = true;
        //                }    
                  
        //       }
        //    catch (Exception)
        //    {

        //    }
        //}

        //private void timer4_Tick(object sender, EventArgs e)//检测鼠标位置自动弹出
        //{
        //    if (Control.MousePosition.X < 20 && hide_flag == true)
        //    {
        //        if (this.Location.Y < Control.MousePosition.Y && Control.MousePosition.Y < this.Location.Y + 300)
        //        {
        //            for (int i = 0; i < 360; i++)
        //            {
        //                this.Left = i - 360;
        //                System.Threading.Thread.Sleep(hide_speed);
        //            }
        //            this.textBox1.Focus();
        //            hide_flag = false;
        //        }
        //    }
        //}




        #region  鼠标移动隐藏仿QQ
        //if ((this.Location.X < 1) && (hide_flag == false) && (this.Location.Y > Control.MousePosition.Y || Control.MousePosition.Y > this.Location.Y + 70))
        //    {
        //        form_location_x = this.Location.X;

        //        for (int i = 0; i < this.Width + form_location_x; i++)
        //        {
        //            this.Left = form_location_x - i;
        //            System.Threading.Thread.Sleep(hide_speed);
        //        }
        //        hide_flag = true;
        //    }
        //    if(hide_flag==true)
        //    {
        //        this.Left = -360;
        //    }
        //    try
        //    {
        //        if(Control.MousePosition.X<1&&hide_flag==true)
        //            {
        //                if (this.Location.Y < Control.MousePosition.Y && Control.MousePosition.Y < this.Location.Y + 70&&hide_flag==true)
        //                {
        //                    for (int i = 0; i < 360; i++)
        //                    {
        //                        this.Left = i - 360;
        //                        System.Threading.Thread.Sleep(hide_speed);
        //                    }
        //                    hide_flag = false;
        //                }
        //            }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("");
        //    }
        #endregion
        #endregion


    }
}
