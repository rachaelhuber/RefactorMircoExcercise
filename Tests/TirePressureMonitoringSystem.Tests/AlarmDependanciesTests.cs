namespace TDDMicroExercises.TirePressureMonitoringSystem.SomeDependencies
{
    using Shouldly;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class AlarmDependanciesTests
    {
        [Fact]
        public async Task can_instantiate_all_dependancies()
        {
            var types = new List<Type>
            {
                typeof(AnAlarmClient1),
                typeof(AnAlarmClient2),
                typeof(AnAlarmClient3),
                typeof(ASensorClient),
            };

            var assertions = types.Select(x => (Action)(() =>
            {
                try
                {
                    Activator.CreateInstance(x);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        $"Failed to instantiate class {x.Name} {ex.Message}");
                }
            }));

            types.ShouldSatisfyAllConditions(assertions.ToArray());
        }    
    }
}
