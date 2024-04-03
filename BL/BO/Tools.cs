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

        public static string ToStringProperty<T>(this T obj)
        {
            // Check if the input object is null
            Console.WriteLine();
            if (obj != null)
            {
                // Get the type of the input object
                Type objType = obj.GetType();
                // Get all the properties of the object's type
                PropertyInfo[] properties = objType.GetProperties();
                // Initialize a string to store the result
                string result = $"\nDetails for {objType.Name}:";

                // Iterate over each property
                foreach (PropertyInfo property in properties)
                {
                    // Get the value of the current property
                    object value = property.GetValue(obj);

                    // Check if the property value is not null
                    if (value != null)
                    {
                        // Check if the property type is not a string and implements IEnumerable (i.e., it's a collection)
                        if (property.PropertyType != typeof(string) && property.PropertyType.GetInterface(nameof(IEnumerable)) != null)
                        {
                            // Initialize a counter for collection items
                            int count = 0;
                            // Iterate over each item in the collection
                            foreach (var item in (IEnumerable<object>)value)
                            {
                                // Append the item index and recursively call ToStringProperty for the item
                                result += "\n {";
                                result += $"{++count}:";
                                result += ToStringProperty(item);
                                result += " }\n";
                            }
                        }
                        // Check if the property type is a class in the "BO" namespace
                        else if (property.PropertyType.Namespace == "BO" && property.PropertyType.IsClass)
                        {
                            // Append details for the nested property and recursively call ToStringProperty for the nested object
                            result += "\n {\n";
                            result += $"\nDetails for nested property {property.Name}:";
                            result += ToStringProperty(value);
                            result += " }\n";
                        }
                        // If the property is neither a collection nor a nested object
                        else
                        {
                            // Append the property name and value
                            result += $"{property.Name} = {value}\n";
                        }
                    }
                }
                // Return the formatted string representation of the object's properties
                return result;
            }
            // If the input object is null
            else
            {
                // Return a string indicating that the object is null
                return $"{typeof(T).Name} object is null.\n";
            }
        }
    }
}
