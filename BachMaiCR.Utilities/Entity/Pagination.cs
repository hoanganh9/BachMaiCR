using System;

namespace BachMaiCR.Utilities.Entity
{
    [Serializable]
    public class Pagination
    {
        private string _inputSearchId;
        private string _tagetReLoadId;

        public int MaxPages { get; set; }

        public int TotalRows { get; set; }

        public int TotalPages
        {
            get
            {
                int num = this.TotalRows / this.PageSize;
                if (this.TotalRows - this.PageSize * num == 0)
                    return num;
                return num + 1;
            }
        }

        public int CurrentPage { get; set; }

        public int CurrentRow { get; set; }

        public int PageSize { get; set; }

        public string ModelName { get; set; }

        public string ActionName { get; set; }

        public string InputSearchId
        {
            get
            {
                if (string.IsNullOrEmpty(this._inputSearchId))
                    return string.Empty;
                if (this._inputSearchId[0].Equals('#'))
                    return this._inputSearchId;
                return "#" + this._inputSearchId;
            }
            set
            {
                this._inputSearchId = value;
            }
        }

        public string TagetReLoadId
        {
            get
            {
                if (string.IsNullOrEmpty(this._tagetReLoadId))
                    return string.Empty;
                if (this._tagetReLoadId[0].Equals('#'))
                    return this._tagetReLoadId;
                return "#" + this._tagetReLoadId;
            }
            set
            {
                this._tagetReLoadId = value;
            }
        }

        public int CtgType { get; set; }

        public Pagination()
        {
            this.MaxPages = 10;
            this.PageSize = 10;
            this.CurrentPage = 0;
            this.TotalRows = 10;
        }
    }
}