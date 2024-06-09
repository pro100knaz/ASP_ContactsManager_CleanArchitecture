using Enities;

namespace RepositoryContracts.interfaces
{
	public interface ICountriesRepository
	{
		Task<Country> AddCountry(Country country); 

		Task<List<Country>> GetCountries();

		Task<Country?> GetCountryByCountryId(Guid countryId);

		Task<Country?> GetCountryByCountryName(string CountryName);

	}
}
