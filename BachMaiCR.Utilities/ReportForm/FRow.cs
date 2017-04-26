using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BachMaiCR.Utilities.ReportForm
{
    [Serializable]
    public class FRow
    {
        private List<FField> _cells;

        public int RowIndex { get; set; }

        public bool IsHeader { get; set; }

        public List<FField> Cells
        {
            get
            {
                return this._cells ?? (this._cells = new List<FField>());
            }
            set
            {
                this._cells = value;
            }
        }

        public FRow()
        {
            this.IsHeader = false;
        }

        public string ToTrTag()
        {
            if (!this.Cells.Any<FField>())
                return "";
            StringBuilder stringBuilder = new StringBuilder("<tr " + (this.IsHeader ? "class='GridHeader'" : "class='GridItem'") + ">");
            for (int index = 0; index < this.Cells.Count; ++index)
                stringBuilder.Append(this.IsHeader ? this.Cells[index].ToThTag() : this.Cells[index].ToTdTag());
            stringBuilder.Append("</tr>");
            return stringBuilder.ToString();
        }
    }
}