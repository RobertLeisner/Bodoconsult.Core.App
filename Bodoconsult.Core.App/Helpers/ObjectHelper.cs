// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Bodoconsult.Core.App.Helpers
{
    /// <summary>
    /// Helper class for copying properties from one object to another
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// Copy property values from one object to the other
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void MapProperties(object source, object target)
        {

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            var sourceType = source.GetType();

            var targetType = target.GetType();

            var propMap = GetMatchingProperties(sourceType, targetType);

            for (var i = 0; i < propMap.Count; i++)
            {

                var prop = propMap[i];

                var sourceValue = prop.SourceProperty.GetValue(source, null);

                prop.TargetProperty.SetValue(target, sourceValue, null);

            }
        }


        /// <summary>
        /// Copy property values from one object to the other
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns>true if objects have the same properties values</returns>
        public static bool CompareProperties(object source, object target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }


            var sourceType = source.GetType();

            var targetType = target.GetType();

            var propMap = GetMatchingProperties(sourceType, targetType);

            for (var i = 0; i < propMap.Count; i++)
            {

                var prop = propMap[i];

                //if (prop.SourceProperty.Name == "RowVersion") continue;

                var sourceValue = prop.SourceProperty.GetValue(source, null);

                var targetValue = prop.TargetProperty.GetValue(target, null);

                if (sourceValue == null || targetValue == null)
                {
                    continue;
                }


                if (sourceValue.ToString() != targetValue.ToString())
                {

                    return false;
                }

            }



            return true;
        }


        internal static IList<PropertyMap> GetMatchingProperties(Type sourceType, Type targetType)
        {
            if (sourceType == null)
            {
                throw new ArgumentNullException(nameof(sourceType));
            }

            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            var sourceProperties = sourceType.GetProperties();

            var targetProperties = targetType.GetProperties();

            var properties = (from s in sourceProperties

                              from t in targetProperties

                              where s.Name == t.Name &&

                                    s.CanRead &&

                                    t.CanWrite &&
                                    s.PropertyType.IsPublic &&
                                    t.PropertyType.IsPublic &&

                                    s.PropertyType == t.PropertyType &&

                                    (

                                        s.PropertyType.IsValueType &&

                                        t.PropertyType.IsValueType ||

                                        s.PropertyType == typeof(string) &&

                                        t.PropertyType == typeof(string)

                                    )

                              select new PropertyMap
                              {

                                  SourceProperty = s,

                                  TargetProperty = t

                              }).ToList();

            return properties;

        }


        /// <summary>
        /// Fill an object with sample data
        /// </summary>
        /// <param name="data"></param>
        public static void FillProperties(object data)
        {
            if (data == null)
            {
                return;
            }

            var theType = data.GetType();
            var p = theType.GetProperties();

            foreach (var pi in p)
            {

                switch (pi.Name.ToUpperInvariant())
                {
                    case "ID":
                    case "ROWVERSION":
                        continue;
                }

                try
                {
                    var custAttr = pi.GetCustomAttributes(true);

                    string typeName;
                    if (pi.PropertyType.IsGenericType &&
                        pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        typeName = pi.PropertyType.GetGenericArguments()[0].Name.ToUpperInvariant();
                    }
                    else
                    {
                        typeName = pi.PropertyType.Name.ToUpperInvariant();
                    }

                    switch (typeName)
                    {
                        case "STRING":
                            var length = 50;
                            if (custAttr.Length > 0)
                            {

                                var attr = custAttr.FirstOrDefault(x => x is StringLengthAttribute);


                                if (attr != null)
                                {
                                    length = ((StringLengthAttribute)attr).MaximumLength;
                                }
                            }

                            pi.SetValue(data, new string('a', length));
                            break;
                        case "INT32":
                            pi.SetValue(data, 1);
                            break;
                        case "DATETIME":
                            pi.SetValue(data, DateTime.Now);
                            break;
                        case "BOOLEAN":
                            pi.SetValue(data, true);
                            break;
                        case "DOUBLE":
                            pi.SetValue(data, 1.99);
                            break;
                        default:
                            Debug.Print($"{pi.Name} {typeName}");
                            break;
                    }

                }
                catch //(Exception)
                {
                    // No action
                }
            }

        }

        /// <summary>
        /// Get the values of an object as string
        /// </summary>
        /// <param name="o">Current object</param>
        /// <returns>Properties of the object and their values as string</returns>
        public static string GetObjectPropertiesAsString(object o)
        {
            var type = o.GetType();
            
            if (o is string)
            {
                return o.ToString();
            }

            if (type.IsValueType)
            {
                return o.ToString();
            }

            var props = type.GetProperties();
            
            var str = new StringBuilder();
            str.Append("{");
            foreach (var prop in props)
            {
                var v = prop.GetValue(o);

                if (v == null)
                {
                    continue;
                }
                str.Append($"{prop.Name}:{prop.GetValue(o)},") ;
            }

            var result = str.ToString();
            return result.Remove(result.Length - 1) + "}";
        }

    }

    internal class PropertyMap
    {

        public PropertyInfo SourceProperty { get; set; }

        public PropertyInfo TargetProperty { get; set; }

    }
}
