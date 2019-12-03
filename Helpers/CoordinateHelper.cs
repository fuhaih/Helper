using System;
namespace Helpers
{
    /// <summary>
    /// 坐标
    /// </summary>
    public class CoordinateHelper
    {
        //  
        // Krasovsky 1940  
        //  
        // a = 6378245.0, 1/f = 298.3  
        // b = a * (1 - f)  
        // ee = (a^2 - b^2) / a^2;  
        const double a = 6378245.0;
        const double ee = 0.00669342162296594323;
        /// <summary>
        /// gps坐标转换为火星坐标
        /// </summary>
        /// <param name="wgLat">gps纬度</param>
        /// <param name="wgLon">gps经度</param>
        /// <param name="mgLat">火星坐标纬度</param>
        /// <param name="mgLon">火星坐标经度</param>
        public static void transform(double wgLat, double wgLon, out double mgLat, out double mgLon)
        {
            if (outOfChina(wgLat, wgLon))
            {
                mgLat = wgLat;
                mgLon = wgLon;
                return;
            }
            double dLat = transformLat(wgLon - 105.0, wgLat - 35.0);
            double dLon = transformLon(wgLon - 105.0, wgLat - 35.0);
            double radLat = wgLat / 180.0 * Math.PI;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * Math.PI);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * Math.PI);
            mgLat = wgLat + dLat;
            mgLon = wgLon + dLon;
        }
        static bool outOfChina(double lat, double lon)
        {
            if (lon < 72.004 || lon > 137.8347)
                return true;
            if (lat < 0.8293 || lat > 55.8271)
                return true;
            return false;
        }
        static double transformLat(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x * Math.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * Math.PI) + 40.0 * Math.Sin(y / 3.0 * Math.PI)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * Math.PI) + 320 * Math.Sin(y * Math.PI / 30.0)) * 2.0 / 3.0;
            return ret;
        }
        static double transformLon(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * Math.PI) + 20.0 * Math.Sin(2.0 * x * Math.PI)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * Math.PI) + 40.0 * Math.Sin(x / 3.0 * Math.PI)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * Math.PI) + 300.0 * Math.Sin(x / 30.0 * Math.PI)) * 2.0 / 3.0;
            return ret;
        }
        /// <summary>
        /// 火星坐标转换为百度地图坐标
        /// </summary>
        /// <param name="gg_lat">火星坐标纬度</param>
        /// <param name="gg_lon">火星坐标经度</param>
        /// <param name="bd_lat">百度坐标纬度</param>
        /// <param name="bd_lon">百度坐标经度</param>
        public static void bd_encrypt(double gg_lat, double gg_lon, out double bd_lat, out double bd_lon)
        {
            double x = gg_lon, y = gg_lat;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * Math.PI);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * Math.PI);
            bd_lon = z * Math.Cos(theta) + 0.0065;
            bd_lat = z * Math.Sin(theta) + 0.006;
        }
        /// <summary>
        /// 百度地图坐标转换为火星坐标
        /// </summary>
        /// <param name="bd_lat">百度坐标纬度</param>
        /// <param name="bd_lon">百度坐标经度</param>
        /// <param name="gg_lat">火星坐标纬度</param>
        /// <param name="gg_lon">火星坐标经度</param>
        public static void bd_decrypt(double bd_lat, double bd_lon, out double gg_lat, out double gg_lon)
        {
            double x = bd_lon - 0.0065, y = bd_lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * Math.PI);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * Math.PI);
            gg_lon = z * Math.Cos(theta);
            gg_lat = z * Math.Sin(theta);
        }
    }
}
