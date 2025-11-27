namespace CSD.UpgradeTestTool.Infrastructure.Settings;
public class IrisDbSettings
{
    public string Server { get; set; } = string.Empty;
    public string Namespace { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool Ssl { get; set; } = false;

    public string GetConnectionString()
    {
        var sslMode = Ssl ? "SSLMode=Required" : "";
        return $"Server={Server};Namespace={Namespace};User ID={UserId};Password={Password};{sslMode};";
    }
}