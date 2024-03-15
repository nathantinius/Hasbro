namespace HitPoints.Application.Models;

public class PlayerCharacter
{
    public required string Name { get; init; }
    public required int Level { get; set; }
    public required int HitPoints { get; set; }
    public required int TemporaryHitPoints { get; set; } = 0;
    public required IEnumerable<PlayerCharacterClass> Classes { get; set; }
    public required PlayerCharacterStats Stats { get; set; }
    public required IEnumerable<PlayerCharacterItem>? Items { get; set; } = default;
    public required IEnumerable<PlayerCharacterDefense>? Defenses { get; set; } = default;
}

public class PlayerCharacterClass
{
    public required string Name { get; set; }
    public required int HitDiceValue { get; set; }
    public required int ClassLevel { get; set; }
}

public class PlayerCharacterStats
{
    public required int Strength { get; set; }
    public required int Dexterity { get; set; }
    public required int Constitution { get; set; }
    public required int Intelligence { get; set; }
    public required int Wisdom { get; set; }
    public required int Charisma { get; set; }
}

public class PlayerCharacterItem
{
    public required string Name { get; set; }
    public required ItemModifier Modifier { get; set; }
}

public class ItemModifier
{
    public required string AffectedObject { get; init; }
    public required string AffectedValue { get; init; }
    public required int Value { get; set; }
}


public class PlayerCharacterDefense
{
    public required string Type { get; set; }
    public required string Defense { get; set; }
}