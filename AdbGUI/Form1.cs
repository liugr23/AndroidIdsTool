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
using System.Threading;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace AdbGUI
{
    public partial class Form1 : Form
    {
        private String adbPath = "";
        private String devIp = "jason.liu";
        //超时
        private const int timeout = 15000;
        //终止当前操作
        private bool end = false;
        //选中的客户端列表
        private ArrayList checkedClientList = new ArrayList();
        //当前操作的Ip
        private String currentIp = "";
        //持续进程
        private Process sustainedProcess = null;
        //短暂进程
        private Process process = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // 必要文件
            if (!System.IO.File.Exists(System.Environment.CurrentDirectory + @"\adb.exe") || !System.IO.File.Exists(System.Environment.CurrentDirectory + @"\AdbWinApi.dll"))
            {
                this.opTextBox.AppendText("错误:adb.exe或AdbWinApi.dll不存在\n");
                this.opTextBox.AppendText("联系jason.liu@amttgroup.com\n");
                this.operateTabControl.Enabled = false;
            }
            adbPath = System.Environment.CurrentDirectory + @"\adb.exe";

            //显示列表
            this.showList();

            if (System.IO.File.Exists(@"jason.txt"))
            {
                adbPath = @"adb.exe";
                //开发者选项
                this.devPanel.Enabled = true;
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
                    this.devIp = txt;
                    foreach (ListViewItem item in clientListView.Items)
                    {
                        String ip = item.SubItems[2].Text.ToString();
                        if (ip == txt)
                        {
                            item.Checked = true;
                            break;
                        }
                    }
                }

                //命令列表
                ArrayList list = new ArrayList();
                list.Add(new DictionaryEntry("debug", "Debug"));
                list.Add(new DictionaryEntry("screenshot", "截图"));
                list.Add(new DictionaryEntry("restartApp", "重启应用"));
                list.Add(new DictionaryEntry("reboot", "重启"));
                list.Add(new DictionaryEntry("stopService", "关闭服务"));
                list.Add(new DictionaryEntry("startProtect", "开启任务保护"));
                list.Add(new DictionaryEntry("stopProtect", "关闭任务保护"));
                list.Add(new DictionaryEntry("startStartup", "开启开机启动"));
                list.Add(new DictionaryEntry("stopStartup", "关闭开机启动"));
                list.Add(new DictionaryEntry("restartTask", "重启任务"));
                list.Add(new DictionaryEntry("resetData", "重置客户端任务数据"));
                list.Add(new DictionaryEntry("resetApp", "重置客户端配置文件*"));
                this.cmdComboBox.DataSource = list;
                this.cmdComboBox.DisplayMember = "Value";//显示的Text值
                this.cmdComboBox.ValueMember = "Key";// 实际value值

                //切换效果列表
                ArrayList effectList = new ArrayList();
                effectList.Add(new DictionaryEntry("e0", "无"));
                effectList.Add(new DictionaryEntry("e1001", "立方体1"));
                effectList.Add(new DictionaryEntry("e1002", "擦除-上-下2*"));
                effectList.Add(new DictionaryEntry("e1003", "旋转4*"));
                effectList.Add(new DictionaryEntry("e1004", "擦除-左-右7*"));
                effectList.Add(new DictionaryEntry("e1005", "切出11*"));
                effectList.Add(new DictionaryEntry("e1006", "翻页15"));
                effectList.Add(new DictionaryEntry("e1007", "淡出17*"));
                effectList.Add(new DictionaryEntry("e1008", "淡入21*"));
                effectList.Add(new DictionaryEntry("-1", "===分割线(不要选我)==="));
                effectList.Add(new DictionaryEntry("e2001", "星形0"));
                effectList.Add(new DictionaryEntry("e2002", "溶解10"));
                effectList.Add(new DictionaryEntry("e2003", "菱形5"));
                effectList.Add(new DictionaryEntry("e2004", "左右百叶窗6"));
                effectList.Add(new DictionaryEntry("e2005", "翻页3"));
                effectList.Add(new DictionaryEntry("e2006", "分割13"));
                effectList.Add(new DictionaryEntry("e2007", "上下百叶窗20"));
                effectList.Add(new DictionaryEntry("e2008", "溶解16"));
                effectList.Add(new DictionaryEntry("e2009", "擦除--左上--右下14"));
                effectList.Add(new DictionaryEntry("e2010", "收缩18"));
                effectList.Add(new DictionaryEntry("-1", "===分割线(不要选我)==="));
                effectList.Add(new DictionaryEntry("e2011", "超级淡入21"));
                effectList.Add(new DictionaryEntry("e2012", "超级淡出17"));
                effectList.Add(new DictionaryEntry("e2013", "超级切出11"));
                effectList.Add(new DictionaryEntry("e2014", "超级旋转4"));
                effectList.Add(new DictionaryEntry("e2015", "超级擦除-左-右7"));
                effectList.Add(new DictionaryEntry("e2016", "超级擦除-上-下2"));

                this.effectComboBox.DataSource = effectList;
                this.effectComboBox.DisplayMember = "Value";//显示的Text值
                this.effectComboBox.ValueMember = "Key";// 实际value值
            }
            else
            {
                this.devPanel.Enabled = false;
            }

            //参看根目录下是否有apk
            String apkPath = System.Environment.CurrentDirectory + @"\Coon.apk";
            if (File.Exists(apkPath))
            {
                this.apkTextBox.Text = apkPath;
            }
        }

        //保存数据
        private void addClient(String ip, String name)
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
            int i = 0;
            foreach (string key in ConfigurationManager.AppSettings)
            {
                i++;
                string val = ConfigurationManager.AppSettings[key];
                Console.WriteLine("{0}: {1}", key, val);
                ListViewItem item = new ListViewItem("");
                item.SubItems.Add(val);
                item.SubItems.Add(key);
                if(i%2 == 0){
                    item.BackColor = Color.AliceBlue;
                }        
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
            }
            catch (Exception)
            {
                MessageBox.Show("输入有误！");
                return;
            }

            for (int i = start; i < end; i++)
            {
                String name = "c_" + i;
                String ip = p1 + "." + p2 + "." + p3 + "." + i;
                this.addClient(ip, name);
            }
            this.showList();

        }

        //清空
        private void button24_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in clientListView.Items)
            {
                item.Checked = false;
            }
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
            currentIp = ip;
            this.updateOutput("正在连接" + ip + " ...\n");

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            //psi.RedirectStandardError = true;
            psi.FileName = adbPath;
            psi.CreateNoWindow = true;
            psi.Arguments = "connect " + ip;

            sustainedProcess = new Process();
            sustainedProcess.StartInfo = psi;
            sustainedProcess.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            //sustainedProcess.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
            sustainedProcess.Start();
            sustainedProcess.BeginOutputReadLine();
            //sustainedProcess.BeginErrorReadLine();
            this.updateOutput("等待前" + ip + " ...\n");
            sustainedProcess.WaitForExit(10000);
            this.updateOutput("等待后" + ip + " ...\n");
            sustainedProcess.Close();
        }

        //批量连接客户端
        private void connectClient()
        {
            String ip = checkedClientList[0].ToString();
            this.updateOutput("===========" + ip + "=============\n");
            connectClient(ip);
            this.updateOutput("==================================\n");      
        }

        //关闭连接
        private void disConnectClient(String ip)
        {
            currentIp = ip;
            this.updateOutput("正在关闭连接" + ip + " ...\n");

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.FileName = adbPath;
            psi.CreateNoWindow = true;
            psi.Arguments = "disconnect " + ip;

            process = new Process();
            process.StartInfo = psi;
            process.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            process.Close();

            killSustainedProcess();
        }

        //批量关闭连接
        private void disConnectClient()
        {
            String ip = checkedClientList[0].ToString();
            this.updateOutput("===========" + ip + "=============\n");
            disConnectClient(ip);
            this.updateOutput("==================================\n"); 
        }

        //重启客户端
        private void rebootClient(String ip)
        {
            currentIp = ip;
            this.updateOutput("正在重启" + ip + "...\n");
            //this.connectClient(ip);

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.FileName = adbPath;
            psi.CreateNoWindow = true;
            psi.Arguments = "-s " + ip + ":5555 reboot";

            process = new Process();
            process.StartInfo = psi;
            process.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            process.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            process.Close();
            killSustainedProcess();
            this.updateOutput("重启完成。如果失败，请重试。\n");
        }

        //批量重启客户端
        private void rebootClient()
        {
            end = false;
            for (int i = 0; i < checkedClientList.Count; i++)
            {
                if (i > 0)
                {
                    this.updateOutput("休眠5秒\n");
                    Thread.Sleep(5000);
                }
                if (end)
                {
                    break;
                }
                String ip = checkedClientList[i].ToString();
                this.updateOutput("===========" + ip + "=============\n");
                rebootClient(ip);
                this.updateOutput("==================================\n");
            }
        }

        //导出日志
        private void exportLog(String ip)
        {
            String logDir = System.Environment.CurrentDirectory + @"\" + ip + @"\log";
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            currentIp = ip;
            this.updateOutput("正在导出"+ip+"日志...\n");
            this.connectClient(ip);

            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = adbPath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "-s " + ip + ":5555 pull  /data/data/com.amtt.ids/files/log " + logDir;
            p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
            p.Start();
            p.WaitForExit();
            p.Close();
            this.updateOutput("日志已保存在" + logDir + "\n");
            this.disConnectClient(ip);
        }


        //导出Debug信息
        private void exportDebug(String ip)
        {
            String debugDir = System.Environment.CurrentDirectory + @"\" + ip + @"\debug";
            if (!Directory.Exists(debugDir))
            {
                Directory.CreateDirectory(debugDir);
            }

            this.connectClient(ip);
            this.opTextBox.AppendText("正在Debug信息...\n");
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = adbPath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "-s " + ip + ":5555 pull  /data/data/com.amtt.ids/files/debug " + debugDir;
            p.Start();
            p.WaitForExit();
            string output = p.StandardOutput.ReadToEnd();
            this.opTextBox.AppendText(output + "\n");
            this.opTextBox.AppendText("debug信息已保存在" + debugDir + "\n");
            p.Close();
            this.disConnectClient(ip);
        }

        //安装应用
        private void installApp(String ip, String appPath)
        {
            this.opTextBox.AppendText("正在安装应用" + ip + " ...\n");
            this.opTextBox.AppendText("如果长时间未反应，请手动安装。\n");
            this.opTextBox.AppendText("如果安装失败，请尝试卸载旧应用。\n");
            this.connectClient(ip);
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = false;
            p.StartInfo.FileName = adbPath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "-s " + ip + ":5555 install -r " + appPath;
            p.Start();
            p.WaitForExit();
            string output = p.StandardOutput.ReadToEnd();
            //string error = p.StandardError.ReadToEnd();
            this.opTextBox.AppendText(output + "\n");
            //this.opTextBox.AppendText(error + "\n");
            p.Close();
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
            p.Close();
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
            p.StartInfo.RedirectStandardError = false;
            p.StartInfo.FileName = adbPath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "-s " + ip + ":5555 shell am start -n com.amtt.ids/com.amtt.ids.AppStart";
            p.Start();
            p.WaitForExit();
            string output = p.StandardOutput.ReadToEnd();
            //string error = p.StandardError.ReadToEnd();
            this.opTextBox.AppendText(output + "\n");
            //this.opTextBox.AppendText(error + "\n");
            p.Close();
            this.disConnectClient(ip);
        }

        //快速启动
        private void quickStartApp(String ip, String clientName, String serverIp)
        {
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
            p.Close();
            this.disConnectClient(ip);
        }

        //结束ADB进程
        private void killAdb()
        {
            System.Diagnostics.Process[] killprocess = System.Diagnostics.Process.GetProcessesByName("adb");
            foreach (System.Diagnostics.Process adbProcess in killprocess)
            {
                adbProcess.Kill();
            }
        }

        //发送命令
        private void sendMessage(String ip, int port, String msg)
        {
            this.opTextBox.AppendText(ip + "----发送命令" + msg + "\n");
            TcpClient client = new TcpClient(ip, port);
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(msg);
            writer.Flush();
            writer.Close();
            stream.Close();
            client.Close();
        }

        //按钮事件

        //发送开发命令
        private void button14_Click(object sender, EventArgs e)
        {
            //保护危险动作
            ArrayList protectList = new ArrayList();
            //protectList.Add("resetApp");

            String cmd = this.cmdComboBox.SelectedValue.ToString();

            String custom = this.cmdTextBox.Text;
            if (custom != "")
            {
                cmd = custom;
            }
            else
            {
                if (protectList.IndexOf(cmd) != -1)
                {
                    MessageBox.Show("危险操作!!!请在自定义框输入'" + cmd + "'再发送");
                    return;
                }
            }

            String portStr = this.portTextBox.Text;
            if (portStr == "")
            {
                portStr = "9997";
            }
            int port = int.Parse(portStr);

            foreach (ListViewItem item in clientListView.Items)
            {
                if (item.Checked)
                {
                    String ip = item.SubItems[2].Text.ToString();
                    this.opTextBox.AppendText("==================" + ip + "==================\n\n");
                    this.sendMessage(ip, port, cmd);
                    this.opTextBox.AppendText("==================END==================\n\n\n");
                }
            }
        }

        //连接客户端
        private void button5_Click_1(object sender, EventArgs e)
        {
            setCheckedClient();
            setClientColor();
            if (checkedClientList.Count == 0 || checkedClientList.Count > 1)
            {
                this.updateOutput("只支持单客户端连接");
                return;
            }
            Thread t = new Thread(new ThreadStart(this.connectClient));
            t.Start();
        }

        //关闭客户端
        private void button10_Click(object sender, EventArgs e)
        {
            setCheckedClient();
            setClientColor();
            if (checkedClientList.Count == 0 || checkedClientList.Count > 1)
            {
                this.updateOutput("只支持单客户端");
                return;
            }
            Thread t = new Thread(new ThreadStart(this.disConnectClient));
            t.Start();
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
                if (item.Checked)
                {
                    String ip = item.SubItems[2].Text.ToString();
                    this.opTextBox.AppendText("从列表中删除" + ip + "\n");
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
            if (appPath == "")
            {
                MessageBox.Show("请选择应用");
                return;
            }

            foreach (ListViewItem item in clientListView.Items)
            {
                if (item.Checked)
                {
                    String ip = item.SubItems[2].Text.ToString();
                    this.opTextBox.AppendText("==================" + ip + "==================\n\n");
                    this.installApp(ip, appPath);
                    this.opTextBox.AppendText("==================END==================\n\n\n");
                }
            }
        }

        //批量重启客户端
        private void button9_Click(object sender, EventArgs e)
        {
            setCheckedClient();
            setClientColor();
            if (checkedClientList.Count == 0)
            {
                return;
            }
            Thread t = new Thread(new ThreadStart(this.rebootClient));
            t.Start();
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
                    this.uninstallApp(ip, "com.amtt.ids");
                    this.opTextBox.AppendText("==================END=========================\n\n\n");
                    this.opTextBox.AppendText("休眠10000毫秒.请勿操作！！！");
                    Thread.Sleep(10000);
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
                    this.quickStartApp(ip, name, serverIp);
                    this.opTextBox.AppendText("==================END=========================\n\n\n");
                }
            }
        }

        //即时修改切换效果
        private void button25_Click(object sender, EventArgs e)
        {
            String cmd = this.effectComboBox.SelectedValue.ToString();
            if (cmd == "-1")
            {
                MessageBox.Show("鄙视你");
                return;
            }

            String custom = this.effectTextBox.Text;
            if (custom != "")
            {
                cmd = "e" + custom;
            }

            String speed = this.speedTextBox.Text;

            if (speed == "")
            {
                speed = "5";
            }

            cmd += "@" + speed;

            String portStr = this.portTextBox.Text;
            if (portStr == "")
            {
                portStr = "9997";
            }
            int port = int.Parse(portStr);

            foreach (ListViewItem item in clientListView.Items)
            {
                if (item.Checked)
                {
                    String ip = item.SubItems[2].Text.ToString();
                    this.opTextBox.AppendText("==================" + ip + "==================\n\n");
                    this.sendMessage(ip, port, cmd);
                    this.opTextBox.AppendText("==================END==================\n\n\n");
                }
            }


        }

        //
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://me.alipay.com/liugr");
        }

        private void button18_Click(object sender, EventArgs e)
        {

        }

        //导出debug信息
        private void button11_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in clientListView.Items)
            {
                if (item.Checked)
                {
                    String ip = item.SubItems[2].Text.ToString();
                    this.opTextBox.AppendText("==================" + ip + "==================\n\n");
                    this.exportDebug(ip);
                    this.opTextBox.AppendText("==================END=========================\n\n\n");
                }
            }
        }

        //获得选中客户端列表
        private void setCheckedClient()
        {
            this.checkedClientList.Clear();
            foreach (ListViewItem item in this.clientListView.Items)
            {
                if (item.Checked)
                {
                    String ip = item.SubItems[2].Text.ToString();
                    this.checkedClientList.Add(ip);
                }
            }
        }



        //测试
        private void button17_Click(object sender, EventArgs e)
        {
            killAdb();
        }

        //更新输出信息
        private delegate void InvokeCallback(string info); //定义回调函数（代理）格式
        //Invoke回调函数
        private void updateOutput(string info)
        {
            if (this.opRichTextBox.InvokeRequired)//当前线程不是创建线程
                this.opRichTextBox.Invoke(new InvokeCallback(updateOutput), new object[] { info });//回调
            else//当前线程是创建线程（界面线程）
                this.opRichTextBox.AppendText(info);//直接更新
        }

        private void test()
        {
            updateOutput("休眠6秒");
            Thread.Sleep(6000);
            updateOutput("启动");
            updateOutput(checkedClientList[0].ToString());
        }


        //即时输出
        private delegate void AddMessageHandler(string msg);
        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            AddMessageHandler handler = delegate(string msg)
            {
                if (msg != null)
                {
                    this.opRichTextBox.SelectionColor = Color.Blue;
                    this.opRichTextBox.AppendText(msg + "\n");
                    this.opRichTextBox.Focus();
                    this.opRichTextBox.Select(this.opRichTextBox.TextLength, 0);
                    this.opRichTextBox.ScrollToCaret();
                }
            };
            if (this.opRichTextBox.InvokeRequired)
                this.opRichTextBox.Invoke(handler, e.Data);
        }

        //即时输出错误
        private delegate void AddErrorHandler(string msg);
        private void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            AddErrorHandler handler = delegate(string msg)
            {
                if (msg != null)
                {
                    this.opRichTextBox.SelectionColor = Color.Red;
                    this.opRichTextBox.AppendText(msg + "\n");
                    this.opRichTextBox.Focus();
                    this.opRichTextBox.Select(this.opRichTextBox.TextLength, 0);
                    this.opRichTextBox.ScrollToCaret();
                    setClientColor(currentIp);
                }
            };
            if (this.opRichTextBox.InvokeRequired)
                this.opRichTextBox.Invoke(handler, e.Data);
        }

        //设置客户端列表颜色
        private void setClientColor(String currentIp = "")
        {
            foreach (ListViewItem item in this.clientListView.Items)
            {
                if (currentIp == "")
                {
                    item.ForeColor = Color.Black;
                }
                else
                {
                    String ip = item.SubItems[2].Text.ToString();
                    if (ip == currentIp)
                    {
                        item.ForeColor = Color.Red;
                    }
                }
            }
        }

        //结束adb持续进程
        private void killSustainedProcess()
        {
            if (sustainedProcess!=null)
            {
                try
                {
                    sustainedProcess.CancelOutputRead();
                    sustainedProcess.CancelErrorRead();
                    sustainedProcess.Close();
                    sustainedProcess = null;
                }
                catch
                {
                }
            }
            killAdb();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox1 = new AboutBox1();
            aboutBox1.Show();
        }

        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://me.alipay.com/liugr");
        }

        private void 报告BugToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:jason.liu@amttgroup.com?Subject=报告Android IDS辅助工具Bug&Body=描述");
            this.updateOutput("如果没有自动打开Email客户端，请发至jason.liu@amttgroup.com\n");
        }
    }
}
