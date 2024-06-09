using AutoFixture;
using Castle.Core.Logging;
using CRUDExample.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RepositoriesImplementation;
using RepositoryContracts.interfaces;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.PersonDto;
using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests
{
	public class PersonsControllerTest
	{


		private readonly Mock<ICountriesService> mockCountryService;

		private readonly Mock<ILogger<PersonsController>> mockLogger;

		private readonly IFixture fixture;
		private readonly ILogger<PersonsController> logger;

		private readonly ICountriesService countryService;

		private readonly Mock<IPersonGetterService> mockPersonGetterService;
		private readonly Mock<IPersonAdderService> mockPersonAdderService;
		private readonly Mock<IPersonUpdaterService> mockPersonUpdaterService;
		private readonly Mock<IPersonDeleterService> mockPersonDeleterService;
		private readonly Mock<IPersonSorterService> mockPersonSorterService;

		private readonly IPersonGetterService PersonGetterService;
		private readonly IPersonAdderService PersonAdderService;
		private readonly IPersonUpdaterService PersonUpdaterService;
		private readonly IPersonDeleterService PersonDeleterService;
		private readonly IPersonSorterService PersonSorterService;


		//repositories
		private readonly IPersonsRepository personsRepository;
		private readonly Mock<IPersonsRepository> mockPersonsRepository;

		public PersonsControllerTest()
		{
			//logger
			mockLogger = new Mock<ILogger<PersonsController>>();

			//Country Services
			mockCountryService = new Mock<ICountriesService>();
			countryService = mockCountryService.Object;

			//MockData For Services
			mockPersonGetterService = new Mock<IPersonGetterService>();
			mockPersonAdderService = new Mock<IPersonAdderService>();
			mockPersonUpdaterService = new Mock<IPersonUpdaterService>();
			mockPersonDeleterService = new Mock<IPersonDeleterService>();
			mockPersonSorterService = new Mock<IPersonSorterService>();


			//personServices
			PersonGetterService = mockPersonGetterService.Object;
			PersonAdderService = mockPersonAdderService.Object;
			PersonUpdaterService = mockPersonUpdaterService.Object;
			PersonDeleterService = mockPersonDeleterService.Object;
			PersonSorterService = mockPersonSorterService.Object;


			fixture = new Fixture();
		}

		#region Index
		[Fact]
		public async Task Index_ShouldReturnIndexViewWithPersonsList()
		{
			//Arrange 
			List<PersonResponse> personResponses = fixture.Create<List<PersonResponse>>();

			PersonsController personsController = new PersonsController(logger, 
				countryService, PersonAdderService , PersonDeleterService, PersonGetterService, PersonSorterService, PersonUpdaterService);

			mockPersonGetterService
				.Setup(temp => temp.GetFilteredPerson
				(It.IsAny<string>(),
				It.IsAny<string>()))
				.ReturnsAsync(personResponses);

			mockPersonSorterService
				.Setup(temp => temp.GetSortedPersons
				(It.IsAny<List<PersonResponse>>(),
				It.IsAny<string>(),
				It.IsAny<SortOrderOptions>())).ReturnsAsync(personResponses);

			//Act
			IActionResult result = await personsController.Index(fixture.Create<string>(),
				fixture.Create<string>(), fixture.Create<string>(),
				fixture.Create<SortOrderOptions>());

			//Assert
			ViewResult viewResult = Assert.IsType<ViewResult>(result);

			viewResult.ViewData.Model.Should().NotBeNull();
			viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
			viewResult.ViewData.Model.Should().Be(personResponses);
		}

		#endregion


		#region Create

		[Fact]
		public async Task Create_IfNoModelErrors_ToReturnRedirectIndex()
		{
			//Arrange 
			List<PersonResponse> personResponses = fixture.Create<List<PersonResponse>>();

			PersonResponse personResponse = fixture.Create<PersonResponse>();


			PersonAddRequest person_Add_Requests = fixture.Create<PersonAddRequest>();

			List<CountryResponse> countryResponses = fixture.Create<List<CountryResponse>>();

			PersonsController personsController = new PersonsController(logger,
				countryService, PersonAdderService, PersonDeleterService, PersonGetterService, PersonSorterService, PersonUpdaterService);


			mockCountryService.Setup(
				temp => temp.GetAllCountrise())
				.ReturnsAsync(countryResponses);


			mockPersonAdderService.Setup(
				temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
				.ReturnsAsync(personResponse);

			IActionResult result = await personsController.Create(person_Add_Requests);

			//Assert
			RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);

			redirectResult.ActionName.Should().Be("Index");


		}
		#endregion

	}
}
