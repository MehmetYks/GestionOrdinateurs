using GestionOrdinateurs.Shared;
using GestionOrdinateurs.API.Events;

namespace GestionOrdinateurs.API.Services;


public interface IOrdinateurService
{

    event EventHandler<OrdinateurAjouteEventArgs> OrdinateurAjoute;
    event EventHandler<OrdinateurSupprimeEventArgs> OrdinateurSupprime;
    event EventHandler<OrdinateurMisAJourEventArgs> OrdinateurMisAJour;

    Task Post(Ordinateur ordinateur);

    Task<bool> Put(int id, Ordinateur ordinateur);
    Task<List<Ordinateur>> GetAll();
    Task<Ordinateur?> GetById(int id);
    Task<bool> Del(int id);
    Task<List<Ordinateur>> GetByMarque(string marque);
    Task<List<Ordinateur>> GetDisponibles();
    Task<List<Ordinateur>> GetParPrix();
    string Diagnostiquer(Ordinateur ordinateur);
    (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations)CalculerStatistiques();

}



