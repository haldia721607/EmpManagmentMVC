using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EmpManagment.Bol.ViewModels.UserAreaViewModels.ViewModels
{
    public class BikeCategoryMainViewModel
    {
        public BikeCategoryMainViewModel()
        {
            this.ErrorDetails = new List<ErrorDetails>();
            this.Genders = new List<SelectListItem>();
        }
        public BikeCategoriesViewModel BikeCategoriesViewModel { get; set; }
        public List<ErrorDetails> ErrorDetails { get; set; }
        public List<SelectListItem> Genders { get; set; }
        //public bool iSuccess { get; set; }
    }
}
