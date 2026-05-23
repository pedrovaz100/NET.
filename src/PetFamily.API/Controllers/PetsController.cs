using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.DTOs.Pets;
using PetFamily.Application.Interfaces;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("api/pets")]
[Produces("application/json")]
public class PetsController : ControllerBase
{
    private readonly IPetService _service;

    public PetsController(IPetService service)
    {
        _service = service;
    }

    /// <summary>Lista todos os pets cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PetDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var pets = await _service.GetAllAsync();
        return Ok(pets);
    }

    /// <summary>Busca um pet pelo ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var pet = await _service.GetByIdAsync(id);
        return pet is null ? NotFound(new { message = $"Pet com ID {id} não encontrado." }) : Ok(pet);
    }

    /// <summary>Lista todos os pets de um tutor específico.</summary>
    [HttpGet("tutor/{tutorId:int}")]
    [ProducesResponseType(typeof(IEnumerable<PetDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByTutor(int tutorId)
    {
        var pets = await _service.GetByTutorIdAsync(tutorId);
        return Ok(pets);
    }

    /// <summary>Lista todos os pets de uma espécie específica.</summary>
    [HttpGet("especie/{especie}")]
    [ProducesResponseType(typeof(IEnumerable<PetDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEspecie(string especie)
    {
        var pets = await _service.GetByEspecieAsync(especie);
        return Ok(pets);
    }

    /// <summary>Lista todos os pets de uma raça específica.</summary>
    [HttpGet("raca/{raca}")]
    [ProducesResponseType(typeof(IEnumerable<PetDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByRaca(string raca)
    {
        var pets = await _service.GetByRacaAsync(raca);
        return Ok(pets);
    }

    /// <summary>Cria um novo pet.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PetDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePetDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza um pet existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePetDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _service.UpdateAsync(id, dto);
        return updated is null ? NotFound(new { message = $"Pet com ID {id} não encontrado." }) : Ok(updated);
    }

    /// <summary>Remove um pet pelo ID.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound(new { message = $"Pet com ID {id} não encontrado." });
    }
}
