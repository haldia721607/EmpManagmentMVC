using EmpManagment.Bol.ViewModels.Security.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit.Sdk;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace EmpManagment.Bol.ViewModels.Security.ViewModels
{
    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            this.Countries = new List<SelectListItem>();
            this.States = new List<SelectListItem>();
            this.Cities = new List<SelectListItem>();
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill name")]
        [MaxLength(50, ErrorMessage = "Max length 50 char.")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill name")]
        [MaxLength(50, ErrorMessage = "Max length 50 char.")]
        [EmailAddress]
        //[Remote(action: "IsEmailInUse", controller: "Account", areaName: "Security")]
        [ValidEmailDomain(allowedDomain: "gmail.com", ErrorMessage = "Email domain must be gmail.com")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please fill confirm password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passord and confirm password does not match.")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select country.")]
        public int countryId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select state.")]
        public int stateId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select city.")]
        public int cityId { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public List<SelectListItem> States { get; set; }
        public List<SelectListItem> Cities { get; set; }
        [IsChecked(status: "true", ErrorMessage = "Please check terms and policy")]
        public bool TermsAndPolicy { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

}
