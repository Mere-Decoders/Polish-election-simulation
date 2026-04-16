using backend.Data;

namespace backend.Models;

/// <summary>
/// Record representing a Lua script requested to be run.
/// </summary>
/// <param name="Script">The Lua script.</param>
public record LuaRequest(string Script);

/// <summary>
/// Record representing the result of running a Lua script.
/// </summary>
/// <param name="Success">Whether the Lua script was run correctly.</param>
/// <param name="Output">Lua script simulation result.</param>
/// <param name="Error">Error message.</param>
public record LuaResult(
    bool Success,
    ElectionResult? Output,
    string? Error
    );