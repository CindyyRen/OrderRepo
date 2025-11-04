using onlineOrder.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// 1️⃣ 注册 DbContext —— AddDbContext 默认是 Scoped 生命周期：
builder.Services.AddDbContext<ECommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2️⃣ 注册 Controller 服务 —— Controller 默认是 Transient 生命周期：
//只有当 某个 Controller 或服务第一次注入 ECommerceContext 并执行操作（如 dbContext.Products.ToList()） 时，才会打开实际的数据库连接。
builder.Services.AddControllers();

// 3️⃣ 注册 Swagger —— 生命周期通常由框架管理，你不用管 Singleton/Scoped/Transient。
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4️⃣ 配置 HTTP 请求管道
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



//把所有 HTTP 请求自动重定向到 HTTPS
app.UseHttpsRedirection();

//如果你没加认证和 [Authorize]，这行不会做任何实际操作，也不会报错
app.UseAuthorization();

//找到合适的 Controller 方法，把请求路由过去
app.MapControllers();

app.Run();

