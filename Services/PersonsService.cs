using Entities;
using Microsoft.VisualBasic;
using ServiceContracts;
using ServiceContracts.Dto;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;
        public PersonsService(bool initialize = true)
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService(false);
            _persons.AddRange(new List<Person>()
            {
                new Person(){
                    PersonID = Guid.Parse("87080267-7F9D-4E9C-BF76-C24B77D595DA"),
                    PersonName = "Aguste",
                    Email ="aleddy@booking.com",
                    Gender = "Male" ,
                    Adress ="0742 Fieldstone Lane",
                    DateOfBirth =DateTime.Parse("1991-06-24"),
                    ReceiveNewsLetters = true ,
                    CountryId =Guid.Parse("45E1313C-5640-4458-B486-7354EC75578A")
                },
				new Person(){
	                PersonID = Guid.Parse("1EA0FED0-6727-43AB-BFC8-1F7F4975B3FD"),
	                PersonName = "Sophia",
	                Email = "sophia.williams@example.com",
	                Gender = "Female",
	                Adress = "123 Maple Street",
	                DateOfBirth = DateTime.Parse("1989-02-15"),
	                ReceiveNewsLetters = false,
	                CountryId = Guid.Parse("401EF388-2C06-477A-B302-0A6398294FDF")
                },
				new Person(){
	                PersonID = Guid.Parse("7DEB7D81-4B35-4121-8BA4-7EFD42E6D7E6"),
                    PersonName = "Liam",
                	Email = "liam.johnson@example.org",
                   	Gender = "Male",
                	Adress = "456 Oak Avenue",
                	DateOfBirth = DateTime.Parse("1993-11-03"),
	                ReceiveNewsLetters = true,
	                CountryId = Guid.Parse("AC2B18CF-FDAC-45CF-AAE7-0B6EBFB730E2")
                },
                new Person(){
                  PersonID = Guid.Parse("1D3E7AEE-CD16-40E5-801D-6D9D885E796B"),
                 PersonName = "Emma",
                 Email = "emma.brown@example.net",
                 Gender = "Female",
                 Adress = "789 Pine Boulevard",
                DateOfBirth = DateTime.Parse("1995-07-28"),
                ReceiveNewsLetters = true,
                CountryId = Guid.Parse("4C697D86-1659-4900-AE7B-FD679EA2CF4F")
                },
                new Person(){
                 PersonID = Guid.Parse("BD8E165C-9D41-4CC7-A9B4-1D22A02E67E2"),
                    PersonName = "Noah",
                    Email = "noah.davis@example.com",
                    Gender = "Male",
                    Adress = "321 Cedar Drive",
                    DateOfBirth = DateTime.Parse("1990-03-12"),
                    ReceiveNewsLetters = false,
                    CountryId = Guid.Parse("06811347-447A-4C95-AD58-1F1026F5AABD")
                },
            }) ;
        }
        public PersonResponse ConvertPerson_To_PersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryID(person.CountryId)?.CountryName;
            return personResponse;

        }
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null) { throw new ArgumentNullException(nameof(personAddRequest)); }

            //Validate PersonName
            if (string.IsNullOrEmpty(personAddRequest.PersonName))
            {
                throw new ArgumentException("PersonName can't be blank");
            }


            Person person = personAddRequest.ToPerson();

            person.PersonID = Guid.NewGuid();
            _persons.Add(person);

            return ConvertPerson_To_PersonResponse(person);
        }
        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(person => person.ToPersonResponse()).ToList();
        }
        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if (personId == null)
            {
                return null;
            }
            Person? GetPersonByPersonId_object = _persons.FirstOrDefault(temp => temp.PersonID == personId);

            if (GetPersonByPersonId_object == null)
                return null;

            return GetPersonByPersonId_object.ToPersonResponse();
        }
        public List<PersonResponse> GetFilteretPerson(string SearchBy, string? SearchString)
        {
            List<PersonResponse> AllPersons = GetAllPersons();
            List<PersonResponse> MatchingPersons = AllPersons;

            if (string.IsNullOrEmpty(SearchBy) || string.IsNullOrEmpty(SearchString))
                return MatchingPersons;

            switch (SearchBy)
            {
                case nameof(PersonResponse.PersonName):
                    MatchingPersons = AllPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.PersonName)) ?
                    temp.PersonName.Contains(SearchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.Email):
                    MatchingPersons = AllPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Email)) ?
                    temp.Email.Contains(SearchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.DateOfBirth):
                    MatchingPersons = AllPersons.Where(temp =>
                    (temp.DateOfBirth != null) ?
                    temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(SearchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.Gender):
                    MatchingPersons = AllPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Gender)) ?
                    temp.Gender.Contains(SearchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.CountryId):
                    MatchingPersons = AllPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Country)) ?
                    temp.Country.Contains(SearchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.Adress):
                    MatchingPersons = AllPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Adress)) ?
                    temp.Adress.Contains(SearchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                default:
                    MatchingPersons = AllPersons;
                    break;
            }

            return MatchingPersons;
        }
        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allpersons, string SortedBy, SortedOrderEnum sortOrder)
        {
            if (string.IsNullOrEmpty(SortedBy))
                return allpersons;

            List<PersonResponse> SortedPersons = (SortedBy, sortOrder)
            switch
            {
                (nameof(PersonResponse.PersonName), SortedOrderEnum.ASC) => allpersons.OrderBy(temp => temp.PersonName).ToList(),
                (nameof(PersonResponse.PersonName), SortedOrderEnum.DESC) => allpersons.OrderByDescending(temp => temp.PersonName).ToList(),
                (nameof(PersonResponse.Email), SortedOrderEnum.ASC) => allpersons.OrderBy(temp => temp.Email).ToList(),
                (nameof(PersonResponse.Email), SortedOrderEnum.DESC) => allpersons.OrderByDescending(temp => temp.Email).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortedOrderEnum.ASC) => allpersons.OrderBy(temp => temp.DateOfBirth).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortedOrderEnum.DESC) => allpersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),
                (nameof(PersonResponse.Adress), SortedOrderEnum.ASC) => allpersons.OrderBy(temp => temp.Adress).ToList(),
                (nameof(PersonResponse.Adress), SortedOrderEnum.DESC) => allpersons.OrderByDescending(temp => temp.Adress).ToList(),
                (nameof(PersonResponse.Country), SortedOrderEnum.ASC) => allpersons.OrderBy(temp => temp.Country).ToList(),
                (nameof(PersonResponse.Country), SortedOrderEnum.DESC) => allpersons.OrderByDescending(temp => temp.Country).ToList(),
                (nameof(PersonResponse.Age), SortedOrderEnum.ASC) => allpersons.OrderBy(temp => temp.Age).ToList(),
                (nameof(PersonResponse.Age), SortedOrderEnum.DESC) => allpersons.OrderByDescending(temp => temp.Age).ToList(),
                (nameof(PersonResponse.Gender), SortedOrderEnum.ASC) => allpersons.OrderBy(temp => temp.Gender).ToList(),
                (nameof(PersonResponse.Gender), SortedOrderEnum.DESC) => allpersons.OrderByDescending(temp => temp.Gender).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortedOrderEnum.ASC) => allpersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortedOrderEnum.DESC) => allpersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

                _ => allpersons    // _ default anlamina gelir 
            };
            return SortedPersons;
        }
        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
                throw new ArgumentNullException(nameof(personUpdateRequest));

            ValidationHelper.ModelValidation(personUpdateRequest);

            Person? matchingPerson = _persons.FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonID);
            if (matchingPerson == null)
            {
                throw new ArgumentNullException("PersonID does not exist");
            }

            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.Adress = personUpdateRequest.Adress;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.CountryId = personUpdateRequest.CountryId;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            return matchingPerson.ToPersonResponse();
        }
        public bool DeletePerson(Guid? PersonID)
        {
            if (PersonID == null) throw new ArgumentNullException(nameof(PersonID));

            Person? person = _persons.FirstOrDefault(person => person.PersonID == PersonID);
            if (person == null)
                return false;

            _persons.RemoveAll(temp => temp.PersonID == PersonID);
            return true;
        }
    }
}
