using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Helpers;
using TTBEMS.Framework.Helper;
namespace WebTest
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            switch (context.Request.Form["Action"])
            {
                case "GetRandomValue":
                    {
                        DateTime Now = DateTime.Now;
                        DateTime StartTime = DateTime.Parse(Now.ToString("yyyy/MM/dd HH:00:00"));
                        DateTime setTime = StartTime;
                        DateTime EndTime = StartTime.AddHours(1);
                        string buildID="310105A008";

                        //历史数据
                        byte[] dataBytes = ServicePort.Factory.Get_ModelData(buildID, buildID + "X00U0000", 0, StartTime.Date, StartTime.Date.AddDays(1), TTBEMS.Framework.DataDefine.TDataType.Cons, TTBEMS.Framework.DataDefine.TCycleType.Hour, new TTBEMS.Framework.DataDefine.TCycleDefinition(), null, ServicePort.Key);
                        DataTable data = TTBEMS.Framework.Adapter.UnBoxing(dataBytes).Tables[0];
                        
                        //预测数据
                        TTBEMS.Framework.DataBase.FilterText ft=new TTBEMS.Framework.DataBase.FilterText();
                        ft.where_text=string.Format(@"F_BuildID='{0}' and F_NodeType='{1}'and  F_NodeObjectID='{2}' and F_Time>='{3}' and F_Time<='{4}'",
                                                    buildID, "ModelNode", buildID + "X00U0000", StartTime.Date.ToString(), EndTime.ToString());
                        byte[] bytes= ServicePort.Factory.Select_T_EP_EnergyPrediction_Custom(ft,ServicePort.Key);
                        DataTable tb=TTBEMS.Framework.Adapter.UnBoxing(bytes).Tables[0];

                        //预测为空时用历史补充
                        if (tb == null || tb.Select().Max(m => DateTime.Parse(m["F_Time"].ToString())) < EndTime)
                        {
                            var fillquery = data.Select().OrderByDescending(m => DateTime.Parse(m["F_Time"].ToString())).Take(3).DefaultIfEmpty().CopyToDataTable().Select().ToList();
                            for(int i=0;i<fillquery.Count();i++)
                            {
                                fillquery[i]["F_Time"]=DateTime.Parse(fillquery[i]["F_Time"].ToString()).AddHours(3);
                            }
                            DataTable fillTB= fillquery.CopyToDataTable();
                            tb = data.Select().CopyToDataTable();
                            tb.Merge(fillTB);
                        }

                        //历史前一个小时为空，用预测补充
                        var query = from pre in tb.Select().Where(m => DateTime.Parse(m["F_Time"].ToString()) < setTime)
                                    join da in data.Select()
                                    on DateTime.Parse(pre["F_Time"].ToString()) equals DateTime.Parse(da["F_Time"].ToString()) into temple
                                    from tt in temple.DefaultIfEmpty()
                                    select new
                                    {
                                        time = DateTime.Parse(pre["F_Time"].ToString()),
                                        value = tt == null ? double.Parse(pre["F_Value"].ToString()) : double.Parse(tt["F_Value"].ToString())
                                    };

                        //获取今日总能耗
                        double TotalValue = query.Sum(m => m.value);
                        List<RandomValue> result = new List<RandomValue>();
                        Random rand=new Random(5);

                        //获取开始时间后每秒的总能耗
                        while (StartTime <= EndTime)
                        {
                            DataRow row = tb.Select().Where(m => DateTime.Parse(m["F_Time"].ToString()) == DateTime.Parse(StartTime.ToString("yyyy/MM/dd HH:00:00"))).FirstOrDefault();
                            if (row == null)
                            {
                                continue;
                            }
                            double value = double.Parse(row["F_Value"].ToString());
                            double partValue = value / 3600.0;
                            DateTime startSet = StartTime;
                            DateTime endSet = startSet.AddHours(1);
                            while (startSet < endSet)
                            {
                                TotalValue = rand.Next(6, 15) * partValue / 10.0 +TotalValue;
                                result.Add(new RandomValue
                                {
                                    time = startSet,
                                    value = TotalValue
                                });
                                startSet = startSet.AddSeconds(1);
                            }
                            StartTime = StartTime.AddHours(1);
                        }
                        context.Response.ContentType = "text/plain";
                        List<RandomValue> respon = result.Where(m => m.time > Now).OrderBy(m => m.time).Take(600).ToList();
                        context.Response.Write(respon.ToJsJson());
                    }
                    break;
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class RandomValue
    {
        public DateTime time { get; set;}
        public double value { get; set; }
    }
}