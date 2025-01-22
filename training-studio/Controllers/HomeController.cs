using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_studio.DAL.Context;
using training_studio.Models;

namespace training_studio.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ICollection<Agent> agents = await _context.Agents
             .Include(x => x.Position)
             .ToListAsync();
        return View(agents);
    }
}
