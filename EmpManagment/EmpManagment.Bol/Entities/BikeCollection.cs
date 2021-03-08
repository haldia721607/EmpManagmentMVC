using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpManagment.Bol.Entities
{
    public class BikeCollection
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BikeCollectionId { get; set; }

        [ForeignKey("ComplaientDetails")]
        [Required]
        public int ComplaientDetailsId { get; set; }
        public virtual ComplaientDetails ComplaientDetails { get; set; }
        [ForeignKey("BikeCategory")]
        [Required]
        public int BikeCategoryId { get; set; }
        public virtual BikeCategory BikeCategory { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
