using CRUDExample.Filters;
using CRUDExample.Filters.ActionFilters;
using CRUDExample.Filters.AuthorizationFilter;
using CRUDExample.Filters.ExceptionFilters;
using CRUDExample.Filters.ResultFilters;
using Enities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO.PersonDto;
using ServiceContracts.Enums;
using System.Text.Json;

namespace CRUDExample.Controllers
{
    [Route("persons")]
	[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[]
		{
			"X-Custom-Key-From-Class","Custom-Value-From-Class", 8   //first value will be supplied autu by ioc but another we can supply personaly
		})] //Just Can And Do and add Filter FOR EVERY METHOD inside that controller

	[TypeFilter(typeof(HandleExceptionFilter))]
	[TypeFilter(typeof(PersonsAlwaysRunResultFilter))]
	public class PersonsController : Controller
	{
		private readonly ILogger<PersonsController> logger;
		
		private readonly ICountriesService countriesService;
		private readonly IPersonAdderService personAdderService;
		private readonly IPersonDeleterService personDeleterService;
		private readonly IPersonGetterService personGetterService;
		private readonly IPersonSorterService personSorterService;
		private readonly IPersonUpdaterService personUpdaterService;

		public PersonsController(ILogger<PersonsController> logger,
			ICountriesService countriesService,
			IPersonAdderService personAdderService,
			IPersonDeleterService personDeleterService,
			IPersonGetterService personGetterService,
			IPersonSorterService personSorterService,
			IPersonUpdaterService personUpdaterService
			)
		{
			this.logger = logger;
			this.countriesService = countriesService;
			this.personAdderService = personAdderService;
			this.personDeleterService = personDeleterService;
			this.personGetterService = personGetterService;
			this.personSorterService = personSorterService;
			this.personUpdaterService = personUpdaterService;
		}


		[Route("[action]")]
		[Route("/")]
		[TypeFilter(typeof(PersonsListActionFilter))] // personal filter
		[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[]
		{
			"X-Custom-Key-From-Action","Custom-Value-From-Action", 3   //first value will be supplied autu by ioc but another we can supply personaly
		} //, Order = 3  //Oorder like 0 , 1, 2 , 3 action 3, 2, 1, 0 )) easy and clear
			)]
		[TypeFilter(typeof(PersonsListResultFilter))]
		[SkipFilter]
		public async Task<IActionResult> Index(string searchBy, string? searchString,
			string sortBy = nameof(PersonResponse.PersonName),
			SortOrderOptions sortOrder = SortOrderOptions.Ascending)
		{
			//Search
			List<PersonResponse> persons = (!string.IsNullOrEmpty(searchString)) 
				? await personGetterService.GetFilteredPerson(searchBy, searchString) : await personGetterService.GetAllPersons();

			//Sort
			List<PersonResponse> sortedPersons = await personSorterService.GetSortedPersons(persons, sortBy, sortOrder);

			return View(sortedPersons);
		}


		[Route("[action]")]
		[HttpGet]
		public async Task<IActionResult> Create()
		{

			var countries = await countriesService.GetAllCountrise();

			ViewBag.Countries = countries.Select(temp => new SelectListItem()
			{
				Text = temp.CountryName,
				Value = temp.CountryId.ToString()
			});


			return View();
		}


		[Route("[action]")]
		[HttpPost]
		[TypeFilter(typeof(PersonCreateAndEditPostCreateActionFilter))]
		public async Task<IActionResult> Create(PersonAddRequest personRequest)
		{
			#region Хлам
//if (!ModelState.IsValid)
			//{
			//	var countries = await countriesService.GetAllCountrise();
			//	ViewBag.Countries = countries.Select(temp => new SelectListItem()
			//	{
			//		Text = temp.CountryName,
			//		Value = temp.CountryId.ToString()
			//	});

			//	ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

			//	return View(personRequest);
			//}

			//Now it is inside filters
			#endregion
			PersonResponse response = await personAdderService.AddPerson(personRequest);
			return RedirectToAction("Index", "Persons");
		}


		[HttpGet]
		[Route("[action]/{personID}")]
		[TypeFilter(typeof(TokenResultFilters))]
		public async Task<IActionResult> Edit(Guid personID)
		{
			var country = (await personGetterService.GetPerson(personID))?.ToPersonUpdateRequest() ?? null;
			if (country == null)
			{
				return RedirectToAction("Index", "Persons");
			}

			var countries = await countriesService.GetAllCountrise();
			ViewBag.Countries = countries.Select(temp => new SelectListItem()
			{
				Text = temp.CountryName,
				Value = temp.CountryId.ToString()
			});

			return View(country);
		}


		[HttpPost]
		[Route("[action]/{personID}")]
		[TypeFilter(typeof(PersonCreateAndEditPostCreateActionFilter))]
		[TypeFilter(typeof(TokenAuthorezationFilter))]
		public async Task<IActionResult> Edit(PersonUpdateRequest personRequest)
		{
			var person = await personGetterService.GetPerson(personRequest.PersonId);
			if (person == null)
			{
				return RedirectToAction("Index", "Persons");
			}
			//validation will be inside filters

			var personResponse = await personUpdaterService.UpdatePerson(personRequest);
			return RedirectToAction("Index", "Persons");

			#region Хлам
//else
			//{

			//	var countries = await countriesService.GetAllCountrise();
			//	ViewBag.Countries = countries.Select(temp => new SelectListItem()
			//	{
			//		Text = temp.CountryName,
			//		Value = temp.CountryId.ToString()
			//	});

			//	ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

			//	return View(personRequest);
			//}

			//var person = personService.GetPerson(personID)?.ToPersonUpdateRequest() ?? null;
			//return RedirectToAction("Index", "Persons");
			//return View(personRequest);
			#endregion
		}


		[HttpGet]
		[Route("[action]/{personID}")]
		public async Task<IActionResult> Delete(Guid personID)
		{
			var person = await personGetterService.GetPerson(personID);
			if (person == null)
			{
				return RedirectToAction("Index", "Persons");
			}

			//var countries = countriesService.GetAllCountrise();
			//ViewBag.Countries = countries.Select(temp => new SelectListItem()
			//{
			//	Text = temp.CountryName,
			//	Value = temp.CountryId.ToString()
			//});

			return View(person);
		}


		[HttpPost]
		[Route("[action]/{personID}")]
		public async Task<IActionResult> Delete(PersonResponse personResponse)
		{

			var person1 = await personGetterService.GetPerson(personResponse.Id);
			if (person1 == null)
			{
				return RedirectToAction("Index", "Persons");
			}
			else
			{

				var person = await personDeleterService.DeletePerson(personResponse.Id);



				return RedirectToAction("Index", "Persons");
			}

			//var person1 = personService.GetPerson(personID)?.ToPersonUpdateRequest() ?? null;
			//return RedirectToAction("Index", "Persons");
		}


		[Route("PersonsPdf")]
		public async Task<IActionResult> PersonsPdf()
		{
			var persons = await personGetterService.GetAllPersons();

			return new ViewAsPdf("PersonsPdf", persons, ViewData)
			{
				PageMargins = new Rotativa.AspNetCore.Options.Margins()
				{
					Top = 20,
					Right = 20,
					Bottom = 20,
					Left = 20
				},
				PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,

			};

		}


		[Route("PersonsCSV")]
		public async Task<IActionResult> PersonsCSV()
		{
			var memoryStream = await personGetterService.GetPersonCSV();
			return File(memoryStream, "application/octet-stream", "persons.csv");
		}


		[Route("[action]")]
		public async Task<IActionResult> PersonsExcel()
		{
			var memoryStream = await personGetterService.GetPersonExcel();
			return File(memoryStream, "\tapplication/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
		}
	}
}
