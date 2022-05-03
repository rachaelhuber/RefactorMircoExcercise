namespace TDDMicroExercises.TirePressureMonitoringSystem
{
    using Shouldly;
    using System;
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

        // TODO (Rach): Desired?
        [Fact]
        public async Task should_stay_on_once_on()
        {
            var testSensor = new TestSensor();
            var alarm = new Alarm(testSensor);

            testSensor.SetSensorValue(21.000001);
            alarm.Check();

            alarm.AlarmOn.ShouldBeTrue();

            testSensor.SetSensorValue(17);
            alarm.Check();

            alarm.AlarmOn.ShouldBeTrue();
        }

        // TODO (Rach): Desired?
        [Fact]
        public async Task should_throw_if_sensor_broken()
        {
            var testSensor = new TestSensor();
            var alarm = new Alarm(testSensor);

            testSensor.SetSensorValue(0);
            Should.Throw<Exception>(() => alarm.Check());
        }

        // Note (Rach): Mock or stub could be used instead if more controlled functionality required
        private class TestSensor : ISensor
        {
            private double sensorValue;

            public double PopNextPressurePsiValue()
            {
                if (sensorValue == 0)
                {
                    throw new Exception("Sensor is broken.");
                }

                return this.sensorValue;
            }

            public void SetSensorValue(double sensorValue)
            {
                this.sensorValue = sensorValue;
            }
        }
    }
}
