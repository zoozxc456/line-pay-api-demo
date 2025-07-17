namespace LinePayDemo.Infrastructure.Settings;

public sealed class LinePaySettings
{
    public required string ChannelId { get; init; }
    public required string ChannelSecret { get; init; }
    public required string ApiBaseUrl { get; init; }
}