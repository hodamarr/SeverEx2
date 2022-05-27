namespace WebChatServer.Models
{
    public class InvitationData
    {
        public int Id { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Server { get; set; }

        public InvitationData(string from, string to, string server)
        {
            From = from;
            To = to;
            Server = server;
        }
    }
}
