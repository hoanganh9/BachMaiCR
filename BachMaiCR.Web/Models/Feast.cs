using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Web.Common.Attributes;

namespace BachMaiCR.Web.Models
{
    public class Feast
    {
        public Feast()
        {

        }

        //[]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyFeast", ErrorMessageResourceType = typeof(Resources.Localization))]
        [LocalizationDisplay("LabelFeast")]
        [StringLength(150, ErrorMessageResourceName = "MsgMaxLenghtRequired", ErrorMessageResourceType = typeof(Resources.Localization))]
        public string Feast_Title { get; set; }

        public int FeastId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyFeastFromDate", ErrorMessageResourceType = typeof(Resources.Localization))]
        public DateTime? Date_Begin { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "MsgEmptyFeastToDate", ErrorMessageResourceType = typeof(Resources.Localization))]
        public DateTime? Date_End { get; set; }
        public int? Feast_Year { get; set; }
        public bool IsDeleted { get; set; }
        [LocalizationDisplay("LabelActived")]
        public bool IsActived { get; set; }



        public Feast(FEAST entity)
        {
            this.FeastId = entity.FEAST_ID;
            this.Feast_Title = entity.FEAST_TITLE;
            this.Date_Begin = entity.DATE_BEGIN;
            this.Date_End = entity.DATE_END;
            this.Feast_Year = entity.FEAST_YEAR;
            this.IsDeleted = entity.ISDELETE;
            this.IsActived = entity.ISACTIVED;

            //i
        }

        public FEAST GetCategoryModel()
        {
            var entity = new FEAST();
            entity.FEAST_ID = this.FeastId;
            entity.FEAST_TITLE = this.Feast_Title.Trim();
            entity.DATE_BEGIN = this.Date_Begin;
            entity.DATE_END = this.Date_End;
            entity.FEAST_YEAR = this.Feast_Year;
            entity.ISDELETE = this.IsDeleted;
            entity.ISACTIVED = this.IsActived;
            return entity;
        }
    }
}