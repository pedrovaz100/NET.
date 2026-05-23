using Moq;
using PetFamily.Application.DTOs.Lembretes;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Application.Services;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Exceptions;

namespace PetFamily.Tests.Services;

public class LembreteServiceTests
{
    private readonly Mock<ILembreteRepository> _lembreteRepositoryMock;
    private readonly Mock<IPetRepository> _petRepositoryMock;
    private readonly LembreteService _service;

    public LembreteServiceTests()
    {
        _lembreteRepositoryMock = new Mock<ILembreteRepository>();
        _petRepositoryMock = new Mock<IPetRepository>();
        _service = new LembreteService(_lembreteRepositoryMock.Object, _petRepositoryMock.Object);
    }

    [Fact]
    public async Task GetProximosAsync_DeveRetornarLembretesNoPrazo()
    {
        var pet = new Pet { Id = 1, Nome = "Mimi", Especie = "Gato", Raca = "Persa", DataNascimento = DateTime.Now.AddYears(-1), TutorId = 1 };
        var lembretes = new List<Lembrete>
        {
            new() { Id = 1, Titulo = "Vacina", Descricao = "Vacina anual", DataLembrete = DateTime.Now.AddDays(3), PetId = 1, Pet = pet },
            new() { Id = 2, Titulo = "Banho", Descricao = "Banho mensal", DataLembrete = DateTime.Now.AddDays(7), PetId = 1, Pet = pet }
        };
        _lembreteRepositoryMock.Setup(r => r.GetProximosAsync(30)).ReturnsAsync(lembretes);

        var resultado = await _service.GetProximosAsync(30);

        Assert.Equal(2, resultado.Count());
    }

    [Fact]
    public async Task CreateAsync_ComPetInvalido_DeveLancarBusinessException()
    {
        var dto = new CreateLembreteDto { Titulo = "Vacina", Descricao = "Anual", DataLembrete = DateTime.Now.AddDays(7), PetId = 99 };
        _petRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Pet?)null);

        await Assert.ThrowsAsync<BusinessException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_ComPetValido_DeveCriarLembrete()
    {
        var pet = new Pet { Id = 1, Nome = "Mimi", Especie = "Gato", Raca = "Persa", DataNascimento = DateTime.Now.AddYears(-1), TutorId = 1 };
        var dto = new CreateLembreteDto { Titulo = "Vacina", Descricao = "Vacina anual", DataLembrete = DateTime.Now.AddDays(7), PetId = 1 };
        var lembreteCriado = new Lembrete { Id = 1, Titulo = dto.Titulo, Descricao = dto.Descricao, DataLembrete = dto.DataLembrete, PetId = 1, Pet = pet };

        _petRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(pet);
        _lembreteRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Lembrete>())).Returns(Task.CompletedTask);
        _lembreteRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        _lembreteRepositoryMock.Setup(r => r.GetByIdWithPetAsync(It.IsAny<int>())).ReturnsAsync(lembreteCriado);

        var resultado = await _service.CreateAsync(dto);

        Assert.NotNull(resultado);
        Assert.Equal("Vacina", resultado.Titulo);
        Assert.Equal("Mimi", resultado.NomePet);
    }

    [Fact]
    public async Task DeleteAsync_ComIdInvalido_DeveRetornarFalse()
    {
        _lembreteRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Lembrete?)null);

        var resultado = await _service.DeleteAsync(99);

        Assert.False(resultado);
    }

    [Fact]
    public async Task GetByIdAsync_ComIdValido_DeveRetornarLembrete()
    {
        var pet = new Pet { Id = 1, Nome = "Mimi", Especie = "Gato", Raca = "Persa", DataNascimento = DateTime.Now.AddYears(-1), TutorId = 1 };
        var lembrete = new Lembrete { Id = 1, Titulo = "Vacina", Descricao = "Anual", DataLembrete = DateTime.Now.AddDays(7), PetId = 1, Pet = pet };
        _lembreteRepositoryMock.Setup(r => r.GetByIdWithPetAsync(1)).ReturnsAsync(lembrete);

        var resultado = await _service.GetByIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal("Vacina", resultado.Titulo);
    }
}
