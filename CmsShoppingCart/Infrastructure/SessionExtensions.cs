using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CmsShoppingCart.Infrastructure
{
    public static class SessionExtensions
    {
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        
        public static T GetJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);

            if (sessionData == null)
                return default;

            var result = JsonConvert.DeserializeObject<T>(sessionData);

            return result;
        }
    }
}
