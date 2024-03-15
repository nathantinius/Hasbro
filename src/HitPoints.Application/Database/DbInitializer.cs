using Dapper;

namespace HitPoints.Application.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DbInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync("""
                                      create table if not exists player_characters (
                                          name VARCHAR(50) primary key,
                                          level INT not null,
                                          hitpoints INT not null,
                                          temporaryhitpoints INT not null
                                      );
                                      """);
        
        await connection.ExecuteAsync("""
                                      create table if not exists player_character_classes (
                                          id SERIAL PRIMARY KEY,
                                          charactername VARCHAR(50) REFERENCES player_characters(name),
                                          name VARCHAR(50) not null,
                                          hitdicevalue INT not null,
                                          classlevel INT not null
                                      );
                                      """);
        
        await connection.ExecuteAsync("""
                                      create table if not exists player_character_stats (
                                          id SERIAL PRIMARY KEY,
                                          charactername VARCHAR(50) REFERENCES player_characters(name),
                                          strength INT not null,
                                          dexterity INT not null,
                                          constitution INT not null,
                                          intelligence INT not null,
                                          wisdom INT not null,
                                          charisma INT not null
                                      );
                                      """);
        
        await connection.ExecuteAsync("""
                                      create table if not exists player_character_items (
                                          charactername VARCHAR(50) REFERENCES player_characters(name),
                                          name VARCHAR(250) PRIMARY KEY
                                      );
                                      """);
        
        await connection.ExecuteAsync("""
                                      create table if not exists item_modifier (
                                          id SERIAL PRIMARY KEY,
                                          itemname VARCHAR(250) REFERENCES player_character_items(name),
                                          affectedobject VARCHAR(50),
                                          affectedvalue VARCHAR(50),
                                          value INT not null
                                      );
                                      """);
        
        await connection.ExecuteAsync("""
                                      create table if not exists player_character_defenses (
                                          id SERIAL PRIMARY KEY,
                                          charactername VARCHAR(50) REFERENCES player_characters(name),
                                          type VARCHAR(50) not null,
                                          defense VARCHAR(50) not null
                                      );
                                      """);
        
    }
}