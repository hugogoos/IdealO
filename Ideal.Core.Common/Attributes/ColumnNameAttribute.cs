namespace Ideal.Core.Common.Attributes
{
    /// <summary>
    /// 标注次特性之后，可用于将此特性的属性值作为DataTable的列名称
    /// </summary>
    public class ColumnNameAttribute : Attribute
    {
        /// <summary>
        /// 表列名
        /// </summary>
        public string TableColumnName { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        /// <param name="tableColumnName"></param>
        public ColumnNameAttribute(string tableColumnName)
        {
            TableColumnName = tableColumnName;
        }
    }
}
