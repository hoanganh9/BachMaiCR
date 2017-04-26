using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BachMaiCR.Utilities.ReportForm
{
    public static class FUtils
    {
        private const string _htmlTableBegin = "<div class='table_kqtk fll invibox'><table class='Grid' width='100%' cellspacing='0' cellpadding='3' border='1'><tbody>";
        private const string _htmlTableEnd = "</tbody></table></div>";

        public static string GetHtmlHeaderTable(TreeItem tree, bool? setWidthAuto = null, int? positionGroup = null, string beginTag = "", string endTag = "")
        {
            int maxLevel = FUtils.GetLevelMax(tree) - 1;
            IEnumerable<FField> ffields = FUtils.ConvertToFField(tree, maxLevel).Where<FField>((t => t.Level != 0));
            return ffields.Any<FField>() ? FUtils.GetHtmlHeaderTable(ffields, maxLevel, setWidthAuto, positionGroup, beginTag, endTag) : "";
        }

        public static string GetHtmlHeaderTable(IEnumerable<FField> ffields, int maxLevel, bool? setWidthAuto = null, int? positionGroup = null, string beginTag = "", string endTag = "")
        {
            StringBuilder stringBuilder = new StringBuilder(string.IsNullOrEmpty(beginTag) ? "" : beginTag);
            if (setWidthAuto.HasValue)
            {
                if (setWidthAuto.Value)
                    FUtils.SetWidthCols(ffields);
            }
            else
                FUtils.SetWidthCols(ffields);
            for (int i = 1; i <= maxLevel; ++i)
            {
                FRow frow = new FRow();
                frow.IsHeader = true;
                frow.Cells = ffields.Where<FField>((t => t.Level == i)).ToList<FField>();
                if (positionGroup.HasValue)
                    frow.Cells[positionGroup.Value].ColumnSpan = new int?(2);
                stringBuilder.Append(frow.ToTrTag());
            }
            stringBuilder.Append(string.IsNullOrEmpty(endTag) ? "" : endTag);
            return stringBuilder.ToString();
        }

        public static string GetHtmlTable(FReport obj)
        {
            return FUtils.GetHtmlTable(obj.HeaderItem, obj.DataContent);
        }

        public static string GetHtmlTable(TreeItem treeHeader, DataTable dtContent)
        {
            int maxLevel = FUtils.GetLevelMax(treeHeader) - 1;
            IEnumerable<FField> ffields = FUtils.ConvertToFField(treeHeader, maxLevel).Where<FField>((t => t.Level != 0));
            if (!ffields.Any<FField>())
                return "";
            StringBuilder stringBuilder = new StringBuilder("<div class='table_kqtk fll invibox'><table class='Grid' width='100%' cellspacing='0' cellpadding='3' border='1'><tbody>");
            stringBuilder.Append(FUtils.GetHtmlHeaderTable(ffields, maxLevel, new bool?(), new int?(), "", ""));
            FRow frow1 = new FRow();
            frow1.IsHeader = true;
            for (int index = 0; index < dtContent.Columns.Count; ++index)
            {
                List<FField> cells = frow1.Cells;
                FField ffield1 = new FField();
                FField ffield2 = ffield1;
                string str1 = "(";
                int num = index + 1;
                string str2 = num.ToString();
                string str3 = ")";
                string str4 = str1 + str2 + str3;
                ffield2.FieldName = str4;
                FField ffield3 = ffield1;
                string str5 = "(";
                num = index + 1;
                string str6 = num.ToString();
                string str7 = ")";
                string str8 = str5 + str6 + str7;
                ffield3.FieldData = str8;
                FField ffield4 = ffield1;
                cells.Add(ffield4);
            }
            stringBuilder.Append(frow1.ToTrTag());
            for (int index1 = 0; index1 < dtContent.Rows.Count; ++index1)
            {
                FRow frow2 = new FRow();
                for (int index2 = 0; index2 < dtContent.Columns.Count; ++index2)
                    frow2.Cells.Add(new FField()
                    {
                        FieldName = dtContent.Rows[index1][index2].ToString(),
                        FieldData = dtContent.Rows[index1][index2].ToString(),
                        ParentValue = dtContent.Columns[index2].ColumnName
                    });
                if (index1 == dtContent.Rows.Count - 1)
                {
                    frow2.Cells.RemoveAt(0);
                    frow2.Cells[0].ColumnSpan = new int?(2);
                    frow2.Cells[0].FieldName = "Tổng số";
                }
                stringBuilder.Append(frow2.ToTrTag());
            }
            stringBuilder.Append("</tbody></table></div>");
            return stringBuilder.ToString();
        }

        public static string GetHtmlTable(TreeItem treeHeader, DataTable dtContent, int positionHeaderGroup, List<string> rowGroup)
        {
            int maxLevel = FUtils.GetLevelMax(treeHeader) - 1;
            IEnumerable<FField> source = FUtils.ConvertToFField(treeHeader, maxLevel).Where<FField>((t => t.Level != 0));
            if (!source.Any<FField>())
                return "";
            StringBuilder stringBuilder = new StringBuilder("<div class='table_kqtk fll invibox'><table class='Grid' width='100%' cellspacing='0' cellpadding='3' border='1'><tbody>");
            stringBuilder.Append(FUtils.GetHtmlHeaderTable((IEnumerable<FField>)source.ToList<FField>(), maxLevel, new bool?(), new int?(), "", ""));
            for (int index1 = 0; index1 < dtContent.Rows.Count; ++index1)
            {
                FRow frow = new FRow();
                for (int index2 = 0; index2 < dtContent.Columns.Count; ++index2)
                    frow.Cells.Add(new FField()
                    {
                        FieldName = dtContent.Rows[index1][index2].ToString(),
                        FieldData = dtContent.Rows[index1][index2].ToString()
                    });
                if (index1 == dtContent.Rows.Count - 1)
                {
                    frow.Cells.RemoveAt(0);
                    frow.Cells[0].ColumnSpan = new int?(2);
                    frow.Cells[0].FieldName = "Tổng số";
                }
                stringBuilder.Append(frow.ToTrTag());
            }
            stringBuilder.Append("</tbody></table></div>");
            return stringBuilder.ToString();
        }

        public static List<FField> ConvertToFField(TreeItem tree, int maxLevel)
        {
            List<FField> ffieldList = new List<FField>();
            ffieldList.Add(new FField()
            {
                FieldName = tree.Name,
                FieldData = tree.Name,
                ColumnSpan = FUtils.GetColumnSpan(tree) == 1 || FUtils.GetColumnSpan(tree) == 0 ? new int?() : new int?(FUtils.GetColumnSpan(tree)),
                RowSpan = tree.Level == maxLevel || tree.Children.Count<TreeItem>() >= 1 ? new int?() : new int?(maxLevel + 1 - tree.Level),
                Level = tree.Level,
                ParentValue = tree.ParentName
            });
            foreach (TreeItem child in tree.Children)
                ffieldList.AddRange((IEnumerable<FField>)FUtils.ConvertToFField(child, maxLevel));
            return ffieldList;
        }

        public static List<FField> ConvertToFField(TreeItem tree)
        {
            int maxLevel = FUtils.GetLevelMax(tree) - 1;
            List<FField> ffieldList = new List<FField>();
            if (maxLevel != 0)
            {
                IEnumerable<FField> source = FUtils.ConvertToFField(tree, maxLevel).Where<FField>((t => t.Level != 0));
                if (source.Any<FField>())
                    ffieldList = source.ToList<FField>();
            }
            return ffieldList;
        }

        public static int GetColumnSpan(TreeItem tree)
        {
            int num = 0;
            foreach (TreeItem child in tree.Children)
            {
                if (child.Children.Count == 0)
                    ++num;
                else if (child.Children.Count == 1)
                    num += 2;
                else
                    num += FUtils.GetColumnSpan(child);
            }
            return num;
        }

        public static int GetLevelMax(TreeItem tree)
        {
            if (tree == null)
                return 0;
            if (tree.Children.Any<TreeItem>())
                return 1 + tree.Children.Select((t => FUtils.GetLevelMax(t))).Max();
            return 1;
        }

        public static int GetLevelMaxNotRoot(TreeItem tree)
        {
            return FUtils.GetLevelMax(tree) - 1;
        }

        public static void SetWidthCols(IEnumerable<FField> ff)
        {
            IEnumerable<FField> source = ff.Where<FField>((t =>
            {
                int num;
                if (t.ColumnSpan.HasValue)
                {
                    int? columnSpan = t.ColumnSpan;
                    num = columnSpan.GetValueOrDefault() != 0 ? 0 : (columnSpan.HasValue ? 1 : 0);
                }
                else
                    num = 1;
                return num != 0;
            }));
            if (source.Count<FField>() <= 2)
                return;
            double num1 = 80.0;
            source.ToList<FField>()[0].Width = new double?(3.0);
            source.ToList<FField>()[1].Width = new double?(17.0);
            double num2 = num1 / (double)(source.Count<FField>() - 2);
            for (int index = 2; index < source.Count<FField>(); ++index)
                source.ToList<FField>()[index].Width = new double?(num2);
        }

        public static void SetWidthFirstRow(List<FField> ff)
        {
            if (ff.Count<FField>() <= 3)
                return;
            ff[0].Width = new double?(3.0);
            ff[1].Width = new double?(10.0);
            ff[2].Width = new double?(47.0);
            double num = (double)(40 / (ff.Count - 3));
            for (int index = 3; index < ff.Count; ++index)
                ff[index].Width = new double?(num);
        }
    }
}