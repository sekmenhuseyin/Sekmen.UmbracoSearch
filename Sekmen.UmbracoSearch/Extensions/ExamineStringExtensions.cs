namespace Sekmen.UmbracoSearch.Extensions
{
    public static class ExamineStringExtensions
    {
        public static string MakeSearchQuerySafe(this string query)
        {
            return string.IsNullOrEmpty(query)
                ? query
                : query
                    .Replace("*", string.Empty)
                    .Replace("?", string.Empty)
                ;
        }
    }
}
