using System.Collections.Generic;
using AutoMapper;
using BachMaiCR.DBMapping.Models;

namespace BachMaiCR.Web.Models
{
    public static class MappingModelEntityExtensions
    {
        #region "Calendar Change"
        public static CalendarChangeModel ToModel(this CALENDAR_CHANGE entity)
        {
            return Mapper.Map<CALENDAR_CHANGE, CalendarChangeModel>(entity);
        }

        public static IEnumerable<CalendarChangeModel> ToListModel(this IEnumerable<CALENDAR_CHANGE> listEntity)
        {
            return Mapper.Map<IEnumerable<CALENDAR_CHANGE>, IEnumerable<CalendarChangeModel>>(listEntity);
        }

        public static CALENDAR_CHANGE ToEntity(this CalendarChangeModel model)
        {
            return Mapper.Map<CalendarChangeModel, CALENDAR_CHANGE>(model);
        }

        public static CALENDAR_CHANGE ToEntity(this CalendarChangeModel model, CALENDAR_CHANGE destination)
        {
            return Mapper.Map(model, destination);
        }
        #endregion

    }
}