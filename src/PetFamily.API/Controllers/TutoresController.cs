using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.DTOs.Tutores;
using PetFamily.Application.Interfaces;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("api/tutores")]
[Produces("application/json")]
public class TutoresController : ControllerBase
{
    private readonly ITutorService _service;

    public TutoresController(ITutorService service)
    {
        _service = service;
    }

    /// <summary>Lista todos os tutores cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TutorDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var tutores = await _service.GetAllAsync();
        return Ok(tutores);
    }

    /// <summary>Busca um tutor pelo ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TutorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var tutor = await _service.GetByIdAsync(id);
        return tutor is null ? NotFound(new { message = $"Tutor com ID {id} não encontrado." }) : Ok(tutor);
    }

    /// <summary>Busca um tutor pelo e-mail.</summary>
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(TutorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var tutor = await _service.GetByEmailAsync(email);
        return tutor is null ? NotFound(new { message = $"Tutor com e-mail '{email}' não encontrado." }) : Ok(tutor);
    }

    /// <summary>Busca tutores pelo nome (parcial).</summary>
    [HttpGet("buscar/{nome}")]
    [ProducesResponseType(typeof(IEnumerable<TutorDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchByNome(string nome)
    {
        var tutores = await _service.SearchByNomeAsync(nome);
        return Ok(tutores);
    }

    /// <summary>Cria um novo tutor.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TutorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTutorDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza um tutor existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TutorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTutorDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _service.UpdateAsync(id, dto);
        return updated is null ? NotFound(new { message = $"Tutor com ID {id} não encontrado." }) : Ok(updated);
    }

    /// <summary>Remove um tutor pelo ID.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound(new { message = $"Tutor com ID {id} não encontrado." });
    }
}
