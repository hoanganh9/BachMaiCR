using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace BachMaiCR.Web.Common.Attributes
{
    /// <summary>
    /// Number valid
    /// </summary>
    /// Description: Accept input number only
    /// Author: LinhLN1
    /// Update: Hungcd1.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DigitAttribute : DataTypeAttribute, IClientValidatable
    {
        private static Regex _regex = new Regex(@"^\d?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public DigitAttribute()
            : base(DataType.Text)
        {

        }

        public override bool IsValid(object value)
        {
            if (value == null || value.ToString().Trim() == "")
            {
                return true;
            }

            var valueAsString = value as string;
            return valueAsString != null && _regex.Match(valueAsString).Length > 0;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {

                ValidationType = "digits",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
        }
    }
}