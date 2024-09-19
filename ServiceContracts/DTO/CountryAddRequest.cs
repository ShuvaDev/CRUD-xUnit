using Entities;

namespace ServiceContracts.DTO
{
	// DTO class for adding a new country
	public class CountryAddRequest
	{
		public string? CountryName { get; set; }

		public Country ToCountry()
		{
			return new Country() { CountryName = CountryName };
		}
	}
}
