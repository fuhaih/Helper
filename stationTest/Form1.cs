using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChargingStation.Console.Station.Entities;
using ChargingStation.Console.Station.Model;
using ChargingStation.Console.Station.Method;
using ChargingStation.Console.Station.sql;
namespace stationTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            string dataSecret = textBoxDataSecret.Text;
            string dataIV = textBoxDataIV.Text;
            string operatorID = textBoxOperatorID.Text;
            string operatorSecret = textBoxOperatorSecret.Text;
            string sigSecret = textBoxSigSecret.Text;
            string url = comboBoxUrl.Text;
            switch(url)
            {
                case "http://210.14.69.112:8600/query_stations_info":
                    {
                        QueryStationInfo param=new QueryStationInfo{
                            LastQueryTime="",
                            PageNo=1,
                            PageSize=5
                        };
                        query.query_station_info(url, dataSecret, dataIV, operatorID, operatorSecret, sigSecret, param);
                    }
                    break;
                case "http://210.14.69.112:8600/query_station_status":
                    {
                        QueryStationStatus param=new QueryStationStatus{
                            StationIDs=new string[]{
                                "5"
                            }
                        };
                        query.query_station_status(url, dataSecret, dataIV, operatorID, operatorSecret, sigSecret, param);
                    }break;
                case "http://210.14.69.112:8600/query_station_stats":
                    {
                        QueryStationStats param=new QueryStationStats{
                            StationID="5",
                            EndTime=DateTime.Now.ToString("yyyy-MM-dd"),
                            StartTime=DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd")
                        };
                        query.query_station_stats(url, dataSecret, dataIV, operatorID, operatorSecret, sigSecret, param);
                    }
                    break;
                case "http://210.14.69.112:8600/query_token":
                    {
                        query.query_token(url, dataSecret, dataIV, operatorID, operatorSecret);
                    }
                    break;
            }


        }

        public static Token query_token(string url, string key, string iv, string operatorID, string operatorSecret, string sigSecret)
        {
            Token result = query.query_token(url, key, iv, operatorID, operatorSecret);
            return result;
        }

        public static QSIResult query_station_info(string url, string key,string iv,string operatorID,string operatorSecret,string sigSecret)
        {
            QueryStationInfo querystation = new QueryStationInfo()
            {
                LastQueryTime=DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"),
                PageNo=1,
                PageSize=5
            };
            QSIResult result = query.queryInfo<QueryStationInfo, QSIResult>(url, querystation, key, iv, operatorID, operatorSecret, sigSecret);
            return result;
        }

        public static QSStatusResult query_station_status(string url, string key, string iv, string operatorID, string operatorSecret, string sigSecret)
        {
            QueryStationStatus stationstatus = new QueryStationStatus()
            {

            };
            QSStatusResult result = query.queryInfo<QueryStationStatus, QSStatusResult>(url, stationstatus, key, iv, operatorID, operatorSecret, sigSecret);
            return result;
        }

        public static QSStatsResult query_station_stats(string url, string key, string iv, string operatorID, string operatorSecret, string sigSecret)
        {
            QueryStationStats stationstats = new QueryStationStats()
            {
            };
            QSStatsResult result = query.queryInfo<QueryStationStats, QSStatsResult>(url, stationstats, key, iv, operatorID, operatorSecret, sigSecret);
            return result;
        }

        private void buttonStats_Click(object sender, EventArgs e)
        {

        }

        private void buttonStats_Click_1(object sender, EventArgs e)
        {
            string dataSecret = textBoxDataSecret.Text;
            string dataIV = textBoxDataIV.Text;
            string operatorSecret = textBoxOperatorSecret.Text;
            string sigSecret = textBoxSigSecret.Text;
            string url = "http://210.14.69.112:8600/query_station_stats";
            DataTable stationInfo = ChargingStationSql.QueryStationInfo();
            foreach (DataRow row in stationInfo.Rows)
            {
                string operatorID = row["OperatorID"].ToString().Trim();
                string stationID = row["StationID"].ToString().Trim();

                QueryStationStats param = new QueryStationStats
                {
                    StationID = stationID,
                    EndTime = DateTime.Now.Date.AddDays(-4).ToString("yyyy-MM-dd"),
                    StartTime = DateTime.Now.Date.AddDays(-5).ToString("yyyy-MM-dd")
                };
                query.query_station_stats(url, dataSecret, dataIV, operatorID, operatorSecret, sigSecret, param);
            }
        }

        private void buttonStatus_Click(object sender, EventArgs e)
        {
            string dataSecret = textBoxDataSecret.Text;
            string dataIV = textBoxDataIV.Text;
            string operatorSecret = textBoxOperatorSecret.Text;
            string sigSecret = textBoxSigSecret.Text;
            string url = "http://210.14.69.112:8600/query_station_stats";
            DataTable stationInfo = ChargingStationSql.QueryStationInfo();
            foreach (DataRow row in stationInfo.Rows)
            {
                string operatorID = row["OperatorID"].ToString().Trim();
                string stationID = row["StationID"].ToString().Trim();

                QueryStationStatus param=new QueryStationStatus{
                    StationIDs=new string[]{
                        stationID
                    }
                };
                query.query_station_status(url, dataSecret, dataIV, operatorID, operatorSecret, sigSecret, param);
            }
        }
    }
}
