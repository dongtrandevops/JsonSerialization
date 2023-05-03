using JsonSerialization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
    o.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    o.JsonSerializerOptions.AllowTrailingCommas = true;
    o.JsonSerializerOptions.WriteIndented = true;
    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(null, false));
    o.JsonSerializerOptions.Converters.Add(new CustomStringConverter());
    o.JsonSerializerOptions.Converters.Add(new CustomSingleGenericConverter(o.JsonSerializerOptions));
    //o.JsonSerializerOptions.Converters.Add(new BasicMyConverter(o.JsonSerializerOptions));
    //o.JsonSerializerOptions.Converters.Add(new MyListConverter(o.JsonSerializerOptions));
});


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
