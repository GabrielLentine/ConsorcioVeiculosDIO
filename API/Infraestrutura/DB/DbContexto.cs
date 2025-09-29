using ConsorcioVeiculos.Dominio.Entidades;
using ConsorcioVeiculos.Dominio.Enums;
using Microsoft.EntityFrameworkCore;

namespace ConsorcioVeiculos.Infraestrutura.DB;
public class DbContexto : DbContext
{
    private readonly IConfiguration _configuration;
    
    public DbContexto(DbContextOptions<DbContexto> options) : base(options)
    {
    }

    public DbContexto()
    {
    }

    public static DbContexto CriarComConfiguracao(IConfiguration configuration)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DbContexto>();
        var conexao = configuration.GetConnectionString("mysql")?.ToString();
        if(!string.IsNullOrEmpty(conexao)) optionsBuilder.UseMySql(conexao , ServerVersion.AutoDetect(conexao));
        
        return new DbContexto(optionsBuilder.Options);
    }

    public DbSet<Administrador> Administradores { get; set; } = default!;
    public DbSet<Veiculo> Veiculos { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrador>().HasData(
            new Administrador
            {
                Id = 1 ,
                Email = "administrador@teste.com",
                Senha = "123456",
                Perfil = Perfil.Leader
            }
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            var conexao = _configuration.GetConnectionString("mysql")?.ToString();

            if(!string.IsNullOrEmpty(conexao)) optionsBuilder.UseMySql(conexao , ServerVersion.AutoDetect(conexao));
        }
    }
}
