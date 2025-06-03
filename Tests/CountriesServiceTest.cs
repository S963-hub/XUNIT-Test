using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.Dto;
using Services;

namespace Tests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _CountryServise;

        public CountriesServiceTest()
        {
            _CountryServise = new CountriesService(new PersonDbContext(new DbContextOptionsBuilder<PersonDbContext>().Options));
        }

        #region Add Country

        //case 1
        [Fact]
        public void addCountry_Null()
        {
            CountryAddRequest? request = null;


            Assert.Throws<ArgumentNullException>(() =>
            {
                _CountryServise.AddCountry(request);
            });
        }

        //case2
        [Fact]
        public void addCountry_Nameisnull()
        {
            CountryAddRequest? request = new CountryAddRequest()
            { CountryName = null };

            Assert.Throws<ArgumentException>(() =>
            {
                _CountryServise.AddCountry(request);
            });
        }
        #endregion


        #region Get all Countries
        [Fact]
        public void GetAllCountries_EmptyList()
        {
            //Act
            List<CountryResponse> countries_response_list = _CountryServise.GetAllCountry();

            //Assert
            Assert.Empty(countries_response_list);
        }


        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> countries_add_request_List
            = new List<CountryAddRequest>() {
            new CountryAddRequest(){ CountryName = "Syira"},
            new CountryAddRequest(){ CountryName = "Palastine"}
            };

            //Act
            List<CountryResponse> Get_All_Countries_Response =
            new List<CountryResponse>();
            foreach (CountryAddRequest Countries_request in countries_add_request_List)
            {
                Get_All_Countries_Response.Add(_CountryServise.AddCountry(Countries_request));
            }

            //Assert
            List<CountryResponse> actual_Countries_List = _CountryServise.GetAllCountry();
            foreach (CountryResponse expected_Countries in actual_Countries_List)
            {
                Assert.Contains(expected_Countries, actual_Countries_List);
            }
        }
        [Fact]
        public void GetAllCountries_PreperCountry()
        {
            //Arrange
            CountryAddRequest? request2 = new CountryAddRequest()
            { CountryName = "Japan" };

            //Act
            CountryResponse response2 = _CountryServise.AddCountry(request2);
            List<CountryResponse> countries_from_Getallcountries =
            _CountryServise.GetAllCountry();
            //Assert
            Assert.True(response2.CountryID != Guid.Empty);
            Assert.Contains(response2, countries_from_Getallcountries);
        }

        #endregion


        #region GetCountryByCountryID

        [Fact]
        public void GetCountryByCountryID_NullCountryID()
        {
            //Arrange

            Guid? countryID = null;

            //Act
            CountryResponse? country_response_form_get_method =
                _CountryServise.GetCountryByCountryID(countryID);

            //Assert
            Assert.Null(country_response_form_get_method);
        }

        [Fact]
        public void GetCountryByCountryID_ValidCountryID()
        {
            //Arrange
            CountryAddRequest GetCountry_request
                = new CountryAddRequest() { CountryName = "Syria" };
            CountryResponse? country_response_form_Add = _CountryServise.AddCountry(GetCountry_request);

            //Act
            CountryResponse? country_response_from_get = _CountryServise.GetCountryByCountryID(country_response_form_Add.CountryID);

            //Assert
            Assert.Equal(country_response_form_Add, country_response_from_get);
        }
        #endregion
    }
}

