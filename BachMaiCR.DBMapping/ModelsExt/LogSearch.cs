using System;

namespace BachMaiCR.DBMapping.ModelsExt
{
    public class LogSearch
    {
        public string SearchName { get; set; }

        public string SearchIpAddress { get; set; }

        public string SearchMenuCode { get; set; }

        public string SearchUserName { get; set; }

        public int? SearchStatus { get; set; }

        public int? SearchAction { get; set; }

        public DateTime? SearchDateStart { get; set; }

        public DateTime? SearchDateEnd { get; set; }

        public LogSearch()
        {
            this.SearchName = "";
            this.SearchUserName = "";
            this.SearchIpAddress = "";
            this.SearchMenuCode = "";
        }
    }
}