using FluentValidation;
using HitPoints.Application.Models;
using HitPoints.Application.Services;
using HitPoints.Contracts.Requests;

namespace HitPoints.Api.Validators;

public class HitPointsValidator : AbstractValidator<UpdateHitPointsRequest>
{
    private readonly List<string> availableDamageTypes = ["bludgeoning", "piercing", "slashing", "fire", "cold", "acid", "thunder", "lightning", "poison", "radiant", "necrotic", "psychic", "force"];
    private readonly List<string> availableActions = ["heal", "damage", "temporary"];
    private readonly IPlayerCharacterService _playerCharacterService;
    
    public HitPointsValidator(IPlayerCharacterService playerCharacterService)
    {
        _playerCharacterService = playerCharacterService;
        
        RuleFor(r => r.Name)
            .NotEmpty()
            .MustAsync(BeAnExistingPlayer)
            .WithMessage($"What are you aiming at? We can't find this character.");
        
        RuleFor(r => r.Action)
            .NotEmpty()
            .Must(BeInAvailableActions)
            .WithMessage($"The Action must be one of the following: {string.Join(", ", availableActions)}");

        RuleFor(r => r.Value)
            .NotEmpty()
            .GreaterThanOrEqualTo(0)
            .WithMessage("You can't use negative values when updating HP");

        When(r => r.Action.ToLower() == "damage", () =>
        {
            RuleFor(r => r.DamageType)
                .NotEmpty()
                .Must(BeInAvailableDamageTypes!)
                .WithMessage($"When choosing damage as the action a valid damage type must be provided the valid types are: {string.Join(", ", availableDamageTypes)}");
        });
    }

    private async Task<bool> BeAnExistingPlayer(string characterName, CancellationToken token = default)
    {
        var playerExists = await _playerCharacterService.GetByName(characterName);
        if (playerExists is not null)
        {
            return true;
        }
        return false;
    }

    private bool BeInAvailableActions(string action)
    {
        return availableActions.Contains(action.ToLower());
    }

    private bool BeInAvailableDamageTypes(string damageType)
    {
        return availableDamageTypes.Contains(damageType.ToLower());
    }

}