using System.ComponentModel;
using Xunit;

namespace Ideal.Core.Common.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal enum StatusEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 0x00,

        /// <summary>
        /// 待机
        /// </summary>
        [Description("待机")]
        Standby = 0x01,

        /// <summary>
        /// 离线
        /// </summary>
        [Description("离线")]
        Offline = 0x02,
    }

    [Flags]
    internal enum TypeEnum
    {
        /// <summary>
        /// Http  
        /// </summary>
        [Description("Http协议")]
        Http = 1,

        /// <summary>
        /// Udp
        /// </summary>
        [Description("Udp协议")]
        Udp = 2,

        /// <summary>
        /// Http,Udp
        /// </summary>
        [Description("Http协议,Udp协议")]
        HttpAndUdp = 3,
    }
}
