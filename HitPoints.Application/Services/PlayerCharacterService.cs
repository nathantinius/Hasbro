using HitPoints.Application.Models;
using HitPoints.Application.Repositories;

namespace HitPoints.Application.Services;

public class PlayerCharacterService : IPlayerCharacterService
{
    private readonly IPlayerCharacterRepository _playerCharacterRepository;

    public PlayerCharacterService(IPlayerCharacterRepository playerCharacterRepository)
    {
        _playerCharacterRepository = playerCharacterRepository;
    }

    public Task<PlayerCharacter?> GetByName(string name)
    {
        return _playerCharacterRepository.GetByName(name.ToLower());
    }

    public async Task<PlayerCharacter?> Update(PlayerCharacter playerCharacter)
    {
        var playerExists = await _playerCharacterRepository.GetByName(playerCharacter.Name.ToLower());

        if (playerExists is null)
        {
            return null;
        }
        
        await _playerCharacterRepository.Update(playerCharacter);
        return playerCharacter;
    }

    public Task<bool> Create(PlayerCharacter playerCharacter)
    {
        return _playerCharacterRepository.Create(playerCharacter);
    }
}