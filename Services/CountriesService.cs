﻿using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
	public class CountriesService : ICountriesService
	{
		private readonly List<Country> _countries;
		public CountriesService()
		{
			_countries = new List<Country>();
		}
		public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
		{
			// Validation : countryAddRequest can't be null
			if(countryAddRequest == null)
			{
				throw new ArgumentNullException(nameof(countryAddRequest));
			}

			// Validation : CountryName can't be null
			if(countryAddRequest.CountryName == null)
			{
				throw new ArgumentException(nameof(countryAddRequest.CountryName));
			}

			// Validation : CountryName can't be duplicate
			if(_countries.Where(c => c.CountryName == countryAddRequest.CountryName).Count() > 0)
			{
				throw new ArgumentException(nameof(countryAddRequest.CountryName));
			}

			// Convert object from CountryAddRequest to Country type
			Country country = countryAddRequest.ToCountry();

			// generate countryId
			country.CountryId = Guid.NewGuid();

			// Add country object into _countries
			_countries.Add(country);

			return country.ToCountryResponse();
		}

		public List<CountryResponse> GetAllCountries()
		{
			return _countries.Select(country => country.ToCountryResponse()).ToList();
		}

		public CountryResponse? GetCountryByCountryId(Guid? countryId)
		{
			if (countryId == null) return null;

			Country? country_response_from_list = _countries.FirstOrDefault(country => country.CountryId == countryId);

			if (country_response_from_list == null) return null;

			return country_response_from_list.ToCountryResponse();
		}
	}
}
