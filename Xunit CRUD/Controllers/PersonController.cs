using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.Dto;
using ServiceContracts.Enums;
using Services;

namespace Xunit_CRUD.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        public PersonController(IPersonsService personsService,ICountriesService countriesService )
        {
            _personsService = personsService;
            _countriesService = countriesService;
        }
        public IActionResult Index(string SearchBy,string? SearchString, string SortBy = nameof(PersonResponse.PersonName), SortedOrderEnum sortOrder = SortedOrderEnum.ASC)
        {
            //Search
            ViewBag.Fields = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName), "Person Name" },
				{nameof(PersonResponse.Email), "Email" },
				{nameof(PersonResponse.Gender), "Gender" },
				{nameof(PersonResponse.DateOfBirth), "Date of birth" },
				{nameof(PersonResponse.Country), "Country" },
				{nameof(PersonResponse.Adress), "Adress" },
			};  
            List<PersonResponse> persons =  _personsService.GetFilteretPerson(SearchBy, SearchString);
            ViewBag.CurrentSearchBy = SearchBy;
            ViewBag.CurrentSearchString = SearchString;

            //Sort
            List<PersonResponse> SortedPersons = _personsService.GetSortedPersons(persons, SortBy, sortOrder);
            ViewBag.CurrentSortBy = SortBy;
            ViewBag.CurrentSortOrder = sortOrder;
            return View(SortedPersons);
        }

        public IActionResult Create()
        {
            List<CountryResponse> countryResponses = _countriesService.GetAllCountry();
            ViewBag.countryResponses = countryResponses;
            return View();
        }
    }
}
