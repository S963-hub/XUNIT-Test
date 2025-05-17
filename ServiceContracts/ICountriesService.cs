using ServiceContracts.Dto;

namespace ServiceContracts
{
    public interface ICountriesService
    {
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
        List<CountryResponse> GetAllCountry();

        CountryResponse? GetCountryByCountryID(Guid? countryID);
    }
}