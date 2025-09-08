using ChangSha_Byd_NetCore8.Handler;
using ChangSha_Byd_NetCore8.Protocols;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//添加中间件mediatr
builder.Services.AddMediatR(
    mr =>{
        mr.RegisterServicesFromAssembly(typeof(QHStockerNotificationHandler).Assembly);
    }
);

//添加跨域
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWithCredentials", policy =>
    {
        policy.WithOrigins("http://localhost:8080") // 明确指定前端地址
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // 允许凭据,signalr需要
    });
});

//添加signalr
builder.Services.AddMemoryCache(); // 注册 IMemoryCache,handler需要

builder.Services.AddSignalR()
        .AddJsonProtocol(options =>
        {
            options.PayloadSerializerOptions.PropertyNamingPolicy = null; // 保持原属性名大小写
            options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;//如果没有，则会报ref错误，在plc_gateway中使用了ref
        });

//添加plc服务
builder.Services.AddPlcServices(builder.Configuration.GetSection("PlcConnections"));
builder.Services.AddScanOpts(builder.Configuration.GetSection("ScanOpts"));//添加扫描配置

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowWithCredentials"); // 使用跨域策略


app.MapControllers();       // 映射 API 控制器
//app.MapHub<ProductionHub>("/hub");  // 映射 SignalR Hub

app.Run();