namespace HitPoints.Contracts.Requests;

public class UpdateHitPointsRequest
{
    public required Guid Id { get; set; }
    public required string Type { get; init; }
    public required int Value { get; init; }
    public string? DamageType { get; init; }
}