namespace HitPoints.Contracts.Data;

public class PlayerCharacterClassDto
{
    public required string Name { get; init; }
    public required int HitDiceValue { get; init; }
    public required int ClassLevel { get; init; }
}