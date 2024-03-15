using FluentValidation;
using HitPoints.Api.Controllers;
using HitPoints.Application.Services;
using HitPoints.Contracts.Requests;
using NSubstitute;

namespace HitPoints.Api.Tests.Unit;

public class HitPointsControllerTests
{
    private readonly HitPointsController _sut;
    private readonly IPlayerCharacterService _playerCharacterService = Substitute.For<IPlayerCharacterService>();
    private readonly IHitPointsService _hitPointsService = Substitute.For<IHitPointsService>();
    private readonly IValidator<UpdateHitPointsRequest> _validator;

    public HitPointsControllerTests()
    {
        _sut = new HitPointsController(_playerCharacterService, _hitPointsService, _validator);
    }

    [Fact]
    public async Task UpdateHp_ReturnsValidationFailure_When
}