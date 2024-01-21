using Firebase.Auth;
using Firebase.Auth.Providers;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

// Add services to the container.
builder.Services.AddSingleton(new FirebaseAuthConfig
{
    ApiKey = builder.Configuration["FirebaseWebApiKey"],
    AuthDomain = $"{firebaseProjectName}.firebaseapp.com",
    Providers = new FirebaseAuthProvider[] { new EmailProvider() }
});
builder.Services.AddSingleton(new FirestoreDbBuilder { ProjectId = firebaseProjectName }.Build());
builder.Services.AddScoped<FirebaseAuthClient>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<IFirebaseAuthService, FirebaseAuthService>();
builder.Services.AddScoped<IUserService, UserService>();

// Add Firebase authentication using JWT tokens.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = $"https://securetoken.google.com/{firebaseProjectName}";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = $"https://securetoken.google.com/{firebaseProjectName}",
        ValidateAudience = true,
        ValidAudience = firebaseProjectName,
        ValidateLifetime = true
    };
});

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
