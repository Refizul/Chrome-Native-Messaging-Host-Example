using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4Net.config", Watch = true)]

class Program {

    // create a static logger field
    #pragma warning disable CS8602 // Dereference of a possibly null reference.
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    #pragma warning restore CS8602 // Dereference of a possibly null reference.

    static void Main(string[] args)
    {
        if(!log4net.LogManager.GetRepository().Configured) {
            // log4net not configured

            Console.WriteLine("Log4Net not loaded!");

            foreach(log4net.Util.LogLog l4n_cnf_message in log4net.LogManager.GetRepository().ConfigurationMessages.Cast<log4net.Util.LogLog>())
            {
                Console.WriteLine(l4n_cnf_message);
            }
        }

        // Connect Message
        string message = "Connected to Native App.";
        OpenStandardStreamOut(message);
        
        string ret = "";
        while((ret = OpenStandardStreamIn()) != null)   
        { 
            // do something... 
            log.Debug("Received in Main:" + ret);

            try {
                JObject obj =  JObject.Parse(ret);

                #pragma warning disable CS8604 // Possible null reference argument.
                OpenStandardStreamOut(obj.Value<string>("text"));
                #pragma warning restore CS8604 // Possible null reference argument.
            }
            catch(JsonReaderException jrex) {
                log.Error($"Invalid Json! ({jrex})");
            }
            catch(Exception ex) {
                log.Error($"General Error ({ex})");
            }
        };

        log.Debug("Exit!");
    }

    private static string OpenStandardStreamIn()
    {
        //// We need to read first 4 bytes for length information
        Stream stdin = Console.OpenStandardInput();
        int length = 0;
        byte[] bytes = new byte[4];
        stdin.Read(bytes, 0, 4);
        length = System.BitConverter.ToInt32(bytes, 0);

        log.Debug("Received Length: " + length.ToString());

        string input = "";
        for (int i = 0; i < length;i++ )    
        {
            input += (char)stdin.ReadByte();
        }
        
        log.Debug("Received: " + input);
        return input;  
    }

    private static void OpenStandardStreamOut(string stringData)
    {
        //// We need to send the 4 btyes of length information
        var msgdata = JObject.FromObject(new { echo = stringData });        
        int DataLength = msgdata.ToString(Formatting.None).Length;

        Stream stdout = Console.OpenStandardOutput();
        stdout.WriteByte((byte)((DataLength >> 0) & 0xFF));
        stdout.WriteByte((byte)((DataLength >> 8) & 0xFF));
        stdout.WriteByte((byte)((DataLength >> 16) & 0xFF));
        stdout.WriteByte((byte)((DataLength >> 24) & 0xFF));
        //Available total length : 4,294,967,295 ( FF FF FF FF )

        log.Debug("Sent: " + msgdata.ToString(Formatting.None));
        Console.Write(msgdata.ToString(Formatting.None));
    }
}
