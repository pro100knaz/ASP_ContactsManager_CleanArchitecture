using Enities;
using ServiceContracts;
using RepositoryContracts.interfaces;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Services
{
	public class PersonDeleterService : IPersonDeleterService
	{
		private readonly ILogger<PersonDeleterService> logger;
		private readonly IPersonsRepository personsRepository;
		private readonly IDiagnosticContext diagnosticContext;
		public PersonDeleterService(IDiagnosticContext diagnosticContext , ILogger<PersonDeleterService> logger, IPersonsRepository personsRepository)
		{
			this.diagnosticContext = diagnosticContext;
			this.logger = logger;
			this.personsRepository = personsRepository;
		}

		public async Task<bool> DeletePerson(Guid? id)
		{
			if (id == null)
			{
				throw new ArgumentNullException(nameof(id));
			}


			Person? resultPerson = await personsRepository.GetPersonById(id.Value);

			if (resultPerson == null)
				return false;

			var result = await personsRepository.DeletePersonByPersonId(id.Value);

			return result;
		}
	}
}
