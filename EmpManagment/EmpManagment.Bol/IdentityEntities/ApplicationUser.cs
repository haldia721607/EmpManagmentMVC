using EmpManagment.Bol.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpManagment.Bol.IdentityEntities
{
    public class ApplicationUser : IdentityUser
    {
        [ForeignKey("Country")]
        //[Required]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        [ForeignKey("State")]
        //[Required]
        public int StateId { get; set; }
        public virtual State State { get; set; }

        [ForeignKey("City")]
        //[Required]
        public int CityId { get; set; }
        public virtual City City { get; set; }
    }
}
