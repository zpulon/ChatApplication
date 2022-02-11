using NPOI.SS.UserModel;
using System;

namespace ApiCore.ExportExcel
{
    public class ColumnItem
    {
        public string Title { get; set; }

        public Func<object, object, object> GetValue { get; set; }

        public CellType CellType { get; set; }

        /// <summary>
        /// 单元格宽度
        /// </summary>
        public int? Width { get; set; }

        public ColumnItem()
        {
            CellType = CellType.String;
        }
    }
}
