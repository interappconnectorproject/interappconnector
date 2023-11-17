using System.Globalization;

namespace InterAppConnector.Attributes
{
    /// <summary>
    /// Define a custom input that can be parsed from the object binded from the parameters.
    /// Use this attribuute when you want to create, for instance, a <seealso cref="DateTime">DateTime</seealso> parameter or a custom object
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class CustomInputStringAttribute : Attribute
    {
        public string StringFormat { get; private set; }

        /// <summary>
        /// Constructor that defines the string format expected from the object.
        /// If you use this constructor, the library will search for a ParseExact method in the object and pass the input digited by the user and the <paramref name="stringFormat">stringFormat</paramref> as parameter with an invariant culture
        /// If you want to use this contructor for a custom object, define a ParseExact method as described in the following code example 
        /// <code>
        /// // Replace ObjectType with your custom object type. Bear in mind that the value passed to culture by the library is <seealso cref="CultureInfo.InvariantCulture">CultureInfo.InvariantCulture</seealso> 
        /// </code>
        /// <code>
        /// public static ObjectType ParseExact(<seealso cref="string">string</seealso> stringToParse, <seealso cref="string">string</seealso> <paramref name="stringFormat">stringFormat</paramref>, <seealso cref="CultureInfo">CultureInfo</seealso> culture)
        /// {
        ///     // your code
        /// }
        /// </code>
        /// Use this constructor if you create an object that receives values that could have different formats. An example is the <see cref="DateTime">DateTime</see> object
        /// </summary>
        /// <param name="stringFormat">The format string to pass in the ParseExact Method</param>
        public CustomInputStringAttribute(string stringFormat)
        {
            StringFormat = stringFormat;
        }

        /// <summary>
        /// Base constructor for custom objects
        /// If you use this constructor, the library will search for a constructor that accept a <seealso cref="string"/> parameter and pass the value digited by the user as value
        /// Use this constructor if you would like to create a custom object that have a simple string 
        /// </summary>
        public CustomInputStringAttribute()
        {

        }
    }
}
