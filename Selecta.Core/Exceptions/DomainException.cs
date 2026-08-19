namespace Selecta.Core.Exceptions;

/// <summary>
/// Violação de uma regra de negócio (ex.: CPF inválido, CPF duplicado).
/// Os controllers da Api traduzem isto para 400 Bad Request.
/// </summary>
public class DomainException(string message) : Exception(message);
