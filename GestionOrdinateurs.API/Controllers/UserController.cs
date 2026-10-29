using GestionOrdinateurs.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ParcInformatiqueContext _context;

    public UserController(ParcInformatiqueContext context)
    {
        _context = context;
    }

    //Les GET -----------------------------------------

    //Pour avoir tous les utilisateurs
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    //Pour avoir un utilisateur par son login
    [HttpGet("{login}")]
    public async Task<ActionResult<User>> GetByLogin(string login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
}