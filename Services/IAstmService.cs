namespace Services
{
    public interface IAstmService
    {
        Task SendMessageAsync(string host, int port, string astmMessage, CancellationToken cancellationToken = default);
    }
}