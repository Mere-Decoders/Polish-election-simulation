using backend.Data;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Tests.Methods
{
    public static class SainteLaguëData
    {

        public static readonly SimulationData Case1 = new SimulationData(
            new[]
            {
                            new Party("Party A", false, false),
                            new Party("Party B", false, false),
                            new Party("Party C", false, false),
                            new Party("Party D", false, false),

            },
            new Dictionary<string, DistrictDetails>
            {
                            { "1", new DistrictDetails { Seats = 8, TerytCodes = new[] { "001" } } }
            },
            new Dictionary<string, int[]>
            {
                            { "001", new[] { 100_000, 80_000, 30_000, 20_000 } }
            }
        );

    }
}
