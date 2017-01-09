namespace RequestsForRights.Domain.Helpers
{
    public static class MatchHelper
    {
        public static bool MatchValueInsensitive<T>(T value1, string value2)
        {
            return value1 != null && value1.ToString().ToLower().Contains(value2.ToLower());
        }
    }
}