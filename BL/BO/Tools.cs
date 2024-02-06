using DalApi;
using System.Reflection;
using System.Text;

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
        public static string ToStringProperty<T>(this T obj)
        {
            string str;
            IEnumerable<T> enumerable = obj as IEnumerable<T>;
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
            foreach (PropertyInfo property in obj.GetType().GetProperties()) // get all the properties of the object
            {
                str += $"{property.Name}: {property.GetValue(obj)}\n";
            }
            return str;
        }



    }
}
