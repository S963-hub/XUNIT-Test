using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Index(string SearchBy,string? SearchString,
        string SortBy = nameof(PersonResponse.PersonName), SortedOrderEnum sortOrder = SortedOrderEnum.ASC)
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

        [HttpGet]
        public IActionResult Create()
        {
            List<CountryResponse> countries = _countriesService.GetAllCountry();
            ViewBag.countries = countries.Select(temp => 
            new SelectListItem { Text = temp.CountryName, Value =  temp.CountryID.ToString() }).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(PersonAddRequest personAddRequest)
		{
			if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countriesService.GetAllCountry();
                ViewBag.countries = countries;
    
                ViewBag.Errors = ModelState.Values.SelectMany(t => t.Errors).ToList();
                return View();
            }

            _personsService.AddPerson(personAddRequest);
            return RedirectToAction("Index","Person");
        }


        [HttpGet]
        public IActionResult Edit(Guid personID)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonId(personID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries = _countriesService.GetAllCountry();
            ViewBag.Countries = countries.Select(temp =>
            new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

            return View(personUpdateRequest);
        }


        [HttpPost]
        public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonId(personUpdateRequest.PersonID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                PersonResponse response = _personsService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");
            }
            else
            {
                List<CountryResponse> countries = _countriesService.GetAllCountry();
                ViewBag.countries = countries;

                ViewBag.Errors = ModelState.Values.SelectMany(t => t.Errors).ToList();
                return View();
            }

        }
    }
}
