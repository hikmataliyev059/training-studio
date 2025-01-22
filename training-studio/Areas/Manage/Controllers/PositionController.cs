using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training_studio.Areas.Manage.Helpers.DTOs.Positions;
using training_studio.DAL.Context;
using training_studio.Models;

namespace training_studio.Areas.Manage.Controllers;
[Area("Manage")]
[Authorize(Roles = "Admin")]
public class PositionController : Controller
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public PositionController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        ICollection<Position> positions = await _context.Positions
             .Include(x => x.Agents)
             .ToListAsync();
        return View(positions);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePositionDto positionDto)
    {
        if (!ModelState.IsValid)
        {
            return View(positionDto);
        }

        if (await _context.Positions.AnyAsync(x => x.Name == positionDto.Name))
        {
            ModelState.AddModelError("Name", "Name already exists");
            return View(positionDto);
        }

        var position = _mapper.Map<Position>(positionDto);

        await _context.Positions.AddAsync(position);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int? id)
    {
        if (id == null) return NotFound();

        var position = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
        if (position == null) return NotFound();

        var vm = _mapper.Map<UpdatePositionDto>(position);
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdatePositionDto newPositionDto)
    {
        if (!ModelState.IsValid)
        {
            return View(newPositionDto);
        }

        var oldPositionDto = await _context.Positions.FirstOrDefaultAsync(x => x.Id == newPositionDto.Id);
        if (oldPositionDto == null) return NotFound();

        if (await _context.Positions.AnyAsync(x => x.Name == newPositionDto.Name && x.Id != newPositionDto.Id))
        {
            ModelState.AddModelError("Name", "Name already exists");
            return View(newPositionDto);
        }

        _mapper.Map(newPositionDto, oldPositionDto);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var position = await _context.Positions.FirstOrDefaultAsync(x => x.Id == id);
        if (position == null) return NotFound();

        _context.Positions.Remove(position);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
