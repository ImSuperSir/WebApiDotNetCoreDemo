using _002_WebApiAutores.Filters;
using _002_WebApiAutores.Middleware;
using _002_WebApiAutores.Services;
using ImSuperSir.BankingServices.Common;
using ImSuperSir.BankingServices.Common.Options;
using ImSuperSir.BankingServices.Common.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;

namespace _002_WebApiAutores
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration Configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();  //OJO, tambien hay inbound/Outbound

            this.Configuration = Configuration;

        }

        public void ConfigureServices(IServiceCollection services)
        {


            //to allow thar anothers sites can acces my api
            services.AddCors(opciones =>
            {
                opciones.AddDefaultPolicy(x =>
                {
                    x.WithOrigins("https://www.apirequest.io").AllowAnyMethod().AllowAnyHeader()
                    //.WithExposedHeaders(); TODO, ver pa que sirve eso  // para que las cabeceras que agregamos en las respuestas puedan ser visibles a clientes como por ejemple angular y/o algun explorador web
                    .WithExposedHeaders(new string[] { "cantidadTotalRegistros" }); 
                });
            });

            services.AddCore();

            services.AddControllers( options => {
                options.Filters.Add(typeof(FiltroDeExcepcion));
            
            }).AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                   
                }
            ).AddNewtonsoftJson();  // newtonsoftjson was added for the patchdocument in patch operations
 
            services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")); 
                });

            services.AddDbContext<BanckingSecurityContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("securityConnection"));
            });



            services.AddAutoMapper(typeof(Startup));


            //services.AddTransient<MiFiltroDeAccion>();
            //services.AddResponseCaching();

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opciones => opciones.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["authSecretKey"])),
                    ClockSkew = TimeSpan.Zero
                });


            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<BanckingSecurityContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IBankingSecurityService, BankingSecurityService>();

            //services.AddApplicationInsightsTelemetry(Configuration["ApplicationInsights:ConnectionString"]);

            services.AddApplicationInsightsTelemetry().AddApplicationInsightsTelemetry(options =>
            {
                options.ConnectionString = Configuration["ApplicationInsights:ConnectionString"];
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //services.AddEndpointsApiExplorer();





            //services.AddEndpointsApiExplorer();
            //All the interior part of AddSwager Method, is for swagger can 
            // use the token in security scenarios where jwt token is used

            //services.AddSwaggerGen( x =>
            //{


            //    //x.SwaggerDoc("V1", new Microsoft.OpenApi.Models.OpenApiInfo() {Title = "Aprendiendo", Version = "V1" });
            //    x.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
            //    {
            //        Name = "Authorization",
            //        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            //        Scheme = "Bearer",
            //        BearerFormat = "JWT",
            //        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            //    });


            //    x.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
            //    {
            //        { 
            //            new OpenApiSecurityScheme()
            //            { 
            //                Reference = new OpenApiReference()
            //                {
            //                    Type = ReferenceType.SecurityScheme,
            //                    Id = "Bearer"
            //                }
            //            },
            //            new string[]{ }
            //        }
            //    });




            //});



            //services.ConfigureOptions<ConfigureSwaggerOptions>();


            //para que la autorizacion sea basada en claims
            services.AddAuthorization(options => 
            {
                options.AddPolicy("EsAdmin", xPolitica => xPolitica.RequireClaim("esAdmin"));
            });


            services.AddDataProtection();

            services.AddTransient<HashService>();
            

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
             IApiVersionDescriptionProvider provider
            ) //, ILogger<Startup> logger )
        {

            //app.Use(async (contexto, siguiente) =>
            //{


            //});


            //app.UseMiddleware<LoguearRespuestaHttpMiddleware>();
            //app.UseLoguearRespuestaHttp();




            //app.Map("/ruta1", app => {      //Map is like a bifurcation, for to create two pipelines
            //                                //the cutting aplies just for this route, slash ignore the controller's route
            //    app.Run(async context =>  //app run interrupts the pipeline, cuts it
            //    {
            //        await context.Response.WriteAsync("the pipeline is being truncated...");
            //    });
            //});

            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); //TODO, ver como es que funciona esto.
                //app.UseSwagger();
                //app.UseSwaggerUI( options =>
                //{
                //    options.SwaggerEndpoint($"/swagger/v1/swagger.json", "WebApi version 1");
                //    options.SwaggerEndpoint($"/swagger/v2/swagger.json", "WebApi version 2");

                //    //foreach (var description in provider.ApiVersionDescriptions)
                //    //{
                //    //    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                //    //        description.GroupName.ToUpperInvariant());
                //    //}
                //}
                    
                    
                    
                    
                //    );
            }
            /*las siguientes dos lineas}
             las sacamos del if, para que funcionen parcialmente*/

            app.UseHttpsRedirection();
            
            app.UseRouting(); // Se agrego


            // Core
            app.UseSwaggerWithVersioning();



            app.UseCors();

            app.UseAuthorization();

            

            //app.UseResponseCaching();

            //pAppBuilder.MapControllers();  // se modifico
            app.UseEndpoints( endpoints =>
            { 
                endpoints.MapControllers();
            });


        }


    }
}
