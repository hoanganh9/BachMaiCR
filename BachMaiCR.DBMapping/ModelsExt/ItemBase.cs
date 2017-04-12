using System;

namespace BachMaiCR.DBMapping.ModelsExt
{
  [Serializable]
  public class ItemBase
  {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public ItemBase()
    {
      this.Id = 0;
      this.Name = string.Empty;
      this.Code = string.Empty;
    }

    public override string ToString()
    {
      return this.Name;
    }

    public int ToInt()
    {
      return this.Id;
    }
  }
}
