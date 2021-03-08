using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EmpManagment.Bol.Entities
{
    public class ComplaientCategory
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ComplaientCategoryId { get; set; }
        [Required]
        [MaxLength(500)]
        [Remote(action: "IsDescriptionInUse", controller: "Complaient", areaName: "User", ErrorMessage = "Description Already Available")]
        public string Description { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
