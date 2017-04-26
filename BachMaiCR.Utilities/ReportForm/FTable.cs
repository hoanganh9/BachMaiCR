using System;
using System.Collections.Generic;
using System.Linq;

namespace BachMaiCR.Utilities.ReportForm
{
    [Serializable]
    public class FTable
    {
        private List<FRow> _rows;

        public List<FField> Columns
        {
            get
            {
                List<FField> ffieldList = new List<FField>();
                if (this.Rows.Any<FRow>())
                {
                    foreach (FRow row in this.Rows)
                        ffieldList.AddRange((IEnumerable<FField>)row.Cells);
                }
                return ffieldList;
            }
        }

        public List<FRow> Rows
        {
            get
            {
                return this._rows ?? (this._rows = new List<FRow>());
            }
        }

        public string HtmlTable { get; set; }

        public FRow NewRow()
        {
            return new FRow()
            {
                Cells = this.Columns.Select((k => new FField()
                {
                    FieldName = k.FieldName,
                    FieldData = k.FieldData,
                    ColumnSpan = k.ColumnSpan,
                    RowSpan = k.RowSpan,
                    Level = k.Level,
                    ParentValue = k.ParentValue
                })).ToList<FField>()
            };
        }
    }
}