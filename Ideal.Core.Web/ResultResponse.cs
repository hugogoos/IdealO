namespace Ideal.Core.Web
{
    /// <summary>
    /// 统一返回类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultResponse<T> : ResultResponse
    {
        /// <summary>
        /// 获取 返回数据
        /// </summary>
        public T? Data { get; set; }
    }

    /// <summary>
    /// 统一返回类型
    /// </summary>
    public class ResultResponse
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSucceed { get; set; }

        /// <summary>
        /// 操作结果类型
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 获取 消息内容
        /// </summary>
        public string Msg { get; set; }
    }
}
