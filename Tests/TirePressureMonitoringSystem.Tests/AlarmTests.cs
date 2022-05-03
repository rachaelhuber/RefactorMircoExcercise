namespace TDDMicroExercises.TirePressureMonitoringSystem
{
    using Shouldly;
    using System.Threading.Tasks;
    using Xunit;

    public class AlarmTests
    {
        [Fact]
        public async Task alarm_on()
        {
            var testSensor = new TestSensor();
            var alarm = new Alarm(testSensor);
            
            testSensor.SetSensorValue(16.999999);
            alarm.Check();
            
            alarm.AlarmOn.ShouldBeTrue();
        }

        [Fact]
        public async Task alarm_off()
        {
            var testSensor = new TestSensor();
            var alarm = new Alarm(testSensor);

            testSensor.SetSensorValue(20);
            alarm.Check();

            alarm.AlarmOn.ShouldBeFalse();
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
