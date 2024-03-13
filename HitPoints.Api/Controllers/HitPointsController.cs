using HitPoints.Application.Models;
using HitPoints.Application.Repositories;
using HitPoints.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HitPoints.Api.Controllers;

[ApiController]
[Route("api")]
public class HitPointsController: ControllerBase
{
    private readonly IPlayerCharacterRepository _playerCharacterRepository;

    public HitPointsController(IPlayerCharacterRepository playerCharacterRepository)
    {
        _playerCharacterRepository = playerCharacterRepository;
    }

    [HttpPut("hp")]
    public async Task<IActionResult> UpdateHp([FromBody] UpdateHitPointsRequest request)
    {
        var player = await _playerCharacterRepository.GetById(request.Id);
        if (player is null)
        {
            return BadRequest("What were you attacking?");
        }

        int currentHitPoints = player.HitPoints;
        int currentTemporaryHitPoints = player.TemporaryHitPoints;
        
        switch (request.Type)
        {
            case "damage":
                int damageDealt = CalculateDamageDealt(request, player);
                int leftover = Math.Max(damageDealt - currentTemporaryHitPoints, 0);
                player.TemporaryHitPoints = Math.Max(currentTemporaryHitPoints - damageDealt, 0);
                player.HitPoints = Math.Max(currentHitPoints - leftover, 0);
                break;
            case "heal":
                player.HitPoints += request.Value;
                break;
            case "temporary":
                player.TemporaryHitPoints += request.Value;
                break;
            default:
                return BadRequest(
                    "Your trying to update HP with an invalid type. The types allowed are \"heal\", \"damage\", and \"temporary\"");
        }

        await _playerCharacterRepository.Update(player);
        return Ok(player);
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

    private int CalculateDamageDealt(UpdateHitPointsRequest request, PlayerCharacter playerCharacter)
    {
        
        int damageDealtPercentage = GetDamageDealtPercentage(request.DamageType, playerCharacter);
        
        if (damageDealtPercentage == 0)
        {
            return 0;
        }

        int damageDealt = request.Value / damageDealtPercentage;
        return damageDealt;
    }
}