using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RobotControl
{
    public partial class Form1 : Form
    {
        private const string V = "T100\r";
        private int value_temp; //避免重复调用
        private DateTime current_time = new DateTime();     //避免重复调用
        private bool time_flag = false; //避免重复调用
        private int time_temp = 0;
        private bool init_flag = true;
        private bool download_flag = false;
        private bool finish_flag = false; //避免重复调用
        private int Num = 0;   //避免重复调用
        private StringBuilder sb = new StringBuilder();    //为了避免在接收处理函数中反复调用，依然声明为一个全局变量
        private String data_receive = "";

        private int cnt = 0;       //发送计数变量
        private int cnt2 = 0;       //发送计数变量
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i;
            //单个添加
            for (i = 300; i <= 38400; i = i * 2)
            {
                comboBox2.Items.Add(i.ToString());  //添加波特率列表
            }

            //批量添加波特率列表
            string[] baud = { "43000", "56000", "57600", "115200", "128000", "230400", "256000", "460800" };
            comboBox2.Items.AddRange(baud);

            //获取电脑当前可用串口并添加到选项列表中
            comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());

            //设置选项默认值
            comboBox2.Text = "115200";
        }

        private void button1_Click(object sender, EventArgs e)
        {
           try
            {
                //将可能产生异常的代码放置在try块中
                //根据当前串口属性来判断是否打开
                if (serialPort1.IsOpen)
                {
                    //串口已经处于打开状态
                    serialPort1.Close();    //关闭串口
                    button1.Text = "打开串口";
                    button1.BackColor = Color.ForestGreen;
                    label17.ForeColor = Color.Red;
                    label17.Text = "串口已关闭！";

                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    button3.Enabled = false;
                    button6.Enabled = false;

                    trackBar1.Enabled = false;
                    trackBar2.Enabled = false;
                    trackBar3.Enabled = false;
                    trackBar4.Enabled = false;
                    trackBar5.Enabled = false;
                    trackBar6.Enabled = false;
                    trackBar7.Enabled = false;
                    trackBar8.Enabled = false;
                    trackBar9.Enabled = false;
                    trackBar10.Enabled = false;
                    trackBar11.Enabled = false;
                    trackBar12.Enabled = false;
                }
                else
                {
                    //串口已经处于关闭状态，则设置好串口属性后打开
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                    serialPort1.Open();     //打开串口
                    button1.Text = "关闭串口";
                    button1.BackColor = Color.Firebrick;
                    label17.ForeColor = Color.Green;
                    label17.Text = "串口已打开！";
                    current_time = System.DateTime.Now;     //获取当前时间
                    serialPort1.WriteLine("#Veri+"+current_time.ToString("yyyyMMddHHmm")+" \r");

                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    button3.Enabled = true;
                    button6.Enabled = true;

                    trackBar1.Enabled =  true;
                    trackBar2.Enabled =  true;
                    trackBar3.Enabled =  true;
                    trackBar4.Enabled =  true;
                    trackBar5.Enabled =  true;
                    trackBar6.Enabled =  true;
                    trackBar7.Enabled =  true;
                    trackBar8.Enabled =  true;
                    trackBar9.Enabled =  true;
                    trackBar10.Enabled = true;
                    trackBar11.Enabled = true;
                    trackBar12.Enabled = true;

                }
            }
            catch (Exception ex)
            {
                //捕获可能发生的异常并进行处理
                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                button1.Text = "打开串口";
                button1.BackColor = Color.ForestGreen;
                label17.ForeColor = Color.Red;
                label17.Text = "串口已关闭！";
                MessageBox.Show(ex.Message);

                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                button3.Enabled = false;
                button6.Enabled = false;

                trackBar1.Enabled = false;
                trackBar2.Enabled = false;
                trackBar3.Enabled = false;
                trackBar4.Enabled = false;
                trackBar5.Enabled = false;
                trackBar6.Enabled = false;
                trackBar7.Enabled = false;
                trackBar8.Enabled = false;
                trackBar9.Enabled = false;
                trackBar10.Enabled = false;
                trackBar11.Enabled = false;
                trackBar12.Enabled = false;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //servo1
            textBox1.Text = trackBar1.Value.ToString();
            value_temp = 1500 + trackBar1.Value;
            serialPort1.WriteLine("#1P"+value_temp.ToString()+ V);
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            //servo3
            textBox2.Text = trackBar2.Value.ToString();
            value_temp = 1500 + trackBar2.Value;
            serialPort1.WriteLine("#3P" + value_temp.ToString()+ V);
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            //servo5
            textBox3.Text = trackBar3.Value.ToString();
            value_temp = 1500 + trackBar3.Value;
            serialPort1.WriteLine("#5P" + value_temp.ToString()+ V);
        }
        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            //servo7
            textBox4.Text = trackBar4.Value.ToString();
            value_temp = 1500 + trackBar4.Value;
            serialPort1.WriteLine("#7P" + value_temp.ToString()+ V);
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            //servo9
            textBox5.Text = trackBar5.Value.ToString();
            value_temp = 1500 + trackBar5.Value;
            serialPort1.WriteLine("#9P" + value_temp.ToString()+ V);
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            //servo2
            textBox6.Text = trackBar6.Value.ToString();
            value_temp = 1500 + trackBar6.Value;
            serialPort1.WriteLine("#2P" + value_temp.ToString()+ V);
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            //servo4
            textBox7.Text = trackBar7.Value.ToString();
            value_temp = 1500 + trackBar7.Value;
            serialPort1.WriteLine("#4P" + value_temp.ToString()+ V);
        }

        private void trackBar8_Scroll(object sender, EventArgs e)
        {
            //servo6
            textBox8.Text = trackBar8.Value.ToString();
            value_temp = 1500 + trackBar8.Value;
            serialPort1.WriteLine("#6P" + value_temp.ToString()+ V);
        }

        private void trackBar9_Scroll(object sender, EventArgs e)
        {
            //servo8
            textBox9.Text = trackBar9.Value.ToString();
            value_temp = 1500 + trackBar9.Value;
            serialPort1.WriteLine("#8P" + value_temp.ToString()+ V);
        }

        private void trackBar10_Scroll(object sender, EventArgs e)
        {
            //servo10
            textBox10.Text = trackBar10.Value.ToString();
            value_temp = 1500 + trackBar10.Value;
            serialPort1.WriteLine("#10P" + value_temp.ToString()+ V);
        }

        private void trackBar11_Scroll(object sender, EventArgs e)
        {
            //servo12
            textBox11.Text = trackBar11.Value.ToString();
            value_temp = 1500 + trackBar11.Value;
            serialPort1.WriteLine("#12P" + value_temp.ToString()+ V);
        }

        private void trackBar12_Scroll(object sender, EventArgs e)
        {
            //servo11
            textBox12.Text = trackBar12.Value.ToString();
            value_temp = 1500 + trackBar12.Value;
            serialPort1.WriteLine("#11P" + value_temp.ToString()+ V);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Num++;  //列表项计数变量自加1
            ListViewItem liv_temp = new ListViewItem(Num.ToString("D2"));     //列表项变量，用于存放值
            String value_temp = "#1P" + (1500 + trackBar1.Value).ToString() 
                              + "#2P" + (1500 + trackBar6.Value).ToString()
                              + "#3P" + (1500 + trackBar2.Value).ToString()
                              + "#4P" + (1500 + trackBar7.Value).ToString()
                              + "#5P" + (1500 + trackBar3.Value).ToString()
                              + "#6P" + (1500 + trackBar8.Value).ToString()
                              + "#7P" + (1500 + trackBar4.Value).ToString()
                              + "#8P" + (1500 + trackBar9.Value).ToString()
                              + "#9P" + (1500 + trackBar5.Value).ToString()
                              + "#10P" + (1500 + trackBar10.Value).ToString()
                              + "#11P" + (1500 + trackBar12.Value).ToString()
                              + "#12P" + (1500 + trackBar11.Value).ToString()
                              + "T1000";
            liv_temp.SubItems.Add(value_temp);
            listView1.Items.Add(liv_temp);
            liv_temp.Selected = true;   //必须要
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Interval = (int)numericUpDown1.Value;    //获取定时值
            timer1.Start();                                 //启动定时器
            label17.ForeColor = Color.Red;
            label17.Text = "动作运行中...";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (cnt >= listView1.Items.Count)
            {
                cnt = 0;
                //发送完成
                if (radioButton2.Checked)
                {
                    //循环运行模式,无动作
                }
                else
                {
                    //单次运行模式
                    timer1.Stop();      //关闭定时器
                    label17.ForeColor = Color.Green;
                    label17.Text = "动作运行完成";
                    System.Media.SystemSounds.Beep.Play(); //响铃提示用户
                    MessageBox.Show("运行完成");
                }
                
            }
            else
            {
                try
                {
                    label17.ForeColor = Color.Red;
                    label17.Text = "动作" + cnt.ToString() +"运行中...";
                    serialPort1.WriteLine(listView1.SelectedItems[cnt].SubItems[1].Text + '\r');     //串口发送
                    cnt++;
                    timer1.Interval = (int)numericUpDown1.Value;    //获取定时值
                    timer1.Start();                                 //启动定时器
                }
                catch (Exception ex)
                {
                    serialPort1.Close();
                    //捕获到异常，创建一个新的对象，之前的不可以再用
                    serialPort1 = new System.IO.Ports.SerialPort();
                    //刷新COM口选项
                    comboBox1.Items.Clear();
                    comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                    //响铃并显示异常给用户
                    System.Media.SystemSounds.Beep.Play();
                    button1.Text = "打开串口";
                    button1.BackColor = Color.ForestGreen;
                    label17.ForeColor = Color.Red;
                    label17.Text = "串口已关闭！";
                    MessageBox.Show(ex.Message);
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    button3.Enabled = false;
                }
               
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cnt = 0;
            cnt = 0;
            timer1.Stop();                          //关闭定时器
            label17.ForeColor = Color.Red;
            label17.Text = "动作停止运行！";
            System.Media.SystemSounds.Beep.Play(); //响铃提示用户
            MessageBox.Show("停止运行");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Num--;  //列表项计数变量自减1
            listView1.Items.Remove(listView1.Items[listView1.Items.Count-1]);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (init_flag)
            {
                //进入下载初始化模式
                init_flag = false;
                label17.ForeColor = Color.Red;
                label17.Text = "动作下载中，请勿操作！！！";
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show("下载动作时请勿操作，确认后开始下载！");
                serialPort1.WriteLine("#Down" + "\r");      //发送下载命令
                time_temp = 0;
                timer2.Interval = 1000;
                timer2.Start();
            }
            else if (time_flag)
            {
                //30s时间到，超时
                time_flag = false;
                time_temp = 0;
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show("下位机未回应，下载超时！！！");
                timer2.Stop();
                label17.ForeColor = Color.Red;
                label17.Text = "动作下载失败,请重试！";
                init_flag = true;

            }
            else if (data_receive.Equals("A"))
            {
                //在30s之内接收到下位机发送的字符'A'，开始下载
                timer2.Stop();  //暂停定时器
                data_receive = "";
                cnt2 = listView1.Items.Count;
                progressBar1.Maximum = cnt2;
                for (int i = 0; i < cnt2; i++)
                {
                    //接收到'A'，开始发送数据
                    serialPort1.WriteLine(listView1.SelectedItems[i].SubItems[1].Text + '\r'); //串口发送数据
                    progressBar1.Value = i + 1;
                }
                download_flag = true;
                //等待1000ms
                timer2.Interval = 500;
                time_temp = 0;
                timer2.Start(); 
            }
            else if (download_flag)
            {
                download_flag = false;
               //1000ms时间到
                serialPort1.WriteLine("#Stop" + "\r");      //发送下载完成命令
            }
            else if(!finish_flag)
            {
                if (data_receive.Length >= 8)
                    if (data_receive.Substring(0, 8).Equals("#Down+OK"))
                    {
                        timer2.Stop();
                        finish_flag = true;
                        serialPort1.WriteLine("#Flist" + "\r");      //发送下载命令
                        label17.ForeColor = Color.Green;
                        label17.Text = "动作下载完成";
                        System.Media.SystemSounds.Beep.Play(); //响铃提示用户
                        MessageBox.Show("动作下载成功!");
                        progressBar1.Value = 0;
                        init_flag = true;
                    }
                        
            }
           
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Interval = 1000;
            timer2.Start();
            //每1000ms进一次中断,检测是否到时间
            time_temp++;
            if (time_temp == 30)
            {
                //30s时间到
                time_flag = true;
                timer2.Stop();      //关闭定时器
            }
            button6_Click(button6,new EventArgs()); //调用button6回调函数
        }

        //接收数据处理
        private void SerialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int num = serialPort1.BytesToRead;      //获取接收缓冲区中的字节数
            byte[] received_buf = new byte[num];    //声明一个大小为num的字节数据用于存放读出的byte型数据
            serialPort1.Read(received_buf, 0, num);   //读取接收缓冲区中num个字节到byte数组中

            sb.Clear();     //防止出错,首先清空字符串构造器
            sb.Append(Encoding.ASCII.GetString(received_buf));  //将整个数组解码为ASCII数组
            data_receive = sb.ToString();
            try
            {
            }
            catch (Exception ex)
            {
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(ex.Message);

            }
        }


        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //servo1 
            if (e.KeyCode == Keys.Enter)
            {
                //正则表达式判断输入是否为数字
                if (!Regex.IsMatch(textBox1.Text.ToString(), @"^-?[1-9]\d*$|^0$"))
                {
                    MessageBox.Show("请输入数字");
                    textBox1.Text = "";
                    return;
                }
                //判断文本是否是整数且在-1000到1000之内
                else if (Convert.ToInt32(textBox1.Text) > 1000 || Convert.ToInt32(textBox1.Text) < -1000)
                {
                    MessageBox.Show("请输入-1000到1000之内的值");
                    textBox1.Text = "";
                    return;
                }
                else
                {
                    trackBar1.Value = Convert.ToInt32(textBox1.Text);
                    value_temp = 1500 + trackBar1.Value;
                    serialPort1.WriteLine("#1P" + value_temp.ToString() + V);

                }
            }
                
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            //servo3
            if (e.KeyCode == Keys.Enter)
            {
                //正则表达式判断输入是否为数字
                if (!Regex.IsMatch(textBox2.Text.ToString(), @"^-?[1-9]\d*$|^0$"))
                {
                    MessageBox.Show("请输入数字");
                    textBox2.Text = "";
                    return;
                }
                //判断文本是否是整数且在-1000到1000之内
                else if (Convert.ToInt32(textBox2.Text) > 1000 || Convert.ToInt32(textBox2.Text) < -1000)
                {
                    MessageBox.Show("请输入-1000到1000之内的值");
                    textBox2.Text = "";
                    return;
                }
                else
                {
                    trackBar2.Value = Convert.ToInt32(textBox2.Text);
                    value_temp = 1500 + trackBar2.Value;
                    serialPort1.WriteLine("#3P" + value_temp.ToString() + V);
                }
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            //servo5
            if (e.KeyCode == Keys.Enter)
            {
                //正则表达式判断输入是否为数字
                if (!Regex.IsMatch(textBox3.Text.ToString(), @"^-?[1-9]\d*$|^0$"))
                {
                    MessageBox.Show("请输入数字");
                    textBox3.Text = "";
                    return;
                }
                //判断文本是否是整数且在-1000到1000之内
                else if (Convert.ToInt32(textBox3.Text) > 1000 || Convert.ToInt32(textBox3.Text) < -1000)
                {
                    MessageBox.Show("请输入-1000到1000之内的值");
                    textBox3.Text = "";
                    return;
                }
                else
                {
                    trackBar3.Value = Convert.ToInt32(textBox3.Text);
                    value_temp = 1500 + trackBar3.Value;
                    serialPort1.WriteLine("#5P" + value_temp.ToString() + V);
                }

            }
                
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            //servo9
            if (e.KeyCode == Keys.Enter)
            {
                //正则表达式判断输入是否为数字
                if (!Regex.IsMatch(textBox4.Text.ToString(), @"^-?[1-9]\d*$|^0$"))
                {
                    MessageBox.Show("请输入数字");
                    textBox4.Text = "";
                    return;
                }
                //判断文本是否是整数且在-1000到1000之内
                else if (Convert.ToInt32(textBox4.Text) > 1000 || Convert.ToInt32(textBox4.Text) < -1000)
                {
                    MessageBox.Show("请输入-1000到1000之内的值");
                    textBox4.Text = "";
                    return;
                }
                else
                {
                    trackBar4.Value = Convert.ToInt32(textBox4.Text);
                    value_temp = 1500 + trackBar4.Value;
                    serialPort1.WriteLine("#9P" + value_temp.ToString() + V);
                }

            }
               
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            //servo9
            if (e.KeyCode == Keys.Enter)
            {
                 //正则表达式判断输入是否为数字
                if (!Regex.IsMatch(textBox5.Text.ToString(), @"^-?[1-9]\d*$|^0$"))
                {
                    MessageBox.Show("请输入数字");
                    textBox5.Text = "";
                    return;
                }
                //判断文本是否是整数且在-1000到1000之内
                else if (Convert.ToInt32(textBox5.Text) > 1000 || Convert.ToInt32(textBox5.Text) < -1000)
                {
                    MessageBox.Show("请输入-1000到1000之内的值");
                    textBox5.Text = "";
                    return;
                }
                else
                {
                    trackBar5.Value = Convert.ToInt32(textBox5.Text);
                    value_temp = 1500 + trackBar5.Value;
                    serialPort1.WriteLine("#9P" + value_temp.ToString() + V);
                }

            }
            
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            //servo2
            if (e.KeyCode == Keys.Enter)
            {
                //正则表达式判断输入是否为数字
                if (!Regex.IsMatch(textBox6.Text.ToString(), @"^-?[1-9]\d*$|^0$"))
                {
                    MessageBox.Show("请输入数字");
                    textBox6.Text = "";
                    return;
                }
                //判断文本是否是整数且在-1000到1000之内
                else if (Convert.ToInt32(textBox6.Text) > 1000 || Convert.ToInt32(textBox6.Text) < -1000)
                {
                    MessageBox.Show("请输入-1000到1000之内的值");
                    textBox6.Text = "";
                    return;
                }
                else
                {
                    trackBar6.Value = Convert.ToInt32(textBox6.Text);
                    value_temp = 1500 + trackBar6.Value;
                    serialPort1.WriteLine("#2P" + value_temp.ToString() + V);
                }
            }
               
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            //servo4
            if (e.KeyCode == Keys.Enter)
            {
                //正则表达式判断输入是否为数字
                if (!Regex.IsMatch(textBox7.Text.ToString(), @"^-?[1-9]\d*$|^0$"))
                {
                    MessageBox.Show("请输入数字");
                    textBox7.Text = "";
                    return;
                }
                //判断文本是否是整数且在-1000到1000之内
                else if (Convert.ToInt32(textBox7.Text) > 1000 || Convert.ToInt32(textBox7.Text) < -1000)
                {
                    MessageBox.Show("请输入-1000到1000之内的值");
                    textBox7.Text = "";
                    return;
                }
                else
                {
                    trackBar7.Value = Convert.ToInt32(textBox7.Text);
                    value_temp = 1500 + trackBar7.Value;
                    serialPort1.WriteLine("#4P" + value_temp.ToString() + V);
                }

            }
                
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            //servo6
            if (e.KeyCode == Keys.Enter)
            {
                //正则表达式判断输入是否为数字
                if (!Regex.IsMatch(textBox8.Text.ToString(), @"^-?[1-9]\d*$|^0$"))
                {
                    MessageBox.Show("请输入数字");
                    textBox8.Text = "";
                    return;
                }
                //判断文本是否是整数且在-1000到1000之内
                else if (Convert.ToInt32(textBox8.Text) > 1000 || Convert.ToInt32(textBox8.Text) < -1000)
                {
                    MessageBox.Show("请输入-1000到1000之内的值");
                    textBox8.Text = "";
                    return;
                }
                else
                {
                    trackBar8.Value = Convert.ToInt32(textBox8.Text);
                    value_temp = 1500 + trackBar8.Value;
                    serialPort1.WriteLine("#6P" + value_temp.ToString() + V);
                }

            }
               
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            //servo8
            if (e.KeyCode == Keys.Enter)
            {
                //正则表达式判断输入是否为数字
                if (!Regex.IsMatch(textBox9.Text.ToString(), @"^-?[1-9]\d*$|^0$"))
                {
                    MessageBox.Show("请输入数字");
                    textBox9.Text = "";
                    return;
                }
                //判断文本是否是整数且在-1000到1000之内
                else if (Convert.ToInt32(textBox9.Text) > 1000 || Convert.ToInt32(textBox9.Text) < -1000)
                {
                    MessageBox.Show("请输入-1000到1000之内的值");
                    textBox9.Text = "";
                    return;
                }
                else
                {
                    trackBar9.Value = Convert.ToInt32(textBox9.Text);
                    value_temp = 1500 + trackBar9.Value;
                    serialPort1.WriteLine("#8P" + value_temp.ToString() + V);
                }
            }
                
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            //servo10
            if (e.KeyCode == Keys.Enter)
            {
                //正则表达式判断输入是否为数字
                if (!Regex.IsMatch(textBox10.Text.ToString(), @"^-?[1-9]\d*$|^0$"))
                {
                    MessageBox.Show("请输入数字");
                    textBox10.Text = "";
                    return;
                }
                //判断文本是否是整数且在-1000到1000之内
                else if (Convert.ToInt32(textBox10.Text) > 1000 || Convert.ToInt32(textBox10.Text) < -1000)
                {
                    MessageBox.Show("请输入-1000到1000之内的值");
                    textBox10.Text = "";
                    return;
                }
                else
                {
                    trackBar10.Value = Convert.ToInt32(textBox10.Text);
                    value_temp = 1500 + trackBar10.Value;
                    serialPort1.WriteLine("#10P" + value_temp.ToString() + V);
                }

            }
                
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            //servo12
            if (e.KeyCode == Keys.Enter)
            {
                //正则表达式判断输入是否为数字
                if (!Regex.IsMatch(textBox11.Text.ToString(), @"^-?[1-9]\d*$|^0$"))
                {
                    MessageBox.Show("请输入数字");
                    textBox11.Text = "";
                    return;
                }
                //判断文本是否是整数且在-1000到1000之内
                else if (Convert.ToInt32(textBox11.Text) > 1000 || Convert.ToInt32(textBox11.Text) < -1000)
                {
                    MessageBox.Show("请输入-1000到1000之内的值");
                    textBox11.Text = "";
                    return;
                }
                else
                {
                    trackBar11.Value = Convert.ToInt32(textBox11.Text);
                    value_temp = 1500 + trackBar11.Value;
                    serialPort1.WriteLine("#12P" + value_temp.ToString() + V);
                }
            }
                
        }
        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            //servo11
            if (e.KeyCode == Keys.Enter)
            {
                //正则表达式判断输入是否为数字
                if (!Regex.IsMatch(textBox12.Text.ToString(), @"^-?[1-9]\d*$|^0$"))
                {
                    MessageBox.Show("请输入数字");
                    textBox12.Text = "";
                    return;
                }
                //判断文本是否是整数且在-1000到1000之内
                else if (Convert.ToInt32(textBox12.Text) > 1000 || Convert.ToInt32(textBox12.Text) < -1000)
                {
                    MessageBox.Show("请输入-1000到1000之内的值");
                    textBox12.Text = "";
                    return;
                }
                else
                {
                    trackBar12.Value = Convert.ToInt32(textBox12.Text);
                    value_temp = 1500 + trackBar12.Value;
                    serialPort1.WriteLine("#11P" + value_temp.ToString() + V);
                }
            }
               
        }
    }
}
