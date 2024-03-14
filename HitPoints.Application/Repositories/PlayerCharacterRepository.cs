using Dapper;
using HitPoints.Application.Database;
using HitPoints.Application.Models;

namespace HitPoints.Application.Repositories;

public class PlayerCharacterRepository : IPlayerCharacterRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public PlayerCharacterRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public Task<PlayerCharacter?> GetByName(string name)
    {
        return null;
    }

    public Task<bool> Update(PlayerCharacter playerCharacter)
    {
        return null;
    }

    public async Task<bool> Create(PlayerCharacter playerCharacter)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(
            new CommandDefinition("""
                                  insert into player_characters (name, level, hitpoints, temporaryhitpoints)
                                  values (@Name, @Level, @HitPoints, @TemporaryHitPoints)
                                  """, playerCharacter)
        );

        if (result > 0)
        {
            foreach (var characterClass in playerCharacter.Classes)
            {
                var classesResult = await connection.ExecuteAsync(
                    new CommandDefinition("""
                                          insert into player_character_classes (charactername, name, hitdicevalue, classlevel)
                                          values (@CharacterName, @Name, @HitDiceValue, @ClassLevel)
                                          """,
                        new
                        {
                            CharacterName = playerCharacter.Name, Name = characterClass.Name,
                            HitDiceValue = characterClass.HitDiceValue, ClassLevel = characterClass.ClassLevel
                        })
                );
            }
            
            var statsResult = await connection.ExecuteAsync(
                new CommandDefinition("""
                                      insert into player_character_stats (charactername, strength, dexterity, constitution, wisdom, intelligence, charisma)
                                      values (@CharacterName, @Strength, @Dexterity, @Constitution, @Wisdom, @Intelligence, @Charisma)
                                      """,
                    new
                    {
                        CharacterName = playerCharacter.Name, Strength = playerCharacter.Stats.Strength,
                        Dexterity = playerCharacter.Stats.Dexterity, Constitution = playerCharacter.Stats.Constitution,
                        Wisdom = playerCharacter.Stats.Wisdom, Intelligence = playerCharacter.Stats.Intelligence,
                        Charisma = playerCharacter.Stats.Charisma
                    })
            );

            foreach (var characterItem in playerCharacter.Items)
            {
                var defenseResult = await connection.ExecuteAsync(
                    new CommandDefinition("""
                                          insert into player_character_items (charactername, name)
                                          values (@CharacterName, @Name)
                                          """, new { CharacterName = playerCharacter.Name, Name = characterItem.Name })
                );

                var modifierResult = await connection.ExecuteAsync(
                    new CommandDefinition("""
                                          insert into item_modifier (itemname, affectedobject, affectedvalue, value)
                                          values (@ItemName, @AffectedObject, @AffectedValue, @Value)
                                          """,
                        new
                        {
                            ItemName = characterItem.Name, AffectedObject = characterItem.Modifier.AffectedObject,
                            AffectedValue = characterItem.Modifier.AffectedValue, Value = characterItem.Modifier.Value
                        })
                );
            }
            
            foreach (var characterDefense in playerCharacter.Defenses)
            {
                var defenseResult = await connection.ExecuteAsync(
                    new CommandDefinition("""
                                          insert into player_character_defenses (charactername, type, defense)
                                          values (@CharacterName, @Type, @Defense)
                                          """,
                        new
                        {
                            CharacterName = playerCharacter.Name, Type = characterDefense.Type,
                            Defense = characterDefense.Defense
                        })
                );

            }

        }

        transaction.Commit();
        return result > 0;
    }
}