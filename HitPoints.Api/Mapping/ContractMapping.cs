using HitPoints.Application.Models;
using HitPoints.Contracts.Responses;

namespace HitPoints.Api.Mapping;

public static class ContractMapping
{
    public static PlayerCharacterResponse MapToResponse(this PlayerCharacter playerCharacter)
    {
        return new PlayerCharacterResponse
        {
            Id = playerCharacter.Id,
            Name = playerCharacter.Name,
            Level = playerCharacter.Level,
            HitPoints = playerCharacter.HitPoints,
            TemporaryHitPoints = playerCharacter.TemporaryHitPoints,
            Classes = playerCharacter.Classes,
            Stats = playerCharacter.Stats,
            Items = playerCharacter.Items,
            Defenses = playerCharacter.Defenses
        };
    }
}