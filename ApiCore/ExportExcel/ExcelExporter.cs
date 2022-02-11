using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Net;

namespace ApiCore.ExportExcel
{

    /// <summary>
    /// 导出数据到EXCEL文件中
    /// </summary>
    /// <typeparam name="TRowEntity">数据对象类</typeparam>
    public class ExcelExporter<TRowEntity> {
        private HttpResponse _response = null;
        private string _sheetName = "Sheet1";

        /// <summary>
        /// 实例化导出类
        /// </summary>
        /// <param name="response">HTTP响应体</param>
        /// <param name="sheetName">文件名称</param>
        /// <param name="lastName">文件二级名称（最后一段名称）默认值为日期，用“_”连接</param>
        public ExcelExporter(HttpResponse response, string sheetName, string lastName = "") {
            _response = response;
            string fileName;
            if (string.IsNullOrEmpty(lastName)) {
                fileName = $"{sheetName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";
            } else {
                fileName = $"{sheetName}{lastName}.xlsx";
            }
            // 输出Excel 2007格式文件流
            _response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            _response.Headers.Add("Content-Disposition", new Microsoft.Extensions.Primitives.StringValues($"attachment; filename=\"{WebUtility.UrlEncode(fileName)}\""));
            _response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
            if (_response.Headers.ContainsKey("Access-Control-Allow-Origin")) {
                _response.Headers["Access-Control-Allow-Origin"] = "*";
            } else {
                _response.Headers.Add("Access-Control-Allow-Origin", "*");
            }

            _titleRow = new TitleRow<TRowEntity>();
        }


        private ExcelFile<TRowEntity> _excelFile = null;

        private TitleRow<TRowEntity> _titleRow = null;

        /// <summary>
        /// 导出EXCEL文件
        /// </summary>
        public void WriteToResponse(List<TRowEntity> data, Action<TitleRow<TRowEntity>> action) {
            
            // 创建行标头（列名）
            action(_titleRow);

            _excelFile = new ExcelFile<TRowEntity>(_titleRow, _sheetName);

            // 创建EXCEL标题
            WriteTitle(_excelFile);  

            // 填充EXCEL行内容
            WriteDataRow(_excelFile, data);

            // 导出EXCEL文件到response stream
            _excelFile.WriteToStream(_response.Body);
        }
        
        /// <summary>
        /// 创建标题
        /// </summary>
        protected virtual void WriteTitle(ExcelFile<TRowEntity> excelFile) {
            excelFile.WriteRow(row => {
                for (int i = 0; i < _titleRow.Columns.Count; i++) {
                    if (_titleRow.Columns[i].Width != null)
                    {
                        excelFile.SetCellWidth(i, _titleRow.Columns[i].Width.Value);
                    }

                    ICell cell = row.CreateCell(i, CellType.String);
                    cell.SetCellValue(_titleRow.Columns[i].Title);
                }
            });
        }

        /// <summary>
        /// 将数据写入EXCEL行
        /// </summary>
        protected virtual void WriteDataRow(ExcelFile<TRowEntity> excelFile, List<TRowEntity> data) {
            if (data != null) {
                data.ForEach(item => {
                    excelFile.WriteRow(row => {
                        for (int i = 0; i < _titleRow.Columns.Count; i++) {
                            var c = _titleRow.Columns[i];
                            ICell cell = row.CreateCell(i, c.CellType);
                            
                            // 写入单元格的值
                            excelFile.SetCellValue(c, cell, item, item);
                        }
                    });
                });
            }
        }
    }
}
