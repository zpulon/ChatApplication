using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;

namespace ApiCore.ExportExcel
{
    public class ExcelFile<TRowEntity> {
        private int rowIndex = 0;
        private TitleRow<TRowEntity> _titleRow = null;
        private ISheet sheet1 = null;
        protected IWorkbook Workbook { get; }

        public TitleRow<TRowEntity> TitleRow {
            get {
                return _titleRow;
            }
        }

        public ExcelFile(TitleRow<TRowEntity> header, string sheetName) {
            _titleRow = header;

            Workbook = new XSSFWorkbook();
            if (string.IsNullOrEmpty(sheetName)) {
                sheetName = "Sheet1";
            }
            sheet1 = Workbook.CreateSheet(sheetName);
        }

        public ICellStyle CreateCellStyle() {
            return Workbook.CreateCellStyle();
        }

        /// <summary>
        /// 设置单元格宽度
        /// </summary>
        /// <param name="cellIndex">单元格索引</param>
        /// <param name="width">单元格宽</param>
        public void SetCellWidth(int cellIndex, int width) {
            sheet1.SetColumnWidth(cellIndex, width);
        }

        public IFont CreateFont(){
            return Workbook.CreateFont();
        }

        public void SetCellValue(ColumnItem c, ICell cell, object entity, object ctx) {
            object cellValue = c.GetValue(entity, ctx);
            if (cellValue != null) {
                switch (c.CellType) {
                    case CellType.Boolean:
                        cell.SetCellValue((bool)cellValue);
                        break;
                    case CellType.Formula:
                        cell.SetCellValue((string)cellValue);
                        break;
                    case CellType.Numeric:
                        cell.SetCellValue((double)cellValue);
                        break;
                    case CellType.String:
                        cell.SetCellValue((string)cellValue);
                        break;
                }
            }
        }
        
        public void SkipRow(int row) {
            rowIndex += row;
        }
        public void WriteRow(Action<IRow> proc) {
            IRow row = sheet1.CreateRow(rowIndex);
            rowIndex++;
            proc?.Invoke(row);
        }

        public void WriteToStream(Stream stream)
        {
            using var ms = new MemoryStream();
            Workbook.Write(ms);
            var buffer = ms.ToArray();
            stream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}
