namespace MicroServicioOferentes.Entities;

public sealed class ErrorResponse
{
    public ErrorDetail Error { get; set; } = new();
}

public sealed class ErrorDetail
{
    public string Code { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
}
