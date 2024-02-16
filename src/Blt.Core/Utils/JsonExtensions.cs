namespace Blt.Core.Utils
{
    public static class JsonExtensions
    {
        public static string ToJson(this object command)
        {
            return System.Text.Json.JsonSerializer.Serialize(command);
        }
    }
}