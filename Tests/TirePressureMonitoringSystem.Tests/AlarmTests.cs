namespace TDDMicroExercises.TirePressureMonitoringSystem
{
    using Shouldly;
    using System.Threading.Tasks;
    using Xunit;

    public class AlarmTests
    {
        [Theory]
        [InlineData(-1, true)]
        [InlineData(16.999999, true)]
        [InlineData(17, false)] // low_threshold
        [InlineData(20, false)]
        [InlineData(21, false)] // high_threshold
        [InlineData(21.000001, true)]
        public async Task should_return_correct_state(double sensorValue, bool expected)
        {
            var testSensor = new TestSensor();
            var alarm = new Alarm(testSensor);
            
            testSensor.SetSensorValue(sensorValue);
            alarm.Check();
            
            alarm.AlarmOn.ShouldBe(expected);
        }

        private class TestSensor : ISensor
        {
            private double sensorValue;

            public double PopNextPressurePsiValue()
            {
                return this.sensorValue;
            }

            public void SetSensorValue(double sensorValue)
            {
                this.sensorValue = sensorValue;
            }
        }
    }
}
