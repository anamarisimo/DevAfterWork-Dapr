namespace PublisherApp.Models;

public class Alert
{
    public string Region { get; set; }

    public AlertType Type { get; set; }
}

public enum AlertType
{
    Storm,
    Bear,
    Heatwave
}
