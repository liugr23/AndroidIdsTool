using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace AdbGUI
{
    public partial class Form1 : Form
    {
        private Process process = null;
        private String adbPath = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(@"jason.txt"))
            {
                //开发者选项
                this.devGroupBox.Enabled = true;
                String txt = "";
                StreamReader sr = new StreamReader(@"jason.txt");
                while (!sr.EndOfStream)
                {
                    string str = sr.ReadLine();
                    txt += str;
                }
                sr.Close();
                if (txt != "")
                {
                    this.dpTextBox.Text = txt;
                }

                //命令列表
                ArrayList list = new ArrayList();
                list.Add(new DictionaryEntry("stopService", "关闭服务"));
                this.cmdComboBox.DataSource = list;
                this.cmdComboBox.DisplayMember = "Value";//显示的Text值
                this.cmdComboBox.ValueMember = "Key";// 实际value值
            }
            else
            {
                this.devGroupBox.Enabled = false;
            }

            // 必要文件
            if (!System.IO.File.Exists(System.Environment.CurrentDirectory + @"\adb.exe") || !System.IO.File.Exists(System.Environment.CurrentDirectory + @"\AdbWinApi.dll"))
            {
                this.opTextBox.AppendText("错误:adb.exe或AdbWinApi.dll不存在\n");
                this.opTextBox.AppendText("联系jason.liu@amttgroup.com\n");
                this.devGroupBox.Enabled = false;
                this.testGroupBox.Enabled = false;
                this.clientGroupBox.Enabled = false;

            }

            adbPath = System.Environment.CurrentDirectory + @"\adb.exe";
            this.opTextBox.AppendText(adbPath);

            //参看根目录下是否有apk
            String apk = System.Environment.CurrentDirectory + @"\Coon.apk";
            if(File.Exists(apk)){
                this.apkTextBox.Text = apk;
            }

            this.showList();
        }

        private Process getProcess()
        {

            process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            return process;
        }

        //保存数据
        private void addClient(String ip,String name)
        {
            Configuration appConf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            appConf.AppSettings.Settings.Remove(ip);
            appConf.AppSettings.Settings.Add(ip, name);
            appConf.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        //显示列表
        private void showList()
        {
            clientListView.Items.Clear();
            foreach (string key in ConfigurationManager.AppSettings)
            {
                string val = ConfigurationManager.AppSettings[key];
                Console.WriteLine("{0}: {1}", key, val);
                ListViewItem item = new ListViewItem("");
                item.SubItems.Add(val);
                item.SubItems.Add(key);
                clientListView.Items.Add(item);
            }
        }

        //选择APK
        private void button3_Click(object sender, EventArgs e)
        {
            //选择文件
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            //文件格式
            openFileDialog.Filter = "|*.apk";
            //默认的文件格式
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;
                this.apkTextBox.Text = path;

            }
        }

        //添加客户端
        private void button1_Click(object sender, EventArgs e)
        {
            String name = this.nameTextBox.Text;
            String ip = this.ipTextBox.Text;
            if (name == "" || ip == "")
            {
                MessageBox.Show("输入有误！");
                return;
            }
            this.addClient(ip, name);
            this.showList();  
        }
        //批量添加客户端
        private void button4_Click(object sender, EventArgs e)
        {
            String p1 = this.p1TextBox.Text;
            String p2 = this.p2TextBox.Text;
            String p3 = this.p3TextBox.Text;
            String p4 = this.p4TextBox.Text;
            String p5 = this.p5TextBox.Text;

            if (p1 == "" || p2 == "" || p3 == "" || p4 == "" || p5 == "")
            {
                MessageBox.Show("输入有误！");
                return;
            }
            int start = 0;
            int end = 0;
            try
            {
                start = int.Parse(p4);
                end = int.Parse(p5);
            } catch(Exception){
                MessageBox.Show("输入有误！");
                return;
            }

            for (int i = start; i < end;i++ )
            {
                String name = "c_" + i;
                String ip = p1 + "." + p2 + "." + p3 + "." + i;
                this.addClient(ip,name);
            }
            this.showList();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            clientListView.Items.Clear();
        }
        //全选
        private void button6_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in clientListView.Items)
            {
                item.Checked = true;
            }
        }
        //反选
        private void button7_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in clientListView.Items)
            {
                item.Checked = !item.Checked;
            }
        }

        //选择背景图片
        private void button12_Click(object sender, EventArgs e)
        {
            //选择文件
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            //文件格式
            openFileDialog.Filter = "|*.jpg";
            //默认的文件格式
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;
                this.imageTextBox.Text = path;

            }
        }
        //选择音乐
        private void button13_Click(object sender, EventArgs e)
        {
            //选择文件
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            //文件格式
            openFileDialog.Filter = "|*.mp3";
            //默认的文件格式
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;
                this.soundTextBox.Text = path;

            }

        }

        //连接客户端
        private void connectClient(String ip)
        {
            Process p = this.getProcess();
            p.Start();
            p.StandardInput.AutoFlush = true;
            p.StandardInput.WriteLine(adbPath + " connect " + ip);
            p.StandardInput.WriteLine("exit");
            string report = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();
            this.opTextBox.AppendText(report);
        }

        //关闭连接
        private void disConnectClient(String ip)
        {
            Process p = this.getProcess();
            p.Start();
            p.StandardInput.AutoFlush = true;
            p.StandardInput.WriteLine(adbPath + " disconnect " + ip);
            p.StandardInput.WriteLine("exit");
            string report = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();
            this.opTextBox.AppendText(report);
        }

        //重启客户端
        private void rebootClient(String ip)
        {
            this.connectClient(ip);
            Process p = this.getProcess();
            p.Start();
            p.StandardInput.AutoFlush = true;
            p.StandardInput.WriteLine(adbPath + " -s " + ip + ":5555 shell reboot" + "\n");
            p.StandardInput.WriteLine("exit");
            //p.WaitForExit();
            p.Close();
        }

        //导出日志
        private void exportLog(String ip)
        {
            String logDir = System.Environment.CurrentDirectory + @"\" + ip;
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            this.connectClient(ip);
            this.opTextBox.AppendText("正在导出日志" + "\n");
            Process p = this.getProcess();
            p.Start();
            p.StandardInput.AutoFlush = true;
            p.StandardInput.WriteLine(adbPath + " -s " + ip + @":5555 pull  /data/data/com.amtt.ids/files/log " + logDir + "\n");
            p.StandardInput.WriteLine("exit");
            string report = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();
            this.opTextBox.AppendText(report + "\n");
            this.opTextBox.AppendText("日志已保存在程序根目录下"+ip + "文件夹中\n");

        }

        //发送命令
        private void button14_Click(object sender, EventArgs e)
        {
            String cmd = this.cmdComboBox.SelectedValue.ToString();
            MessageBox.Show(cmd);
        }

        //adb connect
        private void button5_Click_1(object sender, EventArgs e)
        {
            String ip = this.dpTextBox.Text;
            this.connectClient(ip);
        }

        //adb disconnect
        private void button10_Click(object sender, EventArgs e)
        {
            String ip = this.dpTextBox.Text;
            this.disConnectClient(ip);
            
        }

        //开发 重启
        private void button11_Click(object sender, EventArgs e)
        {
            this.opTextBox.AppendText("重启中\n");
            String ip = this.dpTextBox.Text;
            this.rebootClient(ip);
            this.rebootClient(ip);
        }

        // 导出日志
        private void button15_Click(object sender, EventArgs e)
        {
            String ip = this.dpTextBox.Text;
            this.exportLog(ip);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string fileName = adbPath;
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = fileName;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "-s 192.168.15.176:5555 reboot";//参数以空格分隔，如果某个参数为空，可以传入””
            p.Start();
            p.WaitForExit();
            string output = p.StandardOutput.ReadToEnd();
            this.opTextBox.AppendText(output);
        }
    }
}
