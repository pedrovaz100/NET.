using Moq;
using PetFamily.Application.DTOs.Pets;
using PetFamily.Application.Interfaces.Repositories;
using PetFamily.Application.Services;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Exceptions;

namespace PetFamily.Tests.Services;

public class PetServiceTests
{
    private readonly Mock<IPetRepository> _petRepositoryMock;
    private readonly Mock<ITutorRepository> _tutorRepositoryMock;
    private readonly PetService _service;

    public PetServiceTests()
    {
        _petRepositoryMock = new Mock<IPetRepository>();
        _tutorRepositoryMock = new Mock<ITutorRepository>();
        _service = new PetService(_petRepositoryMock.Object, _tutorRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarTodosOsPets()
    {
        var tutor = new Tutor { Id = 1, Nome = "Maria", Email = "m@m.com", Telefone = "11999990001" };
        var pets = new List<Pet>
        {
            new() { Id = 1, Nome = "Rex", Especie = "Cachorro", Raca = "Labrador", DataNascimento = DateTime.Now.AddYears(-3), TutorId = 1, Tutor = tutor },
            new() { Id = 2, Nome = "Mimi", Especie = "Gato", Raca = "Persa", DataNascimento = DateTime.Now.AddYears(-2), TutorId = 1, Tutor = tutor }
        };
        _petRepositoryMock.Setup(r => r.GetAllWithTutorAsync()).ReturnsAsync(pets);

        var resultado = await _service.GetAllAsync();

        Assert.Equal(2, resultado.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ComIdValido_DeveRetornarPet()
    {
        var tutor = new Tutor { Id = 1, Nome = "Maria", Email = "m@m.com", Telefone = "11999990001" };
        var pet = new Pet { Id = 1, Nome = "Rex", Especie = "Cachorro", Raca = "Labrador", DataNascimento = DateTime.Now.AddYears(-3), TutorId = 1, Tutor = tutor };
        _petRepositoryMock.Setup(r => r.GetByIdWithTutorAsync(1)).ReturnsAsync(pet);

        var resultado = await _service.GetByIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal("Rex", resultado.Nome);
        Assert.Equal("Maria", resultado.NomeTutor);
    }

    [Fact]
    public async Task GetByIdAsync_ComIdInvalido_DeveRetornarNull()
    {
        _petRepositoryMock.Setup(r => r.GetByIdWithTutorAsync(99)).ReturnsAsync((Pet?)null);

        var resultado = await _service.GetByIdAsync(99);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task CreateAsync_ComTutorInvalido_DeveLancarBusinessException()
    {
        var dto = new CreatePetDto { Nome = "Rex", Especie = "Cachorro", Raca = "Labrador", DataNascimento = DateTime.Now.AddYears(-1), TutorId = 99 };
        _tutorRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Tutor?)null);

        await Assert.ThrowsAsync<BusinessException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_ComTutorValido_DeveCriarPet()
    {
        var tutor = new Tutor { Id = 1, Nome = "Maria", Email = "m@m.com", Telefone = "11999990001" };
        var dto = new CreatePetDto { Nome = "Rex", Especie = "Cachorro", Raca = "Labrador", DataNascimento = DateTime.Now.AddYears(-1), TutorId = 1 };
        var petCriado = new Pet { Id = 1, Nome = "Rex", Especie = "Cachorro", Raca = "Labrador", DataNascimento = dto.DataNascimento, TutorId = 1, Tutor = tutor };

        _tutorRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tutor);
        _petRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Pet>())).Returns(Task.CompletedTask);
        _petRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
        _petRepositoryMock.Setup(r => r.GetByIdWithTutorAsync(It.IsAny<int>())).ReturnsAsync(petCriado);

        var resultado = await _service.CreateAsync(dto);

        Assert.NotNull(resultado);
        Assert.Equal("Rex", resultado.Nome);
        Assert.Equal("Maria", resultado.NomeTutor);
    }

    [Fact]
    public async Task DeleteAsync_ComIdValido_DeveRetornarTrue()
    {
        var pet = new Pet { Id = 1, Nome = "Rex", Especie = "Cachorro", Raca = "Labrador", DataNascimento = DateTime.Now, TutorId = 1 };
        _petRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(pet);
        _petRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        var resultado = await _service.DeleteAsync(1);

        Assert.True(resultado);
        _petRepositoryMock.Verify(r => r.Delete(pet), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ComIdInvalido_DeveRetornarFalse()
    {
        _petRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Pet?)null);

        var resultado = await _service.DeleteAsync(99);

        Assert.False(resultado);
    }
}
