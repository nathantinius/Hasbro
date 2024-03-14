namespace HitPoints.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Characters
    {
        private const string Base = $"{ApiBase}/characters";

        public const string Create = Base;
    }
    
    public static class HitPoints
    {
        private const string Base = $"{ApiBase}/hp";
        
        public const string Get = Base;
        public const string Update = Base;
    }
}