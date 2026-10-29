using GestionOrdinateurs.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionOrdinateurs.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReparationController : ControllerBase
{
    private readonly ReparationService _reparationService;
    private readonly IOrdinateurService _ordinateurService;
    private readonly StockService _stockService;

    public ReparationController(ReparationService reparationService, IOrdinateurService ordinateurService, StockService stockService)
    {
        _reparationService = reparationService;
        _ordinateurService = ordinateurService;
        _stockService = stockService;
    }

    [HttpPost("simuler")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SimulerReparation()
    {
        var listOrdinateurs = await _ordinateurService.GetAll();

        await Task.WhenAll( _reparationService.SimulerReparationAsync(listOrdinateurs[0]),
        _reparationService.SimulerReparationAsync(listOrdinateurs[1]),
        _reparationService.SimulerReparationAsync(listOrdinateurs[2]),
        _reparationService.SimulerReparationAsync(listOrdinateurs[3]),
        _reparationService.SimulerReparationAsync(listOrdinateurs[4]));

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var reparations = await _reparationService.GetAll();

        var resultat = new List<object>();

        foreach(Reparation reparation in reparations)
        {
            var ordinateur = await _ordinateurService.GetById(reparation.OrdinateurId);

            resultat.Add(new
            {
                reparation.Id,
                reparation.TypeReparation,
                reparation.DateDebut,
                reparation.Statut,
                Ordinateur = ordinateur 
            });
        }

        return Ok(resultat);
    }

    [HttpGet("stock-ssd")]
    public IActionResult GetStockSSD()
    {
        Parallel.For(0, 10, i => _stockService.ConsommerSSD());

        return Ok(_stockService.StockSSD);
    }

    [HttpGet("historique")]
    public async Task<IActionResult> GetHistorique()
    {
        var historique = await _reparationService.GetHistorique();

        return Ok(historique);
    }

}