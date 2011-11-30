using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


namespace Elmah.Everywhere
{
    // TODO: Silverlight may have problem with reflection

    public static class FormHelper
    {
        public static string FormData(object formValues)
        {
            if (formValues != null)
            {
                IDictionary<string, object> result = AnonymousObjectToFormValues(formValues);
                if (result.Any())
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var pair in result)
                    {
                        if ((sb.Length > 0) && (sb[sb.Length - 1] != '&'))
                        {
                            sb.Append("&");
                        }
                        sb.Append(pair.Key);
                        sb.Append("=");
                        sb.Append(pair.Value);
                    }
                    return sb.ToString();
                }
            }
            return string.Empty;
        }

        private static IDictionary<string, object> AnonymousObjectToFormValues(object formValues)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            if (formValues != null)
            {
                PropertyInfo[] properties = formValues.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
                foreach (var p in properties)
                {
                    result.Add(p.Name, p.GetValue(formValues, null));
                }
            }
            return result;
        }
    }
}
