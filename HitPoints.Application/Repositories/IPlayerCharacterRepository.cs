using HitPoints.Application.Models;

namespace HitPoints.Application.Repositories;

public interface IPlayerCharacterRepository
{
    Task<PlayerCharacter?> GetById(Guid id);
    Task<bool> Update(PlayerCharacter playerCharacter);
}