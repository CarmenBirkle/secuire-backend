namespace PWManagerService
{
    public class Appsettings
    {
        /// <summary>
        /// Singleton Appsetttings Instance - Thread save
        /// </summary>
        public static Appsettings Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Appsettings();
                    }
                    return instance;
                }
            }
        }
        private static Appsettings instance = null;

        private static readonly object padlock = new object();
        private readonly IConfiguration _configuration;


        private Appsettings()
        {
            _configuration = Program.Configuration;
            this.TestValue = _configuration.GetValue<string>("TestValue") ?? "";
            this.Db_connectionstring = _configuration.GetValue<string>("Db_connectionstring") ?? "";

            int permitLimit = _configuration.GetSection("RateLimit").GetValue<int>("PermitLimit");
            int timeWindowinMinutes = _configuration.GetSection("RateLimit").GetValue<int>("TimeWindowInMinutes");
            int queueLimit = _configuration.GetSection("RateLimit").GetValue<int>("QueueLimit");
            this.oRateLimit = new RateLimit(permitLimit, timeWindowinMinutes, queueLimit);
        }

        public string TestValue { get; private set; }
        public string Db_connectionstring { get; private set; }
        public RateLimit oRateLimit { get; private set; }

        public class RateLimit
        {
            public RateLimit(int permitLimit, int timeWindowInMinutes, int queueLimit)
            {
                this.PermitLimit = permitLimit;
                this.TimeWindowInMinutes = timeWindowInMinutes;
                this.QueueLimit = queueLimit;
            }

            public int PermitLimit { get; private set; }
            public int TimeWindowInMinutes { get; private set; }
            public int QueueLimit { get; private set; }
        }

    }
}
