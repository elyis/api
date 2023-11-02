namespace api
{
    public static class Constants
    {
        // public static readonly string serverUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS").Split(";").First();
        public static readonly string serverUrl = "https://b620-185-7-93-108.ngrok-free.app";

        public static readonly string localPathToStorages = @"Resources/";
        public static readonly string localPathToProfileIcons = $"{localPathToStorages}ProfileIcons/";

        public static readonly string webPathToProfileIcons = $"{serverUrl}/api/upload/profileIcon/";
    }
}