using Microsoft.EntityFrameworkCore;

namespace backend.Data;

[Owned]
public class Party
{
    public string Name { get; set; }
    public bool IsCoalition { get; set; }
    public bool NeedsThreshold { get; set; }

    private Party() { }
    public Party(string name, bool isCoalition, bool needsThreshold)
    {
        Name = name;
        IsCoalition = isCoalition;
        NeedsThreshold = needsThreshold;
    }
}