using DalApi;
using System.Reflection;
using System.Text;
using System.Collections;

namespace BO
{
    /// <summary>
    /// // tools class
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// // return the object as string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// 
        //public static string ToStringProperty<T>(this T obj)
        //{
        //    string str;
        //    IEnumerable<T> enumerable = obj as IEnumerable<T>;
        //    if (enumerable != null)
        //    {
        //        str = "";
        //        foreach (T item in enumerable)
        //        {
        //            str += item.ToStringProperty() + "\n";
        //        }
        //        return str;
        //    }
        //    str = "";
        //    foreach (PropertyInfo property in obj.GetType().GetProperties()) // get all the properties of the object
        //    {
        //        str += $"{property.Name}: {property.GetValue(obj)}\n";
        //    }
        //    return str;
        //}
        public static string ToStringProperty<T>(this T obj)
        {
            Console.WriteLine();
            if (obj != null)
            {
                Type objType = obj.GetType();
                PropertyInfo[] properties = objType.GetProperties();

                string result = $"\nDetails for {objType.Name}:";

                foreach (PropertyInfo property in properties)
                {
                    object value = property.GetValue(obj);

                    if (value != null)
                    {
                        if (property.PropertyType != typeof(string) && property.PropertyType.GetInterface(nameof(IEnumerable)) != null)
                        {
                            int count = 0;
                            foreach (var item in (IEnumerable<object>)value)
                            {
                                result += "\n {";
                                result += $"{++count}:";
                                result += ToStringProperty(item);
                                result += " }\n";
                            }
                        }
                        else if (property.PropertyType.Namespace == "BO" && property.PropertyType.IsClass)
                        {
                            result += "\n {\n";
                            result += $"\nDetails for nested property {property.Name}:";
                            result += ToStringProperty(value);
                            result += " }\n";
                        }
                        else
                        {
                            result += $"{property.Name} = {value}\n";
                        }
                    }
                }

                return result;
            }
            else
            {
                return $"{typeof(T).Name} object is null.\n";
            }
        }





    }
}
