using ServiceContracts.DTO;

namespace ServiceContracts
{
	// Represents business logic for manipulating country entity
	public interface ICountriesService
	{
		/// <summary>
		/// Adds a country object to list of countries
		/// </summary>
		/// <param name="countryAddRequest">Country object to add</param>
		/// <returns>Returns the country object after adding it (including newly generated country ID)</returns>
		CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

		/// <summary>
		/// Return all countries from the list
		/// </summary>
		/// <returns>All countries from the list as List of CountryResponse</returns>
		List<CountryResponse> GetAllCountries();

		/// <summary>
		/// Returns a country object based on the given country id
		/// </summary>
		/// <param name="countryId">CountryId to search</param>
		/// <returns>Matching country as CountryResponse object</returns>
		CountryResponse? GetCountryByCountryId(Guid? countryId);
	}
}
