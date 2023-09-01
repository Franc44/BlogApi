using System;
using BlogApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlogApi.Data;
using AutoMapper;
using BlogApi.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using BlogApi.Servicios;

namespace BlogApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",  builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            services.AddMvc().AddNewtonsoftJson(options => {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddDbContext<BlogHCContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BlogApiConnection")));
           
            services.Configure<FormOptions>(o => 
            { 
                o.ValueLengthLimit = int.MaxValue; 
                o.MultipartBodyLengthLimit = int.MaxValue; 
                o.MemoryBufferThreshold = int.MaxValue; 
            });

            services.AddControllers().AddNewtonsoftJson(s => {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "BlogApi",
                    Description = "Una simple api para el consumo de un blog y un foro.",
                    Contact = new OpenApiContact
                    {
                        Name = "Francisco Ramirez",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/franciscormz44"),
                    }
                });
            });
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Configuración de autenticacion por Jwt bearer
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWTSettings:JWT_AUDIENCE_TOKEN"],
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JWTSettings:JWT_ISSUER_TOKEN"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTSettings:JWT_Secret"])),
                    ValidateLifetime = true,
                    LifetimeValidator = this.LifetimeValidator
                };
            });
            //-----------------------------------------------------------
            
            //Obtención de configuracion
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            //iNTERFACES Y CODIGO
            services.AddScoped<IEmpresaRepo, EmpresaConsultas>();
            services.AddScoped<IUsuariosRep, ObtencionUsuarios>();
            services.AddScoped<ISesionRep, IniciaSesion>();
            services.AddScoped<IPublicacionesRepo, PublicacionesConsulta>();
            services.AddScoped<IAccionUsuario, AccionUsuarioConsulta>();
            services.AddScoped<IDocumentosRepo, DocumentoConsultas>();
            services.AddScoped<IForoPreguntasRepo, ForoPreguntaConsulta>();
            services.AddScoped<IForoPreguntasRespuestasRepo, ForoRespuestaConsultas>();
            services.AddScoped<ILikesPR, LikesFPRConsultas>();
            
            //Servicio de Email
            services.AddScoped<IEmailService, EmailService>();
        }

        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "BlogApi V1");
                c.RoutePrefix = "help";
            });

            app.UseRouting();

            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine("C:", @"StaticFiles")),
                RequestPath = new PathString("/StaticFiles")
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
