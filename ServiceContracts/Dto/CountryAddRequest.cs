using Entities;
using System;
using System.Collections.Generic;


namespace ServiceContracts.Dto
{
    public class CountryAddRequest
    {
        public string? CountryName { get; set; }
        public Country ToCountry()
        {
            return new Country { CountryName = CountryName };
        }
    }
}
