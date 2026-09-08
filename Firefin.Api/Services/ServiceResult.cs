namespace Firefin.Api.Services;

/// <summary>
/// Minimal result type so services can report a validation failure with a
/// message instead of throwing, keeping controllers thin.
/// </summary>
public record ServiceResult<T>(T? Value, string? Error)
{
    public bool Ok => Error is null;

    public static ServiceResult<T> Success(T value) => new(value, null);
    public static ServiceResult<T> Fail(string error) => new(default, error);
}
