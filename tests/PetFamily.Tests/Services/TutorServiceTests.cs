using Moq;
using PetFamily.Application.DTOs.Tutores;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Application.Services;
using PetFamily.Domain.Entities;

namespace PetFamily.Tests.Services;

public class TutorServiceTests
{
    private readonly Mock<ITutorRepository> _repositoryMock;
    private readonly TutorService _service;

    public TutorServiceTests()
    {
        _repositoryMock = new Mock<ITutorRepository>();
        _service = new TutorService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarTodosOsTutores()
    {
        var tutores = new List<Tutor>
        {
            new() { Id = 1, Nome = "Maria", Email = "maria@email.com", Telefone = "11999990001" },
            new() { Id = 2, Nome = "João", Email = "joao@email.com", Telefone = "11999990002" }
        };
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tutores);

        var resultado = await _service.GetAllAsync();

        Assert.Equal(2, resultado.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ComIdValido_DeveRetornarTutor()
    {
        var tutor = new Tutor { Id = 1, Nome = "Maria", Email = "maria@email.com", Telefone = "11999990001" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tutor);

        var resultado = await _service.GetByIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal("Maria", resultado.Nome);
        Assert.Equal("maria@email.com", resultado.Email);
    }

    [Fact]
    public async Task GetByIdAsync_ComIdInvalido_DeveRetornarNull()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Tutor?)null);

        var resultado = await _service.GetByIdAsync(99);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task CreateAsync_ComDadosValidos_DeveCriarTutor()
    {
        var dto = new CreateTutorDto { Nome = "Ana", Email = "ana@email.com", Telefone = "11999990003" };
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Tutor>())).Returns(Task.CompletedTask);
        _repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var resultado = await _service.CreateAsync(dto);

        Assert.NotNull(resultado);
        Assert.Equal("Ana", resultado.Nome);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Tutor>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ComIdValido_DeveAtualizarTutor()
    {
        var tutor = new Tutor { Id = 1, Nome = "Maria", Email = "maria@email.com", Telefone = "11999990001" };
        var dto = new UpdateTutorDto { Nome = "Maria Silva", Email = "maria.silva@email.com", Telefone = "11999990009" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tutor);
        _repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var resultado = await _service.UpdateAsync(1, dto);

        Assert.NotNull(resultado);
        Assert.Equal("Maria Silva", resultado.Nome);
        _repositoryMock.Verify(r => r.Update(It.IsAny<Tutor>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ComIdInvalido_DeveRetornarNull()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Tutor?)null);

        var resultado = await _service.UpdateAsync(99, new UpdateTutorDto());

        Assert.Null(resultado);
    }

    [Fact]
    public async Task DeleteAsync_ComIdValido_DeveRetornarTrue()
    {
        var tutor = new Tutor { Id = 1, Nome = "Maria", Email = "maria@email.com", Telefone = "11999990001" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tutor);
        _repositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var resultado = await _service.DeleteAsync(1);

        Assert.True(resultado);
        _repositoryMock.Verify(r => r.Delete(tutor), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ComIdInvalido_DeveRetornarFalse()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Tutor?)null);

        var resultado = await _service.DeleteAsync(99);

        Assert.False(resultado);
    }
}
