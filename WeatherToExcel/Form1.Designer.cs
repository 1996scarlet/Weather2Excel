namespace WeatherToExcel
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.GetWeather = new System.Windows.Forms.Button();
            this.text1 = new System.Windows.Forms.Label();
            this.WeatherBar = new System.Windows.Forms.ProgressBar();
            this.AirBar = new System.Windows.Forms.ProgressBar();
            this.text2 = new System.Windows.Forms.Label();
            this.apikeylabel = new System.Windows.Forms.Label();
            this.MainTimer = new System.Windows.Forms.Timer(this.components);
            this.OnlyGetHLJ = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // GetWeather
            // 
            this.GetWeather.Location = new System.Drawing.Point(313, 50);
            this.GetWeather.Name = "GetWeather";
            this.GetWeather.Size = new System.Drawing.Size(75, 66);
            this.GetWeather.TabIndex = 0;
            this.GetWeather.Text = "GetWeather";
            this.GetWeather.UseVisualStyleBackColor = true;
            this.GetWeather.Click += new System.EventHandler(this.GetWeather_Click);
            // 
            // text1
            // 
            this.text1.AutoSize = true;
            this.text1.Location = new System.Drawing.Point(12, 133);
            this.text1.Name = "text1";
            this.text1.Size = new System.Drawing.Size(47, 12);
            this.text1.TabIndex = 1;
            this.text1.Text = "Weather";
            // 
            // WeatherBar
            // 
            this.WeatherBar.Location = new System.Drawing.Point(13, 148);
            this.WeatherBar.Name = "WeatherBar";
            this.WeatherBar.Size = new System.Drawing.Size(375, 23);
            this.WeatherBar.TabIndex = 3;
            // 
            // AirBar
            // 
            this.AirBar.Location = new System.Drawing.Point(14, 194);
            this.AirBar.Name = "AirBar";
            this.AirBar.Size = new System.Drawing.Size(375, 23);
            this.AirBar.TabIndex = 4;
            // 
            // text2
            // 
            this.text2.AutoSize = true;
            this.text2.Location = new System.Drawing.Point(12, 179);
            this.text2.Name = "text2";
            this.text2.Size = new System.Drawing.Size(23, 12);
            this.text2.TabIndex = 5;
            this.text2.Text = "Air";
            // 
            // apikeylabel
            // 
            this.apikeylabel.AutoSize = true;
            this.apikeylabel.Location = new System.Drawing.Point(11, 77);
            this.apikeylabel.Name = "apikeylabel";
            this.apikeylabel.Size = new System.Drawing.Size(101, 12);
            this.apikeylabel.TabIndex = 6;
            this.apikeylabel.Text = "当前状态：未运行";
            // 
            // OnlyGetHLJ
            // 
            this.OnlyGetHLJ.AutoSize = true;
            this.OnlyGetHLJ.Checked = true;
            this.OnlyGetHLJ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OnlyGetHLJ.Location = new System.Drawing.Point(14, 12);
            this.OnlyGetHLJ.Name = "OnlyGetHLJ";
            this.OnlyGetHLJ.Size = new System.Drawing.Size(108, 16);
            this.OnlyGetHLJ.TabIndex = 7;
            this.OnlyGetHLJ.Text = "只获取黑龙江省";
            this.OnlyGetHLJ.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 229);
            this.Controls.Add(this.OnlyGetHLJ);
            this.Controls.Add(this.apikeylabel);
            this.Controls.Add(this.text2);
            this.Controls.Add(this.AirBar);
            this.Controls.Add(this.WeatherBar);
            this.Controls.Add(this.text1);
            this.Controls.Add(this.GetWeather);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GetWeather;
        private System.Windows.Forms.Label text1;
        private System.Windows.Forms.ProgressBar WeatherBar;
        private System.Windows.Forms.ProgressBar AirBar;
        private System.Windows.Forms.Label text2;
        private System.Windows.Forms.Label apikeylabel;
        private System.Windows.Forms.Timer MainTimer;
        private System.Windows.Forms.CheckBox OnlyGetHLJ;
    }
}

