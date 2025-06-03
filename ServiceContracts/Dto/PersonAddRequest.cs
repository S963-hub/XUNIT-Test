using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.Dto
{
    public class PersonAddRequest
    {
        public Guid? PersonID { get; set; }

        [Required(ErrorMessage = "Person Name can't to be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email can't to be blank")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        public GenderOptions? Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public Guid? CountryID { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        public Person ToPerson()
        {
            return new Person
            {
                PersonName = PersonName,
                Email = Email,
                Gender = Gender.ToString(),
                DateOfBirth = DateOfBirth,
                Address = Address,
                CountryID = CountryID,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
        public override string ToString()
        {
            return $"Perosn ID : {PersonID}, person name : {PersonName}" +
                $"  , Gender : {Gender} , Date of birth : {DateOfBirth?.ToString("dd MMMM yyyy")} " +
                $" CountryID : {CountryID} , Adress : {Address} , Receive News Letters : {ReceiveNewsLetters} ";
        }
    }
}
