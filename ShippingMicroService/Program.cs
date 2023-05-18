using Google.Cloud.Firestore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var projectId = "striking-audio-387012";
var credentialsPath = "striking-audio-387012-ec89f859315f.json";
var db = FirestoreDb.Create(projectId, new Google.Cloud.Firestore.V1.FirestoreClientBuilder
{
    CredentialsPath = credentialsPath
}.Build());
builder.Services.AddSingleton(db);
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
