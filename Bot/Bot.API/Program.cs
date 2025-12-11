using Bot.Core;
using Bot.Core.Plugins;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

//  Semantic kernel setup
builder.Services.AddSingleton<Kernel>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var apiKey = config["OpenAI:ApiKey"];

    var kernelBuilder = Kernel.CreateBuilder();

    // AI model
    kernelBuilder.AddOpenAIChatCompletion(
        "gpt-4o-mini",
        apiKey: apiKey);

    var kernel = kernelBuilder.Build();

    //Register plugins

    kernel.Plugins.AddFromType<AddCarPlugin>("AddCarPlugin");
    kernel.Plugins.AddFromType<GetCarsPlugin>("GetCarsPlugin");
    kernel.Plugins.AddFromType<UpdateCarPlugin>("UpdateCarPlugin");
    kernel.Plugins.AddFromType<GetServicesPlugin>("GetServicesPlugin");
    kernel.Plugins.AddFromType<CreateAppointmentPlugin>("CreateAppointmentPlugin");
    kernel.Plugins.AddFromType<ModifyAppointmentPlugin>("ModifyAppointmentPlugin");
    kernel.Plugins.AddFromType<CancelAppointmentPlugin>("CancelAppointmentPlugin");
  

    return kernel;
});

// KernelService
builder.Services.AddSingleton<KernelService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors();
app.MapControllers();
app.Run();
