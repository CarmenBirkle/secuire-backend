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
            this.Db_connectionstring = _configuration.GetValue<string>(nameof(Db_connectionstring)) ?? "";

            this.BlockUserTimespanInSec = _configuration.GetValue<uint>(nameof(BlockUserTimespanInSec));
            this.BlockUserTries = _configuration.GetValue<uint>(nameof(BlockUserTries));
            
        }

        public string Db_connectionstring { get; private set; }
        public uint BlockUserTimespanInSec { get; private set; }
        public uint BlockUserTries { get; private set; }
    }
}
