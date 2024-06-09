using ServiceContracts.DTO.PersonDto;

namespace ServiceContracts
{
	public interface IPersonUpdaterService
	{
	
		Task<PersonResponse> UpdatePerson(PersonUpdateRequest? updatePerson);

	}
}
