using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Master.Extensions
{
    public static class JTokenExtension
    {
        public static T ToObjectWithDefault<T>(this JToken jToken)
        {
            try
            {
                return jToken.ToObject<T>();

            }
            catch
            {
                return default(T);
            }
        }
    }
}
