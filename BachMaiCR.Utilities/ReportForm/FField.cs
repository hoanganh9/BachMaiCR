using System;

namespace BachMaiCR.Utilities.ReportForm
{
  [Serializable]
  public class FField
  {
    public string FieldName { get; set; }

    public string FieldData { get; set; }

    public int? ColumnSpan { get; set; }

    public int SumIndex { get; set; }

    public int? RowSpan { get; set; }

    public double? Width { get; set; }

    public int Level { get; set; }

    public string ParentValue { get; set; }

    public int ColPosition { get; set; }

    public int RowPosition { get; set; }

    public string ClassName { get; set; }

    public string ToTdTag()
    {
      string str1 = string.Empty;
      if (this.ColumnSpan.HasValue)
      {
        str1 = string.Format(" colspan='{0}'", this.ColumnSpan.Value);
      }
      string str2 = string.Empty;
      if (this.RowSpan.HasValue)
      {
          str2 = string.Format(" rowspan='{0}'", this.RowSpan.Value);
      }
      string str8 = this.Width.HasValue ? " width='" + this.Width.Value.ToString() + "px'" : string.Empty;
      string str9 = !string.IsNullOrEmpty(this.ParentValue) ? " alt='" + this.ParentValue + "'" : string.Empty;
      string str10 = !string.IsNullOrEmpty(this.ClassName) ? " class='" + this.ClassName + "'" : string.Empty;
      return "<td" + str1 + str2 + str8 + str9 + str10 + ">" + this.FieldName + "</td>";
    }

    public string ToThTag()
    {
      int num;
      string str1;
      if (!this.ColumnSpan.HasValue)
      {
        str1 = "";
      }
      else
      {
        string str2 = " colspan='";
        num = this.ColumnSpan.Value;
        string str3 = num.ToString();
        string str4 = "'";
        str1 = str2 + str3 + str4;
      }
      string str5 = str1;
      string str6;
      if (!this.RowSpan.HasValue)
      {
        str6 = "";
      }
      else
      {
        string str2 = " rowspan='";
        num = this.RowSpan.Value;
        string str3 = num.ToString();
        string str4 = "'";
        str6 = str2 + str3 + str4;
      }
      string str7 = str6;
      string str8 = this.Width.HasValue ? " width='" + this.Width.Value.ToString() + "px'" : "";
      string str9 = !string.IsNullOrEmpty(this.ParentValue) ? " alt='" + this.ParentValue + "'" : "";
      string str10 = !string.IsNullOrEmpty(this.ClassName) ? " class='" + this.ClassName + "'" : "";
      return "<th" + str5 + str7 + str8 + str9 + str10 + ">" + this.FieldName + "</th>";
    }
  }
}
