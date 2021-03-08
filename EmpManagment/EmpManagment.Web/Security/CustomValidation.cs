using EmpManagment.Bll.UserBs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmpManagment.Web.Security
{
    public class CustomValidation : ValidationAttribute
    {
        public SqlComplaientBs sqlComplaientBs;

        public CustomValidation()
        {
            sqlComplaientBs = new SqlComplaientBs();
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string description = Convert.ToString(value);
            bool user = sqlComplaientBs.CategoryExisits(description);
            if (user != true)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"{description} is already in use.");
            }
        }
    }
    public class RequiredSpecialWord : ValidationAttribute

    {

        private string _word;

        public RequiredSpecialWord(string word)

        {

            _word = word;

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)

        {

            if (Convert.ToString(value).Contains(_word))

            {

                return ValidationResult.Success;

            }

            else

            {

                return new ValidationResult(_word + " not included");

            }

        }

    }
}