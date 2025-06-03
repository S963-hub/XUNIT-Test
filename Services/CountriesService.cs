using Entities;
using ServiceContracts;
using ServiceContracts.Dto;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly PersonDbContext _db;
        public CountriesService(PersonDbContext personDbContext)
        {
            _db = personDbContext;

        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest));
            }

            if(_db.Countries.Count(temp=>temp.CountryName == countryAddRequest.CountryName) > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            Country country = countryAddRequest.ToCountry();

            country.CountryID = Guid.NewGuid();

            _db.Countries.Add(country);
            _db.SaveChanges();

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountry()
        {
            return _db.Countries.Select(country => country.ToCountryResponse())
                .ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null)
                return null;

            Country? country_resonse_from_list =
                _db.Countries.FirstOrDefault(temp => temp.CountryID == countryID);

            if (country_resonse_from_list == null)
                return null;

            return country_resonse_from_list.ToCountryResponse();
        }
    }
}