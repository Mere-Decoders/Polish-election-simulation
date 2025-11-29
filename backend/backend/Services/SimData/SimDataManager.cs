using backend.Data;

namespace backend.Services;

public class SimDataManager:ISimDataManager
{
    public SimulationData GetSimDataByGuid(Guid dataGuid)
    {
        return new SimulationData(
            new Party[]
            {
                new Party("123", false, false), 
                new Party("456", false, true)
            },
            new Dictionary<string, (int, string[])>
            {
                { "1", (17, new[] { "0052" , "0071" }) },
                { "2", (14, new[] { "1021", "0823", "1304" }) }
            },
            new Dictionary<string, int[]>
            {

                { "0052", new[] {  10, 100 } },
                { "0071", new[] {  30, 90 } },
                { "1021", new[] {  120, 50 } },
                { "0823", new[] {  33, 41 } },
                { "1304", new[] {  70, 24 } }
            }
        );
    }
}