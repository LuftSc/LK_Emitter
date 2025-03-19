using System.Reflection;

namespace Registrator.API.Extensions
{
    public class DateTimeWrapper
    {
        public DateTime Value { get; set; }

        public static ValueTask<DateTimeWrapper?> BindAsync(HttpContext context, ParameterInfo parameter)
        {
            if (context.Request.Query.TryGetValue("r", out var rValue))
            {
                if (DateTime.TryParse(rValue.ToString().Replace(" ", "T"), out var dateTime))
                {
                    return ValueTask.FromResult<DateTimeWrapper?>(new DateTimeWrapper { Value = dateTime });
                }
            }

            return ValueTask.FromResult<DateTimeWrapper?>(null);
        }
        public static bool TryParse(string? value, out DateTimeWrapper result)
        {
            if (DateTime.TryParse(value?.Replace(" ", "T"), out var dateTime))
            {
                result = new DateTimeWrapper { Value = dateTime };
                return true;
            }

            result = null!;
            return false;
        }
    }
}
