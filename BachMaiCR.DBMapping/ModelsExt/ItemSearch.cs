using System;

namespace BachMaiCR.DBMapping.ModelsExt
{
    public class ItemSearch
    {
        public string SearchCode { get; set; }

        public string SearchName { get; set; }

        public string SearchUserCreate { get; set; }

        public string SearchUserReceive { get; set; }

        public int? SearchStatus { get; set; }

        public DateTime? SearchDateStart { get; set; }

        public DateTime? SearchDateEnd { get; set; }

        public DateTime? SearchDateCreate { get; set; }

        public DateTime? SearchDateSent { get; set; }

        public ItemSearch()
        {
            this.SearchCode = "";
            this.SearchName = "";
            this.SearchUserCreate = "";
            this.SearchUserReceive = "";
            DateTime now = DateTime.Now;
            int num = DateTime.DaysInMonth(now.Year, now.Month);
            DateTime dateTime = DateTime.Today.AddDays((double)(1 - DateTime.Today.Day));
            this.SearchDateStart = new DateTime?(dateTime);
            this.SearchDateEnd = new DateTime?(dateTime.AddDays((double)(num - 1)));
        }
    }
}