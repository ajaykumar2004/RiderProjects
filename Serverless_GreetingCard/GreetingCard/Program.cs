using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using GreetingCardApi.Services.Implementations;
using GreetingCardApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure AWS SDK with appsettings
var awsOptions = builder.Configuration.GetSection("AWS");
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

// Explicitly register DynamoDB client with region
builder.Services.AddSingleton<IAmazonDynamoDB>(serviceProvider =>
{
    var region = Amazon.RegionEndpoint.GetBySystemName(awsOptions["Region"]);  // Explicitly setting region
    return new AmazonDynamoDBClient(region);  // Pass region explicitly to the DynamoDB client
});

// Register DynamoDB Context
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();

// Add application-specific services
builder.Services.AddHttpClient<IHttpClientService, HttpClientService>();
builder.Services.AddSingleton<IGreetingCardService, GreetingCardService>();
builder.Services.AddSingleton<IDynamoDBService, DynamoDBService>();

// Add Swagger services for API documentation
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
app.MapControllers();

app.Run();