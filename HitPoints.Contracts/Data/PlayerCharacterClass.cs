namespace HitPoints.Contracts.Data;

public class PlayerCharacterClass
{
    public required string Name { get; init; }
    public required int HitDiceValue { get; init; }
    public required int ClassLevel { get; init; }
}