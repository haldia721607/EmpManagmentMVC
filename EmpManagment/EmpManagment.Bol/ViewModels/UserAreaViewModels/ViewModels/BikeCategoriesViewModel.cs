using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpManagment.Bol.ViewModels.UserAreaViewModels.ViewModels
{
    public class BikeCategoriesViewModel
    {
        public int BikeCategoryId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}
