using HitPoints.Api.Mapping;
using HitPoints.Application.Models;
using HitPoints.Application.Repositories;
using HitPoints.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HitPoints.Api.Controllers;

[ApiController]
public class HitPointsController: ControllerBase
{
    private readonly IPlayerCharacterRepository _playerCharacterRepository;

    public HitPointsController(IPlayerCharacterRepository playerCharacterRepository)
    {
        _playerCharacterRepository = playerCharacterRepository;
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

    private int CalculateDamageDealt(string damageType, int damageValue, PlayerCharacter playerCharacter)
    {
        
        int damageDealtPercentage = GetDamageDealtPercentage(damageType, playerCharacter);
        
        if (damageDealtPercentage == 0)
        {
            return 0;
        }

        int damageDealt = damageValue / damageDealtPercentage;
        return damageDealt;
    }
    
    //ENDPOINTS START HERE ---------------------------------------//

    [HttpPost(ApiEndpoints.Characters.Create)]
    public async Task<IActionResult> CreateCharacter([FromBody] CreateCharacterRequest request)
    {
        var playerCharacter = request.MapToPlayerCharacter();
        var result = await _playerCharacterRepository.Create(playerCharacter);
        return Ok(result);
    }

    [HttpGet(ApiEndpoints.HitPoints.Get)]
    public async Task<IActionResult> GetHp([FromQuery] GetHitPointsRequest request)
    {
        var player = await _playerCharacterRepository.GetByName(request.Name);
        var response = player.MapToHitPointsResponse();
        return Ok(response);
    }
    
    [HttpPut(ApiEndpoints.HitPoints.Update)]
    public async Task<IActionResult> UpdateHp([FromBody] UpdateHitPointsRequest request)
    {
        var player = await _playerCharacterRepository.GetByName(request.Name);
        if (player is null)
        {
            return BadRequest("What were you attacking?");
        }

        int currentHitPoints = player.HitPoints;
        int currentTemporaryHitPoints = player.TemporaryHitPoints;
        string hitPointsMessage = null;
        object result;
        
        switch (request.Type)
        {
            case "damage":
                int damageDealt = CalculateDamageDealt(request.DamageType, request.Value, player);
                if (damageDealt == 0)
                {
                    result = new { message = $"{player.Name} is immune to {request.DamageType} and took 0 points of {request.DamageType} damage.", player };
                    return Ok(result);
                } else if (damageDealt == request.Value / 2)
                {
                    hitPointsMessage = $"{player.Name} is resistant to {request.DamageType} and took {damageDealt} points of {request.DamageType} damage";
                }
                else
                {
                    hitPointsMessage = $"{player.Name} took {request.Value} points of {request.DamageType} damage";
                }
                int leftover = Math.Max(damageDealt - currentTemporaryHitPoints, 0);
                player.TemporaryHitPoints = Math.Max(currentTemporaryHitPoints - damageDealt, 0);
                player.HitPoints = Math.Max(currentHitPoints - leftover, 0);
                break;
            case "heal":
                //TODO: Discuss with team on how we should deal with the characters HP maximum.
                player.HitPoints += request.Value;
                hitPointsMessage = $"{player.Name} received {request.Value} points of healing!";
                break;
            case "temporary":
                player.TemporaryHitPoints += request.Value;
                hitPointsMessage = $"{player.Name} received {request.Value} temporary hit points!";
                break;
            default:
                return BadRequest(
                    "Your trying to update HP with an invalid type. The types allowed are \"heal\", \"damage\", and \"temporary\"");
        }

        await _playerCharacterRepository.Update(player);
        result = new { message = hitPointsMessage, player };
        return Ok(result);
    }
}