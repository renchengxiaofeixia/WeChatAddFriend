using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["ConnectionStrings:SqliteConnection"];
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("cors",
        builder => builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});

var app = builder.Build();

#region middleware

if (app.Environment.IsDevelopment())
{
    //异常处理中间件
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("cors");

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(AppContext.BaseDirectory, @"files")),
    RequestPath = new PathString("/files")
});

#endregion 

#region routers

app.MapPost("/signin", async (AppDbContext db, UserDto userDto) =>
{
    var user = db.Users.FirstOrDefault(k=> k.UserName == userDto.UserName && k.Password == userDto.Password);
    if (user == null)
    {
        return Results.BadRequest();
    }
    var patch = await db.AppPatchs.OrderBy(k=>k.Id).LastOrDefaultAsync(k=>k.PatchVersion > userDto.AppVersion);
    return Results.Ok(new { user.UserName,Patch = patch });
});

app.MapPost("/signup", async (AppDbContext db, UserDto userDto) =>
{
    var user = new User { UserName = userDto.UserName, Password = userDto.Password , Creator = userDto.UserName};
    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
    return Results.Ok(user);
});

app.MapPost("/addwechat", async (AppDbContext db, List<WeChatPhoneDto> wechatPhoneDtos) =>
{
    var wechatPhones = wechatPhoneDtos.Select(wechatPhoneDto =>
        new WeChatPhone()
        {
            CreateTime = DateTime.Now,
            Creator = wechatPhoneDto.Creator,
            PhoneNo = wechatPhoneDto.PhoneNo,
        });
    await db.WeChatPhones.AddRangeAsync(wechatPhones);
    await db.SaveChangesAsync();
    return Results.Ok(new { IsOk = true });
});

app.MapPost("/upload", async Task<IResult> (AppDbContext db, HttpRequest request,string patchFileName,int patchVersion,string tip, bool isForceUpdate) =>
{
    if (!request.HasFormContentType)
        return Results.BadRequest();

    var form = await request.ReadFormAsync();
    var fi = form.Files["fi"];

    if (fi is null || fi.Length == 0)
        return Results.BadRequest();

    var svrpath = Path.Combine(AppContext.BaseDirectory, "files", patchFileName);
    await using var stream = fi.OpenReadStream();
    using var fs = File.Create(svrpath);
    await stream.CopyToAsync(fs);

    var appPatch = new AppPatch()
    {
        PatchFileName = patchFileName,
        CreateTime = DateTime.Now,
        Creator = string.Empty,
        IsForceUpdate = isForceUpdate,
        PatchSize = (int)fs.Length,
        PatchUrl = string.Empty,
        PatchVersion = patchVersion,
        Tip = tip,
    };
    await db.AppPatchs.AddAsync(appPatch);
    await db.SaveChangesAsync();
    return Results.Ok(appPatch);
});
#endregion


app.MapGet("/", () => "Dotnet Minimal API");
app.Run("http://0.0.0.0:31005");


#region Models 
/*
    dotnet add package Microsoft.EntityFrameworkCore 
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet ef migrations add "initial migration"
    dotnet ef database update
*/

class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<AppPatch> AppPatchs { get; set; }
    public DbSet<WeChatPhone> WeChatPhones { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}

class MjAppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json")
                  .Build();

        var builder = new DbContextOptionsBuilder<AppDbContext>();
        var connectionString = configuration.GetConnectionString("SqliteConnection");
        builder.UseSqlite(connectionString);
        return new AppDbContext(builder.Options);
    }
}



class User
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.Now;
    public string? Creator { get; set; }
}

class UserDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public int AppVersion { get; set; }
}

public class AppPatchDto
{
    public bool IsForceUpdate { get; set; }
    public int PatchVersion { get; set; }
    public string PatchUrl { get; set; }
    public string PatchFileName { get; set; }
    public int PatchSize { get; set; }
    public string Tip { get; set; }
    public string? Creator { get; set; }
}

public class AppPatch
{
    public int Id { get; set; }
    public bool IsForceUpdate { get; set; }
    public int PatchVersion { get; set; }
    public string PatchUrl { get; set; }
    public string PatchFileName { get; set; }
    public int PatchSize { get; set; }
    public string Tip { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.Now;
    public string? Creator { get; set; }
}

public class WeChatPhoneDto
{
    public string PhoneNo { get; set; }
    public string? Creator { get; set; }
}

public class WeChatPhone
{
    public int Id { get; set; }
    public string PhoneNo { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.Now;
    public string? Creator { get; set; }
}

#endregion