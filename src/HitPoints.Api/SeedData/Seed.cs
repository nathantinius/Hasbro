using HitPoints.Api.Mapping;
using HitPoints.Application.Services;
using HitPoints.Contracts.Requests;
using Newtonsoft.Json;

namespace HitPoints.Api.SeedData;

public class Seed
{
    private readonly IPlayerCharacterService _playerCharacterService;

    public Seed(IPlayerCharacterService playerCharacterService)
    {
        _playerCharacterService = playerCharacterService;
    }

    public async Task Execute()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "./SeedData/briv.json");
        using (StreamReader r = new StreamReader(filePath))
        {
            string json = r.ReadToEnd();
            CreateCharacterRequest createCharacterRequest = JsonConvert.DeserializeObject<CreateCharacterRequest>(json);

            var playerExists = await _playerCharacterService.GetByName(createCharacterRequest.Name);

            if (playerExists is null)
            {
                var playerCharacter = createCharacterRequest.MapToPlayerCharacter();
                await _playerCharacterService.Create(playerCharacter);
            }
        }
        
        return;
    }
}