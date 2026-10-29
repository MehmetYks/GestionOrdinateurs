using Microsoft.EntityFrameworkCore;
using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

public class ParcInformatiqueContext : DbContext
{
    //table des ordi dans la BDD
    public DbSet<Ordinateur> Ordinateurs {get; set;}

    public DbSet<Reparation> Reparations{get; set;}

    public DbSet<User> Users {get; set;}



    public ParcInformatiqueContext()
    {
        
    }

    // contructeur mis pour adapter avec la méthode CreerContexte() car elle utilise options
    public ParcInformatiqueContext(DbContextOptions<ParcInformatiqueContext> options) : base(options)
    {
        
    }

    //Config de la connexion à la BDD
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=parcinfo.db");
        }
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ordinateur>().OwnsOne(o => o.Caracteristiques);
        modelBuilder.Entity<Ordinateur>().HasMany(o => o.Reparations).WithOne(r => r.UnOrdinateur);
        modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
    }   

}