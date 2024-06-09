using Enities;
using System.Linq.Expressions;

namespace RepositoryContracts.interfaces
{
	public interface IPersonsRepository
	{
		Task<Person> AddPerson(Person person);

		Task<List<Person>> GetAllPersons();

		Task<List<Person>> GetFilteredPerson(Expression<Func<Person, bool>> predicate);

		Task<Person?> GetPersonById(Guid id);

		Task<Person> GetPersonByName(string name);

		Task<bool> DeletePersonByPersonId(Guid id);

		Task<Person> UpdatePersonByPerson(Person person);

	}
}
