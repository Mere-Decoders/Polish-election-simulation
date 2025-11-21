namespace backend.Data;

public class Party
{
    public string Name { get; set; }
    public bool IsCoalition { get; set; }
    public bool NeedsThreshold { get; set; }

    public Party(string name, bool isCoalition, bool needsThreshold)
    {
        Name = name;
        IsCoalition = isCoalition;
        NeedsThreshold = needsThreshold;
    }
}