﻿using Entities;

namespace ServiceContracts.DTO
{
	// DTO Class that is used as return type for most of CountriesService method
	public class CountryResponse
	{
		public Guid CountryId { get; set; }
		public string? CountryName { get; set; }

		public override bool Equals(object? obj)
		{
			if(obj == null) return false;

			if (obj.GetType() != typeof(CountryResponse)) return false;

			CountryResponse country_to_compare = (CountryResponse)obj;
			return this.CountryId == country_to_compare.CountryId && this.CountryName == country_to_compare.CountryName;
		}
	}

	public static class CountryExtensions
	{
		public static CountryResponse ToCountryResponse(this Country country)
		{
			return new CountryResponse
			{
				CountryId = country.CountryId,
				CountryName = country.CountryName
			};
		}
	}
}
