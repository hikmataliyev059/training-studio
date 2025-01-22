using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_studio.Areas.Manage.Helpers.DTOs.Agents;
using training_studio.Areas.Manage.Helpers.Extensions;
using training_studio.DAL.Context;
using training_studio.Models;

namespace training_studio.Areas.Manage.Controllers;
[Area("Manage")]
[Authorize(Roles = "Admin")]
public class AgentController : Controller
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public AgentController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
    {
        _context = context;
        _mapper = mapper;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        ICollection<Agent> agents = await _context.Agents
             .Include(x => x.Position)
             .ToListAsync();
        return View(agents);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Positions = await _context.Positions.ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAgentDto agentDto)
    {
        ViewBag.Positions = await _context.Positions.ToListAsync();

        if (!ModelState.IsValid)
        {
            return View(agentDto);
        }

        if (await _context.Agents.AnyAsync(x => x.Name == agentDto.Name))
        {
            ModelState.AddModelError("Name", "Name already exists");
            return View(agentDto);
        }

        if (!agentDto.File.ContentType.Contains("image"))
        {
            ModelState.AddModelError("File", "File tipini duzgun daxil edin");
            return View(agentDto);
        }

        if (agentDto.File.Length > 3_000_000)
        {

            ModelState.AddModelError("File", "File olcusu maximum 3 mb ola biler");
            return View(agentDto);
        }

        string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload/agent");
        agentDto.ImageUrl = await FileHelper.SaveFileAsync(uploadPath, agentDto.File);

        var agent = _mapper.Map<Agent>(agentDto);

        await _context.Agents.AddAsync(agent);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int? id)
    {
        ViewBag.Positions = await _context.Positions.ToListAsync();

        if (id == null) return NotFound();

        var agent = await _context.Agents.FirstOrDefaultAsync(x => x.Id == id);
        if (agent == null) return NotFound();

        var vm = _mapper.Map<UpdateAgentDto>(agent);
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateAgentDto newAgentDto)
    {
        ViewBag.Positions = await _context.Positions.ToListAsync();

        if (!ModelState.IsValid)
        {
            return View(newAgentDto);
        }

        var oldAgentDto = await _context.Agents.FirstOrDefaultAsync(x => x.Id == newAgentDto.Id);
        if (oldAgentDto == null) return NotFound();

        if (await _context.Agents.AnyAsync(x => x.Name == newAgentDto.Name && x.Id != newAgentDto.Id))
        {
            ModelState.AddModelError("Name", "Name already exists");
            return View(newAgentDto);
        }

        if (newAgentDto.File != null)
        {
            if (!newAgentDto.File.ContentType.Contains("image"))
            {
                ModelState.AddModelError("File", "File tipini duzgun daxil edin");
                return View(newAgentDto);
            }
            if (newAgentDto.File.Length > 3_000_000)
            {
                ModelState.AddModelError("File", "File olcusu maximum 3 mb ola biler");
                return View(newAgentDto);
            }

            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload/agent");

            if (!string.IsNullOrEmpty(oldAgentDto.ImageUrl))
            {
                string existingFilePath = Path.Combine(uploadPath, oldAgentDto.ImageUrl);
                if (System.IO.File.Exists(existingFilePath))
                {
                    System.IO.File.Delete(existingFilePath);
                }
            }

            newAgentDto.ImageUrl = await FileHelper.SaveFileAsync(uploadPath, newAgentDto.File);
        }
        else
        {
            newAgentDto.ImageUrl = oldAgentDto.ImageUrl;
        }

        _mapper.Map(newAgentDto, oldAgentDto);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var agent = await _context.Agents.FirstOrDefaultAsync(x => x.Id == id);
        if (agent == null) return NotFound();

        FileHelper.Delete(_env.WebRootPath, "upload/agent", agent.ImageUrl);

        _context.Agents.Remove(agent);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
