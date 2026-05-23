using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.DTOs.Consultas;
using PetFamily.Application.Interfaces;

namespace PetFamily.API.Controllers;

[ApiController]
[Route("api/consultas")]
[Produces("application/json")]
public class ConsultasController : ControllerBase
{
    private readonly IConsultaService _service;

    public ConsultasController(IConsultaService service)
    {
        _service = service;
    }

    /// <summary>Lista todas as consultas cadastradas.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ConsultaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var consultas = await _service.GetAllAsync();
        return Ok(consultas);
    }

    /// <summary>Busca uma consulta pelo ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ConsultaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var consulta = await _service.GetByIdAsync(id);
        return consulta is null ? NotFound(new { message = $"Consulta com ID {id} não encontrada." }) : Ok(consulta);
    }

    /// <summary>Lista todas as consultas de um pet específico.</summary>
    [HttpGet("pet/{petId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ConsultaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPet(int petId)
    {
        var consultas = await _service.GetByPetIdAsync(petId);
        return Ok(consultas);
    }

    /// <summary>Lista consultas futuras (a partir de agora).</summary>
    [HttpGet("futuras")]
    [ProducesResponseType(typeof(IEnumerable<ConsultaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFuturas()
    {
        var consultas = await _service.GetFuturasAsync();
        return Ok(consultas);
    }

    /// <summary>Lista consultas em um período específico.</summary>
    [HttpGet("periodo")]
    [ProducesResponseType(typeof(IEnumerable<ConsultaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByPeriodo([FromQuery] DateTime inicio, [FromQuery] DateTime fim)
    {
        if (inicio > fim)
            return BadRequest(new { message = "A data de início deve ser anterior à data de fim." });

        var consultas = await _service.GetByPeriodoAsync(inicio, fim);
        return Ok(consultas);
    }

    /// <summary>Cria uma nova consulta.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ConsultaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateConsultaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza uma consulta existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ConsultaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateConsultaDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _service.UpdateAsync(id, dto);
        return updated is null ? NotFound(new { message = $"Consulta com ID {id} não encontrada." }) : Ok(updated);
    }

    /// <summary>Remove uma consulta pelo ID.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound(new { message = $"Consulta com ID {id} não encontrada." });
    }
}
