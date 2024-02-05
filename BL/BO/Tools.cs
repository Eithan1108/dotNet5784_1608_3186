using DalApi;
using System.Reflection;

namespace BO
{


    public static class Tools
    {    
        public static string ToStringProperty<T>(this T obj)
        {
            string str;
            IEnumerable<T> enumerable= obj as IEnumerable<T>;
            if (enumerable != null)
            {
                str = "";
                foreach (T item in enumerable)
                {
                    str += item.ToStringProperty() + "\n";
                }
                return str;
            }
            str = "";
            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                str += $"{property.Name}: {property.GetValue(obj)}\n";
            }
            return str;
        }
    }
}
