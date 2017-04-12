using System;

namespace BachMaiCR.Utilities.Enums
{
  public class StringValueAttribute : Attribute
  {
    private string _value;

    public string Value
    {
      get
      {
        return this._value;
      }
    }

    public StringValueAttribute(string value)
    {
      this._value = value;
    }
  }
}
