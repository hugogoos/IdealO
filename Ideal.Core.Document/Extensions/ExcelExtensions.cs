using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

namespace Ideal.Core.Document.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExcelExtensions
    {
        #region 单元格样式

        private static IWorkbook SetCellStyle(this IWorkbook workbook, ICell cell)
        {
            var fCellStyle = workbook.CreateCellStyle();
            var ffont = workbook.CreateFont();
            ffont.Color = HSSFColor.Red.Index;
            fCellStyle.SetFont(ffont);

            fCellStyle.VerticalAlignment = VerticalAlignment.Center; //垂直对齐
            fCellStyle.Alignment = HorizontalAlignment.Center; //水平对齐
            cell.CellStyle = fCellStyle;

            return workbook;
        }

        #endregion

        public static IWorkbook CellFormula(this IWorkbook workbook, string sheetName, int col, int row)
        {
            var sheet = workbook.GetSheet(sheetName) ?? workbook.GetSheetAt(0);
            var cellStyle = workbook.CreateCellStyle();
            cellStyle.FillForegroundColor = IndexedColors.Red.Index;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            sheet.GetRow(0).GetCell(1).CellStyle = cellStyle;
            return workbook;
        }

        public static IWorkbook CellFormula(
            this IWorkbook workbook,
            int sheetIndex,
            int colStart,
            int rowStart,
            Func<ICell, bool> method)
        {
            var sheet = workbook.GetSheetAt(sheetIndex) ?? workbook.GetSheetAt(0);
            var cellStyle = workbook.CreateCellStyle();
            cellStyle.FillForegroundColor = IndexedColors.Yellow.Index;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            var colEnd = sheet.GetRow(0).LastCellNum;
            var rowEnd = sheet.LastRowNum;
            for (var i = colStart; i < colEnd; i++)
            {
                for (var j = rowStart; j < rowEnd; j++)
                {
                    var cell = sheet.GetRow(j).GetCell(i);
                    if (string.IsNullOrWhiteSpace(cell.ToString()))
                    {
                        continue;
                    }

                    if (method(cell))
                    {
                        cell.CellStyle = cellStyle;
                    }
                }
            }

            return workbook;
        }

        public static IWorkbook CellFormulaFont(
            this IWorkbook workbook,
            int sheetIndex,
            int colStart,
            int rowStart,
            Func<ICell, bool> method)
        {
            var sheet = workbook.GetSheetAt(sheetIndex) ?? workbook.GetSheetAt(0);
            var colEnd = sheet.GetRow(0).LastCellNum;
            var rowEnd = sheet.LastRowNum;
            for (var i = colStart; i < colEnd; i++)
            {
                for (var j = rowStart; j < rowEnd; j++)
                {
                    var cell = sheet.GetRow(j).GetCell(i);
                    if (string.IsNullOrWhiteSpace(cell.ToString()))
                    {
                        continue;
                    }
                    if (!method(cell))
                    {
                        continue;
                    }

                    var cellStyle = workbook.CreateCellStyle();
                    cellStyle.CloneStyleFrom(cell.CellStyle);
                    cellStyle.FillForegroundColor = IndexedColors.Red.Index;
                    cellStyle.FillPattern = FillPattern.SolidForeground;
                    var ffont = workbook.CreateFont();
                    ffont.Color = HSSFColor.White.Index;
                    cellStyle.SetFont(ffont);
                    cell.CellStyle = cellStyle;
                }
            }

            return workbook;
        }

        #region FreezePane 冻结行列

        public static IWorkbook FreezePane(this IWorkbook workbook, string sheetName, int colSplit, int rowSplit)
        {
            var sheet = workbook.GetSheet(sheetName) ?? workbook.GetSheetAt(0);
            CreateFreezePane(sheet, colSplit, rowSplit);
            return workbook;
        }

        public static IWorkbook FreezePane(this IWorkbook workbook, int sheetIndex, int colSplit, int rowSplit)
        {
            var sheet = workbook.GetSheetAt(sheetIndex) ?? workbook.GetSheetAt(0);
            CreateFreezePane(sheet, colSplit, rowSplit);
            return workbook;
        }

        private static void CreateFreezePane(ISheet sheet, int colSplit, int rowSplit)
        {
            sheet?.CreateFreezePane(colSplit, rowSplit);
        }

        public static IWorkbook FreezePane(this IWorkbook workbook, string sheetName, int colSplit, int rowSplit, int leftmostColumn, int topRow)
        {
            var sheet = workbook.GetSheet(sheetName) ?? workbook.GetSheetAt(0);
            CreateFreezePane(sheet, colSplit, rowSplit, leftmostColumn, topRow);
            return workbook;
        }

        public static IWorkbook FreezePane(this IWorkbook workbook, int sheetIndex, int colSplit, int rowSplit, int leftmostColumn, int topRow)
        {
            var sheet = workbook.GetSheetAt(sheetIndex) ?? workbook.GetSheetAt(0);
            CreateFreezePane(sheet, colSplit, rowSplit, leftmostColumn, topRow);
            return workbook;
        }

        private static void CreateFreezePane(ISheet sheet, int colSplit, int rowSplit, int leftmostColumn, int topRow)
        {
            sheet?.CreateFreezePane(colSplit, rowSplit, leftmostColumn, topRow);
        }

        #endregion
    }
}
