using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;


namespace GestionOrdinateurs.API.Services;

public record HistoriqueReparationRecord(int OrdinateurId, string NomOrdinateur, string Technicien, string TypeReparation, DateTime DateIntervention);

public class ReparationService
{
    private readonly ParcInformatiqueContext _context;
    private readonly JournalService _journalService;

    public ReparationService(ParcInformatiqueContext context, JournalService journalService)
    {
        _context = context;
        _journalService = journalService;
    }

    public async Task SimulerReparationAsync(Ordinateur ordinateur)
    {
        var reparation = new Reparation
        {
            UnOrdinateur = ordinateur,
            OrdinateurId = ordinateur.Id,
            TypeReparation = "Test",
            DateDebut = DateTime.Now,
            Statut = "En cours",
            Technicien = "Technicien 1"
        };

        _context.Reparations.Add(reparation);
        await _context.SaveChangesAsync();

        await Task.Delay(2000);

        reparation.Statut = "Terminée";

        await _context.SaveChangesAsync();

        Console.WriteLine($"{ordinateur.Nom} réparé");

        _journalService.Ecrire($"{DateTime.Now:dd/MM/yyyy} - {ordinateur.Nom} réparé");
    }

    public async Task<List<Reparation>> GetAll()
    {
        return await _context.Reparations.ToListAsync();
    }

    public async Task<List<HistoriqueReparationRecord>> GetHistorique()
    {
        return await _context.Reparations.Select(r => new HistoriqueReparationRecord(r.OrdinateurId, r.UnOrdinateur.Nom, r.Technicien, r.TypeReparation, r.DateDebut)).ToListAsync();
    }
}