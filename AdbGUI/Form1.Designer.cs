namespace AdbGUI
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.clientListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.ipTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.apkTextBox = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.p1TextBox = new System.Windows.Forms.TextBox();
            this.p2TextBox = new System.Windows.Forms.TextBox();
            this.p3TextBox = new System.Windows.Forms.TextBox();
            this.p4TextBox = new System.Windows.Forms.TextBox();
            this.p5TextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.opTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.imageTextBox = new System.Windows.Forms.TextBox();
            this.soundTextBox = new System.Windows.Forms.TextBox();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.devGroupBox = new System.Windows.Forms.GroupBox();
            this.cmdComboBox = new System.Windows.Forms.ComboBox();
            this.dpTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button15 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.testGroupBox = new System.Windows.Forms.GroupBox();
            this.opGroupBox = new System.Windows.Forms.GroupBox();
            this.clientGroupBox = new System.Windows.Forms.GroupBox();
            this.button17 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button18 = new System.Windows.Forms.Button();
            this.devGroupBox.SuspendLayout();
            this.testGroupBox.SuspendLayout();
            this.opGroupBox.SuspendLayout();
            this.clientGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // clientListView
            // 
            this.clientListView.CheckBoxes = true;
            this.clientListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.clientListView.FullRowSelect = true;
            this.clientListView.Location = new System.Drawing.Point(6, 22);
            this.clientListView.Name = "clientListView";
            this.clientListView.Size = new System.Drawing.Size(391, 447);
            this.clientListView.TabIndex = 0;
            this.clientListView.UseCompatibleStateImageBehavior = false;
            this.clientListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 167;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "IP";
            this.columnHeader3.Width = 114;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(42, 509);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(113, 20);
            this.nameTextBox.TabIndex = 1;
            this.nameTextBox.Text = "c1";
            // 
            // ipTextBox
            // 
            this.ipTextBox.Location = new System.Drawing.Point(185, 509);
            this.ipTextBox.Name = "ipTextBox";
            this.ipTextBox.Size = new System.Drawing.Size(131, 20);
            this.ipTextBox.TabIndex = 2;
            this.ipTextBox.Text = "192.168.15.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 509);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(161, 509);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "IP";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(322, 509);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "添加客户端";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(322, 475);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "删除客户端";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "APK";
            // 
            // apkTextBox
            // 
            this.apkTextBox.Location = new System.Drawing.Point(46, 19);
            this.apkTextBox.Name = "apkTextBox";
            this.apkTextBox.ReadOnly = true;
            this.apkTextBox.Size = new System.Drawing.Size(355, 20);
            this.apkTextBox.TabIndex = 8;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(421, 17);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "选择";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 543);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "IP段";
            // 
            // p1TextBox
            // 
            this.p1TextBox.Location = new System.Drawing.Point(44, 543);
            this.p1TextBox.Name = "p1TextBox";
            this.p1TextBox.Size = new System.Drawing.Size(43, 20);
            this.p1TextBox.TabIndex = 11;
            this.p1TextBox.Text = "192";
            // 
            // p2TextBox
            // 
            this.p2TextBox.Location = new System.Drawing.Point(91, 543);
            this.p2TextBox.Name = "p2TextBox";
            this.p2TextBox.Size = new System.Drawing.Size(43, 20);
            this.p2TextBox.TabIndex = 12;
            this.p2TextBox.Text = "168";
            // 
            // p3TextBox
            // 
            this.p3TextBox.Location = new System.Drawing.Point(138, 543);
            this.p3TextBox.Name = "p3TextBox";
            this.p3TextBox.Size = new System.Drawing.Size(43, 20);
            this.p3TextBox.TabIndex = 13;
            this.p3TextBox.Text = "15";
            // 
            // p4TextBox
            // 
            this.p4TextBox.Location = new System.Drawing.Point(185, 543);
            this.p4TextBox.Name = "p4TextBox";
            this.p4TextBox.Size = new System.Drawing.Size(43, 20);
            this.p4TextBox.TabIndex = 14;
            // 
            // p5TextBox
            // 
            this.p5TextBox.Location = new System.Drawing.Point(259, 543);
            this.p5TextBox.Name = "p5TextBox";
            this.p5TextBox.Size = new System.Drawing.Size(40, 20);
            this.p5TextBox.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(234, 543);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "至";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(322, 543);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 17;
            this.button4.Text = "批量添加";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(153, 475);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 19;
            this.button6.Text = "全选";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(237, 475);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 20;
            this.button7.Text = "反选";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(21, 45);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 21;
            this.button8.Text = "安装";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(102, 45);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 23);
            this.button9.TabIndex = 22;
            this.button9.Text = "重启";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // opTextBox
            // 
            this.opTextBox.Location = new System.Drawing.Point(17, 19);
            this.opTextBox.Multiline = true;
            this.opTextBox.Name = "opTextBox";
            this.opTextBox.ReadOnly = true;
            this.opTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.opTextBox.Size = new System.Drawing.Size(485, 197);
            this.opTextBox.TabIndex = 23;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "图片";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "音乐";
            // 
            // imageTextBox
            // 
            this.imageTextBox.Location = new System.Drawing.Point(45, 19);
            this.imageTextBox.Name = "imageTextBox";
            this.imageTextBox.ReadOnly = true;
            this.imageTextBox.Size = new System.Drawing.Size(355, 20);
            this.imageTextBox.TabIndex = 27;
            // 
            // soundTextBox
            // 
            this.soundTextBox.Location = new System.Drawing.Point(45, 48);
            this.soundTextBox.Name = "soundTextBox";
            this.soundTextBox.ReadOnly = true;
            this.soundTextBox.Size = new System.Drawing.Size(355, 20);
            this.soundTextBox.TabIndex = 28;
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(420, 14);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(75, 23);
            this.button12.TabIndex = 29;
            this.button12.Text = "选择";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(420, 43);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(75, 23);
            this.button13.TabIndex = 30;
            this.button13.Text = "选择";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(183, 45);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(75, 23);
            this.button16.TabIndex = 7;
            this.button16.Text = "导出日志";
            this.button16.UseVisualStyleBackColor = true;
            // 
            // devGroupBox
            // 
            this.devGroupBox.Controls.Add(this.button17);
            this.devGroupBox.Controls.Add(this.cmdComboBox);
            this.devGroupBox.Controls.Add(this.dpTextBox);
            this.devGroupBox.Controls.Add(this.label9);
            this.devGroupBox.Controls.Add(this.label6);
            this.devGroupBox.Controls.Add(this.button15);
            this.devGroupBox.Controls.Add(this.button5);
            this.devGroupBox.Controls.Add(this.button14);
            this.devGroupBox.Controls.Add(this.button10);
            this.devGroupBox.Controls.Add(this.button11);
            this.devGroupBox.Location = new System.Drawing.Point(421, 30);
            this.devGroupBox.Name = "devGroupBox";
            this.devGroupBox.Size = new System.Drawing.Size(512, 129);
            this.devGroupBox.TabIndex = 9;
            this.devGroupBox.TabStop = false;
            this.devGroupBox.Text = "开发";
            // 
            // cmdComboBox
            // 
            this.cmdComboBox.FormattingEnabled = true;
            this.cmdComboBox.Items.AddRange(new object[] {
            "关闭服务"});
            this.cmdComboBox.Location = new System.Drawing.Point(265, 18);
            this.cmdComboBox.Name = "cmdComboBox";
            this.cmdComboBox.Size = new System.Drawing.Size(121, 21);
            this.cmdComboBox.TabIndex = 8;
            // 
            // dpTextBox
            // 
            this.dpTextBox.Location = new System.Drawing.Point(56, 19);
            this.dpTextBox.Name = "dpTextBox";
            this.dpTextBox.Size = new System.Drawing.Size(152, 20);
            this.dpTextBox.TabIndex = 1;
            this.dpTextBox.Text = "192.168.15.1";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(228, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "命令";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Ip";
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(345, 60);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(75, 23);
            this.button15.TabIndex = 6;
            this.button15.Text = "导出日志";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(21, 60);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 2;
            this.button5.Text = "连接";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(264, 60);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(75, 23);
            this.button14.TabIndex = 5;
            this.button14.Text = "发送命令";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(102, 60);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 23);
            this.button10.TabIndex = 3;
            this.button10.Text = "关闭";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(183, 60);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(75, 23);
            this.button11.TabIndex = 4;
            this.button11.Text = "重启";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // testGroupBox
            // 
            this.testGroupBox.Controls.Add(this.apkTextBox);
            this.testGroupBox.Controls.Add(this.button16);
            this.testGroupBox.Controls.Add(this.button8);
            this.testGroupBox.Controls.Add(this.button9);
            this.testGroupBox.Controls.Add(this.button3);
            this.testGroupBox.Controls.Add(this.label3);
            this.testGroupBox.Location = new System.Drawing.Point(421, 165);
            this.testGroupBox.Name = "testGroupBox";
            this.testGroupBox.Size = new System.Drawing.Size(512, 103);
            this.testGroupBox.TabIndex = 32;
            this.testGroupBox.TabStop = false;
            this.testGroupBox.Text = "部署";
            // 
            // opGroupBox
            // 
            this.opGroupBox.Controls.Add(this.opTextBox);
            this.opGroupBox.Location = new System.Drawing.Point(421, 381);
            this.opGroupBox.Name = "opGroupBox";
            this.opGroupBox.Size = new System.Drawing.Size(512, 222);
            this.opGroupBox.TabIndex = 33;
            this.opGroupBox.TabStop = false;
            this.opGroupBox.Text = "输出";
            // 
            // clientGroupBox
            // 
            this.clientGroupBox.Controls.Add(this.clientListView);
            this.clientGroupBox.Controls.Add(this.p4TextBox);
            this.clientGroupBox.Controls.Add(this.button4);
            this.clientGroupBox.Controls.Add(this.button7);
            this.clientGroupBox.Controls.Add(this.button6);
            this.clientGroupBox.Controls.Add(this.p5TextBox);
            this.clientGroupBox.Controls.Add(this.nameTextBox);
            this.clientGroupBox.Controls.Add(this.label4);
            this.clientGroupBox.Controls.Add(this.button2);
            this.clientGroupBox.Controls.Add(this.label5);
            this.clientGroupBox.Controls.Add(this.p1TextBox);
            this.clientGroupBox.Controls.Add(this.p2TextBox);
            this.clientGroupBox.Controls.Add(this.label2);
            this.clientGroupBox.Controls.Add(this.button1);
            this.clientGroupBox.Controls.Add(this.ipTextBox);
            this.clientGroupBox.Controls.Add(this.p3TextBox);
            this.clientGroupBox.Controls.Add(this.label1);
            this.clientGroupBox.Location = new System.Drawing.Point(12, 30);
            this.clientGroupBox.Name = "clientGroupBox";
            this.clientGroupBox.Size = new System.Drawing.Size(403, 573);
            this.clientGroupBox.TabIndex = 34;
            this.clientGroupBox.TabStop = false;
            this.clientGroupBox.Text = "客户端列表";
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(427, 59);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(75, 23);
            this.button17.TabIndex = 9;
            this.button17.Text = "测试";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button18);
            this.groupBox1.Controls.Add(this.imageTextBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.button13);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.button12);
            this.groupBox1.Controls.Add(this.soundTextBox);
            this.groupBox1.Location = new System.Drawing.Point(421, 274);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(511, 101);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "定制";
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(421, 73);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(75, 23);
            this.button18.TabIndex = 31;
            this.button18.Text = "定制";
            this.button18.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 615);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.opGroupBox);
            this.Controls.Add(this.testGroupBox);
            this.Controls.Add(this.devGroupBox);
            this.Controls.Add(this.clientGroupBox);
            this.Name = "Form1";
            this.Text = "AdbGUI";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.devGroupBox.ResumeLayout(false);
            this.devGroupBox.PerformLayout();
            this.testGroupBox.ResumeLayout(false);
            this.testGroupBox.PerformLayout();
            this.opGroupBox.ResumeLayout(false);
            this.opGroupBox.PerformLayout();
            this.clientGroupBox.ResumeLayout(false);
            this.clientGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView clientListView;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox ipTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox apkTextBox;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox p1TextBox;
        private System.Windows.Forms.TextBox p2TextBox;
        private System.Windows.Forms.TextBox p3TextBox;
        private System.Windows.Forms.TextBox p4TextBox;
        private System.Windows.Forms.TextBox p5TextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.TextBox opTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox imageTextBox;
        private System.Windows.Forms.TextBox soundTextBox;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.GroupBox devGroupBox;
        private System.Windows.Forms.ComboBox cmdComboBox;
        private System.Windows.Forms.TextBox dpTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.GroupBox testGroupBox;
        private System.Windows.Forms.GroupBox opGroupBox;
        private System.Windows.Forms.GroupBox clientGroupBox;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button18;
    }
}

