using Google.Cloud.Firestore;
using DotNetEnv;
using Ass1_BE.Services; // namespace chứa FirestoreProductService
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Load environment variables (.env)
Env.Load();

// ✅ Lấy thông tin từ .env
var projectId = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID");
var credentialsPath = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");

// Kiểm tra tồn tại credentials
if (string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(credentialsPath))
{
    throw new Exception("⚠️ Missing FIREBASE_PROJECT_ID or GOOGLE_APPLICATION_CREDENTIALS in .env file.");
}

// ✅ Set biến môi trường cho Firestore SDK
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);

// ✅ Khởi tạo Firestore
FirestoreDb db = FirestoreDb.Create(projectId);
builder.Services.AddSingleton(db);

// ✅ Đăng ký service cho CRUD products
builder.Services.AddScoped<FirestoreProductService>();

// ✅ Add controllers
builder.Services.AddControllers();

// ✅ Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products API", Version = "v1" });
});

var app = builder.Build();

// ✅ Middleware pipeline
if (app.Environment.IsDevelopment() || true) // luôn bật swagger
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// ✅ Map controller routes
app.MapControllers();

app.Run();
