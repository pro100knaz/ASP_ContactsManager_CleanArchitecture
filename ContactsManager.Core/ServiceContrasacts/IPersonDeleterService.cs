namespace ServiceContracts
{
	public interface IPersonDeleterService
	{
		
		Task<bool> DeletePerson(Guid? id);
		
	}
}
