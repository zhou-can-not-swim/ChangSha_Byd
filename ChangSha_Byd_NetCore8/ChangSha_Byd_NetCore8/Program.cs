using AsZero.DbContexts;
using ChangSha_Byd_NetCore8.App;
using ChangSha_Byd_NetCore8.AsZero;
using ChangSha_Byd_NetCore8.Handler;
using ChangSha_Byd_NetCore8.Hub;
using ChangSha_Byd_NetCore8.OpenAuth.Infra;
using ChangSha_Byd_NetCore8.Protocols;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//����м��mediatr
builder.Services.AddMediatR(
    mr =>{
        mr.RegisterServicesFromAssembly(typeof(QHStockerNotificationHandler).Assembly);
    }
);

//��ӿ���
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWithCredentials", policy =>
    {
        policy.WithOrigins("http://localhost:8080") // ��ȷָ��ǰ�˵�ַ
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // ����ƾ��,signalr��Ҫ
    });
});

//���signalr
builder.Services.AddMemoryCache(); // ע�� IMemoryCache,handler��Ҫ

builder.Services.AddSignalR()
        .AddJsonProtocol(options =>
        {
            options.PayloadSerializerOptions.PropertyNamingPolicy = null; // ����ԭ��������Сд
            options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;//���û�У���ᱨref������plc_gateway��ʹ����ref
        });

//���plc����
builder.Services.AddPlcServices(builder.Configuration.GetSection("PlcConnections"));
builder.Services.AddScanOpts(builder.Configuration.GetSection("ScanOpts"));//���ɨ������


#region infra

//builder.Services.AddLogging(logging => { logging.AddLog4Net(); });//����log4net����һ��Ҫ��log4net.config�ļ�

builder.Services.AddDbContext<AsZeroDbContext>(opts =>
{
    opts.UseSqlServer(
        builder.Configuration.GetConnectionString("AsZeroDbContext"),
        sqlOpts =>
        {
            // ʹ�õ�ǰ������ΪǨ�Ƴ���
            sqlOpts.MigrationsAssembly(typeof(AsZeroDbContext).Assembly.GetName().Name);
        });
});


builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));
builder.Services.AddAsZeroRepositories();
//builder.Services.AddAsZeroHostedServices();  //����ûɶ��
builder.Services.AddApps();

#endregion



 var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowWithCredentials"); // ʹ�ÿ������


app.MapControllers();       // ӳ�� API ������
app.MapHub<ProductionHub>("/hub");  // ӳ�� SignalR Hub

app.Run();