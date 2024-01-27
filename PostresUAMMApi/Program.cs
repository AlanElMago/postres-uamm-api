using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using PostresUAMMApi.Repositories;
using PostresUAMMApi.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Set up configuration sources.
string firebaseProjectName = "postres-uamm-firebase";
string pathToFirebaseSecretKey = Path.Combine(
    "Secrets",
    "postres-uamm-firebase-firebase-adminsdk-ocw9l-fdf9dfbb56.json");

Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToFirebaseSecretKey);
builder.Configuration.AddUserSecrets("382d69e6-8486-43ba-a6b4-edb9cde00db0");

// Initialize Firebase Admin SDK.
FirebaseApp.Create(new AppOptions { Credential = GoogleCredential.FromFile(pathToFirebaseSecretKey) });

// Add services to the container.
builder.Services.AddSingleton(new FirestoreDbBuilder { ProjectId = firebaseProjectName }.Build());

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ISellerRepository, SellerRepository>();
builder.Services.AddScoped<IPastryRequestRepository, PastryRequestRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPastryRequestService, PastryRequestService>();

// Add controllers to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
