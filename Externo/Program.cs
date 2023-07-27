using Externo.Data;
using Externo.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Reflection;
using Stripe;
using Externo.Data.Daos;
using Externo.Services;
using Quartz;
using Externo.Jobs;
using Microsoft.AspNetCore.Mvc;
using Externo.Data.Dtos;
using Microsoft.AspNetCore.Mvc.Controllers;

using var connection = new NpgsqlConnection(Conexao.ConnectionString);
try
{
    // Chave de Conex�o com API de Pagamentos Stripe
    StripeConfiguration.ApiKey = Conexao.ConnectionStripe;

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddDbContext<AppDbContext>(opts =>
        opts.UseNpgsql(Conexao.ConnectionString));

    builder.Services.
        AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddScoped<CobrancaDao>();
    builder.Services.AddScoped<EmailMensagemDao>();
    builder.Services.AddScoped<PagamentoAPI>();
    builder.Services.AddScoped<EmailAPI>();
    builder.Services.AddScoped<AluguelAPI>();

    // Configurando Quartz para processar Fila de Cobran�as

    builder.Services.AddQuartz(q =>
    {
        q.UseMicrosoftDependencyInjectionScopedJobFactory();
        // Just use the name of your job that you created in the Jobs folder.
        var jobKey = new JobKey("CobrancaJob");
        q.AddJob<CobrancaJob>(opts => opts.WithIdentity(jobKey));

        q.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity("CobrancaJob-trigger")
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(60)
                //.WithIntervalInHours(12)
                .RepeatForever())
            .StartNow()
        );
    });
    builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

    // Add services to the container.

    builder.Services.AddControllers()
        .AddNewtonsoftJson()
        .ConfigureApiBehaviorOptions(options =>
        {
            //options.InvalidModelStateResponseFactory = context =>
            //{
            //    var responseObj = new
            //    {
            //        path = context.HttpContext.Request.Path.ToString(),
            //        method = context.HttpContext.Request.Method,
            //        controller = (context.ActionDescriptor as ControllerActionDescriptor)?.ControllerName,
            //        action = (context.ActionDescriptor as ControllerActionDescriptor)?.ActionName,
            //        errors = context.ModelState.Keys.Select(k =>
            //        {
            //            return new
            //            {
            //                field = k,
            //                Messages = context.ModelState[k]?.Errors.Select(e => e.ErrorMessage)
            //            };
            //        })
            //    };
            //    return new UnprocessableEntityObjectResult(responseObj);
            //};
            options.InvalidModelStateResponseFactory = context =>
            {
                var keys = context.ModelState.Keys;
                var values = context.ModelState.Values;

                foreach (var key in keys)
                    Console.WriteLine(key);
                foreach (var value in values)
                    Console.WriteLine(value);

                var errors = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => new
                {
                    Codigo = e.Key,
                    Mensagem = e.Value.Errors.First().ErrorMessage
                }).ToArray();
                //var Erros = new List<ReadErroDto>();

                //foreach (var (key, value) in context.ModelState)
                //    foreach (var item in value.Errors)
                //        Erros.Add(new ReadErroDto
                //        {
                //            Codigo = key,
                //            Mensagem = item.ErrorMessage
                //        });

                return new UnprocessableEntityObjectResult(errors);
            };
        });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Externo API",
            Version = "v1"
        });
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
        c.EnableAnnotations();
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        // Verifica conexao com o banco antes de iniciar a API
        connection.Open();
    }
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

public partial class Program { }