using System;

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
        public string CurrentOrganizationId { get; set; }

        /// <summary>
        /// 机构Id集合
        /// </summary>
        public string[] OrganizationIds { get; set; }

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
            if (!string.IsNullOrWhiteSpace(CurrentOrganizationId))
            {
                if (OrganizationIds == null || OrganizationIds.Length == 0)
                {
                    OrganizationIds = Array.Empty<string>();
                    OrganizationIds[0] = CurrentOrganizationId;
                }

                return true;
            }

            return false;
        }
    }
}
