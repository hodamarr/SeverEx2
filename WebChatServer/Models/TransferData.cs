namespace WebChatServer.Models
{
    public class TransferData
    {
        public string From { get; set; }

        public string To { get; set; }

        public string Content { get; set; }

        public TransferData(string from, string to, string content)
        {
           From = from;
           To = to;    
           Content = content;
        }
    }
}
