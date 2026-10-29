using GestionOrdinateurs.API;
using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.Tests;

public class ReparationServiceTest
{
    private ParcInformatiqueContext CreerContexte()
    {
        var options = new DbContextOptionsBuilder<ParcInformatiqueContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        return new ParcInformatiqueContext(options);
    }

    [Fact]
    public async Task Creer_UneReparation_AjouteLaReparation()
    {
        // Arrange
        var context = CreerContexte();
        var ordinateur = new Ordinateur
        {
            Id = 1,
            Nom = "PC_Mehmet_Test",
            Etat = EtatOrdinateur.EnPanne,
            Stock = 1,
            Prix = 500,
            Caracteristiques = new Caracteristiques(
                8,
                TypeOrdinateur.Portable,
                512,
                "Dell",
                "R7"
            )
        };

        context.Ordinateurs.Add(ordinateur);
        await context.SaveChangesAsync();

        var reparation = new Reparation
        {
            OrdinateurId = ordinateur.Id,
            UnOrdinateur = ordinateur,
            TypeReparation = "Changement écran",
            DateDebut = DateTime.Now,
            Statut = "En cours",
            Technicien = "Technicien 1"
        };

        // Act
        context.Reparations.Add(reparation);
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(1, context.Reparations.Count());
    }
}