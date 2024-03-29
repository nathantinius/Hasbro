using FluentValidation;
using HitPoints.Api.Mapping;
using HitPoints.Application.Services;
using HitPoints.Contracts.Requests;
using HitPoints.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace HitPoints.Api.Controllers;

[ApiController]
public class HitPointsController: ControllerBase
{
    private readonly IPlayerCharacterService _playerCharacterService;
    private readonly IHitPointsService _hitPointsService;
    private readonly IValidator<UpdateHitPointsRequest> _updateHitPointsValidator;

    public HitPointsController(IPlayerCharacterService playerCharacterService, IHitPointsService hitPointsService, IValidator<UpdateHitPointsRequest> updateHitPointsValidator)
    {
        _playerCharacterService = playerCharacterService;
        _hitPointsService = hitPointsService;
        _updateHitPointsValidator = updateHitPointsValidator;
    }
    
    
    /// <summary>
    /// Takes a name as a query parameter and returns a playerCharacter's HitPoints
    /// </summary>
    /// <param name="name">The name of the playerCharacter</param>
    /// <returns>
    /// {
    ///     name: String,
    ///     hitPoints: Int,
    ///     teporaryHitPoints
    /// }
    /// </returns>
    [HttpGet(ApiEndpoints.HitPoints.Get)]
    public async Task<IActionResult> GetHp([FromQuery] GetHitPointsRequest request)
    {
        var player = await _playerCharacterService.GetByName(request.Name);
        if (player is null)
        {
            return NotFound();
        }
        var response = player.MapToHitPointsResponse();
        return Ok(response);
    }
    
    /// <summary>
    /// Receives a Request body and returns
    /// </summary>
    /// <param name="name">The name of the playerCharacter being affected</param>
    /// <param name="action">heal, damage, or temporary</param>
    /// <param name="value">the hp value associated with the action</param>
    /// <param name="damageType">Bludgeoning, Piercing, Slashing, Fire, Cold, Acid, Thunder, Lightning, Poison, Radiant, Necrotic, Psychic, Force</param>
    /// <returns>
    /// {
    ///     message: String,
    ///     playerCharacter
    /// }
    /// </returns>
    [HttpPut(ApiEndpoints.HitPoints.Update)]
    public async Task<IActionResult> UpdateHp([FromBody] UpdateHitPointsRequest request)
    {
        await _updateHitPointsValidator.ValidateAndThrowAsync(request);
        
        var player = await _playerCharacterService.GetByName(request.Name);

        int currentHitPoints = player.HitPoints;
        int currentTemporaryHitPoints = player.TemporaryHitPoints;
        string hitPointsMessage;
        object result;
        
        switch (request.Action.ToLower())
        {
            case "damage":
                int damageDealt = await _hitPointsService.DealDamage(request.DamageType!.ToLower(), request.Value, player);
                hitPointsMessage =
                    await _hitPointsService.BuildDamageMessage(request.DamageType.ToLower(), damageDealt, request.Value, player);
                int leftover = Math.Max(damageDealt - currentTemporaryHitPoints, 0);
                player.TemporaryHitPoints = Math.Max(currentTemporaryHitPoints - damageDealt, 0);
                player.HitPoints = Math.Max(currentHitPoints - leftover, 0);
                break;
            case "heal":
                player.HitPoints = await _hitPointsService.Heal(request.Value, player);
                hitPointsMessage = $"{player.Name} received {request.Value} points of healing!";
                break;
            case "temporary":
                player.TemporaryHitPoints = await _hitPointsService.AddTemporary(request.Value, player);
                hitPointsMessage = $"{player.Name} received {request.Value} temporary hit points!";
                break;
            default: 
                return null;
        }

        var updatedPlayer = await _playerCharacterService.Update(player);

        if (updatedPlayer is null)
        {
            return NotFound();
        }
        
        var response = updatedPlayer.MapToResponse();
        result = new UpdateHitPointResponse { Message = hitPointsMessage, PlayerCharacter = response };
        return Ok(result);
    }
}