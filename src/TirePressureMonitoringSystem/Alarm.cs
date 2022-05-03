namespace TDDMicroExercises.TirePressureMonitoringSystem
{
    public class Alarm
    {
        // TODO (Rach): Could be moved to config if different settings were required for different vehicles 
        private const double LowPressureThreshold = 17;
        private const double HighPressureThreshold = 21;

        private readonly ISensor sensor;
        private bool alarmOn = false;

        public Alarm()
        {
            this.sensor = new Sensor();
        }

        public Alarm(ISensor sensor)
        {
            this.sensor = sensor;
        }

        public void Check()
        {
            double psiPressureValue = this.sensor.PopNextPressurePsiValue();

            if (psiPressureValue < LowPressureThreshold || HighPressureThreshold < psiPressureValue)
            {
                this.alarmOn = true;
            }
        }

        public bool AlarmOn
        {
            get { return this.alarmOn; }
        }
    }
}
