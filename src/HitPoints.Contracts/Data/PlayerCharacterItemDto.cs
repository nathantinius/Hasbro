namespace HitPoints.Contracts.Data;

public class PlayerCharacterItemDto
{
    public required string Name { get; init; }
    public required ItemModifierDto Modifier { get; init; }
}

public class ItemModifierDto
{
    public required string AffectedObject { get; init; }
    public required string AffectedValue { get; init; }
    public required int Value { get; init; }
}