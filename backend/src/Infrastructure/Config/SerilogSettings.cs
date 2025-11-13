namespace Infrastructure.Config;

public class SerilogSettings
{
    public string LogPath { get; set; } = "logs/log-.txt";
    public string MinimumLevel { get; set; } = "Information";
    public bool WriteToConsole { get; set; } = true;
    public bool WriteToFile { get; set; } = true;
}
