namespace HitPoints.Contracts.Responses;

public class HitPointsResponse
{
    public required string Name { get; init; }
    public required int HitPoints { get; init; }
    public required int TemporaryHitPoints { get; init; }
}