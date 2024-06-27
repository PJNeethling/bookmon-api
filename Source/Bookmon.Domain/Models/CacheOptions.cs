namespace Bookmon.Domain.Models;

public class CacheOptions
{
    public bool Enabled { get; set; }
    public int TimeoutInMinutes { get; set; }
}