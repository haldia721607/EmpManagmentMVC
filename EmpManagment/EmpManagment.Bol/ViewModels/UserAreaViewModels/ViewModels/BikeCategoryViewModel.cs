using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpManagment.Bol.ViewModels.UserAreaViewModels.ViewModels
{
    public class BikeCategoryViewModel
    {
        [Required]
        public int BikeCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public bool Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string sCreatedDate { get; set; }
    }
}
