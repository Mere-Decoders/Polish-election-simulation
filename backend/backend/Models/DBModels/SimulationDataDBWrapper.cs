using backend.Data;

namespace backend.Models.DBModels
{
    public class SimulationDataDBWrapper
    {
        public Guid Id { get; set; }
        //commented until issue #61 is resolved
        //public SimulationData Data { get; set; } = null!;
        public string Foo { get; set; }

    }
}
