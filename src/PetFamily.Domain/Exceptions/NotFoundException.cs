namespace PetFamily.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, object key)
        : base($"{entityName} com ID '{key}' não encontrado.") { }

    public NotFoundException(string message) : base(message) { }
}
