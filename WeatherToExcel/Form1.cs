using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeatherToExcel
{
    public partial class Form1 : Form
    {
        private static string weatherUrl = "https://api.thinkpage.cn/v3/weather/now.json?";
        private static string airUrl = "https://api.thinkpage.cn/v3/air/now.json?scope=all";
        private DataSet cityList = new DataSet();
        private string key;
        private string path;
        private string todaypath;
        private string nowfilepath;
        private string keypath;

        private DateTime when;

        public delegate void TextInvoke(string str);
        public delegate void BarInvoke(int value);
        public Form1()
        {
            InitializeComponent();
            path = Directory.GetCurrentDirectory() + "\\citylist.xls";
            keypath = Directory.GetCurrentDirectory() + "\\key.txt";
            cityList = GetCityList();
        }

        private void UpdateText2(string str)
        {
            text2.Text = str;
        }

        private void UpdateText1(string str)
        {
            text1.Text = str;
        }

        private void UpdateAirBar(int value)
        {
            AirBar.Value = value;
        }

        private void UpdateWeatherBar(int value)
        {
            WeatherBar.Value = value;
        }

        private void GetWeather_Click(object sender, EventArgs e)
        {
            GetWeather.Enabled = false;
            //OnlyGetHLJ.Enabled = false;
            StartFunction();
            //OnlyGetHLJ.Enabled = true;

            MainTimer.Interval = 1000 * 60 * 60;
            MainTimer.Enabled = true;
            MainTimer.Tick += new EventHandler(MainTimer_Tick);
            MainTimer.Start();
            when = DateTime.Now.AddHours(1);
            //System.Threading.Timer timer = 
            //    new System.Threading.Timer(new TimerCallback(StartFunction), null, 0, 1000*60*5);
            //GetWeather.Enabled = true;
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            when = DateTime.Now.AddHours(1);
            StartFunction();
        }

        /*private void TestFunction()
        {
            MessageBox.Show("未找到文件");
        }*/

        private void StartFunction()
        {
            if (!File.Exists(path))
            {
                MessageBox.Show("未找到文件 citylist.xls 请确保该文件和应用程序在同一目录下");
                return;
            }

            if (!File.Exists(keypath))
            {
                MessageBox.Show("未找到文件 key.txt 请确保该文件和应用程序在同一目录下");
                return;
            }

            key = File.ReadAllText(keypath);

            todaypath = Directory.GetCurrentDirectory() + "\\" + DateTime.Now.ToLongDateString();
            nowfilepath = todaypath + "\\" + DateTime.Now.ToString("yyyy年MM月dd日 hh：mm：ss") + ".xls";

            if (!Directory.Exists(todaypath))
            {
                Directory.CreateDirectory(todaypath);
            }

            if (File.Exists(nowfilepath))
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("文件 " + DateTime.Now.ToString("yyyy年MM月dd日 hh：mm：ss") + ".xls 已经存在 是否重新获取数据（将覆盖原数据）", "是否替换", messButton);
                if (dr == DialogResult.OK)
                {
                    File.Delete(nowfilepath);
                }
                else
                {
                    return;
                }
            }

            apikeylabel.Text = "当前状态：正在获取数据";

            WriteCityWeather(cityList.Tables["AllCity"]);
            WriteCityAir(cityList.Tables["BigCity"]);
        }

        /*private void GetAir_Click(object sender, EventArgs e)
        {
            GetAir.Enabled = false;
            //WriteCityAir(cityList);
            GetAir.Enabled = true;
        }*/

        private DataSet GetCityList()
        {
            String sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;"
                + "Data Source=" + path + ";"
                + "Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";

            OleDbConnection objConn = new OleDbConnection(sConnectionString);

            objConn.Open();

            OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM [CN_City$]", objConn);
            OleDbDataAdapter objAdapter1 = new OleDbDataAdapter();

            objAdapter1.SelectCommand = objCmdSelect;
            DataSet objDataset1 = new DataSet();
            objAdapter1.Fill(objDataset1, "AllCity");

            OleDbCommand objCmdSelect2 = new OleDbCommand("SELECT * FROM [CN_City$] WHERE LEVEL = '中国地级市'", objConn);
            objAdapter1.SelectCommand = objCmdSelect2;
            objAdapter1.Fill(objDataset1, "BigCity");

            objConn.Close();

            return objDataset1;
        }

        private async void WriteCityAir(DataTable cityList)
        {
            String sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;"
                + "Data Source=" + nowfilepath + ";"
                + "Extended Properties=Excel 8.0;";

            OleDbConnection objConn = new OleDbConnection(sConnectionString);

            objConn.Open();

            OleDbCommand objCmdSelect = objConn.CreateCommand();

            objCmdSelect.CommandText = "create TABLE Air ("
                + "[SF] VarChar,"
                + "[CITY] VarChar,"
                + "[STATION] VarChar,"
                + "[AQI] VarChar,"
                + "[PM25] VarChar,"
                + "[PM10] VarChar,"
                + "[SO2] VarChar,"
                + "[NO2] VarChar,"
                + "[CO] VarChar,"
                + "[O3] VarChar,"
                + "[LAST_UPDATE] VarChar,"
                + "[CITYID] VarChar,"
                + "[LATITUDE] VarChar,"
                + "[LONGITUDE] VarChar)";
            objCmdSelect.ExecuteNonQuery();

            int count = 0;

            //DataRow[] daraRow = cityList.Rows.Remove();

            if (OnlyGetHLJ.Checked)
            {
                //cityList.DefaultView.Table.Select("SF='黑龙江'");
                //cityList = cityList.DefaultView.ToTable();
                DataView view = new DataView();
                view.Table = cityList;
                view.RowFilter = "SF='黑龙江'";
                cityList = view.ToTable();
            }

            foreach (DataRow row in cityList.Rows)
            {
                string url = airUrl + "&key=" + key + "&location=" + row[0];
                HttpClient client = new HttpClient();
                Task<string> getContentsTask = client.GetStringAsync(url);
                string urlContents = await getContentsTask;

                JObject jsonObject = JObject.Parse(urlContents);

                JArray jsonArray = JArray.Parse(jsonObject["results"].ToString());

                if (jsonArray[0]["air"].ToString() == "")
                {
                    ++count;
                    continue;
                }

                JArray jsonStations = JArray.Parse(jsonArray[0]["air"]["stations"].ToString());

                foreach (JObject station in jsonStations)
                {
                    objCmdSelect.CommandText = "insert into Air "
                        + "values('" + row[3] + "','" + row[4]
                        + "','" + station["station"] + "','" + station["aqi"]
                        + "','" + station["pm25"] + "','" + station["pm10"]
                        + "','" + station["so2"] + "','" + station["no2"]
                        + "','" + station["co"] + "','" + station["o3"]
                        + "','" + station["last_update"] + "','" + row[0]
                        + "','" + station["latitude"] + "','" + station["longitude"]
                        + "')";
                    objCmdSelect.ExecuteNonQuery();
                }

                TextInvoke invoke1 = new TextInvoke(UpdateText2);
                BeginInvoke(invoke1, new object[] { "已完成:" + ++count + "/" + cityList.Rows.Count });
                //text2.Text = "已完成:" + ++count + "/" + cityList.Rows.Count;
                BarInvoke invoke2 = new BarInvoke(UpdateAirBar);
                BeginInvoke(invoke2, new object[] { count * 100 / cityList.Rows.Count });
                //AirBar.Value = count * 100 / cityList.Rows.Count;
            }

            objConn.Close();

            //MessageBox.Show("空气质量数据获取完毕");
        }

        private async void WriteCityWeather(DataTable cityList)
        {
            String sConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;"
                + "Data Source=" + nowfilepath + ";"
                + "Extended Properties=Excel 8.0;";

            OleDbConnection objConn = new OleDbConnection(sConnectionString);

            objConn.Open();

            OleDbCommand objCmdSelect = objConn.CreateCommand();

            objCmdSelect.CommandText = "create TABLE Weather ("
                + "[QX_ZH] VarChar,"
                + "[QX_EN] VarChar,"
                + "[SF] VarChar,"
                + "[CITY] VarChar,"
                + "[TEXT] VarChar,"
                + "[CODE] VarChar,"
                + "[TEMPERATURE] VarChar,"
                + "[FEELS_LIKE] VarChar,"
                + "[PRESSURE] VarChar,"
                + "[HUMIDITY] VarChar,"
                + "[VISIBILITY] VarChar,"
                + "[WIND_DIRECTION] VarChar,"
                + "[WIND_DIRECTION_DEGREE] VarChar,"
                + "[WIND_SPEED] VarChar,"
                + "[WIND_SCALE] VarChar,"
                + "[CLOUDS] VarChar,"
                + "[DEW_POINT] VarChar,"
                + "[LAST_UPDATE] VarChar)";
            objCmdSelect.ExecuteNonQuery();

            int count = 0;

            if (OnlyGetHLJ.Checked)
            {
                //cityList.DefaultView.Table.Select("SF='黑龙江'");
                //cityList = cityList.DefaultView.ToTable();
                DataView view = new DataView();
                view.Table = cityList;
                view.RowFilter = "SF='黑龙江'";
                cityList = view.ToTable();
            }

            foreach (DataRow row in cityList.Rows)
            {
                string url = weatherUrl + "key=" + key + "&location=" + row[0];
                HttpClient client = new HttpClient();
                Task<string> getContentsTask = client.GetStringAsync(url);
                string urlContents = await getContentsTask;

                JObject jsonObj = JObject.Parse(urlContents);
                JArray jsonArray = JArray.Parse(jsonObj["results"].ToString());

                //string body = await client.GetStringAsync(url);
                objCmdSelect.CommandText = "insert into Weather "
                    + "values('" + row[1] + "','" + row[2] + "','" + row[3] + "','" + row[4]
                    + "','" + jsonArray[0]["now"]["text"] + "','" + jsonArray[0]["now"]["code"]
                    + "','" + jsonArray[0]["now"]["temperature"] + "','" + jsonArray[0]["now"]["feels_like"]
                    + "','" + jsonArray[0]["now"]["pressure"] + "','" + jsonArray[0]["now"]["humidity"]
                    + "','" + jsonArray[0]["now"]["visibility"] + "','" + jsonArray[0]["now"]["wind_direction"]
                    + "','" + jsonArray[0]["now"]["wind_direction_degree"] + "','" + jsonArray[0]["now"]["wind_speed"]
                    + "','" + jsonArray[0]["now"]["wind_scale"] + "','" + jsonArray[0]["now"]["clouds"]
                    + "','" + jsonArray[0]["now"]["dew_point"] + "','" + jsonArray[0]["last_update"]
                    + "')";
                objCmdSelect.ExecuteNonQuery();

                TextInvoke invoke1 = new TextInvoke(UpdateText1);
                BeginInvoke(invoke1, new object[] { "已完成:" + ++count + "/" + cityList.Rows.Count });
                BarInvoke invoke2 = new BarInvoke(UpdateWeatherBar);
                BeginInvoke(invoke2, new object[] { count * 100 / cityList.Rows.Count });

                //text1.Text = "已完成:" + ++count + "/" + cityList.Rows.Count;
                //WeatherBar.Value = count * 100 / cityList.Rows.Count;

            }
            objConn.Close();

            apikeylabel.Text = "本轮数据获取完毕\n\n下一轮预计开始时间：" + when.ToLocalTime();
            //MessageBox.Show("本轮数据获取完毕 下一轮开始时间：");
        }
    }
}
