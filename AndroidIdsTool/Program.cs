using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AndroidIdsTool
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (System.Environment.CurrentDirectory.IndexOf(" ") != -1 || Common.isChinese(System.Environment.CurrentDirectory))
            {
                MessageBox.Show("程序路径不能含有空格或者中文");
                Application.Exit();
                return;
            }

            if (!System.IO.File.Exists(@"license.txt"))
            {
                MessageBox.Show("用户协议\r\n"+Global.userAgreement);
                FileStream fs = new FileStream("license.txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入
                sw.Write(Global.userAgreement);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }

            Authorize authorize = new Authorize();
            authorize.ShowDialog();

            if (authorize.pass)
            {
                Application.Run(new Form1());
            }           
        }
    }
}
