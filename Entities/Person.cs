using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    /// <summary>
    /// person class  
    /// </summary>
    ///
    public class Person
    {
        [Key]
        public Guid? PersonID { get; set; }

        [StringLength(50)]
        public string? PersonName { get; set; }

		[StringLength(50)]
		public string? Email { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [StringLength (200)]
        [Column("Address")]
        public string? Address { get; set; }

		[Column("CountryID")]
		public Guid? CountryID { get; set; }
        public bool ReceiveNewsLetters { get; set; }

    }
}
