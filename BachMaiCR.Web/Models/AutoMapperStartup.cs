using AutoMapper;
using BachMaiCR.DBMapping.Models;


namespace BachMaiCR.Web.Models
{
    //public class AutoMapperStartup
    //{
    //    public static void Initialize()
    //    {
    //        #region "Calendar Change"
    //        Mapper.CreateMap<CALENDAR_CHANGE, CalendarChangeModel>();
    //        Mapper.CreateMap<CalendarChangeModel, CALENDAR_CHANGE>();
    //        #endregion
    //    }
    //}
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CALENDAR_CHANGE, CalendarChangeModel>();
            CreateMap<CalendarChangeModel, CALENDAR_CHANGE>();
        }
    }
}