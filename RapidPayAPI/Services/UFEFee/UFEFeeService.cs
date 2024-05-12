namespace RapidPayAPI.Services.UFEFee
{
    public class UFEFeeService
    {
        public UFEFeeService()
        {
            var random = new Random();
            Fee = Math.Round((decimal)random.NextDouble(), 2);
            Timer = new System.Timers.Timer(3600000);
            Timer.Elapsed += (sender, e) => UpdateFee();
            Timer.Start();
        }

        public virtual decimal Fee { get; private set; }

        private System.Timers.Timer Timer { get; set; }

        private void UpdateFee()
        {
            var randomFee = new Random();
            decimal factor = (decimal)randomFee.NextDouble() * 2;
            Fee = Math.Round(Fee * factor, 2);
        }
    }
}
