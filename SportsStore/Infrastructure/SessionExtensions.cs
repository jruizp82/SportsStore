using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
namespace SportsStore.Infrastructure
{
    public static class SessionExtensions
    {

        //These methods rely on the Json.Net package to serialize objects into the JavaScript Object Notation format
        //The Json.Net package doesn’t have to be added to the project because it is already used behind the scenes by MVC to provide the JSON helper feature
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static T GetJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);
            return sessionData == null
            ? default(T) : JsonConvert.DeserializeObject<T>(sessionData);
        }
    }
}
