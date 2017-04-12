using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Resources;

namespace BachMaiCR.Web.Common.Attributes
{
    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    //public sealed class DoubleAttribute : DataTypeAttribute, IClientValidatable
    //{
    //    private static Regex _regex = new Regex(@"^[0-9]([.][0-9])?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    //    public DoubleAttribute()
    //        : base()
    //    {

    //    }

    //    public override bool IsValid(object value)
    //    {
    //        if (value == null || value.ToString().Trim() == "")
    //        {
    //            return true;
    //        }

    //        var valueAsString = value as string;
    //        return valueAsString != null && _regex.Match(valueAsString).Length > 0;
    //    }

    //    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    //    {
    //        yield return new ModelClientValidationRule
    //        {

    //            ValidationType = "Double",
    //            ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
    //        };
    //    }
    //}
}