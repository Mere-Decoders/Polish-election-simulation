using backend.Data;
using System.Text;

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
         new Dictionary<string, (int, string[])>
         {
                { "1", (7, new[] { "0052", "0721" }) },
                { "2", (4, new[] { "1021", "0823", "1304" }) }
         },
         new Dictionary<string, int[]>
         {
                { "0052",  new[] { 10, 100 } },
                { "0721", new[] { 20, 100 } },
                { "1021", new[] { 30, 0 } },
                { "0823", new[] { 40, 0 } },
                { "1304", new[] { 50, 0 } }
         });
        SimulationData simulationData2 = new SimulationData(
        new Party[]
        {
        new Party("A", false, true),
        new Party("B", false, true)
        },
        new Dictionary<string, (int, string[])>
        {
        { "1", (5, new[] { "001", "002" }) }
        },
        new Dictionary<string, int[]>
        {

        { "001", new[] { 50, 500 } },
        { "002", new[] {  0, 500 } }
        });

        SimulationData simulationData3 = new SimulationData(
        new Party[]
        {
        new Party("A", false, false),
        new Party("B", false, true)
        },
        new Dictionary<string, (int, string[])>
        {
        { "1", (5, new[] { "001", "002" }) }
        },
        new Dictionary<string, int[]>
        {

        { "001", new[] { 50, 500 } },
        { "002", new[] {  0, 500 } }
        });

        SimulationData simulationData4 = new SimulationData(
        new Party[]
        {
        new Party("A", false, true),   // MA próg (5%)
        new Party("B", false, true)
        },
        new Dictionary<string, (int, string[])>
        {
        { "1", (100, new[] { "001" }) }
        },
        new Dictionary<string, int[]>
        {
        // głosy na [A, B]
        { "001", new[] { 4 , 100 } }   // NIE przechodzi progu
        }
    );
        SimulationData simulationData5 = new SimulationData(
        new Party[]
        {
        new Party("A", false, false),  //NIE MA progu
        new Party("B", false, true)
        },
        new Dictionary<string, (int, string[])>
        {
        { "1", (100, new[] { "001" }) }
        },
        new Dictionary<string, int[]>
        {

        { "001", new[] {  4, 100 } }
        }
    );

        SimulationData simulationData6 = new SimulationData(
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


        public string TestMethod()
        {
            StringBuilder sb = new StringBuilder();
            var result = DhondtMethod.RunDhondt(simulationData);
            Console.Write(result.ToJson());
            Console.WriteLine();
            var result2 = DhondtMethod.RunDhondt(simulationData2);
            Console.Write(result2.ToJson());
            Console.WriteLine();
            var result3 = DhondtMethod.RunDhondt(simulationData3);
            Console.Write(result3.ToJson());
            Console.WriteLine();
            var result4 = DhondtMethod.RunDhondt(simulationData4);
            Console.Write(result4.ToJson());
            Console.WriteLine();
            var result5 = DhondtMethod.RunDhondt(simulationData5);
            Console.Write(result5.ToJson());
            var result6 = DhondtMethod.RunDhondt(simulationData6);
            Console.Write(result5.ToJson());
            var testResult = sb.Append(result.ToJson()).Append("\n")
              .Append(result2.ToJson()).Append("\n")
              .Append(result3.ToJson()).Append("\n")
              .Append(result4.ToJson()).Append("\n")
              .Append(result5.ToJson()).Append("\n")
              .Append(result6.ToJson()).Append("\n");
            return testResult.ToString();
        }
    }
}