using HitPoints.Application.Models;

namespace HitPoints.Application.Services;

public interface IPlayerCharacterService
{
    Task<PlayerCharacter?> GetByName(string name);
    Task<PlayerCharacter?> Update(PlayerCharacter playerCharacter);
    Task<bool> Create(PlayerCharacter playerCharacter);
}