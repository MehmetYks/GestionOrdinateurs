using System.Data.Common;
using GestionOrdinateurs.API;
using GestionOrdinateurs.API.Services;
using GestionOrdinateurs.Shared;

using Microsoft.EntityFrameworkCore;


namespace GestionOrdinateurs.Tests;

public class OrdinateurServiceTest
{

    private ParcInformatiqueContext CreerContexte()
    {
        var options =
            new DbContextOptionsBuilder<ParcInformatiqueContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        return new ParcInformatiqueContext(options);
    }

    [Fact]
    public async Task Ajouter_UnOrdinateur_AugmenteLaListe()
    {
        //Arrange
        var context = CreerContexte();
        var journalService = new JournalService();
        var service = new OrdinateurService(context, journalService);


        var ordinateur = new Ordinateur{
            Id = 800, 
            Nom="PC_HP", 
            Etat = EtatOrdinateur.EnMaintenance,
            Caracteristiques = new Caracteristiques(8,TypeOrdinateur.Portable,512,"HP","I5")
        };

        int nbrAv = (await service.GetAll()).Count;
        //Act
        await service.Post(ordinateur);

        //Assert
        Assert.Equal(nbrAv + 1, (await service.GetAll()).Count);
    }

    [Fact]
    public async Task Supprimer_UnOrdinateurExistant_RetourneTrue()
    {
        //Arrange
        var context = CreerContexte();
        var journalService = new JournalService();
        var service = new OrdinateurService(context, journalService);
        var ordinateur = new Ordinateur{
            Id = 12,
            Nom = "PC_SUP_Test",
            Etat = EtatOrdinateur.EnMaintenance,
            Caracteristiques = new Caracteristiques(32, TypeOrdinateur.Fixe, 512, "Acer", "I7")
        };

        //Act
        await service.Post(ordinateur);

        bool resultat = await service.Del(12);

        //Assert
        Assert.True(resultat);
    }

    [Fact]
    public async Task Supprimer_UnOrdinateurInexistant_RetourneFalse()
    {
        //Arrange
        var context = CreerContexte();
        var journalService = new JournalService();
        var service = new OrdinateurService(context, journalService);

        //Act
        bool resultat = await service.Del(999);

        //Assert
        Assert.False(resultat);
    }

}
