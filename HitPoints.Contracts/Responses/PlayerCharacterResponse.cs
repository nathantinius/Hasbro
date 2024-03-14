namespace HitPoints.Contracts.Responses;

public class PlayerCharacterResponse
{
    public required string Name { get; init; }
    public required int Level { get; set; }
    public required int HitPoints { get; set; }
    public required int TemporaryHitPoints { get; set; }
    public required IEnumerable<object> Classes { get; set; }
    public required object Stats { get; set; }
    public required IEnumerable<object>? Items { get; set; } = default;
    public required IEnumerable<object>? Defenses { get; set; } = default;
}