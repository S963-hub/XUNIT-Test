using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Threading.Tasks;
using Services;
using ServiceContracts.Dto;
using ServiceContracts.Enums;
using Entities;
using Xunit.Abstractions;
using Xunit.Sdk;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _outputHelper;
        public PersonsServiceTest(ITestOutputHelper testOutputHelper, IPersonsService personService, ICountriesService countriesService)
        {
            _countriesService = new CountriesService(new PersonDbContext(new DbContextOptionsBuilder<PersonDbContext>().Options));

            _personService = new PersonsService(new PersonDbContext(new DbContextOptionsBuilder<PersonDbContext>().Options), _countriesService);

            _outputHelper = testOutputHelper;
        }

        #region AddPerson
        //case1 
        [Fact]
        public void AddPerson_NullValue()
        {
            //Arrange
            PersonAddRequest? request = null;

            //Act
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.AddPerson(request);
            });
        }

        //case2
        [Fact]
        public void AddPerson_PersonNameNullValue()
        {
            //Arrange
            PersonAddRequest? request2 = new PersonAddRequest()
            {
                PersonName = null
            };

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _personService.AddPerson(request2);
            });

        }

        //case3
        [Fact]
        public void AddPerson_PersontoList()
        {
            //Arrange
            PersonAddRequest? request3 = new PersonAddRequest()
            {
                PersonName = "subhi",
                Address = "syria"
            };

            //Act
            PersonResponse person_response_from_add = _personService.AddPerson(request3);
            List<PersonResponse> persons_list = _personService.GetAllPersons();

            //Assert 
            Assert.Contains(person_response_from_add, persons_list);
            Assert.True(request3.PersonID != Guid.Empty);
        }
        #endregion

        #region GetPersonByPersonId

        [Fact]
        public void GetPersonByPersonId_NullPersonId()
        {
            //Arrange
            Guid? personId = null;

            //Act
            PersonResponse? personResponse_form_get = _personService.GetPersonByPersonId(personId);

            //Assert
            Assert.Null(personResponse_form_get);
        }

        [Fact]
        public void GetPersonByPersonId_withValidPersonId()
        {
            //Arrange
            CountryAddRequest? Country_request = new CountryAddRequest() { CountryName = "syria" };
            CountryResponse response_Country_Name = new CountryResponse();
            _countriesService.AddCountry(Country_request);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonID = Guid.Empty,
                PersonName = "subhi",
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("01.01.2004"),
                Address = "Aleppo",
                CountryID = response_Country_Name.CountryID,
                ReceiveNewsLetters = true,
            };
            PersonResponse PersonResponse2 = _personService.AddPerson(personAddRequest);

            //Act
            PersonResponse? personResponse_form_get = _personService.GetPersonByPersonId(PersonResponse2.PersonID);

            //Assert
            Assert.Equal(PersonResponse2, personResponse_form_get);
        }
        #endregion

        #region GetAllPersons

        [Fact]
        public void GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse> personlist_from_get = _personService.GetAllPersons();

            //Assert
            Assert.Empty(personlist_from_get);
        }

        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "syria" };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "Palastine" };
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryAddRequest2);

            //Arrange
            PersonAddRequest request = new PersonAddRequest()
            {
                PersonName = "subhi",
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("01.01.2001"),
                Address = "Homs",
                CountryID = countryResponse2.CountryID,
                ReceiveNewsLetters = true,
            };
            PersonAddRequest request2 = new PersonAddRequest()
            {
                PersonName = "ahmet",
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("01.01.2002"),
                Address = "syria",
                CountryID = countryResponse1.CountryID,
                ReceiveNewsLetters = true,
            };
            PersonAddRequest request3 = new PersonAddRequest()
            {
                PersonName = "muhammed",
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("01.01.2003"),
                Address = "halep",
                CountryID = countryResponse2.CountryID,
                ReceiveNewsLetters = true,
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
            request, request2, request3
            };

            List<PersonResponse> personlist_from_Add = new List<PersonResponse>();

            foreach (var person_request in person_requests)
            {
                PersonResponse response = _personService.AddPerson(person_request);
                personlist_from_Add.Add(response);
            }
            //output test
            _outputHelper.WriteLine("Beklenilen : ");
            foreach (var item in personlist_from_Add)
            {
                _outputHelper.WriteLine(item.ToString());
            }

            //Act
            List<PersonResponse> personlist_from_method = _personService.GetAllPersons();// Bu tum personelleri getirir.

            //output test
            _outputHelper.WriteLine("sonuc : ");
            foreach (var item in personlist_from_method)
            {
                _outputHelper.WriteLine(item.ToString());
            }

            //Assert
            foreach (var i in personlist_from_Add)
            {
                Assert.Contains(i, personlist_from_method);
            }
        }

        #endregion

        #region GetFilteredPerson

        //eger bos SearchBy olursa tum peronelleri getirmesi gerekmektedir
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "syria" };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "Palastine" };
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryAddRequest2);

            //Arrange
            PersonAddRequest request = new PersonAddRequest()
            {
                PersonName = "subhi",
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("01.01.2001"),
                Address = "Homs",
                CountryID = countryResponse2.CountryID,
                ReceiveNewsLetters = true,
            };
            PersonAddRequest request2 = new PersonAddRequest()
            {
                PersonName = "ahmet",
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("01.01.2002"),
                Address = "syria",
                CountryID = countryResponse1.CountryID,
                ReceiveNewsLetters = true,
            };
            PersonAddRequest request3 = new PersonAddRequest()
            {
                PersonName = "muhammed",
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("01.01.2003"),
                Address = "halep",
                CountryID = countryResponse2.CountryID,
                ReceiveNewsLetters = true,
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
            request, request2, request3
            };

            List<PersonResponse> personlist_from_Add = new List<PersonResponse>();

            foreach (var person_request in person_requests)
            {
                PersonResponse response = _personService.AddPerson(person_request);
                personlist_from_Add.Add(response);
            }

            //Act
            List<PersonResponse> personlist_from_SearchList = _personService.GetFilteretPerson(nameof(Person.PersonName), ""); // Bu tum personelleri getirir.

            //Assert
            foreach (var i in personlist_from_Add)
            {
                Assert.Contains(i, personlist_from_SearchList);
            }
        }

        // persons eklenilir ve PersonName gore aranir ondan sonra person dondurulecek
        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
        {
            CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "syria" };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "Palastine" };
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryAddRequest2);

            //Arrange
            PersonAddRequest request = new PersonAddRequest()
            {
                PersonName = "subhi",
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("01.01.2001"),
                Address = "Homs",
                CountryID = countryResponse2.CountryID,
                ReceiveNewsLetters = true,
            };
            PersonAddRequest request2 = new PersonAddRequest()
            {
                PersonName = "ahmet",
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("01.01.2002"),
                Address = "syria",
                CountryID = countryResponse1.CountryID,
                ReceiveNewsLetters = true,
            };
            PersonAddRequest request3 = new PersonAddRequest()
            {
                PersonName = "muhammed",
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("01.01.2003"),
                Address = "halep",
                CountryID = countryResponse2.CountryID,
                ReceiveNewsLetters = true,
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
            request, request2, request3
            };

            List<PersonResponse> personlist_from_Add = new List<PersonResponse>();


            foreach (var person_request in person_requests)
            {
                PersonResponse response = _personService.AddPerson(person_request);
                personlist_from_Add.Add(response);
                _outputHelper.WriteLine("1");
                _outputHelper.WriteLine(personlist_from_Add.ToString());
            }

            //Act
            List<PersonResponse> personlist_from_SearchList = _personService.GetFilteretPerson(nameof(Person.PersonName), "bhi"); // Bu tum personelleri getirir.
            _outputHelper.WriteLine("2");
            _outputHelper.WriteLine(personlist_from_SearchList.ToString());
            //Assert

            foreach (var i in personlist_from_Add)
            {
                if (i.PersonName != null)
                {

                    if (i.PersonName.Contains("subhi", StringComparison.OrdinalIgnoreCase)) // StringComparison.OrdinalIgnoreCase kucuk buyuk harf duyarli olmadan ara
                    {
                        Assert.Contains(i, personlist_from_SearchList);
                    }
                }
            }
        }
        #endregion

        #region GetSortedPersons
        [Fact]
        public void GetSortedPersons_De()
        {
            CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "syria" };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "Palastine" };
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryAddRequest2);

            //Arrange
            PersonAddRequest request = new PersonAddRequest()
            {
                PersonName = "subhi",
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("01.01.2001"),
                Address = "Homs",
                CountryID = countryResponse2.CountryID,
                ReceiveNewsLetters = true,
            };
            PersonAddRequest request2 = new PersonAddRequest()
            {
                PersonName = "ahmet",
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("01.01.2002"),
                Address = "syria",
                CountryID = countryResponse1.CountryID,
                ReceiveNewsLetters = true,
            };
            PersonAddRequest request3 = new PersonAddRequest()
            {
                PersonName = "muhammed",
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("01.01.2003"),
                Address = "halep",
                CountryID = countryResponse2.CountryID,
                ReceiveNewsLetters = true,
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
            request, request2, request3
            };

            List<PersonResponse> personlist_from_Add = new List<PersonResponse>();


            foreach (var person_request in person_requests)
            {
                PersonResponse response = _personService.AddPerson(person_request);
                personlist_from_Add.Add(response);
            }

            //Act
            List<PersonResponse> allpersons = _personService.GetAllPersons();

            List<PersonResponse> personlist_from_GetSortedPersons = _personService.GetSortedPersons(allpersons, nameof(Person.PersonName), SortedOrderEnum.DESC); // Bu tum personelleri getirir.

            List<PersonResponse> personlist_from_Desc = allpersons.OrderByDescending(person => person.PersonName).ToList();



            //Assert
            for (int i = 0; i < allpersons.Count; i++)
            {
                Assert.Equal(personlist_from_Desc[i], personlist_from_GetSortedPersons[i]);
            }
        }
        #endregion

        #region UpdateRequest
        // null person 
        [Fact]
        public void UpdateRequest_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? Person_Update_Request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personService.UpdatePerson(Person_Update_Request);
            });

        }

        // Invalid personID it should ArgumentException
        [Fact]
        public void UpdateRequest_InvalidPersonID()
        {
            //Arrange
            PersonUpdateRequest? Person_Update_Request = new PersonUpdateRequest() { PersonID = Guid.NewGuid() };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.UpdatePerson(Person_Update_Request);
            });
        }

        // when it PersonName is null it should ArgumentException
        [Fact]
        public void UpdateRequest_NullPersonName()
        {
            //Arrage
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "syria" };
            CountryResponse countryResponse_from_add = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "subhi",
                CountryID = countryResponse_from_add.CountryID,
                DateOfBirth = DateTime.Parse("01.01.2001"),
                Gender = GenderOptions.Male,
                Address = "Homs",
                ReceiveNewsLetters = true,
            };
            PersonResponse personResponse_from_add = _personService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = personResponse_from_add.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            //Assert
            Assert.Throws<ArgumentException>(() => {

                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdateRequest_FullPersonUpdatDetails()
        {
            //Arrage
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "syria" };
            CountryResponse countryResponse_from_add = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "subhi",
                CountryID = countryResponse_from_add.CountryID,
                Email = "subhi@gmail.com",
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2004-01-01"),
                Address = "syria",
                ReceiveNewsLetters = true,
            };
            PersonResponse personResponse_from_add = _personService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = personResponse_from_add.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "Ahmed";
            personUpdateRequest.Email = "ahmed@example.com";

            //Act
            PersonResponse person_response_from_update = _personService.UpdatePerson(personUpdateRequest);
            PersonResponse? person_response_from_get = _personService.GetPersonByPersonId(personResponse_from_add.PersonID);

            //Assert
            Assert.Equal(person_response_from_update, person_response_from_get);

        }
        #endregion

        #region DeletePerson

        [Fact]
        public void DeletePerson_ValidPerson()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "syria" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "subhi",
                Email = "subhi@example.com",
                Address = "Halep",
                ReceiveNewsLetters = true,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                CountryID = countryResponse.CountryID
            };
            PersonResponse personResponse = _personService.AddPerson(personAddRequest);


            //Act 
            bool isDeleted = _personService.DeletePerson(personResponse.PersonID);

            //Assert
            Assert.True(isDeleted);


        }

        [Fact]
        public void DeletePerson_InvalidPerson()
        {
            //Act 
            bool isDeleted = _personService.DeletePerson(Guid.NewGuid());

            //Assert
            Assert.False(isDeleted);
        }
        #endregion
    }
}
