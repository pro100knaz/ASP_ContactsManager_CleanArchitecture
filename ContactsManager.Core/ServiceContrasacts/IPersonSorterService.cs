using ServiceContracts.DTO.PersonDto;
using ServiceContracts.Enums;

namespace ServiceContracts
{
	public interface IPersonSorterService
	{

		public  Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy,
			SortOrderOptions sortOrderOptions);

	}
}
