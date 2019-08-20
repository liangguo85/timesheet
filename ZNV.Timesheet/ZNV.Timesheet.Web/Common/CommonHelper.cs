using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ZNV.Timesheet.UserSetting;

namespace ZNV.Timesheet.Web.Common
{
    public class CommonHelper
    {
        public static string _currentUser;

        /// <summary>
        /// 当前登陆的用户
        /// </summary>
        public static string CurrentUser { get { return _currentUser ?? ""; } set { _currentUser = value; } }

        public static string GetProjectNameByProjectID(List<Project.Project> projects, int projectID)
        {
            if (projects != null && projects.Count > 0)
            {
                var w = projects.Where(p => p.Id == projectID).ToList();
                if (w.Count > 0)
                {
                    return w[0].ProjectName;
                }
            }
            return "";
        }

        /// <summary>
        /// 把数据库返回的表格数据生成excel对象，如果某些列需要相同值合并，则在第三个参数设置需要合并的列集合即可，暂不支持横向合并
        /// </summary>
        /// <param name="exportTableName">excel的第一个sheet的名称</param>
        /// <param name="dt">数据库返回的表格数据</param>
        /// <param name="mergeCellIndexList">需要合并的列集合，暂不支持行合并，为空则不需要合并（需要数据库返回的结果已经按照这些字段进行了排序，本方法仅实现这些列相同值合并）</param>
        /// <returns></returns>
        public static HSSFWorkbook CreateHSSFromDataTable(string exportTableName, DataTable dt, List<int> mergeCellIndexList)
        {
            //创建excel对象
            HSSFWorkbook book = new HSSFWorkbook();
            //创建sheet
            ISheet sheet = book.CreateSheet(exportTableName);
            //创建标题行
            IRow rowTitle = sheet.CreateRow(0);

            //设置标题表格样式
            ICellStyle cellStyleTitle = book.CreateCellStyle();
            cellStyleTitle.BorderBottom = BorderStyle.Thin;
            cellStyleTitle.BorderLeft = BorderStyle.Thin;
            cellStyleTitle.BorderRight = BorderStyle.Thin;
            cellStyleTitle.BorderTop = BorderStyle.Thin;
            cellStyleTitle.VerticalAlignment = VerticalAlignment.Center;
            cellStyleTitle.Alignment = HorizontalAlignment.Center;
            cellStyleTitle.FillBackgroundColor = HSSFColor.LightBlue.Index;

            //设置数据表格样式
            ICellStyle cellStyle = book.CreateCellStyle();
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.Alignment = HorizontalAlignment.Center;

            //设置表头
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cellTitle = rowTitle.CreateCell(i);
                cellTitle.CellStyle = cellStyleTitle;
                cellTitle.SetCellValue(dt.Columns[i].ColumnName);
            }
            //填充数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow rowData = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    var cellValue = dt.Rows[i][j].ToString().Trim();
                    if (cellValue == "0" || cellValue == "0.00" || cellValue == ".00")
                    {
                        cellValue = "";
                    }
                    ICell cellData = rowData.CreateCell(j);
                    cellData.CellStyle = cellStyle;
                    cellData.SetCellValue(cellValue);
                }
            }
            //需要合并的列（需要数据库返回的结果已经按照这些字段进行了排序，本方法仅实现某列相同值合并）
            if (mergeCellIndexList != null && mergeCellIndexList.Count > 0)
            {
                foreach (var cellIndex in mergeCellIndexList)
                {
                    //索引从1开始是因为sheet表中首行是标题
                    for (int i = 1; i < dt.Rows.Count + 1; i++)
                    {
                        string value = sheet.GetRow(i).GetCell(cellIndex).StringCellValue;
                        int end = i;
                        //找到结束为止
                        for (int j = i + 1; j < dt.Rows.Count + 1; j++)
                        {
                            string value1 = sheet.GetRow(j).GetCell(cellIndex).StringCellValue;
                            if (value != value1)
                            {
                                end = j - 1;
                                break;
                            }
                            else if (value == value1 && j == dt.Rows.Count)
                            {
                                end = j;
                                break;
                            }
                        }
                        sheet.AddMergedRegion(new CellRangeAddress(i, end, cellIndex, cellIndex));
                        i = end;
                    }
                }
            }
            //所有数据填充完毕后，合并处理完毕后，再来设置列的自动宽度
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                //这里第二个参数必须设置成true，否则合并的列不会设置自动宽度
                sheet.AutoSizeColumn(i, true);
            }
            return book;
        }
        
    }
}