namespace backend.DTOs;

/// <summary>
/// Wrapper for controller responses, standardizes information about the request. 
/// </summary>
/// <typeparam name="T">The type of data the controller would've returned had it not been wrapped, needs to be serializable</typeparam>
public class DataResponse<T>
{ 
    public bool Success { get; set; } = true; 
    public T? Data { get; set; }
    public string? Error { get; set; }
}