using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Dto
{
    public class PersonUpdateRequest
    {
        public Guid? PersonID { get; set; }

        [Required(ErrorMessage = "Person Name can't to be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can't to be blank")]
        [EmailAddress]
        public string? Email { get; set; }
        public GenderOptions? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Adress { get; set; }
        public Guid? CountryId { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public Person ToPerson()
        {
            return new Person
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Gender = Gender.ToString(),
                DateOfBirth = DateOfBirth,
                Adress = Adress,
                CountryId = CountryId,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
        public override string ToString()
        {
            return $"Perosn ID : {PersonID}, person name : {PersonName}" +
                $"  , Gender : {Gender} , Date of birth : {DateOfBirth?.ToString("dd MMMM yyyy")} " +
                $" CountryID : {CountryId} , Adress : {Adress} , Receive News Letters : {ReceiveNewsLetters} ";
        }
    }
}
