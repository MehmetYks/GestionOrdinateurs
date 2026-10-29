using GestionOrdinateurs.Shared;
using GestionOrdinateurs.API.Events;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.API.Services;


public class OrdinateurService : IOrdinateurService
{
    private readonly ParcInformatiqueContext _context;
    private readonly JournalService _journalService;


    // Les evenements ---------------------------------------------
    public event EventHandler<OrdinateurAjouteEventArgs> OrdinateurAjoute;
    public event EventHandler<OrdinateurMisAJourEventArgs> OrdinateurMisAJour;
    public event EventHandler<OrdinateurSupprimeEventArgs> OrdinateurSupprime;

    //-----------------------------------------

    public OrdinateurService(ParcInformatiqueContext context, JournalService journalService)
    {
        _context = context;
        _journalService = journalService;
    }
    
    //Methode Post pour ajout d'un ordinateur
    public async Task Post(Ordinateur ordinateur)
    {
        _context.Ordinateurs.Add(ordinateur);
        await _context.SaveChangesAsync();

        _journalService.EcrireOrdinateur($"{DateTime.Now:dd/MM/yyyy} - Ordinateur ajouté : {ordinateur.Nom}");

        OrdinateurAjoute?.Invoke(this, new OrdinateurAjouteEventArgs
        {
            Nom = ordinateur.Nom
        });
    }


    // Methode PUT pour modifier un ordinateur
    public async Task<bool> Put(int id, Ordinateur ordinateur)
    {
        Ordinateur? ordinateurExistant = await GetById(id);

        if (ordinateurExistant == null)
        {
            return false;
        }

        ordinateurExistant.ModifPc(
            ordinateur.Nom,
            ordinateur.Etat,
            ordinateur.Stock,
            ordinateur.Prix,
            ordinateur.Caracteristiques
        );

        await _context.SaveChangesAsync();

        _journalService.EcrireOrdinateur($"{DateTime.Now:dd/MM/yyyy} - Ordinateur modifié : {ordinateur.Nom}");

        OrdinateurMisAJour?.Invoke(this, new OrdinateurMisAJourEventArgs
        {
           Nom = ordinateur.Nom 
        });

        return true;
    }

    //GetAll pour récupérer tous les ordinateurs 
    public async Task<List<Ordinateur>> GetAll()
    {
        return await _context.Ordinateurs.ToListAsync();
    }

    //GetById pour récupérer un ordinateur à partir de son id 
    public async Task<Ordinateur?> GetById(int id)
    {

        return await _context.Ordinateurs.FirstOrDefaultAsync(o => o.Id == id);
    }
    
    //Delete pour supprimer un ordinateur
    public async Task<bool> Del(int id)
    {
        Ordinateur? ordinateur = await GetById(id);

        if (ordinateur == null)
        {
            return false;
        }

        _context.Ordinateurs.Remove(ordinateur);
        await _context.SaveChangesAsync();

        _journalService.EcrireOrdinateur($"{DateTime.Now:dd/MM/yyyy} - Ordinateur supprimé : {ordinateur.Nom}");

        OrdinateurSupprime?.Invoke(this, new OrdinateurSupprimeEventArgs
        {
           Nom = ordinateur.Nom 
        });

        return true;
    }

    // recherche ordi par marque
    public async Task<List<Ordinateur>> GetByMarque(string marque)
    {
        return await _context.Ordinateurs.Where(o => o.Caracteristiques.Marque == marque).ToListAsync();
    }

    //Récupère les oridnateurs qui sont disponible avec du stock
    public async Task<List<Ordinateur>> GetDisponibles()
    {
        return await _context.Ordinateurs.Where(o => o.Etat == EtatOrdinateur.EnService && o.Stock > 0).ToListAsync();
    }

    //Recherche ordi par prix
    public async Task<List<Ordinateur>> GetParPrix()
    {
        return await _context.Ordinateurs.OrderByDescending(o => o.Prix).ToListAsync();
    }

    public String Diagnostiquer(Ordinateur ordinateur)
    {
        return ordinateur switch
        {
            {
                Etat : EtatOrdinateur.HorsService
            }
                => "Remplacement",
            {
                Etat : EtatOrdinateur.Lent,
                Caracteristiques:
                {
                    RamGo: < 8
                }
            }
                => "Ajouter RAM",

                _
                    => "Aucune action"
        };
    }

    public (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations)
        CalculerStatistiques()
    {
        int totalOrdinateur = _context.Ordinateurs.Count();
        int ordiDisponibles = _context.Ordinateurs.Count(o => o.Etat == EtatOrdinateur.EnService && o.Stock > 0);
        decimal valeurStock = _context.Ordinateurs.Sum(o => o.Prix * o.Stock);
        int nbReparations = _context.Reparations.Count();

        return (
            totalOrdinateur,
            ordiDisponibles,
            valeurStock,
            nbReparations
        );



    }
}