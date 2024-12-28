global using System.ComponentModel.DataAnnotations;
global using WebApiBIMU.Data;
global using AutoMapper;
global using Pomelo.EntityFrameworkCore;
global using System.Linq.Expressions;
global using WebApiBIMU.Helpers.Query;
global using WebApiBIMU.Models;
global using Microsoft.EntityFrameworkCore;
global using WebApiBIMU.Services.UnitOfWork;  // Importa os DTOs utilizados na aplicação.
 
using WebApiBIMU.Services.GenericosService;
using System.Text.Json.Serialization;
using HiveMQtt.Client.Options;
using WebApiBIMU.Services.AuthService;

namespace WebApiBIMU
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDbContext<DataContext>();
            //builder.Services.AddSingleton<MqttService>(); // Registre o MqttService
            builder.Services.AddScoped(typeof(IGenericoService<>), typeof(GenericoService<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddControllersWithViews().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors(builder => builder
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowAnyOrigin()
                        );

            app.MapControllers();

            //MqttService.Connect().Wait();
            //Task.Run(() => Connect()); // Garante que a conexão seja feita em uma tarefa separada

            MqttService.Configure(app.Services);
            app.Run(); 
        }
    }
}
