using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using OpenSearch.Client;
using TimeSeriesPoc;
using TimeSeriesPoc.Domain;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddNpgsqlDataSource(builder.Configuration.GetConnectionString("Timescale")!)
    .AddTransient<IDbConnection>(sp => sp.GetRequiredService<NpgsqlDataSource>().CreateConnection())
    .AddScoped<IOpenSearchClient>(sp => new OpenSearchClient(new Uri(builder.Configuration["OpenSearchNode"]!)))
    .AddHostedService<DatabaseInit>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapPost("/MeterValues",
        async ([FromHeader] string chargerId, [FromHeader] string transactionId, IOpenSearchClient openSearchClient, IDbConnection connection) =>
        {
            QueryContainer query =
                new TermQuery
                {
                    Field = "fields.ChargePointIdentifier.keyword",
                    Value = chargerId
                } &&
                new TermQuery
                {
                    Field = "fields.OcppMessage.MessageType.keyword",
                    Value = "Request"
                } &&
                new TermQuery
                {
                    Field = "fields.OcppMessage.Name.keyword",
                    Value = "MeterValues"
                } &&
                new TermQuery
                {
                    Field = "fields.OcppMessage.Payload.transactionId",
                    Value = transactionId
                };

            var searchRequest = new SearchRequest("cs-ocppj-*")
            {
                Size = 10000,
                Query = query,
                Sort = new List<ISort>
                {
                    new FieldSort
                    {
                        Field = "@timestamp",
                        Order = SortOrder.Ascending,
                        NumericType = NumericType.DateNanos
                    }
                }
            };

            ISearchResponse<ChargingStationLogDto>? response = await openSearchClient.SearchAsync<ChargingStationLogDto>(searchRequest);

            if (!response.IsValid)
            {
                return;
            }

            var sql =
                """
                INSERT INTO public.sample (chargerid, connectorid, transactionid, "timestamp", value, context, format, measurand, phase, location, unit)
                VALUES (@ChargerId, @ConnectorId, @TransactionId, @Timestamp, @Value, @Context, @Format, @Measurand, @Phase, @Location, @Unit);
                """;

            IEnumerable<Sample> values = response.Documents.SelectMany(d => d.Fields.OcppMessage.Payload.MeterValue.SelectMany(mv => mv.SampledValue.Select(s =>
                new Sample
                {
                    ChargerId = chargerId,
                    ConnectorId = d.Fields.OcppMessage.Payload.ConnectorId,
                    TransactionId = d.Fields.OcppMessage.Payload.TransactionId,
                    Timestamp = mv.Timestamp,
                    Value = s.Value,
                    Context = s.Context,
                    Format = s.Format,
                    Measurand = s.Measurand,
                    Phase = s.Phase,
                    Location = s.Location,
                    Unit = s.Unit
                })));

            await connection.ExecuteAsync(sql, values);
        })
    .WithName("AddMeterValues")
    .WithOpenApi();

app.Run();