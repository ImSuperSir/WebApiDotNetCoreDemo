using _002_WebApiAutores;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);


var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);




var app = builder.Build();

//this line is adde becuase of the use of the logger 
//on the startup class, on the method Configure
//where we added lo del middleware inline

//var lServiceLogger =  (ILogger<Startup>)app.Services.GetService(typeof(ILogger<Startup>));
//var lServiceLogger2 = (ILogger<Startup>)app.Services.GetRequiredService(typeof(ILogger<Startup>));
var provider =  app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

startup.Configure(app, app.Environment, provider);//, lServiceLogger);


app.Run();
