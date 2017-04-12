/*
 * Được tạo bởi hiepth6
 * Nếu bạn thấy class có vấn đề, hoặc có cách viết tốt hơn, xin liên hệ với hiepth6@viettel.com.vn để thông tin cho tác giả
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Resources;

namespace BachMaiCR.Web.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class PhoneNumberAttribute : DataTypeAttribute, IClientValidatable
    {
        private static Regex _regex = new Regex(@"^\+?[0-9]{7,15}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);


        public PhoneNumberAttribute()
            : base(DataType.PhoneNumber)
        {
            ErrorMessage = Localization.MsgPhoneInvalid;
            
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
                //Đặc biệt chú ý ở đây ValidationType ="???" sẽ giúp gọi hàm validate tương ứng dưới client thông qua js
                //Như cái email này là có js tương ứng rồi, nếu chưa có thì phải đặt ra 1 tên mới, rồi vào file setup.project.custom.validate.js để định nghĩa
                //ra luật validate của bạn tương ứng
                //Attribute.cs này chỉ giúp bạn 2 việc
                //Gen ra code để tự động gọi hàm validate dưới client qua js
                //Và server validate thôi
                ValidationType = "phonenumber",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
        }
    }
}