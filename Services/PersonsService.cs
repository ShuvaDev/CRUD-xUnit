using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class PersonsService : IPersonsService
	{
		private readonly List<Person> _persons;
		private readonly ICountriesService _countriesService;
		public PersonsService()
		{
			_persons = new List<Person>();
			_countriesService = new CountriesService();
		}
		public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
		{
			// check if personAddRequest is not null
			if (personAddRequest == null)
			{
				throw new ArgumentNullException(nameof(personAddRequest));
			}

			ValidationHelper.ModelValidation(personAddRequest);

			// Convert personAddRequest into Person type
			Person person = personAddRequest.ToPerson();

			// generate PersonId
			person.PersonID = Guid.NewGuid();

			// add person object to persons list
			_persons.Add(person);

			// convert the person object into PersonResponse type
			PersonResponse personResponse = person.ToPersonResponse();
			personResponse.Country = _countriesService.GetCountryByCountryId(personResponse.CountryID)?.CountryName;

			return personResponse;
		}


		public List<PersonResponse> GetAllPersons()
		{
			return _persons.Select(temp => temp.ToPersonResponse()).ToList();
		}

		public PersonResponse? GetPersonByPersonID(Guid? personID)
		{
			if (personID == null)
				return null;

			Person? person = _persons.FirstOrDefault(temp => temp.PersonID == personID);
			if (person == null)
				return null;

			return person.ToPersonResponse();
		}

		public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
		{
			List<PersonResponse> allPersons = GetAllPersons();
			List<PersonResponse> matchingPerson = allPersons;

			if (string.IsNullOrEmpty(searchString) || string.IsNullOrEmpty(searchBy))
				return matchingPerson;

			switch (searchBy)
			{
				case nameof(Person.PersonName):
					matchingPerson = matchingPerson.Where(person => !string.IsNullOrEmpty(person.PersonName) ? person.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
					break;
				case nameof(Person.Email):
					matchingPerson = matchingPerson.Where(person => !string.IsNullOrEmpty(person.Email) ? person.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
					break;
				case nameof(Person.DateOfBirth):
					matchingPerson = matchingPerson.Where(person => (person.DateOfBirth != null) ? person.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
					break;
				case nameof(Person.Gender):
					matchingPerson = matchingPerson.Where(person => !string.IsNullOrEmpty(person.Gender) ? person.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
					break;

			}
			return matchingPerson;
		}

		public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
		{
			if (string.IsNullOrEmpty(sortBy))
				return allPersons;

			List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
			{
				(nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

				(nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

				(nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),

				(nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),

				(nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

				(nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

				_ => allPersons
			};

			return sortedPersons;
		}

		public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
		{
			if (personUpdateRequest == null)
				throw new ArgumentNullException(nameof(Person));

			//validation
			ValidationHelper.ModelValidation(personUpdateRequest);

			//get matching person object to update
			Person? matchingPerson = _persons.FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonID);
			if (matchingPerson == null)
			{
				throw new ArgumentException("Given person id doesn't exist");
			}

			//update all details
			matchingPerson.PersonName = personUpdateRequest.PersonName;
			matchingPerson.Email = personUpdateRequest.Email;
			matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
			matchingPerson.Gender = personUpdateRequest.Gender.ToString();
			matchingPerson.CountryID = personUpdateRequest.CountryID;
			matchingPerson.Address = personUpdateRequest.Address;
			matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

			return matchingPerson.ToPersonResponse();
		}

		public bool DeletePerson(Guid? personID)
		{
			if (personID == null)
			{
				throw new ArgumentNullException(nameof(personID));
			}

			Person? person = _persons.FirstOrDefault(temp => temp.PersonID == personID);
			if (person == null)
				return false;

			_persons.RemoveAll(temp => temp.PersonID == personID);

			return true;
		}


	}
}
