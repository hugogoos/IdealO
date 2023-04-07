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
        /// ����
        /// </summary>
        [Description("����")]
        Normal = 0x00,

        /// <summary>
        /// ����
        /// </summary>
        [Description("����")]
        Standby = 0x01,

        /// <summary>
        /// ����
        /// </summary>
        [Description("����")]
        Offline = 0x02,
    }

    [Flags]
    internal enum TypeEnum
    {
        /// <summary>
        /// Http  
        /// </summary>
        [Description("HttpЭ��")]
        Http = 1,

        /// <summary>
        /// Udp
        /// </summary>
        [Description("UdpЭ��")]
        Udp = 2,

        /// <summary>
        /// Http,Udp
        /// </summary>
        [Description("HttpЭ��,UdpЭ��")]
        HttpAndUdp = 3,
    }
}
