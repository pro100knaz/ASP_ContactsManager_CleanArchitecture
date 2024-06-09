using Enities;
using ServiceContracts;
using ServiceContracts.DTO.PersonDto;
using Services.Helpers;
using RepositoryContracts.interfaces;
using Microsoft.Extensions.Logging;
using Serilog;
using Exceptions;

namespace Services
{
	public class PersonUpdaterService : IPersonUpdaterService
	{
		private readonly ILogger<PersonUpdaterService> logger;
		private readonly IPersonsRepository personsRepository;
		private readonly IDiagnosticContext diagnosticContext;
		public PersonUpdaterService(IDiagnosticContext diagnosticContext , ILogger<PersonUpdaterService> logger, IPersonsRepository personsRepository)
		{
			this.diagnosticContext = diagnosticContext;
			this.logger = logger;
			this.personsRepository = personsRepository;
		}

		public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? updatePerson)
		{
			if (updatePerson is null)
				throw new ArgumentNullException(nameof(updatePerson));

			ValidationHelper.ModelValidation(updatePerson);

			Person? matchingPerson = await personsRepository.GetPersonById(updatePerson.PersonId);

			if (matchingPerson is null)
			{
				throw new InvalidPersonIdException("Given person id does not exist");
			}

			//update all deetails

			matchingPerson.PersonName = updatePerson.PersonName;
			matchingPerson.Gender = updatePerson.Gender.ToString();
			matchingPerson.Address = updatePerson.Address;
			matchingPerson.CountryId = updatePerson.CountryId;
			matchingPerson.DateOfBirth = updatePerson.DateOfBirth;
			matchingPerson.Email = updatePerson.Email;
			matchingPerson.ReceiveNewsLatters = updatePerson.ReceiveNewsLatters;

			await personsRepository.UpdatePersonByPerson(matchingPerson);

			return (matchingPerson).ToPersonResponse() ?? throw new Exception();
		}
	}
}
