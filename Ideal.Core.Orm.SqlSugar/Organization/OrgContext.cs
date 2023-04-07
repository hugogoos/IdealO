namespace Ideal.Core.Orm.SqlSugar.Organization
{
    /// <summary>
    /// 机构上下文
    /// </summary>
    public class OrgContext
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 机构Id
        /// </summary>
        public string CurrentOrgId { get; set; }

        /// <summary>
        /// 机构Id集合
        /// </summary>
        public string[] OrgIds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GetUserIdAndName => $"{UserId}|{UserName}";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasOrganization()
        {
            if (!string.IsNullOrWhiteSpace(CurrentOrgId))
            {
                if (OrgIds == null || OrgIds.Length == 0)
                {
                    OrgIds = Array.Empty<string>();
                    OrgIds[0] = CurrentOrgId;
                }

                return true;
            }

            return false;
        }
    }
}
