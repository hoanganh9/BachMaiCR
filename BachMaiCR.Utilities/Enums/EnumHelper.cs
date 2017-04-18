using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace BachMaiCR.Utilities.Enums
{
  public static class EnumHelper<T> where T : struct
  {
    public static List<T> GetValues()
    {
      return ((IEnumerable<FieldInfo>) typeof (T).GetFields(BindingFlags.Static | BindingFlags.Public)).Select<FieldInfo, T>((t => (T) t.GetValue(null))).ToList();
    }

    public static List<KeyTextItem> ConvertToKeyValueList()
    {
      List<T> values = EnumHelper<T>.GetValues();
      List<KeyTextItem> keyTextItemList = new List<KeyTextItem>();
      if (null == values)
        return keyTextItemList;
      StringEnum stringEnum = new StringEnum(typeof (T));
      keyTextItemList.AddRange(values.Select<T, KeyTextItem>((item => new KeyTextItem()
      {
        Id = item.ToString(),
        Text = stringEnum.GetStringValue(item.ToString()) ?? item.ToString()
      })));
      return keyTextItemList;
    }

    public static List<SelectListItem> ConvertToSelectListItem(bool selectAll = false, string text = "-- Tất cả --")
    {
      List<T> values = EnumHelper<T>.GetValues();
      List<SelectListItem> selectListItemList = new List<SelectListItem>();
      if (null == values)
        return selectListItemList;
      StringEnum stringEnum = new StringEnum(typeof (T));
      selectListItemList.AddRange(values.Select<T, SelectListItem>((item => new SelectListItem()
      {
        Value = ((int) Enum.Parse(typeof (T), item.ToString())).ToString(),
        Text = stringEnum.GetStringValue(item.ToString()) ?? item.ToString()
      })));
      if (selectAll)
      {
        SelectListItem selectListItem = new SelectListItem()
        {
          Value = "-1",
          Text = text
        };
        selectListItemList.Insert(0, selectListItem);
      }
      return selectListItemList;
    }

    public static T GetByValue(string value)
    {
      return EnumHelper<T>.GetValues().FirstOrDefault((o => o.ToString().ToUpper() == value.ToUpper()));
    }

    public static T GetByPosition(int position)
    {
      List<T> field = EnumHelper<T>.GetValues();
      return field.FirstOrDefault((o => field.IndexOf(o) == position));
    }

    public static string GetStringName(int val)
    {
      string name = Enum.GetName(typeof (T), val);
      return new StringEnum(typeof (T)).GetStringValue(name.ToString()) ?? name.ToString();
    }
  }
}
