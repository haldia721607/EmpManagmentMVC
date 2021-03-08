using EmpManagment.Bol.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EmpManagment.Bol.ViewModels.UserAreaViewModels.ViewModels
{
    //class UserViewModels
    //{
    //}
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
            this.Countries = new List<SelectListItem>();
            this.States = new List<SelectListItem>();
            this.Cities = new List<SelectListItem>();
        }

        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [ForeignKey("Country")]
        public int CountryId { get; set; }
        [Required]
        public virtual Country Country { get; set; }

        [ForeignKey("State")]
        public int StateId { get; set; }
        [Required]
        public virtual State State { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }
        [Required]
        public virtual City City { get; set; }
        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public List<SelectListItem> States { get; set; }
        public List<SelectListItem> Cities { get; set; }
    }
    public class UserClaim
    {
        public string ClaimType { get; set; }
        public bool IsSelected { get; set; }
    }
    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            Cliams = new List<UserClaim>();
        }

        public string UserId { get; set; }
        public List<UserClaim> Cliams { get; set; }
    }
}
