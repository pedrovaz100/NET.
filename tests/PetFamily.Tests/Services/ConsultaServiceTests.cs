using Moq;
using PetFamily.Application.DTOs.Consultas;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Application.Services;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Exceptions;

namespace PetFamily.Tests.Services;

public class ConsultaServiceTests
{
    private readonly Mock<IConsultaRepository> _consultaRepositoryMock;
    private readonly Mock<IPetRepository> _petRepositoryMock;
    private readonly ConsultaService _service;

    public ConsultaServiceTests()
    {
        _consultaRepositoryMock = new Mock<IConsultaRepository>();
        _petRepositoryMock = new Mock<IPetRepository>();
        _service = new ConsultaService(_consultaRepositoryMock.Object, _petRepositoryMock.Object);
    }

    [Fact]
    public async Task GetFuturasAsync_DeveRetornarApenasConsultasFuturas()
    {
        var pet = new Pet { Id = 1, Nome = "Rex", Especie = "Cachorro", Raca = "Labrador", DataNascimento = DateTime.Now.AddYears(-2), TutorId = 1 };
        var consultas = new List<Consulta>
        {
            new() { Id = 1, DataConsulta = DateTime.Now.AddDays(5), Observacoes = "Vacina", PetId = 1, Pet = pet },
            new() { Id = 2, DataConsulta = DateTime.Now.AddDays(10), Observacoes = "Retorno", PetId = 1, Pet = pet }
        };
        _consultaRepositoryMock.Setup(r => r.GetFuturasAsync()).ReturnsAsync(consultas);

        var resultado = await _service.GetFuturasAsync();

        Assert.Equal(2, resultado.Count());
        Assert.All(resultado, c => Assert.True(c.DataConsulta > DateTime.Now));
    }

    [Fact]
    public async Task CreateAsync_ComPetInvalido_DeveLancarBusinessException()
    {
        var dto = new CreateConsultaDto { DataConsulta = DateTime.Now.AddDays(1), Observacoes = "Vacina", PetId = 99 };
        _petRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Pet?)null);

        await Assert.ThrowsAsync<BusinessException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_ComPetValido_DeveCriarConsulta()
    {
        var pet = new Pet { Id = 1, Nome = "Rex", Especie = "Cachorro", Raca = "Labrador", DataNascimento = DateTime.Now.AddYears(-2), TutorId = 1 };
        var dto = new CreateConsultaDto { DataConsulta = DateTime.Now.AddDays(1), Observacoes = "Vacina anual", PetId = 1 };
        var consultaCriada = new Consulta { Id = 1, DataConsulta = dto.DataConsulta, Observacoes = dto.Observacoes, PetId = 1, Pet = pet };

        _petRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(pet);
        _consultaRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Consulta>())).Returns(Task.CompletedTask);
        _consultaRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        _consultaRepositoryMock.Setup(r => r.GetByIdWithPetAsync(It.IsAny<int>())).ReturnsAsync(consultaCriada);

        var resultado = await _service.CreateAsync(dto);

        Assert.NotNull(resultado);
        Assert.Equal("Vacina anual", resultado.Observacoes);
        Assert.Equal("Rex", resultado.NomePet);
    }

    [Fact]
    public async Task DeleteAsync_ComIdValido_DeveRetornarTrue()
    {
        var consulta = new Consulta { Id = 1, DataConsulta = DateTime.Now, PetId = 1 };
        _consultaRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(consulta);
        _consultaRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var resultado = await _service.DeleteAsync(1);

        Assert.True(resultado);
        _consultaRepositoryMock.Verify(r => r.Delete(consulta), Times.Once);
    }
}
