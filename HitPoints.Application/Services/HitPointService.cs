using System.Net;
using HitPoints.Application.Models;

namespace HitPoints.Application.Services;

public class HitPointService : IHitPointsService
{
    //TODO: Discuss with team on how we should deal with the characters HP maximum.
    //We should not allow a character to be healed beyond their max.
    public Task<int> Heal(int value, PlayerCharacter playerCharacter)
    {
        return Task.FromResult(playerCharacter.HitPoints += value);
    }

    public Task<int> AddTemporary(int value, PlayerCharacter playerCharacter)
    {
        return Task.FromResult(playerCharacter.TemporaryHitPoints += value);
    }
    
    public Task<int> DealDamage(string damageType, int damageValue, PlayerCharacter playerCharacter)
    {
        int damageDealtPercentage = GetDamageDealtPercentage(damageType, playerCharacter);
        
        if (damageDealtPercentage == 0) 
        { 
            return Task.FromResult(0);
        }

        int damageDealt = damageValue / damageDealtPercentage;
        return Task.FromResult(damageDealt);
    }

    public Task<string> BuildDamageMessage(string damageType, int damageDealt, int value, PlayerCharacter playerCharacter )
    {
        string? message;
        int halfDamage = value / 2;
        if (damageDealt == 0)
        {
            message = $"{playerCharacter.Name} is immune to {damageType} and took 0 points of {damageType} damage.";
            return Task.FromResult(message);
        } else if (damageDealt == halfDamage)
        {
            message = $"{playerCharacter.Name} is resistant to {damageType} and took {damageDealt} points of {damageType} damage";
            return Task.FromResult(message);
        }
        else
        {
            message = $"{playerCharacter.Name} took {value} points of {damageType} damage";
            return Task.FromResult(message);
        }
        
    }

    private int GetDamageDealtPercentage(string damageType, PlayerCharacter playerCharacter)
    {
        foreach (var defense in playerCharacter.Defenses!)
        {
            if (defense.Type == damageType)
            {
                switch (defense.Defense)
                {
                    case "immunity":
                        return 0;
                    case "resistance":
                        return 2;
                    default:
                        return 1;
                }
            }
        }
        return 1;
    }
}