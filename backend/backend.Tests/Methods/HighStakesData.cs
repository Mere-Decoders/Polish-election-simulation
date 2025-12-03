using backend.Data;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Tests.Methods
{
    public static class HighStakesData
    {

        public static readonly SimulationData Case1 = new SimulationData(
            new[]
            {
                    new Party("A", false, false),
                    new Party("B", false, true)
            },
            new Dictionary<string, DistrictDetails>
            {
                    { "1", new DistrictDetails { Seats = 100, TerytCodes = new[] { "001" } } }
            },
            new Dictionary<string, int[]>
            {
                    { "001", new[] { 4, 100 } }
            }
        );

        public static readonly SimulationData Case2 = new SimulationData(
            new[]
            {
            new Party("123", false, false),
            new Party("456", false, true)
            },
            new Dictionary<string, DistrictDetails>
            {
            { "1", new DistrictDetails { Seats = 17, TerytCodes = new[] { "0052", "0071" } } },
            { "2", new DistrictDetails { Seats = 14, TerytCodes = new[] { "1021", "0823", "1304" } } }
            },
            new Dictionary<string, int[]>
            {
            { "0052", new[] { 10, 100 } },
            { "0071", new[] { 30, 90 } },
            { "1021", new[] { 120, 50 } },
            { "0823", new[] { 33, 41 } },
            { "1304", new[] { 70, 24 } }
            }
        );

    }
}
