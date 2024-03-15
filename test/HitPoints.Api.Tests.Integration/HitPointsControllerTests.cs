using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using HitPoints.Contracts.Requests;
using HitPoints.Contracts.Responses;

namespace HitPoints.Api.Tests.Integration;

public class HitPointsControllerTests : IClassFixture<HitPointsApiFactory>
{
    private readonly HitPointsApiFactory _applicationFactory;
    private readonly HttpClient _httpClient;

    public HitPointsControllerTests(HitPointsApiFactory applicationFactory)
    {
        _applicationFactory = applicationFactory;
        _httpClient = applicationFactory.CreateClient();
    }

    [Theory]
    [InlineData("Briv")]
    [InlineData("briv")]
    public async Task GetHp_ReturnsOk_WhenUserExists(string name)
    {
        var response = await _httpClient.GetAsync($"api/hp?name={name}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var json = await response.Content.ReadFromJsonAsync<HitPointsResponse>();
        json!.Name.Should().Be("Briv");
    }
    
    [Fact]
    public async Task GetHp_ReturnsNotFound_WhenNoUserExists()
    {
        var response = await _httpClient.GetAsync("api/hp?name=dan");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData("dan", "damage", 5, "fire")]
    [InlineData("", "damage", 5, "fire")]
    public async Task UpdateHp_ReturnsValidationFailure_WhenNoUserExists(string name, string action, int value, string damageType)
    {
        var request = new UpdateHitPointsRequest
        {
            Name = name,
            Action = action,
            Value = value,
            DamageType = damageType
        };
    
        var content = JsonContent.Create(request);
        var response = await _httpClient.PutAsync("api/hp", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    
        var json = await response.Content.ReadFromJsonAsync<ValidationFailureResponse>();
        json!.Errors.Should().Contain(vr => vr.Message == "What are you aiming at? We can't find this character.");
    }
    
    [Theory]
    [InlineData("briv", "damage", 5, "")]
    [InlineData("briv", "damage", 5, "laser")]
    public async Task UpdateHp_ReturnsValidationFailure_WhenTheActionIsDamageButAValidDamageTypeIsNotGiven(string name, string action, int value, string damageType)
    {
        var request = new UpdateHitPointsRequest
        {
            Name = name,
            Action = action,
            Value = value,
            DamageType = damageType
        };
    
        var content = JsonContent.Create(request);
        var response = await _httpClient.PutAsync("api/hp", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    
        var json = await response.Content.ReadFromJsonAsync<ValidationFailureResponse>();
        json!.Errors.Should().Contain(vr => vr.Message.Contains("When choosing damage as the action a valid damage type must be provided the valid types are:"));
    }
    
    [Fact]
    public async Task UpdateHp_ReturnsValidationFailure_WhenANegativeValueIsGiven()
    {
        var request = new UpdateHitPointsRequest
        {
            Name = "briv",
            Action = "damage",
            Value = -5,
            DamageType = "piercing"
        };
    
        var content = JsonContent.Create(request);
        var response = await _httpClient.PutAsync("api/hp", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    
        var json = await response.Content.ReadFromJsonAsync<ValidationFailureResponse>();
        json!.Errors.Should().Contain(vr => vr.Message == "You can't use negative values when updating HP");
    }
    
    [Fact]
    public async Task UpdateHp_ReturnsValidationFailure_WhenAnInvalidActionIsGiven()
    {
        var request = new UpdateHitPointsRequest
        {
            Name = "briv",
            Action = "attack",
            Value = 5,
            DamageType = "piercing"
        };
    
        var content = JsonContent.Create(request);
        var response = await _httpClient.PutAsync("api/hp", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    
        var json = await response.Content.ReadFromJsonAsync<ValidationFailureResponse>();
        json!.Errors.Should().Contain(vr => vr.Message.Contains("The Action must be one of the following:"));
    }
    
    [Theory]
    [InlineData("Briv", "Damage", 5, "Fire")]
    [InlineData("BRIV", "DAMAGE", 5, "FIRE")]
    [InlineData("briv", "damage", 5, "fire")]
    public async Task UpdateHp_ReturnsOk_WhenGivenAValidPayloadRegardlessOfCase(string name, string action, int value, string damageType)
    {
        var request = new UpdateHitPointsRequest
        {
            Name = name,
            Action = action,
            Value = value,
            DamageType = damageType
        };
    
        var content = JsonContent.Create(request);
        var response = await _httpClient.PutAsync("api/hp", content);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}