using Enities;
using Microsoft.EntityFrameworkCore;
using RepositoriesImplementation;
using RepositoryContracts.interfaces;
using ServiceContracts;
using Services;
using Serilog;
using CRUDExample.Filters.ActionFilters;
using CRUDExample;
using CRUDExample.Middleware;

var builder = WebApplication.CreateBuilder(args);



//Logging Configuration
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();a
//builder.Logging.AddDebug();
//builder.Logging.AddEventLog();


//Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) => 
{

	loggerConfiguration
	.ReadFrom.Configuration(context.Configuration) //read configuration settings from built-in IConfiguration
	.ReadFrom.Services(services); //read out current app's services and make them available to serilog
});

//
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();



if (builder.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Error");
	app.UseExceptionHandlingMiddleware();
}


app.UseSerilogRequestLogging();


app.UseHttpLogging();



if (!builder.Environment.IsEnvironment("Test"))
{
	Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

}

app.UseStaticFiles  ();

//middleware have to have exactly strict order !!! vise vereca it wont't work
app.UseRouting();//identification action mrthod based route

app.UseAuthentication();//при создании запроса и если уже авторизирован то куки более не будут сохраняться в памяти
//если куки уже есть то ничего иначе оно отправит необходимые данные
//может читать данные куки и извлекать из куки необходимые ему данные типа имени , эмайла все те даннные что присуутствуют в куки

app.UseAuthorization();//validate user has permisssion to get resources or not


app.MapControllers(); // Execute filters pipline (action  + filter)


app.Run();


public partial class Program { } // make the auto-generated Program accessible programmatically
								 //to make it available in our code