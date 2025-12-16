namespace MainSystem
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxIPAddress = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CRIConnect = new System.Windows.Forms.Button();
            this.labelConnectionStatus = new System.Windows.Forms.Label();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.textBoxPort = new System.Windows.Forms.NumericUpDown();
            this.tabCamera = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.buttonSendC3 = new System.Windows.Forms.Button();
            this.labelTargetAngle = new System.Windows.Forms.Label();
            this.labelTargetZ = new System.Windows.Forms.Label();
            this.labelTargetY = new System.Windows.Forms.Label();
            this.labelTargetX = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonC3Localization = new System.Windows.Forms.Button();
            this.buttonStopCamera = new System.Windows.Forms.Button();
            this.buttonStartCamera = new System.Windows.Forms.Button();
            this.pictureBoxCamera = new System.Windows.Forms.PictureBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button45Angle = new System.Windows.Forms.Button();
            this.labelAdjCh1 = new System.Windows.Forms.Label();
            this.labelAdjCh0 = new System.Windows.Forms.Label();
            this.buttonJogBPlus = new System.Windows.Forms.Button();
            this.buttonJogBMinus = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonJogZPlus = new System.Windows.Forms.Button();
            this.buttonJogZMinus = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonA5Plus = new System.Windows.Forms.Button();
            this.buttonA5Minus = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.labelPositionCart = new System.Windows.Forms.Label();
            this.labelPositionJointsCurrent = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelSensorStatus = new System.Windows.Forms.Label();
            this.labelLoadStatus = new System.Windows.Forms.Label();
            this.labelNormCh0 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelNormCh1 = new System.Windows.Forms.Label();
            this.textBoxLogMessages = new System.Windows.Forms.TextBox();
            this.labelBaseLineCh1 = new System.Windows.Forms.Label();
            this.labelBaseLineCh0 = new System.Windows.Forms.Label();
            this.buttonPauseReading = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxPort)).BeginInit();
            this.tabCamera.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxIPAddress
            // 
            this.textBoxIPAddress.FormattingEnabled = true;
            this.textBoxIPAddress.Items.AddRange(new object[] {
            "127.0.0.1",
            "192.168.3.11"});
            this.textBoxIPAddress.Location = new System.Drawing.Point(22, 58);
            this.textBoxIPAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxIPAddress.Name = "textBoxIPAddress";
            this.textBoxIPAddress.Size = new System.Drawing.Size(155, 29);
            this.textBoxIPAddress.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP Address:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port:";
            // 
            // CRIConnect
            // 
            this.CRIConnect.Location = new System.Drawing.Point(22, 130);
            this.CRIConnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CRIConnect.Name = "CRIConnect";
            this.CRIConnect.Size = new System.Drawing.Size(155, 31);
            this.CRIConnect.TabIndex = 3;
            this.CRIConnect.Text = "CRI Connect";
            this.CRIConnect.UseVisualStyleBackColor = true;
            this.CRIConnect.Click += new System.EventHandler(this.CRIConnect_Click);
            // 
            // labelConnectionStatus
            // 
            this.labelConnectionStatus.AutoSize = true;
            this.labelConnectionStatus.Location = new System.Drawing.Point(18, 164);
            this.labelConnectionStatus.Name = "labelConnectionStatus";
            this.labelConnectionStatus.Size = new System.Drawing.Size(144, 21);
            this.labelConnectionStatus.TabIndex = 4;
            this.labelConnectionStatus.Text = "Not Connected";
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(22, 188);
            this.buttonDisconnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(155, 31);
            this.buttonDisconnect.TabIndex = 5;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(72, 94);
            this.textBoxPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(104, 28);
            this.textBoxPort.TabIndex = 6;
            this.textBoxPort.Value = new decimal(new int[] {
            3920,
            0,
            0,
            0});
            // 
            // tabCamera
            // 
            this.tabCamera.Controls.Add(this.tabPage1);
            this.tabCamera.Controls.Add(this.tabPage2);
            this.tabCamera.Location = new System.Drawing.Point(220, 12);
            this.tabCamera.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabCamera.Name = "tabCamera";
            this.tabCamera.SelectedIndex = 0;
            this.tabCamera.Size = new System.Drawing.Size(1189, 733);
            this.tabCamera.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonSendC3);
            this.tabPage1.Controls.Add(this.labelTargetAngle);
            this.tabPage1.Controls.Add(this.labelTargetZ);
            this.tabPage1.Controls.Add(this.labelTargetY);
            this.tabPage1.Controls.Add(this.labelTargetX);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.buttonC3Localization);
            this.tabPage1.Controls.Add(this.buttonStopCamera);
            this.tabPage1.Controls.Add(this.buttonStartCamera);
            this.tabPage1.Controls.Add(this.pictureBoxCamera);
            this.tabPage1.Location = new System.Drawing.Point(4, 31);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage1.Size = new System.Drawing.Size(1181, 698);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Camera";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonSendC3
            // 
            this.buttonSendC3.Location = new System.Drawing.Point(1025, 136);
            this.buttonSendC3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonSendC3.Name = "buttonSendC3";
            this.buttonSendC3.Size = new System.Drawing.Size(150, 36);
            this.buttonSendC3.TabIndex = 13;
            this.buttonSendC3.Text = "Go To Target";
            this.buttonSendC3.UseVisualStyleBackColor = true;
            this.buttonSendC3.Click += new System.EventHandler(this.buttonSendC3_Click);
            // 
            // labelTargetAngle
            // 
            this.labelTargetAngle.AutoSize = true;
            this.labelTargetAngle.Location = new System.Drawing.Point(1062, 314);
            this.labelTargetAngle.Name = "labelTargetAngle";
            this.labelTargetAngle.Size = new System.Drawing.Size(0, 21);
            this.labelTargetAngle.TabIndex = 12;
            // 
            // labelTargetZ
            // 
            this.labelTargetZ.AutoSize = true;
            this.labelTargetZ.Location = new System.Drawing.Point(1062, 266);
            this.labelTargetZ.Name = "labelTargetZ";
            this.labelTargetZ.Size = new System.Drawing.Size(0, 21);
            this.labelTargetZ.TabIndex = 10;
            // 
            // labelTargetY
            // 
            this.labelTargetY.AutoSize = true;
            this.labelTargetY.Location = new System.Drawing.Point(1062, 236);
            this.labelTargetY.Name = "labelTargetY";
            this.labelTargetY.Size = new System.Drawing.Size(0, 21);
            this.labelTargetY.TabIndex = 9;
            // 
            // labelTargetX
            // 
            this.labelTargetX.AutoSize = true;
            this.labelTargetX.Location = new System.Drawing.Point(1062, 206);
            this.labelTargetX.Name = "labelTargetX";
            this.labelTargetX.Size = new System.Drawing.Size(0, 21);
            this.labelTargetX.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1021, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 21);
            this.label3.TabIndex = 4;
            this.label3.Text = "Target Point:";
            // 
            // buttonC3Localization
            // 
            this.buttonC3Localization.Location = new System.Drawing.Point(1025, 96);
            this.buttonC3Localization.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonC3Localization.Name = "buttonC3Localization";
            this.buttonC3Localization.Size = new System.Drawing.Size(150, 34);
            this.buttonC3Localization.TabIndex = 3;
            this.buttonC3Localization.Text = "Locate C3";
            this.buttonC3Localization.UseVisualStyleBackColor = true;
            this.buttonC3Localization.Click += new System.EventHandler(this.buttonC3Localization_Click);
            // 
            // buttonStopCamera
            // 
            this.buttonStopCamera.Location = new System.Drawing.Point(1025, 48);
            this.buttonStopCamera.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonStopCamera.Name = "buttonStopCamera";
            this.buttonStopCamera.Size = new System.Drawing.Size(150, 31);
            this.buttonStopCamera.TabIndex = 2;
            this.buttonStopCamera.Text = "Camera OFF";
            this.buttonStopCamera.UseVisualStyleBackColor = true;
            this.buttonStopCamera.Click += new System.EventHandler(this.buttonStopCamera_Click_1);
            // 
            // buttonStartCamera
            // 
            this.buttonStartCamera.Location = new System.Drawing.Point(1025, 8);
            this.buttonStartCamera.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonStartCamera.Name = "buttonStartCamera";
            this.buttonStartCamera.Size = new System.Drawing.Size(150, 33);
            this.buttonStartCamera.TabIndex = 1;
            this.buttonStartCamera.Text = "Camera ON";
            this.buttonStartCamera.UseVisualStyleBackColor = true;
            this.buttonStartCamera.Click += new System.EventHandler(this.buttonStartCamera_Click_1);
            // 
            // pictureBoxCamera
            // 
            this.pictureBoxCamera.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxCamera.Location = new System.Drawing.Point(3, 4);
            this.pictureBoxCamera.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBoxCamera.Name = "pictureBoxCamera";
            this.pictureBoxCamera.Size = new System.Drawing.Size(1012, 690);
            this.pictureBoxCamera.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCamera.TabIndex = 0;
            this.pictureBoxCamera.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button45Angle);
            this.tabPage2.Controls.Add(this.buttonJogBPlus);
            this.tabPage2.Controls.Add(this.buttonJogBMinus);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.buttonJogZPlus);
            this.tabPage2.Controls.Add(this.buttonJogZMinus);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.buttonA5Plus);
            this.tabPage2.Controls.Add(this.buttonA5Minus);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.labelPositionCart);
            this.tabPage2.Controls.Add(this.labelPositionJointsCurrent);
            this.tabPage2.Location = new System.Drawing.Point(4, 31);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage2.Size = new System.Drawing.Size(1181, 698);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Manual Control";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button45Angle
            // 
            this.button45Angle.Location = new System.Drawing.Point(738, 48);
            this.button45Angle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button45Angle.Name = "button45Angle";
            this.button45Angle.Size = new System.Drawing.Size(153, 37);
            this.button45Angle.TabIndex = 16;
            this.button45Angle.Text = "Move to 45°";
            this.button45Angle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button45Angle.UseVisualStyleBackColor = true;
            this.button45Angle.Click += new System.EventHandler(this.button45Angle_Click);
            // 
            // labelAdjCh1
            // 
            this.labelAdjCh1.AutoSize = true;
            this.labelAdjCh1.Location = new System.Drawing.Point(18, 254);
            this.labelAdjCh1.Name = "labelAdjCh1";
            this.labelAdjCh1.Size = new System.Drawing.Size(78, 21);
            this.labelAdjCh1.TabIndex = 15;
            this.labelAdjCh1.Text = "AdjCh1:";
            // 
            // labelAdjCh0
            // 
            this.labelAdjCh0.AutoSize = true;
            this.labelAdjCh0.Location = new System.Drawing.Point(18, 221);
            this.labelAdjCh0.Name = "labelAdjCh0";
            this.labelAdjCh0.Size = new System.Drawing.Size(78, 21);
            this.labelAdjCh0.TabIndex = 14;
            this.labelAdjCh0.Text = "AdjCh0:";
            // 
            // buttonJogBPlus
            // 
            this.buttonJogBPlus.Location = new System.Drawing.Point(652, 175);
            this.buttonJogBPlus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonJogBPlus.Name = "buttonJogBPlus";
            this.buttonJogBPlus.Size = new System.Drawing.Size(67, 37);
            this.buttonJogBPlus.TabIndex = 11;
            this.buttonJogBPlus.Text = "+";
            this.buttonJogBPlus.UseVisualStyleBackColor = true;
            this.buttonJogBPlus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonJogBPlus_MouseDown);
            this.buttonJogBPlus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonA5Minus_MouseUp);
            // 
            // buttonJogBMinus
            // 
            this.buttonJogBMinus.Location = new System.Drawing.Point(568, 175);
            this.buttonJogBMinus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonJogBMinus.Name = "buttonJogBMinus";
            this.buttonJogBMinus.Size = new System.Drawing.Size(67, 37);
            this.buttonJogBMinus.TabIndex = 10;
            this.buttonJogBMinus.Text = "-";
            this.buttonJogBMinus.UseVisualStyleBackColor = true;
            this.buttonJogBMinus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonJogBMinus_MouseDown);
            this.buttonJogBMinus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonA5Minus_MouseUp);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(456, 183);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 21);
            this.label6.TabIndex = 9;
            this.label6.Text = "Tilt Angle:";
            // 
            // buttonJogZPlus
            // 
            this.buttonJogZPlus.Location = new System.Drawing.Point(652, 111);
            this.buttonJogZPlus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonJogZPlus.Name = "buttonJogZPlus";
            this.buttonJogZPlus.Size = new System.Drawing.Size(67, 37);
            this.buttonJogZPlus.TabIndex = 8;
            this.buttonJogZPlus.Text = "+";
            this.buttonJogZPlus.UseVisualStyleBackColor = true;
            this.buttonJogZPlus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonJogZPlus_MouseDown);
            this.buttonJogZPlus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonJogZMinus_MouseUp);
            // 
            // buttonJogZMinus
            // 
            this.buttonJogZMinus.Location = new System.Drawing.Point(568, 111);
            this.buttonJogZMinus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonJogZMinus.Name = "buttonJogZMinus";
            this.buttonJogZMinus.Size = new System.Drawing.Size(67, 37);
            this.buttonJogZMinus.TabIndex = 7;
            this.buttonJogZMinus.Text = "-";
            this.buttonJogZMinus.UseVisualStyleBackColor = true;
            this.buttonJogZMinus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonJogZMinus_MouseDown);
            this.buttonJogZMinus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonJogZMinus_MouseUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(412, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(140, 21);
            this.label5.TabIndex = 6;
            this.label5.Text = "Forward/Back:";
            // 
            // buttonA5Plus
            // 
            this.buttonA5Plus.Location = new System.Drawing.Point(652, 48);
            this.buttonA5Plus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonA5Plus.Name = "buttonA5Plus";
            this.buttonA5Plus.Size = new System.Drawing.Size(67, 37);
            this.buttonA5Plus.TabIndex = 5;
            this.buttonA5Plus.Text = "+";
            this.buttonA5Plus.UseVisualStyleBackColor = true;
            this.buttonA5Plus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonA5Plus_MouseDown);
            this.buttonA5Plus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonA5Minus_MouseUp);
            // 
            // buttonA5Minus
            // 
            this.buttonA5Minus.Location = new System.Drawing.Point(568, 48);
            this.buttonA5Minus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonA5Minus.Name = "buttonA5Minus";
            this.buttonA5Minus.Size = new System.Drawing.Size(67, 37);
            this.buttonA5Minus.TabIndex = 4;
            this.buttonA5Minus.Text = "-";
            this.buttonA5Minus.UseVisualStyleBackColor = true;
            this.buttonA5Minus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonA5Minus_MouseDown);
            this.buttonA5Minus.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonA5Minus_MouseUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(425, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 21);
            this.label4.TabIndex = 3;
            this.label4.Text = "Coil Rotation:";
            // 
            // labelPositionCart
            // 
            this.labelPositionCart.AutoSize = true;
            this.labelPositionCart.Location = new System.Drawing.Point(204, 29);
            this.labelPositionCart.Name = "labelPositionCart";
            this.labelPositionCart.Size = new System.Drawing.Size(81, 21);
            this.labelPositionCart.TabIndex = 2;
            this.labelPositionCart.Text = "Position";
            // 
            // labelPositionJointsCurrent
            // 
            this.labelPositionJointsCurrent.AutoSize = true;
            this.labelPositionJointsCurrent.Location = new System.Drawing.Point(35, 29);
            this.labelPositionJointsCurrent.Name = "labelPositionJointsCurrent";
            this.labelPositionJointsCurrent.Size = new System.Drawing.Size(142, 21);
            this.labelPositionJointsCurrent.TabIndex = 1;
            this.labelPositionJointsCurrent.Text = "Joints Current:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxPort);
            this.groupBox1.Controls.Add(this.buttonDisconnect);
            this.groupBox1.Controls.Add(this.labelConnectionStatus);
            this.groupBox1.Controls.Add(this.CRIConnect);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxIPAddress);
            this.groupBox1.Location = new System.Drawing.Point(12, 38);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(196, 250);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Remote Control";
            // 
            // labelSensorStatus
            // 
            this.labelSensorStatus.AutoSize = true;
            this.labelSensorStatus.Location = new System.Drawing.Point(18, 34);
            this.labelSensorStatus.Name = "labelSensorStatus";
            this.labelSensorStatus.Size = new System.Drawing.Size(113, 21);
            this.labelSensorStatus.TabIndex = 10;
            this.labelSensorStatus.Text = "Calibrating...";
            // 
            // labelLoadStatus
            // 
            this.labelLoadStatus.AutoSize = true;
            this.labelLoadStatus.Location = new System.Drawing.Point(18, 64);
            this.labelLoadStatus.Name = "labelLoadStatus";
            this.labelLoadStatus.Size = new System.Drawing.Size(82, 21);
            this.labelLoadStatus.TabIndex = 11;
            this.labelLoadStatus.Text = "No Load";
            // 
            // labelNormCh0
            // 
            this.labelNormCh0.AutoSize = true;
            this.labelNormCh0.Location = new System.Drawing.Point(18, 159);
            this.labelNormCh0.Name = "labelNormCh0";
            this.labelNormCh0.Size = new System.Drawing.Size(85, 21);
            this.labelNormCh0.TabIndex = 12;
            this.labelNormCh0.Text = "NrmCh0:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonPauseReading);
            this.groupBox2.Controls.Add(this.labelBaseLineCh1);
            this.groupBox2.Controls.Add(this.labelBaseLineCh0);
            this.groupBox2.Controls.Add(this.labelNormCh1);
            this.groupBox2.Controls.Add(this.labelAdjCh1);
            this.groupBox2.Controls.Add(this.labelNormCh0);
            this.groupBox2.Controls.Add(this.labelAdjCh0);
            this.groupBox2.Controls.Add(this.labelLoadStatus);
            this.groupBox2.Controls.Add(this.labelSensorStatus);
            this.groupBox2.Location = new System.Drawing.Point(12, 299);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(196, 442);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sensors";
            // 
            // labelNormCh1
            // 
            this.labelNormCh1.AutoSize = true;
            this.labelNormCh1.Location = new System.Drawing.Point(18, 191);
            this.labelNormCh1.Name = "labelNormCh1";
            this.labelNormCh1.Size = new System.Drawing.Size(85, 21);
            this.labelNormCh1.TabIndex = 13;
            this.labelNormCh1.Text = "NrmCh1:";
            // 
            // textBoxLogMessages
            // 
            this.textBoxLogMessages.Location = new System.Drawing.Point(8, 752);
            this.textBoxLogMessages.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxLogMessages.Multiline = true;
            this.textBoxLogMessages.Name = "textBoxLogMessages";
            this.textBoxLogMessages.Size = new System.Drawing.Size(1401, 219);
            this.textBoxLogMessages.TabIndex = 7;
            // 
            // labelBaseLineCh1
            // 
            this.labelBaseLineCh1.AutoSize = true;
            this.labelBaseLineCh1.Location = new System.Drawing.Point(18, 126);
            this.labelBaseLineCh1.Name = "labelBaseLineCh1";
            this.labelBaseLineCh1.Size = new System.Drawing.Size(74, 21);
            this.labelBaseLineCh1.TabIndex = 17;
            this.labelBaseLineCh1.Text = "BLCh1:";
            // 
            // labelBaseLineCh0
            // 
            this.labelBaseLineCh0.AutoSize = true;
            this.labelBaseLineCh0.Location = new System.Drawing.Point(18, 94);
            this.labelBaseLineCh0.Name = "labelBaseLineCh0";
            this.labelBaseLineCh0.Size = new System.Drawing.Size(74, 21);
            this.labelBaseLineCh0.TabIndex = 16;
            this.labelBaseLineCh0.Text = "BLCh0:";
            // 
            // buttonPauseReading
            // 
            this.buttonPauseReading.Location = new System.Drawing.Point(25, 291);
            this.buttonPauseReading.Name = "buttonPauseReading";
            this.buttonPauseReading.Size = new System.Drawing.Size(136, 33);
            this.buttonPauseReading.TabIndex = 18;
            this.buttonPauseReading.Text = "Pause";
            this.buttonPauseReading.UseVisualStyleBackColor = true;
            this.buttonPauseReading.Click += new System.EventHandler(this.buttonPauseReading_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1421, 985);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabCamera);
            this.Controls.Add(this.textBoxLogMessages);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Main System";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.textBoxPort)).EndInit();
            this.tabCamera.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCamera)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox textBoxIPAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CRIConnect;
        private System.Windows.Forms.Label labelConnectionStatus;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.NumericUpDown textBoxPort;
        private System.Windows.Forms.TabControl tabCamera;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.PictureBox pictureBoxCamera;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button buttonStopCamera;
        private System.Windows.Forms.Button buttonStartCamera;
        private System.Windows.Forms.Button buttonC3Localization;
        private System.Windows.Forms.Label labelTargetZ;
        private System.Windows.Forms.Label labelTargetY;
        private System.Windows.Forms.Label labelTargetX;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelTargetAngle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelSensorStatus;
        private System.Windows.Forms.Label labelLoadStatus;
        private System.Windows.Forms.Label labelNormCh0;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelNormCh1;
        private System.Windows.Forms.Label labelPositionCart;
        private System.Windows.Forms.Label labelPositionJointsCurrent;
        private System.Windows.Forms.Button buttonA5Minus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonA5Plus;
        private System.Windows.Forms.Button buttonSendC3;
        internal System.Windows.Forms.TextBox textBoxLogMessages;
        private System.Windows.Forms.Button buttonJogZPlus;
        private System.Windows.Forms.Button buttonJogZMinus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonJogBPlus;
        private System.Windows.Forms.Button buttonJogBMinus;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelAdjCh1;
        private System.Windows.Forms.Label labelAdjCh0;
        private System.Windows.Forms.Button button45Angle;
        private System.Windows.Forms.Label labelBaseLineCh1;
        private System.Windows.Forms.Label labelBaseLineCh0;
        private System.Windows.Forms.Button buttonPauseReading;
    }
}

