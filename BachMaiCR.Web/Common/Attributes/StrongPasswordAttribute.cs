using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Resources;

namespace BachMaiCR.Web.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class StrongPasswordAttribute : DataTypeAttribute, IClientValidatable
    {
        private static Regex _regex = new Regex(@"(?=^.{6,100}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{:;'?/>.<,])(?!.*\s).*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public StrongPasswordAttribute()
            : base(DataType.Password)
        {
            ErrorMessage = Localization.StrongPasswordAttributeInvalid;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ValidationType = "strongpassword",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var valueAsString = value as string;
            return valueAsString != null && _regex.Match(valueAsString).Length > 0;
        }
    }
}