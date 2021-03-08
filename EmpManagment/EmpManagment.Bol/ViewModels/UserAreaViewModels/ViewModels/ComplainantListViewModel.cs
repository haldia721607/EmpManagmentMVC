using EmpManagment.Bol.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpManagment.Bol.ViewModels.UserAreaViewModels.ViewModels
{
    public class ComplainantListViewModel
    {
        public ComplainantListViewModel()
        {
            this.BikeCategory = new List<BikeCategoryViewModel>();
        }
        public List<ComplainantAndDetailsViewModel> complainantAndDetailsViewModels { get; set; }
        public List<BikeCategoryViewModel> BikeCategory { get; set; }
    }
}
