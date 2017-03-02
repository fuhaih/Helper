namespace stationTest
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
            this.textBoxOperatorID = new System.Windows.Forms.TextBox();
            this.textBoxOperatorSecret = new System.Windows.Forms.TextBox();
            this.textBoxDataSecret = new System.Windows.Forms.TextBox();
            this.textBoxDataIV = new System.Windows.Forms.TextBox();
            this.labelUrl = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxSigSecret = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxUrl = new System.Windows.Forms.ComboBox();
            this.buttonTest = new System.Windows.Forms.Button();
            this.buttonStatus = new System.Windows.Forms.Button();
            this.buttonStats = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxOperatorID
            // 
            this.textBoxOperatorID.Location = new System.Drawing.Point(109, 72);
            this.textBoxOperatorID.Name = "textBoxOperatorID";
            this.textBoxOperatorID.Size = new System.Drawing.Size(392, 21);
            this.textBoxOperatorID.TabIndex = 1;
            this.textBoxOperatorID.Text = "332670086";
            // 
            // textBoxOperatorSecret
            // 
            this.textBoxOperatorSecret.Location = new System.Drawing.Point(109, 111);
            this.textBoxOperatorSecret.Name = "textBoxOperatorSecret";
            this.textBoxOperatorSecret.Size = new System.Drawing.Size(392, 21);
            this.textBoxOperatorSecret.TabIndex = 2;
            this.textBoxOperatorSecret.Text = "1234567890abcdef";
            // 
            // textBoxDataSecret
            // 
            this.textBoxDataSecret.Location = new System.Drawing.Point(109, 148);
            this.textBoxDataSecret.Name = "textBoxDataSecret";
            this.textBoxDataSecret.Size = new System.Drawing.Size(392, 21);
            this.textBoxDataSecret.TabIndex = 3;
            this.textBoxDataSecret.Text = "1234567890abcdef";
            // 
            // textBoxDataIV
            // 
            this.textBoxDataIV.Location = new System.Drawing.Point(109, 190);
            this.textBoxDataIV.Name = "textBoxDataIV";
            this.textBoxDataIV.Size = new System.Drawing.Size(392, 21);
            this.textBoxDataIV.TabIndex = 4;
            this.textBoxDataIV.Text = "1234567890abcdef";
            // 
            // labelUrl
            // 
            this.labelUrl.AutoSize = true;
            this.labelUrl.Location = new System.Drawing.Point(41, 39);
            this.labelUrl.Name = "labelUrl";
            this.labelUrl.Size = new System.Drawing.Size(23, 12);
            this.labelUrl.TabIndex = 5;
            this.labelUrl.Text = "Url";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "运营商ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "运营商密钥";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "数据密钥";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "密钥向量";
            // 
            // textBoxSigSecret
            // 
            this.textBoxSigSecret.Location = new System.Drawing.Point(109, 231);
            this.textBoxSigSecret.Name = "textBoxSigSecret";
            this.textBoxSigSecret.Size = new System.Drawing.Size(392, 21);
            this.textBoxSigSecret.TabIndex = 10;
            this.textBoxSigSecret.Text = "1234567890abcdef";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 234);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "签名密钥";
            // 
            // comboBoxUrl
            // 
            this.comboBoxUrl.FormattingEnabled = true;
            this.comboBoxUrl.Items.AddRange(new object[] {
            "http://210.14.69.112:8600/query_stations_info",
            "http://210.14.69.112:8600/query_station_status",
            "http://210.14.69.112:8600/query_station_stats",
            "http://210.14.69.112:8600/query_token"});
            this.comboBoxUrl.Location = new System.Drawing.Point(109, 36);
            this.comboBoxUrl.Name = "comboBoxUrl";
            this.comboBoxUrl.Size = new System.Drawing.Size(392, 20);
            this.comboBoxUrl.TabIndex = 12;
            this.comboBoxUrl.Text = "http://m.evchargings.com:8600/query_stations_info";
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(43, 285);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 13;
            this.buttonTest.Text = "测试";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // buttonStatus
            // 
            this.buttonStatus.Location = new System.Drawing.Point(558, 36);
            this.buttonStatus.Name = "buttonStatus";
            this.buttonStatus.Size = new System.Drawing.Size(85, 23);
            this.buttonStatus.TabIndex = 14;
            this.buttonStatus.Text = "查询状态信息";
            this.buttonStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonStatus.UseVisualStyleBackColor = true;
            this.buttonStatus.Click += new System.EventHandler(this.buttonStatus_Click);
            // 
            // buttonStats
            // 
            this.buttonStats.Location = new System.Drawing.Point(558, 103);
            this.buttonStats.Name = "buttonStats";
            this.buttonStats.Size = new System.Drawing.Size(85, 23);
            this.buttonStats.TabIndex = 15;
            this.buttonStats.Text = "查询统计信息";
            this.buttonStats.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonStats.UseVisualStyleBackColor = true;
            this.buttonStats.Click += new System.EventHandler(this.buttonStats_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 335);
            this.Controls.Add(this.buttonStats);
            this.Controls.Add(this.buttonStatus);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.comboBoxUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSigSecret);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelUrl);
            this.Controls.Add(this.textBoxDataIV);
            this.Controls.Add(this.textBoxDataSecret);
            this.Controls.Add(this.textBoxOperatorSecret);
            this.Controls.Add(this.textBoxOperatorID);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxOperatorID;
        private System.Windows.Forms.TextBox textBoxOperatorSecret;
        private System.Windows.Forms.TextBox textBoxDataSecret;
        private System.Windows.Forms.TextBox textBoxDataIV;
        private System.Windows.Forms.Label labelUrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxSigSecret;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxUrl;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Button buttonStatus;
        private System.Windows.Forms.Button buttonStats;
    }
}

