using HitPoints.Application.Models;

namespace HitPoints.Application.Repositories;

public interface IPlayerCharacterRepository
{
    Task<PlayerCharacter?> GetByName(string name);
    Task<bool> Update(PlayerCharacter playerCharacter);
    Task<bool> Create(PlayerCharacter playerCharacter);
}