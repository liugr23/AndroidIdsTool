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
            this.opTextBox.AppendText("正在连接"+ip + " ...\n");
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = adbPath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "connect " + ip;
            p.Start();
            p.WaitForExit();
            string output = p.StandardOutput.ReadToEnd();
            this.opTextBox.AppendText(output + "\n");
        }

        //关闭连接
        private void disConnectClient(String ip)
        {
            this.opTextBox.AppendText("正在关闭连接" + ip + " ...\n");
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = adbPath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "disconnect " + ip;
            p.Start();
            p.WaitForExit();
            string output = p.StandardOutput.ReadToEnd();
            if (output != "")
            {
                this.opTextBox.AppendText(output + "\n");
            }
            else
            {
                this.opTextBox.AppendText("关闭连接" + ip + "\n");
            }            
        }

        //重启客户端
        private void rebootClient(String ip)
        {
            this.opTextBox.AppendText("正在重启" + ip + "...\n");
            this.connectClient(ip);        
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = adbPath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "-s "+ip+":5555 reboot";
            p.Start();
            //p.WaitForExit();
            //string output = p.StandardOutput.ReadToEnd();
            this.opTextBox.AppendText("重启完成。如果失败，请重试。\n");
        }

        //发送命令
        private void sendCmd(String cmd)
        {
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
            this.opTextBox.AppendText("正在导出日志...\n");
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = adbPath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "-s " + ip + ":5555 pull  /data/data/com.amtt.ids/files/log " + logDir;
            p.Start();
            p.WaitForExit();
            string output = p.StandardOutput.ReadToEnd();
            this.opTextBox.AppendText(output + "\n");
            this.opTextBox.AppendText("日志已保存在程序根目录下"+ip + "文件夹中\n");
            this.disConnectClient(ip);
        }

        //安装应用
        private void installApp(String ip,String appPath)
        {
            this.opTextBox.AppendText("正在安装应用" + ip + " ...\n");
            this.opTextBox.AppendText("如果长时间未反应，请手动安装。\n");
            this.opTextBox.AppendText("如果安装失败，请尝试卸载旧应用。\n");
            this.connectClient(ip);
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = adbPath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "-s " + ip + ":5555 install " + appPath;
            p.Start();
            p.WaitForExit();
            string output = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();
            this.opTextBox.AppendText(output + "\n");
            this.opTextBox.AppendText(error + "\n");
            this.disConnectClient(ip);
        }

        //卸载应用
        private void uninstallApp(String ip, String package)
        {
            this.opTextBox.AppendText("正在卸载旧应用" + package + " ...\n");
            this.connectClient(ip);
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = adbPath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "-s " + ip + ":5555 uninstall " + package;
            p.Start();
            p.WaitForExit();
            string output = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();
            this.opTextBox.AppendText(output + "\n");
            this.opTextBox.AppendText(error + "\n");
            this.disConnectClient(ip);
        }

        //启动应用
        private void startApp(String ip)
        {
            this.opTextBox.AppendText("正在启动应用...\n");
            this.connectClient(ip);
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = adbPath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "-s " + ip + ":5555 shell am start -n com.amtt.ids/com.amtt.ids.AppStart";
            p.Start();
            p.WaitForExit();
            string output = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();
            this.opTextBox.AppendText(output + "\n");
            this.opTextBox.AppendText(error + "\n");
            this.disConnectClient(ip);
        }

        //快速启动
        private void quickStartApp(String ip,String clientName,String serverIp){
            this.opTextBox.AppendText("正在快速启动应用...\n");
            this.connectClient(ip);
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = adbPath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "-s " + ip + ":5555 shell am start -n com.amtt.ids/com.amtt.ids.AdbStart --es clientName '" + clientName + "'" + " --es serverIp '" + serverIp + "'";
            p.Start();
            p.WaitForExit();
            string output = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();
            this.opTextBox.AppendText(output + "\n");
            this.opTextBox.AppendText(error + "\n");
            this.disConnectClient(ip);
        }


        //发送开发命令
        private void button14_Click(object sender, EventArgs e)
        {
            String cmd = this.cmdComboBox.SelectedValue.ToString();
            MessageBox.Show(cmd);
        }

        //连接开发机
        private void button5_Click_1(object sender, EventArgs e)
        {
            String ip = this.dpTextBox.Text;
            this.connectClient(ip);
        }

        //关闭开发机连接
        private void button10_Click(object sender, EventArgs e)
        {
            String ip = this.dpTextBox.Text;
            this.disConnectClient(ip);
            
        }

        //重启开发机
        private void button11_Click(object sender, EventArgs e)
        {
            String ip = this.dpTextBox.Text;
            this.rebootClient(ip);
        }

        // 导出日志
        private void button15_Click(object sender, EventArgs e)
        {
            String ip = this.dpTextBox.Text;
            this.exportLog(ip);
        }

        //测试
        private void button17_Click(object sender, EventArgs e)
        {
            this.opTextBox.AppendText("正在启动应用...\n");
            this.connectClient("192.168.15.176");
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = adbPath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "-s 192.168.15.176:5555 shell am start -n com.amtt.test8/com.amtt.test8.MainActivity --es str 'liugr'";
            p.Start();
            p.WaitForExit();
            string output = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();
            this.opTextBox.AppendText(output + "\n");
            this.opTextBox.AppendText(error + "\n");
        }

        //批量删除客户端
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认删除？", "此删除不可恢复", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            Configuration appConf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (ListViewItem item in clientListView.Items)
            {
                if(item.Checked){
                    String ip = item.SubItems[2].Text.ToString();
                    this.opTextBox.AppendText("从列表中删除"+ip+"\n");
                    appConf.AppSettings.Settings.Remove(ip);
                }
                
            }
            appConf.Save();
            ConfigurationManager.RefreshSection("appSettings");
            showList();
        }

        //批量安装客户端
        private void button8_Click(object sender, EventArgs e)
        {
            String appPath = this.apkTextBox.Text;
            if (appPath=="")
            {
                MessageBox.Show("请选择应用");
                return;
            }

            foreach (ListViewItem item in clientListView.Items)
            {
                if (item.Checked)
                {
                    String ip = item.SubItems[2].Text.ToString();
                    this.opTextBox.AppendText("=================="+ip+"==================\n\n");
                    this.installApp(ip,appPath);
                    this.opTextBox.AppendText("==================END==================\n\n\n");
                }

            }
        }

        //批量重启客户端
        private void button9_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in clientListView.Items)
            {
                if (item.Checked)
                {
                    String ip = item.SubItems[2].Text.ToString();
                    this.opTextBox.AppendText("==================" + ip + "==================\n\n");
                    this.rebootClient(ip);
                    this.opTextBox.AppendText("==================END=========================\n\n\n");
                }

            }
        }

        //批量导出日志
        private void button16_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in clientListView.Items)
            {
                if (item.Checked)
                {
                    String ip = item.SubItems[2].Text.ToString();
                    this.opTextBox.AppendText("==================" + ip + "==================\n\n");
                    this.exportLog(ip);
                    this.opTextBox.AppendText("==================END=========================\n\n\n");
                }

            }
        }

        //批量启动应用
        private void button19_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in clientListView.Items)
            {
                if (item.Checked)
                {
                    String ip = item.SubItems[2].Text.ToString();
                    this.opTextBox.AppendText("==================" + ip + "==================\n\n");
                    this.startApp(ip);
                    this.opTextBox.AppendText("==================END=========================\n\n\n");
                }

            }
        }

        //批量卸载应用
        private void button20_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in clientListView.Items)
            {
                if (item.Checked)
                {
                    String ip = item.SubItems[2].Text.ToString();
                    this.opTextBox.AppendText("==================" + ip + "==================\n\n");
                    this.uninstallApp(ip,"com.amtt.ids");
                    this.opTextBox.AppendText("==================END=========================\n\n\n");
                }

            }
        }

        //批量快速启动
        private void button21_Click(object sender, EventArgs e)
        {
            String serverIp = this.sIpTextBox.Text;
            if (serverIp == "")
            {
                MessageBox.Show("服务器IP有误！");
                return;
            }

            foreach (ListViewItem item in clientListView.Items)
            {
                if (item.Checked)
                {
                    String name = item.SubItems[1].Text.ToString();
                    String ip = item.SubItems[2].Text.ToString();
                    this.opTextBox.AppendText("==================" + ip + "==================\n\n");
                    this.quickStartApp(ip,name,serverIp);
                    this.opTextBox.AppendText("==================END=========================\n\n\n");
                }

            }
        }
    }
}
