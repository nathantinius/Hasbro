using HitPoints.Contracts.Data;

namespace HitPoints.Contracts.Requests;

public class CreateCharacterRequest
{
    public required string Name { get; init; }
    public required int Level { get; set; }
    public required int HitPoints { get; set; }
    public int? TemporaryHitPoints { get; set; }
    public required IEnumerable<PlayerCharacterClass> Classes { get; set; }
    public required PlayerCharacterStats Stats { get; set; }
    public required IEnumerable<PlayerCharacterItem>? Items { get; set; } = default;
    public required IEnumerable<PlayerCharacterDefense>? Defenses { get; set; } = default;
}