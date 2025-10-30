namespace OpcDAManage.DaServices
{
    using GodSharp.Opc.Da;
    using GodSharp.Opc.Da.Options;
    public class OpcSettingService
    {
        List<GroupData> groups;
        ServerData server;
        IOpcDaClient client;
        public OpcSettingService()
        {
        }

        public void IniGroupAndTags()
        {
            groups = new List<GroupData>();
            server = new ServerData
            {
                Host = "127.0.0.1",
                ProgId = "Matrikon.OPC.Simulation.1",
                // initial with data info,after connect will be add to client
                // if this is null,you should add group and tag manually
                Groups = groups
            };
            client = DaClientFactory.Instance.CreateOpcAutomationClient(new DaClientOptions(
                server,
                OnDataChangedHandler,
                OnShoutdownHandler,
                OnAsyncReadCompletedHandler,
                OnAsyncWriteCompletedHandler));

            // Connect to Opc Server
            var connected = client.Connect();

            // Add Group to Client
            client.Add(new Group() { Name = "Group0", UpdateRate = 100, ClientHandle = 100, IsSubscribed = true });

            // Add Tag to Group
            // add one by one
            client.Groups["Group0"].Add(new Tag("Group0.tag1", 100));
        }

        public static void OnDataChangedHandler(DataChangedOutput output)
        {
            Console.WriteLine($"{output.Data.ItemName}:{output.Data.Value},{output.Data.Quality} / {output.Data.Timestamp}");
        }

        public static void OnAsyncReadCompletedHandler(AsyncReadCompletedOutput output)
        {
            Console.WriteLine(
                $"Async Read {output.Data.Result.ItemName}:{output.Data.Result.Value},{output.Data.Result.Quality} / {output.Data.Result.Timestamp} / {output.Data.Code}");
        }

        public static void OnAsyncWriteCompletedHandler(AsyncWriteCompletedOutput output)
        {
            Console.WriteLine($"Async Write {output.Data.Result.ItemName}:{output.Data.Code}");
        }

        public static void OnShoutdownHandler(Server server, string reason)
        {
        }
    }
}
