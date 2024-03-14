namespace HitPoints.Contracts.Data;

public class PlayerCharacterItem
{
    public required string Name { get; init; }
    public required ItemModifier Modifier { get; init; }
}

public class ItemModifier
{
    public required string AffectedObject { get; init; }
    public required string AffectedValue { get; init; }
    public required int Value { get; init; }
}