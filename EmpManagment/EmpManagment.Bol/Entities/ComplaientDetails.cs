﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpManagment.Bol.Entities
{
    public class ComplaientDetails
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ComplaientDetailsId { get; set; }

        [ForeignKey("Complaient")]
        public int ComplaientId { get; set; }
        [Required]
        public virtual Complaients Complaient { get; set; }

        [ForeignKey("ComplaientCategory")]
        public int ComplaientCategoryId { get; set; }
        [Required]
        public virtual ComplaientCategory ComplaientCategory { get; set; }

        [ForeignKey("Gender")]
        public int GenderId { get; set; }
        [Required]
        public virtual Gender Gender { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        [ForeignKey("State")]
        public int StateId { get; set; }
        public virtual State State { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime? ComplaientDate { get; set; }

    }
}
