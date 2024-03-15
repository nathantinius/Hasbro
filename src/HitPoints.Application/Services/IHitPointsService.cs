using HitPoints.Application.Models;

namespace HitPoints.Application.Services;

public interface IHitPointsService
{
    Task<int> Heal(int Value, PlayerCharacter playerCharacter);
    Task<int> AddTemporary(int Value, PlayerCharacter playerCharacter);
    Task<int> DealDamage(string damageType, int damageValue, PlayerCharacter playerCharacter);

    Task<string> BuildDamageMessage(string DamageType, int damageDealt, int Value, PlayerCharacter playerCharacter);
}