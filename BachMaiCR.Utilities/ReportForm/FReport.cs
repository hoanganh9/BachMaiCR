using System;
using System.Data;

namespace BachMaiCR.Utilities.ReportForm
{
  [Serializable]
  public class FReport
  {
    private DataTable _dataContent;
    private TreeItem _treeItem;

    public string SheetName { get; set; }

    public int TotalLevel { get; set; }

    public string FileName { get; set; }

    public FHeader HeaderCommon { get; set; }

    public DataTable DataContent
    {
      get
      {
        return this._dataContent ?? new DataTable();
      }
      set
      {
        this._dataContent = value;
      }
    }

    public TreeItem HeaderItem
    {
      get
      {
        return this._treeItem ?? new TreeItem();
      }
      set
      {
        this._treeItem = value;
      }
    }

    public FTable TableContent { get; set; }

    public DateTime DateFrom { get; set; }

    public DateTime DateTo { get; set; }

    public DateTime CurrentDate { get; set; }

    public FReport()
    {
      this.HeaderCommon = new FHeader();
    }
  }
}
