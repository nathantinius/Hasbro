namespace HitPoints.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";
    
    //TODO: Discuss with Team if we want to break up the update endpoint into multiple endpoints
    //An argument could be made for splitting but it means the client needs to request multiple Api Endpoints vs simply changing the payload.
    public static class HitPoints
    {
        private const string Base = $"{ApiBase}/hp";
        
        public const string Get = Base;
        public const string Update = Base;
    }
}