using ServiceContracts;
using ServiceContracts.DTO.PersonDto;
using Services.Helpers;
using RepositoryContracts.interfaces;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Services
{
	public class PersonAdderService : IPersonAdderService
	{
		private readonly ILogger<PersonAdderService> logger;
		private readonly IPersonsRepository personsRepository;
		private readonly IDiagnosticContext diagnosticContext;
		public PersonAdderService(IDiagnosticContext diagnosticContext , ILogger<PersonAdderService> logger, IPersonsRepository personsRepository)
		{
			this.diagnosticContext = diagnosticContext;
			this.logger = logger;
			this.personsRepository = personsRepository;
		}

		public async Task<PersonResponse> AddPerson(PersonAddRequest? addPerson)
		{
			if (addPerson is null)
			{
				throw new ArgumentNullException(nameof(addPerson));
			}

			ValidationHelper.ModelValidation(addPerson);

			var person = addPerson.ToPerson();
			person.Id = Guid.NewGuid();

			await personsRepository.AddPerson(person);

			return person.ToPersonResponse() ?? throw new Exception();
		}
	}
}
