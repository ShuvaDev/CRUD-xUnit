﻿using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
	/// <summary>
	/// Act as a DTO for inserting a new Person
	/// </summary>
	public class PersonAddRequest
	{
		[Required(ErrorMessage = "Person name can't be blank.")]
		public string? PersonName { get; set; }
		[Required(ErrorMessage = "Email can't be blank.")]
		[EmailAddress(ErrorMessage = "Email value should be a valid email")]
		public string? Email { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public GenderOptions? Gender { get; set; }
		public Guid? CountryID { get; set; }
		public string? Address { get; set; }
		public bool ReceiveNewsLetters { get; set; }

		/// <summary>
		/// Converts the current object of PersonAddRequest into a new object of Person type
		/// </summary>
		/// <returns></returns>
		public Person ToPerson()
		{
			return new Person() { PersonName = PersonName, Email = Email, DateOfBirth = DateOfBirth, Gender = Gender.ToString(), Address = Address, CountryID = CountryID, ReceiveNewsLetters = ReceiveNewsLetters };
		}
	}
}
