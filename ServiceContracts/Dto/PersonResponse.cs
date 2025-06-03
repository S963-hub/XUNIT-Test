using Entities;
using Microsoft.VisualBasic;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;

namespace ServiceContracts.Dto
{
    public class PersonResponse
    {
        public Guid? PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public double? Age { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse person = (PersonResponse)obj;

            return
                PersonID == person.PersonID &&
                PersonName == person.PersonName &&
                Email == person.Email &&
                Gender == person.Gender &&
                DateOfBirth == person.DateOfBirth &&
                CountryID == person.CountryID &&
                Address == person.Address &&
                ReceiveNewsLetters == person.ReceiveNewsLetters;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Perosn ID : {PersonID}, person name : {PersonName}" +
                $" , Email : {Email} , Gender : {Gender} , Date of birth : {DateOfBirth?.ToString("dd MMMM yyyy")} " +
                $" CountryID : {CountryID} , Adress : {Address} , Receive News Letters : {ReceiveNewsLetters} ";
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
                DateOfBirth = DateOfBirth,
                Address = Address,
                CountryID = CountryID,
                ReceiveNewsLetters = ReceiveNewsLetters,

            };
        }

    }



    public static class PersonExtentions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                PersonName = person.PersonName,
                Address = person.Address,
                Email = person.Email,
                Gender = person.Gender,
                DateOfBirth = person.DateOfBirth,
                CountryID = person.CountryID,
                ReceiveNewsLetters = person.ReceiveNewsLetters,
                Age = (person.DateOfBirth != null) ? Math.Round
                ((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null
            };
        }
    }

}
