using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;

namespace ApiCore.ExportExcel
{
    public class TitleRow<TRowEntity>
    {
        private List<ColumnItem> _columns = null;
        public TitleRow()
        {
            _columns = new List<ColumnItem>();
        }

        public void AddColumn(string title, Func<TRowEntity, object, object> getValue, CellType cellType = CellType.String, int? width = null)
        {
            _columns.Add(new ColumnItem()
            {
                Title = title,
                CellType = cellType,
                Width = width,
                GetValue = (obj, ctx) =>
                {
                    return getValue((TRowEntity)obj, ctx);
                }
            });
        }


        public List<ColumnItem> Columns
        {
            get
            {
                return _columns;
            }
        }
    }
}
