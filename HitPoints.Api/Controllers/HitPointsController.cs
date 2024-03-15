using FluentValidation;
using HitPoints.Api.Mapping;
using HitPoints.Application.Services;
using HitPoints.Contracts.Requests;
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

    [HttpGet(ApiEndpoints.HitPoints.Get)]
    public async Task<IActionResult> GetHp([FromQuery] GetHitPointsRequest request)
    {
        var player = await _playerCharacterService.GetByName(request.Name);
        var response = player.MapToHitPointsResponse();
        return Ok(response);
    }
    
    [HttpPut(ApiEndpoints.HitPoints.Update)]
    public async Task<IActionResult> UpdateHp([FromBody] UpdateHitPointsRequest request)
    {
        await _updateHitPointsValidator.ValidateAndThrowAsync(request);
        
        var player = await _playerCharacterService.GetByName(request.Name);

        int currentHitPoints = player.HitPoints;
        int currentTemporaryHitPoints = player.TemporaryHitPoints;
        string hitPointsMessage;
        object result;
        
        switch (request.Action)
        {
            case "damage":
                int damageDealt = await _hitPointsService.DealDamage(request.DamageType, request.Value, player);
                hitPointsMessage =
                    await _hitPointsService.BuildDamageMessage(request.DamageType, damageDealt, request.Value, player);
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
                return BadRequest(
                    "Your trying to update HP with an invalid action. The actions allowed are \"heal\", \"damage\", and \"temporary\"");
        }

        var updatedPlayer = await _playerCharacterService.Update(player);

        if (updatedPlayer is null)
        {
            return NotFound();
        }
        
        var response = updatedPlayer.MapToResponse();
        result = new { message = hitPointsMessage, response };
        return Ok(result);
    }
}