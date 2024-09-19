using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests
{
	public class CountriesServiceTest
	{
		private readonly ICountriesService _countriesService;

		public CountriesServiceTest()
		{
			_countriesService = new CountriesService();
		}

		#region AddCountry
		// When CountryAddRequest is null, it should throw ArgumentNullException
		[Fact]
		public void AddCountry_NullCountry()
		{
			// Arrange
			CountryAddRequest? request = null;

			// Assert
			Assert.Throws<ArgumentNullException>(() =>
			{
				// Act
				_countriesService.AddCountry(request);
			});
		}

		// When the CountryName is null, it should throw ArgumentException
		[Fact]
		public void AddCountry_CountryNameIsNull()
		{
			CountryAddRequest request = new() { CountryName = null };
			Assert.Throws<ArgumentException>(() =>
			{
				_countriesService.AddCountry(request);
			});
		}

		// When the CountryName is duplicate, it should throw ArgumentException
		[Fact]
		public void AddCountry_CountryNameIsDuplicate()
		{
			CountryAddRequest request1 = new() { CountryName = "USA" };
			CountryAddRequest request2 = new() { CountryName = "USA" };

			Assert.Throws<ArgumentException>(() =>
			{
				_countriesService.AddCountry(request1);
				_countriesService.AddCountry(request2);
			});
		}

		// When you supply proper country name, it should add the country to the existing list of countries
		[Fact]
		public void AddCountry_ProperCountryDetails()
		{
			CountryAddRequest request = new() { CountryName = "BD" };
			CountryResponse response = _countriesService.AddCountry(request);

			List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();

			Assert.True(response.CountryId != Guid.Empty);
			Assert.Contains(response, countries_from_GetAllCountries);
		}
		#endregion

		#region GetAllCountries
		// The list of countries should be empty by default
		[Fact]
		public void GetAllCountries_EmptyList()
		{
			List<CountryResponse> actual_country_response_list = _countriesService.GetAllCountries();
			Assert.Empty(actual_country_response_list);
		}

		[Fact]
		public void GetAllCountries_AddFewCountries()
		{
			List<CountryAddRequest> country_request_list = new()
			{
				new() {CountryName = "USA"},
				new() {CountryName = "UK"}
			};

			List<CountryResponse> countries_list_from_add_country = new();
			foreach(var country_request in  country_request_list)
			{
				countries_list_from_add_country.Add(_countriesService.AddCountry(country_request));
			}

			List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

			foreach(CountryResponse expected_country in countries_list_from_add_country)
			{
				Assert.Contains(expected_country, actualCountryResponseList);
			}
		}

		#endregion

		#region GetCountryByCountryId
		// If we supply null as countryId, it should return null as CountryResponse
		[Fact]
		public void GetCountryByCountryId_NullCountryId()
		{
			Guid? countryId = null;

			CountryResponse? country_response_from_get_method = _countriesService.GetCountryByCountryId(countryId);

			Assert.Null(country_response_from_get_method);
		}

		// If we supply valid countryId, it should return the matching country details as CountryResponse object
		public void GetCountryByCountryId_ValidCountryId()
		{
			CountryAddRequest? country_add_request = new CountryAddRequest() { CountryName = "India" };
			CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

			CountryResponse? country_response_from_get = _countriesService.GetCountryByCountryId(country_response_from_add.CountryId);

			Assert.Equal(country_response_from_add, country_response_from_get);
		}
		#endregion
	}
}