using ServiceContracts;
using ServiceContracts.DTO.PersonDto;
using ServiceContracts.Enums;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Services
{
	public class PersonSorterService : IPersonSorterService
	{
		private readonly ILogger<PersonSorterService> logger;
		private readonly IDiagnosticContext diagnosticContext;
		public PersonSorterService(IDiagnosticContext diagnosticContext , ILogger<PersonSorterService> logger)
		{
			this.diagnosticContext = diagnosticContext;
			this.logger = logger;
		}
		public async  Task<List<PersonResponse>> GetSortedPersons
			(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrderOptions)
		{
			if (string.IsNullOrEmpty(sortBy))
			{
				return allPersons;
			}
			List<PersonResponse> result = (sortBy, sortOrderOptions) switch
			{
				(nameof(PersonResponse.PersonName), SortOrderOptions.Ascending)
				=> allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.PersonName), SortOrderOptions.Descending)
				=> allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),


				(nameof(PersonResponse.Email), SortOrderOptions.Ascending)
				=> allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Email), SortOrderOptions.Descending)
				=> allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.DateOfBirth), SortOrderOptions.Ascending)
				=> allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

				(nameof(PersonResponse.DateOfBirth), SortOrderOptions.Descending)
				=> allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),



				(nameof(PersonResponse.Age), SortOrderOptions.Ascending)
				=> allPersons.OrderBy(temp => temp.Age).ToList(),

				(nameof(PersonResponse.Age), SortOrderOptions.Descending)
				=> allPersons.OrderByDescending(temp => temp.Age).ToList(),



				(nameof(PersonResponse.Gender), SortOrderOptions.Ascending)
				=> allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Gender), SortOrderOptions.Descending)
				=> allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),




				(nameof(PersonResponse.Address), SortOrderOptions.Ascending)
				=> allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Address), SortOrderOptions.Descending)
				=> allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),


				(nameof(PersonResponse.Country), SortOrderOptions.Ascending)
			=> allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

				(nameof(PersonResponse.Country), SortOrderOptions.Descending)
				=> allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),



				(nameof(PersonResponse.ReceiveNewsLatters), SortOrderOptions.Ascending)
				=> allPersons.OrderBy(temp => temp.ReceiveNewsLatters).ToList(),

				(nameof(PersonResponse.ReceiveNewsLatters), SortOrderOptions.Descending)
				=> allPersons.OrderByDescending(temp => temp.ReceiveNewsLatters).ToList(),

				_ => allPersons
			};

			return result;

		}

	}
}
