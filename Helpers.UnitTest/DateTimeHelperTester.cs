using Helpers.DateTimeExtend;
using System;
using System.Globalization;
using Xunit;

namespace Helpers.UnitTest
{
    public class DateTimeHelperTester 
    {
        [Fact]
        public void TestMethod1()
        {
            Assert.Equal(1,1);
        }
        [Theory]
        [InlineData("2019/10/01 21:59:59.001", "2019/10/01 21:55:00.000", 5)] //五分钟
        [InlineData("2019/10/01 21:59:59.001", "2019/10/01 21:50:00.000", 10)] //十分钟
        [InlineData("2019/10/01 21:59:59.001", "2019/10/01 21:45:00.000", 15)] //十五分钟
        [InlineData("2019/10/01 21:59:59.001", "2019/10/01 21:30:00.000", 30)] //30分钟
        [InlineData("2019/10/01 21:55:00.000", "2019/10/01 21:55:00.000", 5)] //五分钟
        [InlineData("2019/10/01 21:50:00.000", "2019/10/01 21:50:00.000", 10)] //十分钟
        [InlineData("2019/10/01 21:45:00.000", "2019/10/01 21:45:00.000", 15)] //十五分钟
        [InlineData("2019/10/01 21:30:00.000", "2019/10/01 21:30:00.000", 30)] //30分钟
        public void Test_PreMinutes(string time,string target,int timespan)
        {
            DateTime testtime = DateTime.ParseExact(time,"yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            DateTime targettime = DateTime.ParseExact(target, "yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            DateTime result = testtime.PreMinutes(timespan);
            Assert.Equal(result,targettime);
        }
        [Theory]
        [InlineData("2019/10/01 23:59:59.001", "2019/10/02 00:00:00.000", 5)] //五分钟 跨天
        [InlineData("2019/10/01 23:59:59.001", "2019/10/02 00:00:00.000", 10)] //十分钟 跨天
        [InlineData("2019/10/01 23:59:59.001", "2019/10/02 00:00:00.000", 15)] //十五分钟 跨天
        [InlineData("2019/10/01 23:59:59.001", "2019/10/02 00:00:00.000", 30)] //30分钟 跨天
        [InlineData("2019/10/01 21:55:00.000", "2019/10/01 21:55:00.000", 5)] //五分钟
        [InlineData("2019/10/01 21:50:00.000", "2019/10/01 21:50:00.000", 10)] //十分钟
        [InlineData("2019/10/01 21:45:00.000", "2019/10/01 21:45:00.000", 15)] //十五分钟
        [InlineData("2019/10/01 21:30:00.000", "2019/10/01 21:30:00.000", 30)] //30分钟
        [InlineData("2019/09/30 23:59:59.001", "2019/10/01 00:00:00.000", 5)] //五分钟 跨月份
        [InlineData("2019/09/30 23:59:59.001", "2019/10/01 00:00:00.000", 10)] //十分钟 跨月份
        [InlineData("2019/09/30 23:59:59.001", "2019/10/01 00:00:00.000", 15)] //十五分钟 跨月份
        [InlineData("2019/09/30 23:59:59.001", "2019/10/01 00:00:00.000", 30)] //30分钟 跨月份
        public void Test_NextMinutes(string time, string target, int timespan)
        {
            DateTime testtime = DateTime.ParseExact(time, "yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            DateTime targettime = DateTime.ParseExact(target, "yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            DateTime result = testtime.NextMinutes(timespan);
            Assert.Equal(result, targettime);
        }
        [Theory]
        [InlineData("2019/10/01 23:59:59.001", "2019/10/02 00:00:00.000", 5)] //五分钟 跨天
        [InlineData("2019/10/01 23:59:59.001", "2019/10/02 00:00:00.000", 10)] //十分钟 跨天
        [InlineData("2019/10/01 23:59:59.001", "2019/10/02 00:00:00.000", 15)] //十五分钟 跨天
        [InlineData("2019/10/01 23:59:59.001", "2019/10/02 00:00:00.000", 30)] //30分钟 跨天
        [InlineData("2019/10/01 21:52:30.000", "2019/10/01 21:55:00.000", 5)] //五分钟
        [InlineData("2019/10/01 21:45:00.000", "2019/10/01 21:50:00.000", 10)] //十分钟
        [InlineData("2019/10/01 21:37:30.000", "2019/10/01 21:45:00.000", 15)] //十五分钟
        [InlineData("2019/10/01 21:15:00.000", "2019/10/01 21:30:00.000", 30)] //30分钟
        [InlineData("2019/09/30 23:59:59.001", "2019/10/01 00:00:00.000", 5)] //五分钟 跨月份
        [InlineData("2019/09/30 23:59:59.001", "2019/10/01 00:00:00.000", 10)] //十分钟 跨月份
        [InlineData("2019/09/30 23:59:59.001", "2019/10/01 00:00:00.000", 15)] //十五分钟 跨月份
        [InlineData("2019/09/30 23:59:59.001", "2019/10/01 00:00:00.000", 30)] //30分钟 跨月份
        public void Test_NearMinutes(string time, string target, int timespan)
        {
            DateTime testtime = DateTime.ParseExact(time, "yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            DateTime targettime = DateTime.ParseExact(target, "yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            DateTime result = testtime.NearMinutes(timespan);
            Assert.Equal(result, targettime);
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(55)]
        [InlineData(60)]
        [InlineData(100)]
        public void Throw_next_outofrange(int timespan)
        {
            DateTime now = DateTime.Now;
            Assert.Throws<ArgumentOutOfRangeException>(() => now.NextMinutes(timespan));
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(55)]
        [InlineData(60)]
        [InlineData(100)]

        public void Throw_pre_outofrange(int timespan)
        {
            DateTime now = DateTime.Now;
            Assert.Throws<ArgumentOutOfRangeException>(() => now.PreMinutes(timespan));
        }
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(55)]
        [InlineData(60)]
        [InlineData(100)]
        public void Throw_near_outofrange(int timespan)
        {
            DateTime now = DateTime.Now;
            Assert.Throws<ArgumentOutOfRangeException>(() => now.NextMinutes(timespan));
        }
    }
}
