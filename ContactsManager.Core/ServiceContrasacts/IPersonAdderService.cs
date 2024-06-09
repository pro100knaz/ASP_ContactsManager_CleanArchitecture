using ServiceContracts.DTO.PersonDto;

namespace ServiceContracts
{
	public interface IPersonAdderService
	{
		Task <PersonResponse> AddPerson(PersonAddRequest? addPerson);
	}
}
