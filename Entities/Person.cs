﻿using System;
using System.Collections.Generic;
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
        public Guid? PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Adress { get; set; }
        public Guid? CountryId { get; set; }
        public bool ReceiveNewsLetters { get; set; }

    }
}
