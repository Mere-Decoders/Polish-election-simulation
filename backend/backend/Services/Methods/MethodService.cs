using backend.Data;

namespace backend.Services.Methods;
/// <summary>
/// Temporarily this is a simple switch
/// Guid "00000000-0000-0000-0000-000000000000" - Dhondt method
/// Current Implementation is certainly not final
/// </summary>
public class MethodService : IMethodService
{
    public Func<SimulationData, ElectionResult> GetMethodByGuid(Guid methodGuid)
    {
        switch (methodGuid.ToString())
        {
            case ("00000000-0000-0000-0000-000000000000"):
                return (data => DhondtMethod.RunDhondt(data) );
            case ("00000000-0000-0000-0000-000000000001"):
                return (data => HighStakesMethod.RunHighStakes(data) );
            case ("00000000-0000-0000-0000-000000000002"):
                return (data => SainteLaguëMethod.RunSainteLaguë(data));
            default:
                throw new ArgumentException("The method you are requesting doesn't exist");
        }
    }
}