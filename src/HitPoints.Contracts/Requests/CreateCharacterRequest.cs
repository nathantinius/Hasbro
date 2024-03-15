using HitPoints.Contracts.Data;

namespace HitPoints.Contracts.Requests;

public class CreateCharacterRequest
{
    public required string Name { get; init; }
    public required int Level { get; set; }
    public required int HitPoints { get; set; }
    public int? TemporaryHitPoints { get; set; }
    public required IEnumerable<PlayerCharacterClassDto> Classes { get; set; }
    public required PlayerCharacterStatsDto Stats { get; set; }
    public required IEnumerable<PlayerCharacterItemDto>? Items { get; set; } = default;
    public required IEnumerable<PlayerCharacterDefenseDto>? Defenses { get; set; } = default;
}