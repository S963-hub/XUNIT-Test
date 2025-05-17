using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.Dto;
using Services;

namespace Xunit_CRUD.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonsService _personsService;
        public PersonController(IPersonsService personsService)
        {
            _personsService = personsService;
        }
        public IActionResult Index(string SearchBy,string? SearchString)
        {
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
            return View(persons);
        }
    }
}
