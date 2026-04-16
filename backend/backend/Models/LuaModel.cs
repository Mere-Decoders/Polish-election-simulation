using backend.Data;

namespace backend.Models;

public record LuaRequest(string Script);

public record LuaResult(
    bool Success,
    ElectionResult? Output,
    string? Error
    );