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
        private readonly PersonDbContext _db;
        private readonly ICountriesService _countriesService;
        public PersonsService(PersonDbContext personDbContext, ICountriesService countriesService)
        {
            _db = personDbContext;
            _countriesService = countriesService;
        }
        public PersonResponse ConvertPerson_To_PersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;
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
            _db.Persons.Add(person);
            _db.SaveChanges();

            return ConvertPerson_To_PersonResponse(person);
        }
        public List<PersonResponse> GetAllPersons()
        {
            return _db.Persons.ToList().
                Select(temp => ConvertPerson_To_PersonResponse(temp)).ToList();
        }
        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if (personId == null)
            {
                return null;
            }
            Person? GetPersonByPersonId_object = _db.Persons.FirstOrDefault(temp => temp.PersonID == personId);

            if (GetPersonByPersonId_object == null)
                return null;

            return ConvertPerson_To_PersonResponse(GetPersonByPersonId_object);
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
					MatchingPersons = AllPersons
						.Where(temp =>
							!string.IsNullOrEmpty(temp.Gender) &&
							(
								string.Equals(SearchString, "Male", StringComparison.OrdinalIgnoreCase)
									? string.Equals(temp.Gender, "Male", StringComparison.OrdinalIgnoreCase)
									: temp.Gender.Contains(SearchString, StringComparison.OrdinalIgnoreCase)
							))
						.ToList();
					break;

				case nameof(PersonResponse.CountryID):
                    MatchingPersons = AllPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Country)) ?
                    temp.Country.Contains(SearchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.Address):
                    MatchingPersons = AllPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Address)) ?
                    temp.Address.Contains(SearchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
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
                (nameof(PersonResponse.Address), SortedOrderEnum.ASC) => allpersons.OrderBy(temp => temp.Address).ToList(),
                (nameof(PersonResponse.Address), SortedOrderEnum.DESC) => allpersons.OrderByDescending(temp => temp.Address).ToList(),
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

            Person? matchingPerson = _db.Persons.FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonID);
            if (matchingPerson == null)
            {
                throw new ArgumentNullException("PersonID does not exist");
            }

            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            _db.SaveChanges();
            return ConvertPerson_To_PersonResponse(matchingPerson);
        }
        public bool DeletePerson(Guid? PersonID)
        {
            if (PersonID == null) throw new ArgumentNullException(nameof(PersonID));

            Person? person = _db.Persons.FirstOrDefault(person => person.PersonID == PersonID);
            if (person == null)
                return false;

            _db.Persons.Remove(_db.Persons.First(temp => temp.PersonID == PersonID));
            return true;
        }
    }
}
