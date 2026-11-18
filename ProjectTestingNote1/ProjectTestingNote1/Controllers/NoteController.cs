using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTestingNote.Data;
using ProjectTestingNote.DTO;
using ProjectTestingNote.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectTestingNote.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : Controller
    {

        private readonly DataBaseContext _db;
        private readonly IMapper _maper;


        public NoteController(DataBaseContext db, IMapper maper)
        {
            _db = db;
            _maper = maper;
        }
        public IActionResult Index()
        {
            return View();
        }

        //Get

        //[HttpGet("{id:guid}")]
        //public async Task<ActionResult<NoteDTO>> GetById(Guid Id, CancellationToken ct)
        //{
        //    var item = await _db.Notes.AsNoTracking()
        //        .FirstOrDefaultAsync(x => x.Id == Id, ct);

        //    if (item == null) { return NotFound(); }

        //    return Ok(_maper.Map<NoteReadDTO>(item));
        //    //Return new NoteDTO(note.Id, note.Title, note.Content, note.CreatedAt);
        //}

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<NoteDTO>> GetById(Guid id, CancellationToken ct)
        {
            var note = await _db.Notes.FirstOrDefaultAsync(n => n.Id == id, ct);
            if (note == null)
            {
                return NotFound();
            }
            return new NoteDTO(note.Id, note.Title, note.Content, note.CreatedAt);
        }

        //Create Note

        //public async Task<ActionResult<NoteReadDTO>> NoteCreate([FromBody] NoteCreateDto dto, CancellationToken ct)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return ValidationProblem(ModelState);
        //    }

        //    if (string.IsNullOrEmpty(dto.Title) || string.IsNullOrEmpty(dto.Content))
        //    {
        //        return BadRequest("Title and Content cannot be empty");
        //    }

        //    //Teacher's variant
        //    //var note = new Note
        //    //{
        //    //    Id = Guid.NewGuid(),
        //    //    Title = dto.Title,
        //    //    Content = dto.Content,
        //    //    CreatedAt = DateTime.UtcNow,
        //    //};

        //    var entity = _maper.Map<Note>(dto);

        //    entity.Id = Guid.NewGuid();
        //    _db.Notes.Add(entity);
        //    await _db.SaveChangesAsync(ct);

        //    var result = _maper.Map<NoteReadDTO>(entity);
        //    return CreatedAtAction(nameof(GetById), new { Id = entity.Id }, result);

        //}

        [HttpPost("create")]
        public async Task<IActionResult> CreateNote([FromBody] NoteCreateDto dto, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(dto.Title) || string.IsNullOrEmpty(dto.Content))
            {
                return BadRequest("Title and Content cannot be empty.");
            }

            var note = new Note
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow
            };

            _db.Notes.Add(note);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
        }

        //Create Notes in List
        [HttpGet("all")]
        public async Task<ActionResult<List<NoteDTO>>> GetAll(CancellationToken ct)
        {
            var notes = await _db.Notes
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NoteDTO(n.Id, n.Title, n.Content, n.CreatedAt))
                .ToListAsync(ct);
            return notes;
        }
    }
}
