using Microsoft.EntityFrameworkCore;
using TurismoApp.Application.Implementations;
using TurismoApp.Application.Interfaces;
using TurismoApp.Infraestructure.Context;
using TurismoApp.Infraestructure.Mappings;
using TurismoApp.Infraestructure.Repositories.Implementations;
using TurismoApp.Infraestructure.Repositories.Interfaces;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                                 b => b.MigrationsAssembly("TurismoApp.Infraestructure")));
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
        services.AddScoped<IDepartamentoApplication, DepartamentoApplication>();

        services.AddScoped<ICiudadRepository, CiudadRepository>();
        services.AddScoped<ICiudadApplication, CiudadApplication>();

        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IClienteApplication, ClienteApplication>();

        services.AddTransient<EmailService>();

        services.AddScoped<IRecorridoRepository, RecorridoRepository>();
        services.AddScoped<IRecorridoApplication, RecorridoApplication>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}