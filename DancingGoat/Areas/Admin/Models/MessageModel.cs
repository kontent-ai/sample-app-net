namespace DancingGoat.Areas.Admin.Models
{
    public class MessageModel
    {
        public string Caption { get; set; }
        public string Message { get; set; }
        public MessageType MessageType { get; set; }
    }

    public enum MessageType
    {
        Info,
        Error
    }
}