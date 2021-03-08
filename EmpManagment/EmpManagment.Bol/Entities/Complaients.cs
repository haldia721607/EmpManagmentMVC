using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpManagment.Bol.Entities
{
    public class Complaients : EntityTypeConfiguration<Complaients>
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ComplaientId { get; set; }
        [Required]
        [MaxLength(200)]
        public string ComplainantName { get; set; }
        [Required]
        [MaxLength(200)]
        public string ComplainantEmail { get; set; }
        [Required]
        public bool ComplaientStatus { get; set; }
        public DateTime? CompaientDate { get; set; }
        //public virtual ComplaientDetails ComplaientDetails { get; set; }
    }
}
