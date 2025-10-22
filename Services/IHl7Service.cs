namespace Services
{
    public interface IHl7Service
    {
        Task SendMessageAsync(string host, int port, string hl7Message, CancellationToken cancellationToken = default);
    }
}