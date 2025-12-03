namespace backend.Data;

public class SimContext
{
    public Func<SimulationData, ElectionResult> CountingMethod;

    public SimulationData Data;

    public SimContext(Func<SimulationData, ElectionResult> countingMethod, SimulationData data)
    {
        CountingMethod = countingMethod;
        Data = data;
    }
}