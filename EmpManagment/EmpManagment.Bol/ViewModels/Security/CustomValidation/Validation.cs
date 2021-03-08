using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EmpManagment.Bol.ViewModels.Security.CustomValidation
{
    public class Validation
    {
    }
    public class ValidEmailDomain : ValidationAttribute
    {
        private readonly string allowedDomain;

        public ValidEmailDomain(string allowedDomain)
        {
            this.allowedDomain = allowedDomain;
        }
        public override bool IsValid(object value)
        {
            string[] strings = value.ToString().Split('@');
            return strings[1].ToUpper() == allowedDomain.ToUpper();
        }
    }
    public class IsChecked : ValidationAttribute
    {
        private readonly string status;
        public IsChecked(string status)
        {
            this.status = status;
        }
        public override bool IsValid(object value)
        {
            return Convert.ToBoolean(status) == Convert.ToBoolean(value);
        }
    }
}
