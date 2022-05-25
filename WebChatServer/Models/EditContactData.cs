namespace WebChatServer.Models
{
    public class EditContactData
    {
        public string Name { get; set; }
        public string Server { get; set; }
        public EditContactData(string name, string server)
        {
            Name = name;
            Server = server;
        }
    }
}
