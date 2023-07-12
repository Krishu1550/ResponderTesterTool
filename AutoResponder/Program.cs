using AutoResponderPackage;

var builder = WebApplication.CreateBuilder(args);
var myPolicy = "_MyCorsPolicy";

// Add services to the container.

builder.Services.AddCors(option =>
{
	option.AddPolicy(myPolicy,
			builder =>
			{
				builder.AllowAnyOrigin();
				builder.AllowAnyMethod();
				builder.AllowAnyHeader();
			}
		);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IApiEndpointService, ApiDocumentationGenerator>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(myPolicy);

app.UseMiddleware<ProxyResponseMiddleWare>();

app.UseAuthorization();

app.MapControllers();

app.Run();
