using HitPoints.Application.Models;
using HitPoints.Contracts.Data;
using HitPoints.Contracts.Requests;
using HitPoints.Contracts.Responses;

namespace HitPoints.Api.Mapping;

public static class ContractMapping
{
    public static PlayerCharacter MapToPlayerCharacter(this CreateCharacterRequest request)
    {
        var classes = new List<PlayerCharacterClass>();
        var items = new List<PlayerCharacterItem>();
        var defenses = new List<PlayerCharacterDefense>();
        
        foreach (var characterClass in request.Classes)
        {
            classes.Add(new PlayerCharacterClass
            {
                Name = characterClass.Name,
                HitDiceValue = characterClass.HitDiceValue,
                ClassLevel = characterClass.ClassLevel
            });
        }

        var stats = new PlayerCharacterStats
        {
            Strength = request.Stats.Strength,
            Dexterity = request.Stats.Dexterity,
            Constitution = request.Stats.Constitution,
            Intelligence = request.Stats.Intelligence,
            Wisdom = request.Stats.Wisdom,
            Charisma = request.Stats.Charisma
        };

        if (request.Items is not null)
        {
            foreach (var characterItem in request.Items)
            {
                items.Add(new PlayerCharacterItem
                {
                    Name = characterItem.Name,
                    Modifier = new ItemModifier
                    {
                        AffectedObject = characterItem.Modifier.AffectedObject,
                        AffectedValue = characterItem.Modifier.AffectedValue,
                        Value = characterItem.Modifier.Value
                    }
                });
            }
        }

        if (request.Defenses is not null)
        {
            foreach (var characterDefense in request.Defenses)
            {
                defenses.Add(new PlayerCharacterDefense
                {
                    Type = characterDefense.Type,
                    Defense = characterDefense.Defense
                });
            }
        }


        return new PlayerCharacter
        {
            Name = request.Name,
            Level = request.Level,
            HitPoints = request.HitPoints,
            TemporaryHitPoints = 0,
            Classes = classes,
            Stats = stats,
            Items = items,
            Defenses = defenses
        };
    }
    public static PlayerCharacterResponse MapToResponse(this PlayerCharacter playerCharacter)
    {
        var classes = new List<PlayerCharacterClassDto>();
        var items = new List<PlayerCharacterItemDto>();
        var defenses = new List<PlayerCharacterDefenseDto>();
        
        foreach (var characterClass in playerCharacter.Classes)
        {
            classes.Add(new PlayerCharacterClassDto
            {
                Name = characterClass.Name,
                HitDiceValue = characterClass.HitDiceValue,
                ClassLevel = characterClass.ClassLevel
            });
        }

        var stats = new PlayerCharacterStatsDto
        {
            Strength = playerCharacter.Stats.Strength,
            Dexterity = playerCharacter.Stats.Dexterity,
            Constitution = playerCharacter.Stats.Constitution,
            Intelligence = playerCharacter.Stats.Intelligence,
            Wisdom = playerCharacter.Stats.Wisdom,
            Charisma = playerCharacter.Stats.Charisma
        };

        if (playerCharacter.Items is not null)
        {
            foreach (var characterItem in playerCharacter.Items)
            {
                items.Add(new PlayerCharacterItemDto
                {
                    Name = characterItem.Name,
                    Modifier = new ItemModifierDto
                    {
                        AffectedObject = characterItem.Modifier.AffectedObject,
                        AffectedValue = characterItem.Modifier.AffectedValue,
                        Value = characterItem.Modifier.Value
                    }
                });
            }
        }

        if (playerCharacter.Defenses is not null)
        {
            foreach (var characterDefense in playerCharacter.Defenses)
            {
                defenses.Add(new PlayerCharacterDefenseDto
                {
                    Type = characterDefense.Type,
                    Defense = characterDefense.Defense
                });
            }
        }
        
        return new PlayerCharacterResponse
        {
            Name = playerCharacter.Name,
            Level = playerCharacter.Level,
            HitPoints = playerCharacter.HitPoints,
            TemporaryHitPoints = playerCharacter.TemporaryHitPoints,
            Classes = classes,
            Stats = stats,
            Items = items,
            Defenses = defenses
        };
    }

    public static HitPointsResponse MapToHitPointsResponse(this PlayerCharacter playerCharacter)
    {
        return new HitPointsResponse
        {
            Name = playerCharacter.Name,
            HitPoints = playerCharacter.HitPoints,
            TemporaryHitPoints = playerCharacter.TemporaryHitPoints
        };

    }
}