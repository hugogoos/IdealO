using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Xml;

namespace Ideal.Core.Document
{
    /// <summary>
    /// Excel帮助类
    /// </summary>
    public static class ExcelHelper
    {
        /// <summary>
        /// 读取Excel到DataSet
        /// </summary>
        /// <param name="filename">Excel完全名称</param>
        /// <param name="firstRowAsHeader">是否把Excel第一行做为Table表头</param>
        /// <returns>DataSet</returns>
        public static DataSet Read(string filename, bool firstRowAsHeader)
        {
            return Read(filename, null, firstRowAsHeader);
        }

        /// <summary>
        /// 读取Excel到DataSet
        /// </summary>
        /// <param name="filename">Excel完全名称</param>
        /// <param name="sheetname">读取指定的Sheet表</param>
        /// <param name="firstRowAsHeader">是否把Excel第一行做为Table表头</param>
        /// <returns>DataSet</returns>
        public static DataSet Read(string filename, string sheetname = null, bool firstRowAsHeader = false)
        {
            using var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            if (sheetname == null)
            {
                return FillDataSetWithStream(fileStream, filename, firstRowAsHeader);
            }

            return FillDataSetWithStream(fileStream, filename, sheetname, firstRowAsHeader);
        }

        /// <summary>
        /// 读取Excel文件流到DataSet
        /// </summary>
        /// <param name="stream">Excel文件流</param>
        /// <param name="filename">Excel完全名称</param>
        /// <param name="firstRowAsHeader">是否把Excel第一行做为Table表头</param>
        /// <returns>DataSet</returns>
        public static DataSet Read(Stream stream, string filename, bool firstRowAsHeader)
        {
            return Read(stream, filename, null, firstRowAsHeader);
        }

        /// <summary>
        /// 读取Excel文件流到DataSet
        /// </summary>
        /// <param name="stream">Excel文件流</param>
        /// <param name="filename">Excel完全名称</param>
        /// <param name="sheetname">读取指定工作簿名</param>
        /// <param name="firstRowAsHeader">是否把Excel第一行做为Table表头</param>
        /// <returns>DataSet</returns>
        public static DataSet Read(Stream stream, string filename, string sheetname = null, bool firstRowAsHeader = false)
        {
            if (sheetname == null)
            {
                return FillDataSetWithStream(stream, filename, firstRowAsHeader);
            }

            return FillDataSetWithStream(stream, filename, sheetname, firstRowAsHeader);
        }

        private static DataSet FillDataSetWithStream(Stream stream, string filename, bool firstRowAsHeader)
        {
            using var ds = new DataSet();
            CreateWorkBookAndEvaluator(stream, filename, out var workbook, out var evaluator);
            FillDataSetWithAllSheets(ds, workbook, evaluator, firstRowAsHeader);
            return ds;
        }

        private static DataSet FillDataSetWithStream(Stream stream, string filename, string sheetname, bool firstRowAsHeader)
        {
            using var ds = new DataSet();
            CreateWorkBookAndEvaluator(stream, filename, out var workbook, out var evaluator);
            FillDataSetWithSheet(ds, workbook, sheetname, evaluator, firstRowAsHeader);
            return ds;
        }

        private static IWorkbook CreateWorkBook(string filename)
        {
            if (IsXlsxFile(filename))
            {
                return new XSSFWorkbook();
            }

            return new HSSFWorkbook();
        }

        private static IWorkbook CreateWorkBook(Stream stream, string filename)
        {
            if (IsXlsxFile(filename))
            {
                return new XSSFWorkbook(stream);
            }

            return new HSSFWorkbook(stream);
        }

        private static void CreateWorkBookAndEvaluator(Stream stream, string filename, out IWorkbook workbook, out IFormulaEvaluator evaluator)
        {
            if (IsXlsxFile(filename))
            {
                workbook = new XSSFWorkbook(stream);
                evaluator = new XSSFFormulaEvaluator(workbook);
            }
            else
            {
                workbook = new HSSFWorkbook(stream);
                evaluator = new HSSFFormulaEvaluator(workbook);
            }
        }

        private static void FillDataSetWithAllSheets(
            DataSet ds,
            IWorkbook workbook,
            IFormulaEvaluator evaluator,
            bool firstRowAsHeader)
        {
            for (var i = 0; i < workbook.NumberOfSheets; ++i)
            {
                var sheet = workbook.GetSheetAt(i);
                ds.Tables.Add(ConvertSheetToDataTable(sheet, evaluator, firstRowAsHeader));
            }
        }

        private static void FillDataSetWithSheet(
            DataSet ds,
            IWorkbook workbook,
            string sheetname,
            IFormulaEvaluator evaluator,
            bool firstRowAsHeader)
        {
            var sheet = workbook.GetSheet(sheetname);
            ds.Tables.Add(ConvertSheetToDataTable(sheet, evaluator, firstRowAsHeader));
        }

        private static DataTable ConvertSheetToDataTable(ISheet sheet, IFormulaEvaluator evaluator, bool firstRowAsHeader)
        {
            using var dt = new DataTable();
            var maxCellNum = GetMaxCellNum(sheet);
            if (firstRowAsHeader)
            {
                var firstRow = sheet.GetRow(0);
                for (var i = 0; i < maxCellNum; i++)
                {
                    var columnName = $"F{i + 1}";
                    var cell = firstRow?.GetCell(i);
                    if (cell != null)
                    {
                        cell.SetCellType(CellType.String);
                        if (cell.StringCellValue != null)
                        {
                            columnName = cell.StringCellValue;
                        }
                    }

                    dt.Columns.Add(columnName, typeof(string));
                }
            }
            else
            {
                for (var i = 0; i < maxCellNum; i++)
                {
                    dt.Columns.Add($"F{i}", typeof(string));
                }
            }

            for (var i = firstRowAsHeader ? sheet.FirstRowNum + 1 : sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                var dr = dt.NewRow();
                FillDataRowBySheetRow(row, evaluator, dr);

                var isNullRow = true;
                for (var j = 0; j < maxCellNum; j++)
                {
                    isNullRow = isNullRow && dr.IsNull(j);
                }

                if (!isNullRow)
                {
                    dt.Rows.Add(dr);
                }
            }

            dt.TableName = sheet.SheetName;
            return dt;
        }

        private static void FillDataRowBySheetRow(IRow row, IFormulaEvaluator evaluator, DataRow dr)
        {
            if (row == null)
            {
                return;
            }

            for (var j = 0; j < dr.Table.Columns.Count; j++)
            {
                var cell = row.GetCell(j);
                if (cell != null)
                {
                    switch (cell.CellType)
                    {
                        case CellType.Blank:
                            dr[j] = DBNull.Value;
                            break;
                        case CellType.Boolean:
                            dr[j] = cell.BooleanCellValue;
                            break;
                        case CellType.Numeric:
                            if (DateUtil.IsCellDateFormatted(cell))
                            {
                                dr[j] = cell.DateCellValue;
                            }
                            else
                            {
                                dr[j] = cell.NumericCellValue;
                            }
                            break;
                        case CellType.String:
                            dr[j] = !string.IsNullOrWhiteSpace(cell.StringCellValue) ? cell.StringCellValue : DBNull.Value;
                            break;
                        case CellType.Error:
                            dr[j] = cell.ErrorCellValue;
                            break;
                        case CellType.Formula:
                            dr[j] = evaluator.EvaluateInCell(cell).ToString();
                            break;
                        default:
                            throw new NotSupportedException("不支持该枚举值");
                    }
                }
            }
        }

        private static int GetMaxCellNum(ISheet sheet)
        {
            var maxCellNum = 0;
            for (var i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row != null && row.LastCellNum > maxCellNum)
                {
                    maxCellNum = row.LastCellNum;
                }
            }

            return maxCellNum;
        }

        /// <summary>
        /// 下载Excel
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="source">泛型类</param>
        /// <param name="filename">下载文件完全名称</param>
        /// <param name="templateFileName">模板名称</param>
        /// <param name="templateConfigXml">模板配置Xml</param>
        public static void Write<T>(T source, string filename, string templateFileName, string templateConfigXml)
        {
            var stream = new FileStream(templateFileName, FileMode.Open, FileAccess.Read);
            var workbook = CreateWorkBook(stream, templateFileName);
            var sheet = workbook.GetSheetAt(0);
            var doc = new XmlDocument();
            doc.Load(templateConfigXml);
            var rootElem = doc.DocumentElement;
            if (rootElem != null)
            {
                var childNodes = rootElem.ChildNodes;
                var entity = Activator.CreateInstance<T>();
                var propertys = entity.GetType().GetProperties().Where(u => u.CanWrite);
                foreach (var p in propertys)
                {
                    foreach (XmlNode node in childNodes)
                    {
                        var row = Convert.ToInt32(((XmlElement)node).GetAttribute("row"));
                        var col = Convert.ToInt32(((XmlElement)node).GetAttribute("column"));

                        sheet.GetRow(row).GetCell(col).SetCellValue(p.GetValue(source).ToString());
                    }
                }
            }

            using var fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            workbook.Write(fs);
        }

        /// <summary>
        /// 下载Excel文件流
        /// </summary>
        /// <param name="source">Table数组</param>
        /// <param name="filename">Excel文件名</param>
        /// <param name="fillheader">Table列名是否做为Excel第一行</param>
        /// <returns>MemoryStream</returns>
        public static MemoryStream WriteMemoryStream(DataTable[] source, string filename, bool fillheader)
        {
            return ExportMemoryStream(FillWorkbook(source, filename, fillheader));
        }

        /// <summary>
        /// 下载Excel文件字节数组
        /// </summary>
        /// <param name="source">Table数组</param>
        /// <param name="filename">Excel文件名</param>
        /// <param name="fillheader">Table列名是否做为Excel第一行</param>
        /// <returns>byte[]</returns>
        public static byte[] WriteBytes(DataTable[] source, string filename, bool fillheader)
        {
            return WriteBytes(FillWorkbook(source, filename, fillheader));
        }

        /// <summary>
        /// 下载Excel
        /// </summary>
        /// <param name="source">Table数组</param>
        /// <param name="filename">Excel文件名</param>
        /// <param name="fillheader">Table列名是否做为Excel第一行</param>
        public static void Write(DataTable[] source, string filename, bool fillheader)
        {
            var path = Path.GetDirectoryName(filename);
            if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var name = Path.GetFileName(filename);
            if (string.IsNullOrEmpty(name))
            {
                path = Path.GetFullPath(filename);
                name = DateTime.UtcNow.ToString("yyyyMMdd-hhmmss-") + new Random().Next(0000, 9999).ToString("D4") + ".xlsx";
                filename = Path.Combine(path, name);
            }

            Export(source, filename, fillheader);
        }

        private static void Export(DataTable[] source, string filename, bool fillheader)
        {
            ExportFileStream(FillWorkbook(source, filename, fillheader), filename);
        }

        private static IWorkbook FillWorkbook(DataTable[] source, string filename, bool fillheader)
        {
            var workbook = CreateWorkBook(filename);
            foreach (var dt in source)
            {
                FillSheetByDataTable(workbook, dt, fillheader);
            }

            return workbook;
        }

        private static void ExportFileStream(IWorkbook workbook, string filename)
        {
            using var fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            workbook.Write(fs);
        }

        private static MemoryStream ExportMemoryStream(IWorkbook workbook)
        {
            var stream = new MemoryStream();
            workbook.Write(stream);
            stream.Flush();
            return stream;
        }

        private static byte[] WriteBytes(IWorkbook workbook)
        {
            using var stream = new MemoryStream();
            workbook.Write(stream);
            stream.Flush();
            return stream.ToArray();
        }

        private static void FillSheetByDataTable(IWorkbook workbook, DataTable dt, bool fillheader)
        {
            var sheet = string.IsNullOrWhiteSpace(dt.TableName) ? workbook.CreateSheet() : workbook.CreateSheet(dt.TableName);
            var dataRow = sheet.CreateRow(0);
            if (fillheader)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                }
            }

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                dataRow = sheet.CreateRow(i + (fillheader ? 1 : 0));
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    dataRow.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                }
            }
        }

        private static bool IsXlsxFile(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            if (extension == null)
            {
                throw new NotSupportedException("不支持的文件类型");
            }

            var ex = extension.ToLower();
            if (ex is not ".xls" and not ".xlsx")
            {
                throw new NotSupportedException("不支持的文件类型");
            }

            return ex == ".xlsx";
        }
    }
}
