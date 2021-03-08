using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpManagment.Bol.ViewModels.UserAreaViewModels.ViewModels
{
    public class ComplaientCategoryViewModel
    {
        public int ComplaientCategoryId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Complaient category description required")]
        [MaxLength(500, ErrorMessage = "Max length 100")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Complaient category status required")]
        public bool Status { get; set; }
        public string UserStatus { get; set; }
    }
}
