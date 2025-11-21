using backend.Data;

namespace backend.Services.Methods
{
    public class MethodTest
    {
        SimulationData simulationData = new SimulationData(
            new Party[]
            {
                new Party("123", false, false),
                new Party("456", false, true)
            },
            new Dictionary<string, string[]>
            {
                { "1", new[] { "0052", "0721" } },
                { "District 2", new[] { "1021", "0823", "1304" } }
            },
            new Dictionary<string, (int, int[])>
            {
                { "Area 1", (5, new[] { 10, 100 }) },
                { "Area 2", (7, new[] { 20, 100, 3000 }) }
            }

        );

        public void TestMethod()
        {
            var method = new DhondtMethod();
            ElectionResult result = method.Run(simulationData);
            Console.Write(result.ToJson());
            // Add assertions or checks here as needed
        }

    }
}
