namespace signalr01
{
    public class UserDetails
    {
        public UserDetails(string connectionId, string name)
        {
            ConnectionId = connectionId;
            Name = name;
        }

        public string ConnectionId { get; }
        public string Name { get; }
    }
}
