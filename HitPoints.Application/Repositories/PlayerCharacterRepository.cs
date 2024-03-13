using HitPoints.Application.Models;

namespace HitPoints.Application.Repositories;

public class PlayerCharacterRepository : IPlayerCharacterRepository
{
    private readonly List<PlayerCharacter> _playerCharacters = new List<PlayerCharacter>([
        new PlayerCharacter
        {
            Id = Guid.Parse("7195d3a0-06fb-455f-871c-2520d1e9da1e"),
            Name = "Briv",
            Level = 5,
            HitPoints = 25,
            TemporaryHitPoints = 0,
            Stats = new PlayerCharacterStats
            {
                Strength = 15,
                Dexterity = 12,
                Constitution = 14,
                Intelligence = 13,
                Wisdom = 10,
                Charisma = 8
            },
            Classes = new List<PlayerCharacterClass>([
                new PlayerCharacterClass
                {
                    Name = "fighter",
                    ClassLevel = 5,
                    HitDiceValue = 10
                }
            ]),
            Items = new List<PlayerCharacterItem>([
                new PlayerCharacterItem
                {
                    Name = "Ioun Stone of Fortitude",
                    Modifier = new ItemModifier
                    {
                        AffectedObject = "stats",
                        AffectedValue = "constitution",
                        Value = 2
                    },
                }
            ]),
            Defenses = new List<PlayerCharacterDefense>([
                new PlayerCharacterDefense
                {
                    Type = "fire",
                    Defense = "immunity"
                },
                new PlayerCharacterDefense
                {
                    Type = "slashing",
                    Defense = "resistance"
                }
            ])
        }
    ]);

    public Task<PlayerCharacter?> GetById(Guid id)
    {
        var playerCharacter = _playerCharacters.SingleOrDefault(p => p.Id == id);
        return Task.FromResult(playerCharacter);
    }

    public Task<bool> Update(PlayerCharacter playerCharacter)
    {
        var characterIndex = _playerCharacters.FindIndex(p => p.Id == playerCharacter.Id);
        if (characterIndex == -1)
        {
            return Task.FromResult(false);
        }

        _playerCharacters[characterIndex] = playerCharacter;
        return Task.FromResult(true);
    }
}