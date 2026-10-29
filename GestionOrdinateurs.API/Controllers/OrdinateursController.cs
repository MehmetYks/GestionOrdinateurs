using GestionOrdinateurs.API.Services;
using GestionOrdinateurs.Shared;
using GestionOrdinateurs.API.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GestionOrdinateurs.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdinateursController : ControllerBase
{

    private readonly IOrdinateurService _service;
    private readonly ILogger<OrdinateursController> _logger;
    private readonly IConfiguration _configuration;

    public OrdinateursController(IOrdinateurService service, ILogger<OrdinateursController> logger, IConfiguration configuration)
    {
        _service = service;
        _logger = logger;
        _configuration = configuration;

        _service.OrdinateurAjoute += AfficherMessageAjout;
        _service.OrdinateurSupprime += AfficherMessageSupprime;
        _service.OrdinateurMisAJour += AfficherMessageMisAJour;
    }

    //Les endpoints--------------------------------------

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Post(Ordinateur ordinateur)
    {
        _logger.LogInformation($"Ordinateur ajouté : {ordinateur.Nom}");

        _service.Post(ordinateur);
        return Created("", ordinateur);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Del(int id)
    {
        _logger.LogInformation($"Ordinateur avec l'id {id} à supprimer");
        bool resultat = await _service.Del(id);
        if (resultat == false)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Put(int id, Ordinateur ordinateur)
    {
        _logger.LogInformation($"Ordinateur à modifier : {id}");
        bool resultat = await _service.Put(id, ordinateur);
        if (resultat == false)
        {
            return NotFound();
        }
        return Ok();
    }

    //Les Get -----------------------------------

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Liste de tous les ordinateurs");

        var ordinateurs = await _service.GetAll();
        var resultat = new List<Object>();

        foreach(Ordinateur ordinateur in ordinateurs)
        {
            resultat.Add(new
            {
                ordinateur.Id,
                ordinateur.Nom,
                ordinateur.Vendu,
                Etat = ordinateur.EtatEnTexte(),
                ordinateur.Stock,
                ordinateur.Prix,
                ordinateur.Caracteristiques         
            });

        }
        return Ok(resultat);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        _logger.LogInformation($"Ordinateur avec l'id : {id} à récupérer");
        var ordinateur = await _service.GetById(id);
        if (ordinateur == null)
        {
            return NotFound();
        }
        return Ok(new
        {
            ordinateur.Id,
            ordinateur.Nom,
            ordinateur.Vendu,
            Etat = ordinateur.EtatEnTexte(),
            ordinateur.Stock,
            ordinateur.Prix,
            ordinateur.Caracteristiques
        });
    }


    [HttpGet("marque/{marque}")]
    public async Task<IActionResult> GetByMarque(string marque)
    {
        _logger.LogInformation($"Ordinateur avec la marque {marque} à récupérer");

        var ordinateurs = await _service.GetByMarque(marque);
        var resultat = new List<Object>();

        foreach(Ordinateur ordinateur in ordinateurs)
        {
            resultat.Add(new
            {
                ordinateur.Id,
                ordinateur.Nom,
                ordinateur.Vendu,
                Etat = ordinateur.EtatEnTexte(),
                ordinateur.Stock,
                ordinateur.Prix,
                ordinateur.Caracteristiques          
            });

        }
        return Ok(resultat);
    }

    [HttpGet("info")]
    public IActionResult GetInformation()
    {
        _logger.LogInformation("Toutes les informations de l'application");

        var nomAppli = _configuration["Application:Name"];
        var versionAppli = _configuration["Application:Version"];

        return Ok(new
        {
            Name = nomAppli,
            Version = versionAppli
        });

    }

    [HttpGet("disponibles")]
    public async Task<IActionResult> GetDisponibles()
    {
        var ordinateurs = await _service.GetDisponibles();
        return Ok(ordinateurs);
    }

    [HttpGet("prix")]
    public async Task<IActionResult> GetParPrix()
    {
        var ordinateurs = await _service.GetParPrix();
        return Ok(ordinateurs);
    }

    [HttpGet("{id}/diagnostic")]
    public async Task<IActionResult> Diagnostic(int id)
    {
        var ordinateur = await _service.GetById(id);

        if(ordinateur == null)
        {
            return NotFound();
        }

        var diagnostic = _service.Diagnostiquer(ordinateur);

        return Ok(diagnostic);
    }

    [HttpGet("statistiques")]
    public IActionResult GetStatistiques()
    {
        var(total, enStock, valeurStock, nbReparations) = _service.CalculerStatistiques();

        return Ok(new
        {
            TotalOrdinateur = total,
            OrdiDisponibles = enStock,
            ValeurStock = valeurStock,
            NbReparations = nbReparations
        });
    }

    [HttpGet("inventaire/{nom}")]
    public async Task<IActionResult> Chercher(string nom)
    {
        var inventaire = new InventaireOrdinateurs();

        inventaire.Ordinateurs = await _service.GetAll();

        var ordinateur = inventaire[nom];

        if(ordinateur == null)
        {
            return NotFound();
        }

        return Ok(ordinateur);
    }
    
    //--------------------------------------

    //Les méthode pour les affichages-------------

    void AfficherMessageAjout(object? sender, OrdinateurAjouteEventArgs ev)
    {
        Console.WriteLine($"Ordinateur {ev.Nom} ajouté");
    }

    void AfficherMessageSupprime(object? sender, OrdinateurSupprimeEventArgs ev)
    {
        Console.WriteLine($"Ordinateur {ev.Nom} supprimé");
    }

    void AfficherMessageMisAJour(object? sender, OrdinateurMisAJourEventArgs ev)
    {
        Console.WriteLine($"Ordinateur {ev.Nom} mise à jour");
    }


}