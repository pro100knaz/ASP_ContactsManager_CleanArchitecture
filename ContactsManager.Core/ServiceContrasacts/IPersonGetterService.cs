using ServiceContracts.DTO.PersonDto;

namespace ServiceContracts
{
	public interface IPersonGetterService
	{
		Task<PersonResponse?> GetPerson(Guid? id);
		Task<List<PersonResponse>> GetAllPersons();
		Task<List<PersonResponse>> GetFilteredPerson(string searchBy, string? searchString);
		Task<MemoryStream> GetPersonCSV();
		Task<MemoryStream> GetPersonExcel();
	}
}
