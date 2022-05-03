namespace TDDMicroExercises.TirePressureMonitoringSystem
{
    using Shouldly;
    using Xunit;

    public class AlarmTests
    {
        // TODO: Test runs but passes only intermitantly as we cannot control the pressure
        [Fact]
        public void alarm_on()
        {
            var alarm = new Alarm();

            alarm.Check();

            alarm.AlarmOn.ShouldBeTrue();
        }
    }
}
