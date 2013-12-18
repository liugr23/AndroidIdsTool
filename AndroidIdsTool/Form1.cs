using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Net;

namespace AndroidIdsTool
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
        private Process briefProcess = null;
        //apk路径
        private String apkPath = "";
        //包名
        private String packageName = "com.amtt.ids";
        //服务器IP
        private String serverIp = Global.serverIp;
        //启动界面
        private String appStart = "com.amtt.ids.AppStart";
        //快速启动界面
        private String adbStart = "com.amtt.ids.AdbStart";
        //背景图片路径
        private String imagePath = "";
        //背景音乐路径
        private String musicPath = "";


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + "@" + Common.getIPAddress();

            //显示列表
            this.showList();
            //释放文件
            if (!System.IO.File.Exists(System.Environment.CurrentDirectory + @"\adb.exe"))
            {
                byte[] Save = global::IdsAndroidTool.Properties.Resources.adb;
                FileStream fsObj = new FileStream(System.Environment.CurrentDirectory + @"\adb.exe", FileMode.CreateNew);
                fsObj.Write(Save, 0, Save.Length);
                fsObj.Close();
            }
            if (!System.IO.File.Exists(System.Environment.CurrentDirectory + @"\AdbWinApi.dll"))
            {
                byte[] Save = global::IdsAndroidTool.Properties.Resources.AdbWinApi;
                FileStream fsObj = new FileStream(System.Environment.CurrentDirectory + @"\AdbWinApi.dll", FileMode.CreateNew);
                fsObj.Write(Save, 0, Save.Length);
                fsObj.Close();
            }
            if (!System.IO.File.Exists(System.Environment.CurrentDirectory + @"\cmd.txt"))
            {
                byte[] Save = System.Text.Encoding.UTF8.GetBytes(global::IdsAndroidTool.Properties.Resources.cmd);
                FileStream fsObj = new FileStream(System.Environment.CurrentDirectory + @"\cmd.txt", FileMode.CreateNew);
                fsObj.Write(Save, 0, Save.Length);
                fsObj.Close();
            }

            // 必要文件
            if (!System.IO.File.Exists(System.Environment.CurrentDirectory + @"\adb.exe") || !System.IO.File.Exists(System.Environment.CurrentDirectory + @"\AdbWinApi.dll"))
            {
                this.updateOutput("错误:adb.exe或AdbWinApi.dll不存在\n");
                this.updateOutput("联系jason.liu@amttgroup.com\n");
                this.operateGroupBox.Enabled = false;
            }
            //adb路径
            adbPath = System.Environment.CurrentDirectory + @"\adb.exe";
            if (System.IO.File.Exists(@"E:\Free\android\sdk\platform-tools\adb.exe"))
            {
                adbPath = @"E:\Free\android\sdk\platform-tools\adb.exe";
            }

            fillCmd();

            //切换效果列表
            ArrayList effectList = new ArrayList();
            effectList.Add(new DictionaryEntry("e-1", "无"));
            effectList.Add(new DictionaryEntry("e1", "立方体1"));
            effectList.Add(new DictionaryEntry("e2", "擦除-上-下2*"));
            effectList.Add(new DictionaryEntry("e4", "旋转4*"));
            effectList.Add(new DictionaryEntry("e7", "擦除-左-右7*"));
            effectList.Add(new DictionaryEntry("e11", "切出11*"));
            effectList.Add(new DictionaryEntry("e15", "翻页15"));
            effectList.Add(new DictionaryEntry("e17", "淡出17*"));
            effectList.Add(new DictionaryEntry("e21", "淡入21*"));
            effectList.Add(new DictionaryEntry("-1", "===分割线(不要选我)==="));
            effectList.Add(new DictionaryEntry("e0", "星形0"));
            effectList.Add(new DictionaryEntry("e10", "溶解10"));
            effectList.Add(new DictionaryEntry("e5", "菱形5"));
            effectList.Add(new DictionaryEntry("e6", "左右百叶窗6"));
            effectList.Add(new DictionaryEntry("e3", "翻页3"));
            effectList.Add(new DictionaryEntry("e13", "分割13"));
            effectList.Add(new DictionaryEntry("e20", "上下百叶窗20"));
            effectList.Add(new DictionaryEntry("e16", "溶解16"));
            effectList.Add(new DictionaryEntry("e14", "擦除--左上--右下14"));
            effectList.Add(new DictionaryEntry("e18", "收缩18"));
            effectList.Add(new DictionaryEntry("-1", "===分割线(不要选我)==="));
            effectList.Add(new DictionaryEntry("e21", "超级淡入21"));
            effectList.Add(new DictionaryEntry("e17", "超级淡出17"));
            effectList.Add(new DictionaryEntry("e11", "超级切出11"));
            effectList.Add(new DictionaryEntry("e4", "超级旋转4"));
            effectList.Add(new DictionaryEntry("e7", "超级擦除-左-右7"));
            effectList.Add(new DictionaryEntry("e2", "超级擦除-上-下2"));
            this.effectComboBox.DataSource = effectList;
            this.effectComboBox.DisplayMember = "Value";//显示的Text值
            this.effectComboBox.ValueMember = "Key";// 实际value值

            //默认值
            this.packageNameTextBox.Text = packageName;
            this.serverIpTextBox.Text = serverIp;
            this.appStartTextBox.Text = appStart;
            this.adbStartTextBox.Text = adbStart;

            this.cIpTextBox.Text = Common.getIPAddress();
            this.SafeCheckBox.Checked = Global.safe;

            //默认文件
            String[] filenames = Directory.GetFiles(System.Environment.CurrentDirectory);
            foreach (String file in filenames)
            {
                String tmp = file.Replace(System.Environment.CurrentDirectory + "\\", "");
                //应用
                if (tmp.IndexOf("ids") != -1 && tmp.IndexOf(".apk") != -1)
                {
                    apkPath = file;
                }
                //背景图片
                if (tmp == "bg.jpg")
                {
                    imagePath = file;
                }
                //背景音乐
                if (tmp == "bg.mp3")
                {
                    musicPath = file;
                }

            }
            this.apkTextBox.Text = apkPath;
            this.imageTextBox.Text = imagePath;
            this.musicTextBox.Text = musicPath;

            //禁用部分控件
            if(!Global.debug){
                this.button21.Enabled = false;
                this.button25.Enabled = false;    
            }

            //输出信息
            this.updateOutput("===============================\n");
            this.updateOutput("你好,"+Global.username+"\n");
            this.updateOutput("欢迎使用Android IDS 辅助工具\n");
            this.updateOutput("版本:" + Global.version + "\n");
            this.updateOutput("部分功能可能与虚拟机网卡冲突，请暂时禁用虚拟网卡.\n");
            this.updateOutput("如遇问题，请联系jason.liu@amttgroup.com\n");
            this.updateOutput("===============================\n");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            killSustainedProcess();
            killBriefProcessProcess();
            if(!Global.debug){
                killAdb();
            }
        }

        //填充命令列表
        private void fillCmd()
        {
            this.cmdComboBox.Items.Clear();
            if (System.IO.File.Exists(@"cmd.txt"))
            {
                ArrayList list = new ArrayList();
                string ReadLine;
                string[] array;
                StreamReader reader = new StreamReader(@"cmd.txt");
                while (reader.Peek() >= 0)  
                {  
                    try  
                    {  
                        ReadLine = reader.ReadLine();
                        if (ReadLine != "" && ReadLine.IndexOf("<!!!") == -1)   
                        {
                            array = ReadLine.Split('#');  
                            if (array.Length != 0)  
                            {
                                list.Add(new DictionaryEntry(array[0].Trim(), array[1].Trim()));
                            }
                        }  
                    }  
                    catch (Exception ex)  
                    {   
                    }                  
                }
                this.cmdComboBox.DataSource = list;
                this.cmdComboBox.DisplayMember = "key";//显示的Text值
                this.cmdComboBox.ValueMember = "Value";// 实际value值
            }
        }

        //获得cmd进程
        private Process getProcess()
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            cmd.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
            cmd.Start();
            //cmd.StandardInput.AutoFlush = true;
            cmd.BeginOutputReadLine();
            cmd.BeginErrorReadLine();
            return cmd;
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
                if (i % 2 == 0)
                {
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
                this.updateOutput("输入有误！\n");
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
                this.updateOutput("输入有误！\n");
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
                this.updateOutput("输入有误！\n");
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
                this.musicTextBox.Text = path;
            }
        }

        //连接客户端
        private void connectClient(String ip)
        {
            currentIp = ip;
            this.updateOutput("正在连接" + ip + " ...\n");
            String order = adbPath + " connect " + ip;
            sustainedProcess = getProcess();
            sustainedProcess.StandardInput.WriteLine(order);
        }

        //批量连接客户端
        private void connectClient()
        {
            killSustainedProcess();
            String ip = checkedClientList[0].ToString();
            this.updateOutput("==============" + ip + "===============\n");
            connectClient(ip);
        }

        //关闭连接
        private void disConnectClient(String ip)
        {
            currentIp = ip;
            this.updateOutput("正在关闭连接" + ip + " ...\n");
            String order = adbPath + " disconnect " + ip;
            Process process = getProcess();
            process.StandardInput.WriteLine(order);
            process.Close();
        }

        //批量关闭连接
        private void disConnectClient()
        {
            String ip = checkedClientList[0].ToString();
            this.updateOutput("===========" + ip + "=============\n");
            disConnectClient(ip);
        }

        //重启客户端
        private void rebootClient(String ip)
        {
            this.updateOutput("正在重启" + ip + "...\n");
            this.updateOutput("如果失败，请重试。\n");

            String order = "";
            currentIp = ip;
            Thread.Sleep(1000);

            order = adbPath + " connect " + ip;
            Process process = getProcess();
            process.StandardInput.WriteLine(order);
            Thread.Sleep(1000);
            order = adbPath + " -s " + ip + ":5555 reboot";
            process.StandardInput.WriteLine(order);
            process.Close();
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

            this.updateOutput("正在导出" + ip + "日志...\n");
            this.updateOutput("日志将会保存在" + logDir + "\n");

            if (briefProcess == null)
            {
                briefProcess = getProcess();
            }

            String order = "";
            order = adbPath + " connect " + ip;
            briefProcess.StandardInput.WriteLine(order);
            Thread.Sleep(1000);
            order = adbPath + " -s " + ip + ":5555 pull  /data/data/com.amtt.ids/files/log " + logDir;
            briefProcess.StandardInput.WriteLine(order);
            if (!Global.debug)
            {
                Thread.Sleep(1000);
                order = adbPath + " disconnect " + ip;
                briefProcess.StandardInput.WriteLine(order);
            }
        }

        //批量导出日志
        private void exportLog()
        {
            killBriefProcessProcess();
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
                exportLog(ip);
            }
        }

        //导出Debug信息
        private void exportDebug(String ip)
        {
            String debugDir = System.Environment.CurrentDirectory + @"\" + ip + @"\debug";
            if (!Directory.Exists(debugDir))
            {
                Directory.CreateDirectory(debugDir);
            }

            currentIp = ip;
            this.updateOutput("正在导出" + ip + "Debug信息...\n");
            this.updateOutput("debug信息将会保存在" + debugDir + "\n");

            if (briefProcess == null)
            {
                briefProcess = getProcess();
            }

            String order = "";
            order = adbPath + " connect " + ip;
            briefProcess.StandardInput.WriteLine(order);
            Thread.Sleep(1000);
            order = adbPath + " -s " + ip + ":5555 pull  /data/data/com.amtt.ids/files/debug " + debugDir;
            briefProcess.StandardInput.WriteLine(order);
            if(!Global.debug){
                Thread.Sleep(1000);
                order = adbPath + " disconnect " + ip;
                briefProcess.StandardInput.WriteLine(order);
            }
        }

        //批量导出debug信息
        private void exportDebug()
        {
            killBriefProcessProcess();
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
                exportDebug(ip);
            }
        }

        //安装应用
        private void installApp(String ip)
        {
            currentIp = ip;
            this.updateOutput("正在安装应用" + ip + " ...\n");
            this.updateOutput("如果安装失败，请尝试卸载旧应用。\n");
            this.updateOutput("如果长时间未反应，请手动安装。\n");

            if (briefProcess == null)
            {
                briefProcess = getProcess();
            }

            String order = "";
            order = adbPath + " connect " + ip;
            briefProcess.StandardInput.WriteLine(order);
            Thread.Sleep(1000);
            order = adbPath + " -s " + ip + ":5555 install -r " + apkPath;
            briefProcess.StandardInput.WriteLine(order);
            Thread.Sleep(1000);
            order = adbPath + " disconnect " + ip;
            briefProcess.StandardInput.WriteLine(order);
        }

        //批量安装应用
        private void installApp()
        {
            killBriefProcessProcess();
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
                installApp(ip);
            }
        }

        //卸载应用
        private void uninstallApp(String ip)
        {
            currentIp = ip;
            this.updateOutput("正在卸载应用" + ip + " ...\n");
            this.updateOutput("如果长时间未反应，请手动卸载。\n");

            if (briefProcess == null)
            {
                briefProcess = getProcess();
            }

            String order = "";
            order = adbPath + " connect " + ip;
            briefProcess.StandardInput.WriteLine(order);
            Thread.Sleep(1000);
            order = adbPath + " -s " + ip + ":5555 uninstall " + packageName;
            briefProcess.StandardInput.WriteLine(order);
            Thread.Sleep(1000);
            order = adbPath + " disconnect " + ip;
            briefProcess.StandardInput.WriteLine(order);
        }

        //批量卸载应用
        private void uninstallApp()
        {
            killBriefProcessProcess();
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
                uninstallApp(ip);
            }
        }

        //启动应用
        private void startApp(String ip)
        {
            currentIp = ip;
            this.updateOutput("正在启动应用" + ip + " ...\n");

            if (briefProcess == null)
            {
                briefProcess = getProcess();
            }

            String order = "";
            order = adbPath + " connect " + ip;
            briefProcess.StandardInput.WriteLine(order);
            Thread.Sleep(1000);
            order = adbPath + " -s " + ip + ":5555 shell am start -n " + packageName + "/" + appStart;
            briefProcess.StandardInput.WriteLine(order);
            Thread.Sleep(1000);
            order = adbPath + " disconnect " + ip;
            briefProcess.StandardInput.WriteLine(order);
        }

        //批量启动应用
        private void startApp()
        {
            killBriefProcessProcess();
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
                startApp(ip);
            }
        }

        //快速启动
        private void quickStartApp(String ip, String clientName)
        {
            currentIp = ip;
            this.updateOutput("正在快速启动应用" + ip + " ...\n");

            if (briefProcess == null)
            {
                briefProcess = getProcess();
            }

            String order = "";
            order = adbPath + " connect " + ip;
            briefProcess.StandardInput.WriteLine(order);
            Thread.Sleep(1000);
            order = adbPath + " -s " + ip + ":5555 shell am start -n " + packageName + "/" + adbStart + " --es clientName '" + clientName + "'" + " --es serverIp '" + serverIp + "'";
            briefProcess.StandardInput.WriteLine(order);
            Thread.Sleep(1000);
            order = adbPath + " disconnect " + ip;
            briefProcess.StandardInput.WriteLine(order);
        }

        //批量快速启动应用
        private void quickStartApp()
        {
            killBriefProcessProcess();
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
                String clientName = "";
                foreach (ListViewItem item in clientListView.Items)
                {
                    if (ip == item.SubItems[2].Text.ToString())
                    {
                        clientName = item.SubItems[1].Text.ToString();
                    }
                }
                this.updateOutput("===========" + ip + "=============\n");
                quickStartApp(ip, clientName);
            }
        }


        //定制
        private void build(String ip)
        {
            currentIp = ip;
            this.updateOutput("正在定制应用" + ip + " ...\n");

            if (briefProcess == null)
            {
                briefProcess = getProcess();
            }

            String order = "";
            order = adbPath + " connect " + ip;
            briefProcess.StandardInput.WriteLine(order);
            Thread.Sleep(1000);
            if (imagePath != "")
            {
                order = adbPath + " -s " + ip + ":5555 push \"" + imagePath + "\" /data/data/" + packageName + "/files/image/";
                briefProcess.StandardInput.WriteLine(order);
                Thread.Sleep(1000);
            }
            if (musicPath != "")
            {
                order = adbPath + " -s " + ip + ":5555 push \"" + musicPath + "\" /data/data/" + packageName + "/files/sound/";
                briefProcess.StandardInput.WriteLine(order);
                Thread.Sleep(1000);
            }
            order = adbPath + " disconnect " + ip;
            briefProcess.StandardInput.WriteLine(order);
        }
        //批量定制
        private void build()
        {
            killBriefProcessProcess();
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
                build(ip);
            }
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
            this.updateOutput("发送命令--" + msg + "至" + ip + "\n");
            TcpClient client = new TcpClient(ip, port);
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(msg);
            writer.Flush();
            writer.Close();
            stream.Close();
            client.Close();
        }


        //发送开发命令
        private void button14_Click(object sender, EventArgs e)
        {
            String cmd = this.cmdComboBox.SelectedValue.ToString();

            String custom = this.cmdTextBox.Text;
            if (custom != "")
            {
                cmd = custom;
            }

            if (cmd == "-1")
            {
                return;
            }

            if (cmd == "ifconfig")
            {
                cmd = "ifconfig@0#1#2#3#4#5?";
            }

            if (cmd == "setServerIp")
            {
                cmd = "setServerIp@?";
            }

            if (cmd == "kill")
            {
                cmd = "kill@?";
            }

            if(cmd.IndexOf("?")!=-1){
                this.cmdTextBox.Text = cmd;
                return;
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
                    this.updateOutput("==================" + ip + "==================\n");
                    this.sendMessage(ip, port, cmd);
                    this.updateOutput("==================END==================\n");
                }
            }
            this.cmdTextBox.Text = "";
        }

        //连接客户端
        private void button5_Click_1(object sender, EventArgs e)
        {
            setCheckedClient();
            setClientColor();
            if (checkedClientList.Count != 1)
            {
                this.updateOutput("只支持单客户端连接\n");
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
                this.updateOutput("只支持单客户端\n");
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
                    this.updateOutput("从列表中删除" + ip + "\n");
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
            apkPath = this.apkTextBox.Text;
            if (apkPath == "")
            {
                this.updateOutput("请选择应用\n");
                return;
            }

            setCheckedClient();
            setClientColor();
            if (checkedClientList.Count == 0)
            {
                return;
            }
            Thread t = new Thread(new ThreadStart(this.installApp));
            t.Start();
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
            setCheckedClient();
            setClientColor();
            if (checkedClientList.Count == 0)
            {
                return;
            }
            Thread t = new Thread(new ThreadStart(this.exportLog));
            t.Start();
        }

        //导出debug信息
        private void button11_Click(object sender, EventArgs e)
        {
            setCheckedClient();
            setClientColor();
            if (checkedClientList.Count == 0)
            {
                return;
            }
            Thread t = new Thread(new ThreadStart(this.exportDebug));
            t.Start();
        }

        //批量启动应用
        private void button19_Click(object sender, EventArgs e)
        {
            setCheckedClient();
            setClientColor();
            if (checkedClientList.Count == 0)
            {
                return;
            }
            packageName = this.packageNameTextBox.Text;
            if (packageName == "")
            {
                this.updateOutput("请输入包名\n");
                return;
            }
            appStart = appStartTextBox.Text;
            if (appStart == "")
            {
                this.updateOutput("请输入启动界面\n");
                return;
            }
            Thread t = new Thread(new ThreadStart(this.startApp));
            t.Start();
        }

        //批量卸载应用
        private void button20_Click(object sender, EventArgs e)
        {
            setCheckedClient();
            setClientColor();
            if (checkedClientList.Count == 0)
            {
                return;
            }
            packageName = this.packageNameTextBox.Text;
            if (packageName == "")
            {
                this.updateOutput("请输入包名\n");
                return;
            }
            Thread t = new Thread(new ThreadStart(this.uninstallApp));
            t.Start();
        }

        //批量快速启动
        private void button21_Click(object sender, EventArgs e)
        {
            setCheckedClient();
            setClientColor();
            if (checkedClientList.Count == 0)
            {
                return;
            }
            packageName = this.packageNameTextBox.Text;
            if (packageName == "")
            {
                this.updateOutput("请输入包名\n");
                return;
            }
            serverIp = this.serverIpTextBox.Text;
            if (serverIp == "")
            {
                this.updateOutput("请输入服务器IP\n");
                return;
            }
            adbStart = adbStartTextBox.Text;
            if (adbStart == "")
            {
                this.updateOutput("请输入快速启动界面\n");
                return;
            }
            Thread t = new Thread(new ThreadStart(this.quickStartApp));
            t.Start();
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
                    this.updateOutput("==================" + ip + "==================\n\n");
                    this.sendMessage(ip, port, cmd);
                    this.updateOutput("==================END==================\n\n\n");
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://me.alipay.com/liugr");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            setCheckedClient();
            setClientColor();
            if (checkedClientList.Count == 0)
            {
                return;
            }
            imagePath = imageTextBox.Text;
            musicPath = musicTextBox.Text;
            if (imagePath == "" && musicPath == "")
            {
                this.updateOutput("请选择图片或者音乐");
                return;
            }
            if (imagePath.IndexOf(" ") != -1 || musicPath.IndexOf(" ") != -1)
            {
                this.updateOutput("路径不能含有空格");
                return;
            }
            Thread t = new Thread(new ThreadStart(this.build));
            t.Start();
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
        }

        //更新输出信息
        private delegate void InvokeCallback(string info); //定义回调函数（代理）格式
        //Invoke回调函数
        private void updateOutput(string info)
        {
            if (this.opRichTextBox.InvokeRequired)//当前线程不是创建线程
            {
                this.opRichTextBox.Invoke(new InvokeCallback(updateOutput), new object[] { info });//回调
            }
            else//当前线程是创建线程（界面线程）
            {
                this.opRichTextBox.AppendText(info);//直接更新
                this.opRichTextBox.Focus();
                this.opRichTextBox.Select(this.opRichTextBox.TextLength, 0);
                this.opRichTextBox.ScrollToCaret();
            }
        }

        private void test()
        {
            updateOutput("休眠6秒\n");
            Thread.Sleep(6000);
            updateOutput("启动\n");
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
            if (sustainedProcess != null)
            {
                sustainedProcess.CancelOutputRead();
                sustainedProcess.CancelErrorRead();
                sustainedProcess.Close();
                sustainedProcess = null;

            }
        }

        //结束adb短暂进程
        private void killBriefProcessProcess()
        {
            if (briefProcess != null)
            {
                briefProcess.CancelOutputRead();
                briefProcess.CancelErrorRead();
                briefProcess.Close();
                briefProcess = null;
            }
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

        //终止当前操作
        private void button23_Click(object sender, EventArgs e)
        {
            this.killBriefProcessProcess();
        }
        //终止所有操作
        private void button22_Click(object sender, EventArgs e)
        {
            this.killSustainedProcess();
        }
        //清空输出
        private void button15_Click(object sender, EventArgs e)
        {
            this.opRichTextBox.Text = "";
        }

        private void button26_Click(object sender, EventArgs e)
        {
            killAdb();
        }

        /**
         * 发送UDP
        */
        private void sendUdp(int port,string msg)
        {
            string cmd = "";
            bool safe = this.SafeCheckBox.Checked;
            if (safe)
            {
                String ip = this.cIpTextBox.Text;
                String key = Common.formatStr(ip, 16);
                String iv = Common.formatStr(Global.iv, 16);
                cmd = Common.encrypt(msg, key, iv).Trim();

                if(Global.debug){
                    this.updateOutput("当前处于安全模式\n");
                    this.updateOutput("本机IP: " + ip + "\n");
                    this.updateOutput("加密前的命令: " + msg + "\n");
                    this.updateOutput("加密key: " + key + "\n");
                    this.updateOutput("加密iv: " + iv + "\n");
                    this.updateOutput("加密后的命令: " + cmd + "\n");
                }
            }
            else
            {
                this.updateOutput("当前处于非安全模式\n");
                this.updateOutput("不加密命令\n");
                cmd = msg.Trim();
            }

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, port);
            sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            byte[] buffer = Encoding.UTF8.GetBytes(cmd);
            sock.SendTo(buffer, iep);
            sock.Close();
            this.updateOutput("向端口" + port + "发送"+msg+"\n");
            this.button29.Enabled = true;
        }

        //重启所有客户端
        private void button27_Click(object sender, EventArgs e)
        {
            string portStr = this.cUdpPortTextBox.Text;
            if (portStr == "")
            {
                portStr = "9996";
            }
            int port = int.Parse(portStr);
            sendUdp(port,"reboot");
        }

        //发送UDP命令
        private void button28_Click(object sender, EventArgs e)
        {
            string cmd = this.cUdpCmdTextBox.Text;
            if (cmd == "")
            {
                return;
            }
            string portStr = this.cUdpPortTextBox.Text;
            if (portStr == "")
            {
                portStr = "9996";
            }
            int port = int.Parse(portStr);
            sendUdp(port,cmd);
        }

        private void listenCmd()
        {
            string portStr = this.cUdpPortTextBox.Text;
            if (portStr == "")
            {
                portStr = "9996";
            }
            int port = int.Parse(portStr);

            this.updateOutput("正在端口" + port + "监听\n");
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket协议
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, port);//初始化一个侦听局域网内部所有IP和指定端口
            EndPoint ep = (EndPoint)iep;
            socket.Bind(iep);//绑定这个实例
            byte[] buffer = new byte[1024];//设置缓冲数据流
            socket.ReceiveFrom(buffer, ref ep);//接收数据,并确把数据设置到缓冲流里面
            string msg = Encoding.UTF8.GetString(buffer).TrimEnd('\u0000'); 
            socket.Close();
            this.updateOutput("监听到" + msg + "\n");
        }

        private void button29_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(this.listenCmd));
            t.Start();
            this.button29.Enabled = false;
        }

        //获取服务器IP
        private void waitServerIp()
        {
            string portStr = this.cPortTextBox.Text;
            if (portStr == "")
            {
                portStr = "9997";
            }
            int port = int.Parse(portStr);
            this.updateOutput("正在端口" + port + "监听服务器信息\n");

            string remoteIp = "";
            string clientIp = "";

            TcpListener myListener = new TcpListener(IPAddress.Any, port);//本地监听端口
            //Socket listenerSocket = myListener.Server;
            //LingerOption lingerOption = new LingerOption(true, 10);
            //listenerSocket.SetSocketOption(SocketOptionLevel.Socket,
             //                 SocketOptionName.Linger,
             //                 lingerOption);   
            myListener.Start();
            try
            {
                TcpClient client = myListener.AcceptTcpClient();
                client.ReceiveTimeout = 5000;
                remoteIp = client.Client.RemoteEndPoint.ToString().Split(':')[0];//获取到服务器IP

                NetworkStream streamToClient = client.GetStream();
                byte[] buffer = new byte[1024];
                streamToClient.Read(buffer, 0, buffer.Length);
                clientIp = Encoding.UTF8.GetString(buffer);

                client.Close();  
            }catch(Exception e){
                this.updateOutput(e.ToString());
            }
            myListener.Stop();

            if (remoteIp != "")
            {
                this.updateOutput("本机IP" + clientIp + "\n");
                this.updateOutput("服务器IP" + remoteIp + "\n");
            }
            else
            {
                this.updateOutput("获取服务器IP失败\n");
            }
            
        }

        //获取服务器IP
        private void udpServer()
        {
            string portStr = this.sPortTextBox.Text;
            if (portStr == "")
            {
                portStr = "9998";
            }
            int port = int.Parse(portStr);
            for (int i = 0; i < 6;i++ )
            {
                Thread.Sleep(1000);
                sendUdp(port, "ANDROID");
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(this.waitServerIp));
            t.Start();
            Thread t2 = new Thread(new ThreadStart(this.udpServer));
            t2.Start();
        }

        private void 注销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(@"auth.txt");
            if (file.Exists)
            {
                file.Delete(); //删除单个文件
            }
            file = new FileInfo(@"license.txt");
            if (file.Exists)
            {
                file.Delete(); //删除单个文件
            }
            Application.Exit();
        }
    }
}
