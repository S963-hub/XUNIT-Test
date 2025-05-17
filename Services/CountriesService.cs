using Entities;
using ServiceContracts;
using ServiceContracts.Dto;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _Countries;
        public CountriesService( bool initialize = true)
        {
            _Countries = new List<Country>();
            if (initialize)
            {
				_Countries.AddRange(new List<Country>()
				{
					new Country() { CountryID =  Guid.Parse("45E1313C-5640-4458-B486-7354EC75578A"), CountryName = "syria"},
					new Country() { CountryID =  Guid.Parse("401EF388-2C06-477A-B302-0A6398294FDF"), CountryName = "palastine"},
					new Country() { CountryID =  Guid.Parse("AC2B18CF-FDAC-45CF-AAE7-0B6EBFB730E2"), CountryName = "Irak"},
					new Country() { CountryID =  Guid.Parse("4C697D86-1659-4900-AE7B-FD679EA2CF4F"), CountryName = "turkiye"},
					new Country() { CountryID =  Guid.Parse("06811347-447A-4C95-AD58-1F1026F5AABD"), CountryName = "jordan"},
				});
			}
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

            Country country = countryAddRequest.ToCountry();

            country.CountryID = Guid.NewGuid();

            _Countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountry()
        {
            return _Countries.Select(country => country.ToCountryResponse())
                .ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null)
                return null;

            Country? country_resonse_from_list =
                _Countries.FirstOrDefault(temp => temp.CountryID == countryID);

            if (country_resonse_from_list == null)
                return null;

            return country_resonse_from_list.ToCountryResponse();
        }
    }
}