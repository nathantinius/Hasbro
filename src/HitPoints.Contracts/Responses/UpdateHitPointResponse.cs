namespace HitPoints.Contracts.Responses;

public class UpdateHitPointResponse
{
    public required string Message { get; init; }
    public required PlayerCharacterResponse PlayerCharacter { get; init; }
}