using System;

public class JDiscordStatus
{
    private string details;
    private string state;

    public string Details => details;
    public string State => state;

    public JDiscordStatus(string details, string state = null)
    {
        this.details = details;
        this.state = state;
    }
}