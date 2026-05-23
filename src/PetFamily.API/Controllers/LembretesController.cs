using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.DTOs.Lembretes;
using PetFamily.Application.Interfaces;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("api/lembretes")]
[Produces("application/json")]
public class LembretesController : ControllerBase
{
    private readonly ILembreteService _service;

    public LembretesController(ILembreteService service)
    {
        _service = service;
    }

    /// <summary>Lista todos os lembretes cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LembreteDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var lembretes = await _service.GetAllAsync();
        return Ok(lembretes);
    }

    /// <summary>Busca um lembrete pelo ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(LembreteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var lembrete = await _service.GetByIdAsync(id);
        return lembrete is null ? NotFound(new { message = $"Lembrete com ID {id} não encontrado." }) : Ok(lembrete);
    }

    /// <summary>Lista todos os lembretes de um pet específico.</summary>
    [HttpGet("pet/{petId:int}")]
    [ProducesResponseType(typeof(IEnumerable<LembreteDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPet(int petId)
    {
        var lembretes = await _service.GetByPetIdAsync(petId);
        return Ok(lembretes);
    }

    /// <summary>Lista lembretes por data específica (formato: yyyy-MM-dd).</summary>
    [HttpGet("data/{data}")]
    [ProducesResponseType(typeof(IEnumerable<LembreteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByData(string data)
    {
        if (!DateTime.TryParse(data, out var parsedDate))
            return BadRequest(new { message = "Formato de data inválido. Use yyyy-MM-dd." });

        var lembretes = await _service.GetByDataAsync(parsedDate);
        return Ok(lembretes);
    }

    /// <summary>Lista lembretes dos próximos N dias.</summary>
    [HttpGet("proximos/{dias:int}")]
    [ProducesResponseType(typeof(IEnumerable<LembreteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProximos(int dias)
    {
        if (dias <= 0)
            return BadRequest(new { message = "O número de dias deve ser maior que zero." });

        var lembretes = await _service.GetProximosAsync(dias);
        return Ok(lembretes);
    }

    /// <summary>Cria um novo lembrete.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(LembreteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateLembreteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza um lembrete existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(LembreteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateLembreteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _service.UpdateAsync(id, dto);
        return updated is null ? NotFound(new { message = $"Lembrete com ID {id} não encontrado." }) : Ok(updated);
    }

    /// <summary>Remove um lembrete pelo ID.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound(new { message = $"Lembrete com ID {id} não encontrado." });
    }
}
