using Catalog.Core.Constants;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable

namespace Catalog.Core.Extensions
{
    public static class IQueryableExtensions
    {
        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string propertyName, bool ascending)
        {
            var orderByMethodName = ascending ? "OrderBy" : "OrderByDescending";

            Type entityType = typeof(TEntity);
            var property = entityType.GetProperty(propertyName);
            var parameterExpression = Expression.Parameter(entityType, "p");
            var memberExpression = Expression.PropertyOrField(parameterExpression, property.Name);
            var lambda = Expression.Lambda(memberExpression, new[] { parameterExpression });

            var orderByExpression = Expression.Call(typeof(Queryable), orderByMethodName, new[] { entityType, memberExpression.Type }, source.Expression, lambda);
            return (IOrderedQueryable<TEntity>)source.Provider.CreateQuery<TEntity>(orderByExpression);
        }

        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source, string value, StringCondition condition = StringCondition.Equal)
        {
            if (string.IsNullOrEmpty(value) && condition != StringCondition.Blank)
            {
                return source;
            }

            var containsMethodInfo = typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) });
            var endsWithMethodInfo = typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) });
            var toLowerMethodInfo = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes);
            var startsWithMethodInfo = typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) });
            var trimMethodInfo = typeof(string).GetMethod(nameof(string.Trim), Type.EmptyTypes);

            Type entityType = typeof(TEntity);
            var parameterExp = Expression.Parameter(entityType, "x");
            var expressions = new List<Expression>();

            var properties = entityType.GetProperties();
            foreach (var property in properties)
            {
                if (!property.PropertyType.Equals(typeof(string)))
                {
                    continue;
                }

                var propertyExp = Expression.PropertyOrField(parameterExp, property.Name);
                var constValueExp = Expression.Convert(Expression.Constant(value), property.PropertyType);
                switch (condition)
                {
                    case StringCondition.Blank:
                        // Translate to Expression
                        // x.Property == null || x.Property.Trim() == string.Empty
                        var constNullExp = Expression.Convert(Expression.Constant(null), property.PropertyType);
                        var constEmptyExp = Expression.Convert(Expression.Constant(string.Empty), property.PropertyType);
                        var equalToNullExp = Expression.Equal(propertyExp, constNullExp);
                        var equalToEmptyExp = Expression.Equal(Expression.Call(propertyExp, trimMethodInfo), constEmptyExp);
                        var blankExp = Expression.OrElse(equalToNullExp, equalToEmptyExp);

                        // string.IsNullOrWhiteSpace(x.Property)
                        // Performance wise, this method is faster..
                        var isNullOrWhiteSpaceExp = Expression.Call(typeof(string), nameof(string.IsNullOrWhiteSpace), null, propertyExp);
                        expressions.Add(isNullOrWhiteSpaceExp);
                        break;
                    case StringCondition.Equal:
                        var equalExp = Expression.Equal(propertyExp, constValueExp);
                        expressions.Add(equalExp);
                        break;
                    case StringCondition.NotEqual:
                        var notEqualExp = Expression.NotEqual(propertyExp, constValueExp);
                        expressions.Add(notEqualExp);
                        break;
                    case StringCondition.Contains:
                        var containsExp = Expression.Call(propertyExp, containsMethodInfo, constValueExp);
                        expressions.Add(containsExp);
                        break;
                    case StringCondition.StartsWith:
                        var startsWithExp = Expression.Call(propertyExp, startsWithMethodInfo, constValueExp);
                        expressions.Add(startsWithExp);
                        break;
                    case StringCondition.EndsWith:
                        var endsWithExp = Expression.Call(propertyExp, endsWithMethodInfo, constValueExp);
                        expressions.Add(endsWithExp);
                        break;
                    default:
                        break;
                }
            }

            if (expressions.Count == 0)
            {
                return source;
            }

            var resultExp = expressions.Aggregate(Expression.OrElse);
            var lambda = Expression.Lambda(resultExp, false, parameterExp);
            var whereExpression = Expression.Call(typeof(Queryable), "Where", new[] { entityType }, source.Expression, lambda);
            return source.Provider.CreateQuery<TEntity>(whereExpression);
        }

        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source, object value, NumberCondition condition = NumberCondition.Equal)
        {
            if ((value == null || !IsValueNumeric(value)) && condition != NumberCondition.Blank)
            {
                return source;
            }

            Type entityType = typeof(TEntity);
            var parameterExp = Expression.Parameter(entityType, "x");
            var expressions = new List<Expression>();

            var properties = entityType.GetProperties();
            foreach (var property in properties)
            {
                if (!IsTypeNumeric(property.PropertyType))
                {
                    continue;
                }

                var propertyExp = Expression.PropertyOrField(parameterExp, property.Name);
                var constValueExp = Expression.Convert(Expression.Constant(GetMaxValueIfOutOfRange(value, property.PropertyType)), property.PropertyType);
                switch (condition)
                {
                    case NumberCondition.Blank:
                        if (Nullable.GetUnderlyingType(property.PropertyType) == null)
                        {
                            // Property is not nullable ..
                            continue;
                        }

                        var equalToNullExp = Expression.Equal(propertyExp, Expression.Convert(Expression.Constant(null), property.PropertyType));
                        expressions.Add(equalToNullExp);
                        break;
                    case NumberCondition.Equal:
                        var equalExp = Expression.Equal(propertyExp, constValueExp);
                        expressions.Add(equalExp);
                        break;
                    case NumberCondition.NotEqual:
                        var notEqualExp = Expression.NotEqual(propertyExp, constValueExp);
                        expressions.Add(notEqualExp);
                        break;
                    case NumberCondition.GreaterThan:
                        var greaterExp = Expression.GreaterThan(propertyExp, constValueExp);
                        expressions.Add(greaterExp);
                        break;
                    case NumberCondition.GreaterThanOrEqual:
                        var greaterOrEqualExp = Expression.GreaterThanOrEqual(propertyExp, constValueExp);
                        expressions.Add(greaterOrEqualExp);
                        break;
                    case NumberCondition.LessThan:
                        var lessExp = Expression.LessThan(propertyExp, constValueExp);
                        expressions.Add(lessExp);
                        break;
                    case NumberCondition.LessThanOrEqual:
                        var lessOrEqualExp = Expression.LessThanOrEqual(propertyExp, constValueExp);
                        expressions.Add(lessOrEqualExp);
                        break;
                    default:
                        break;
                }
            }

            if (expressions.Count == 0)
            {
                return source;
            }

            var resultExp = expressions.Aggregate(Expression.OrElse);
            var lambda = Expression.Lambda(resultExp, false, parameterExp);
            var whereExpression = Expression.Call(typeof(Queryable), "Where", new[] { entityType }, source.Expression, lambda);
            return source.Provider.CreateQuery<TEntity>(whereExpression);
        }

        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source, string propertyName, string value, StringCondition condition = StringCondition.Equal)
        {
            if (string.IsNullOrWhiteSpace(propertyName) || (string.IsNullOrWhiteSpace(value) && condition != StringCondition.Blank))
            {
                return source;
            }

            var containsMethodInfo = typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) });
            var endsWithMethodInfo = typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) });
            var startsWithMethodInfo = typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) });
            var trimMethodInfo = typeof(string).GetMethod(nameof(string.Trim), Type.EmptyTypes);

            Type entityType = typeof(TEntity);
            Type propertyType = GetPropertyType(entityType, propertyName);

            ParameterExpression parameterExp = Expression.Parameter(entityType, "x");
            MemberExpression propertyExp = CreateMemberExpression(entityType, parameterExp, propertyName);
            UnaryExpression constValueExp = Expression.Convert(Expression.Constant(value), propertyType);
            Expression resultExp = default;

            switch (condition)
            {
                case StringCondition.Blank:
                    resultExp = Expression.Call(typeof(string), nameof(string.IsNullOrWhiteSpace), null, propertyExp);
                    break;
                case StringCondition.Equal:
                    resultExp = Expression.Equal(propertyExp, constValueExp);
                    break;
                case StringCondition.NotEqual:
                    resultExp = Expression.NotEqual(propertyExp, constValueExp);
                    break;
                case StringCondition.Contains:
                    resultExp = Expression.Call(propertyExp, containsMethodInfo, constValueExp);
                    break;
                case StringCondition.StartsWith:
                    resultExp = Expression.Call(propertyExp, startsWithMethodInfo, constValueExp);
                    break;
                case StringCondition.EndsWith:
                    resultExp = Expression.Call(propertyExp, endsWithMethodInfo, constValueExp);
                    break;
                default:
                    break;
            }

            if (resultExp == default)
            {
                return source;
            }

            var lambda = Expression.Lambda(resultExp, false, parameterExp);
            var whereExpression = Expression.Call(typeof(Queryable), "Where", new[] { entityType }, source.Expression, lambda);
            return source.Provider.CreateQuery<TEntity>(whereExpression);
        }

        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source, string propertyName, object value, NumberCondition condition = NumberCondition.Equal)
        {
            if (string.IsNullOrWhiteSpace(propertyName) || ((value == null || !IsValueNumeric(value)) && condition != NumberCondition.Blank))
            {
                return source;
            }

            Type entityType = typeof(TEntity);
            var parameterExp = Expression.Parameter(entityType, "x");
            var property = entityType.GetProperty(propertyName);
            if (property == null)
            {
                return source;
            }

            if (!IsTypeNumeric(property.PropertyType))
            {
                return source;
            }

            var propertyExp = Expression.PropertyOrField(parameterExp, property.Name);
            var constValueExp = Expression.Convert(Expression.Constant(GetMaxValueIfOutOfRange(value, property.PropertyType)), property.PropertyType);

            Expression resultExp = default;
            switch (condition)
            {
                case NumberCondition.Blank:
                    if (Nullable.GetUnderlyingType(property.PropertyType) == null)
                    {
                        // Property is not nullable ..
                        return source;
                    }

                    resultExp = Expression.Equal(propertyExp, Expression.Convert(Expression.Constant(null), property.PropertyType));
                    break;
                case NumberCondition.Equal:
                    resultExp = Expression.Equal(propertyExp, constValueExp);
                    break;
                case NumberCondition.NotEqual:
                    resultExp = Expression.NotEqual(propertyExp, constValueExp);
                    break;
                case NumberCondition.GreaterThan:
                    resultExp = Expression.GreaterThan(propertyExp, constValueExp);
                    break;
                case NumberCondition.GreaterThanOrEqual:
                    resultExp = Expression.GreaterThanOrEqual(propertyExp, constValueExp);
                    break;
                case NumberCondition.LessThan:
                    resultExp = Expression.LessThan(propertyExp, constValueExp);
                    break;
                case NumberCondition.LessThanOrEqual:
                    resultExp = Expression.LessThanOrEqual(propertyExp, constValueExp);
                    break;
                default:
                    break;
            }

            if (resultExp == default)
            {
                return source;
            }

            var lambda = Expression.Lambda(resultExp, false, parameterExp);
            var whereExpression = Expression.Call(typeof(Queryable), "Where", new[] { entityType }, source.Expression, lambda);
            return source.Provider.CreateQuery<TEntity>(whereExpression);
        }

        #region Generic ValueType .Where() extension method overload
        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> source, ValueType value, ValueCondition condition = ValueCondition.Equal)
        {
            if (value == null && condition != ValueCondition.Blank)
            {
                return source;
            }

            Type entityType = typeof(TEntity);
            var parameterExp = Expression.Parameter(entityType, "x");
            var expressions = new List<Expression>();

            var properties = entityType.GetProperties();
            foreach (var property in properties)
            {
                if (!property.PropertyType.IsValueType)
                {
                    continue;
                }

                var propertyExp = Expression.PropertyOrField(parameterExp, property.Name);
                UnaryExpression constValueExp;
                switch (condition)
                {
                    case ValueCondition.Blank:
                        if (Nullable.GetUnderlyingType(property.PropertyType) == null)
                        {
                            // Property is not nullable ..
                            continue;
                        }

                        var nullExp = Expression.Constant(null);
                        var equalToNullExp = Expression.Equal(propertyExp, Expression.Convert(nullExp, property.PropertyType));
                        expressions.Add(equalToNullExp);
                        break;
                    case ValueCondition.Equal:
                        if (!CanPerformEqualityOperation(value.GetType(), property.PropertyType))
                        {
                            continue;
                        }

                        constValueExp = CreateConstantExpression(value, property.PropertyType);
                        var equalExp = Expression.Equal(propertyExp, constValueExp);
                        expressions.Add(equalExp);
                        break;
                    case ValueCondition.NotEqual:
                        if (!CanPerformEqualityOperation(value.GetType(), property.PropertyType))
                        {
                            continue;
                        }

                        constValueExp = CreateConstantExpression(value, property.PropertyType);
                        var notEqualExp = Expression.NotEqual(propertyExp, constValueExp);
                        expressions.Add(notEqualExp);
                        break;
                    case ValueCondition.GreaterThan:
                        if (!CanPerformComparisonOperation(value.GetType(), property.PropertyType))
                        {
                            continue;
                        }

                        constValueExp = CreateConstantExpression(value, property.PropertyType);
                        var greaterExp = Expression.GreaterThan(propertyExp, constValueExp);
                        expressions.Add(greaterExp);
                        break;
                    case ValueCondition.GreaterThanOrEqual:
                        if (!CanPerformComparisonOperation(value.GetType(), property.PropertyType))
                        {
                            continue;
                        }

                        constValueExp = CreateConstantExpression(value, property.PropertyType);
                        var greaterOrEqualExp = Expression.GreaterThanOrEqual(propertyExp, constValueExp);
                        expressions.Add(greaterOrEqualExp);
                        break;
                    case ValueCondition.LessThan:
                        if (!CanPerformComparisonOperation(value.GetType(), property.PropertyType))
                        {
                            continue;
                        }

                        constValueExp = CreateConstantExpression(value, property.PropertyType);
                        var lessExp = Expression.LessThan(propertyExp, constValueExp);
                        expressions.Add(lessExp);
                        break;
                    case ValueCondition.LessThanOrEqual:
                        if (!CanPerformComparisonOperation(value.GetType(), property.PropertyType))
                        {
                            continue;
                        }

                        constValueExp = CreateConstantExpression(value, property.PropertyType);
                        var lessOrEqualExp = Expression.LessThanOrEqual(propertyExp, constValueExp);
                        expressions.Add(lessOrEqualExp);
                        break;
                    default:
                        break;
                }
            }

            if (expressions.Count == 0)
            {
                return source;
            }

            var resultExp = expressions.Aggregate(Expression.OrElse);
            var lambda = Expression.Lambda(resultExp, false, parameterExp);
            var whereExpression = Expression.Call(typeof(Queryable), "Where", new[] { entityType }, source.Expression, lambda);
            return source.Provider.CreateQuery<TEntity>(whereExpression);
        }

        private static bool IsValueNumeric(ValueType value)
        {
            return value is byte or short or int or long or sbyte or ushort or uint or ulong or decimal or double or float;
        }

        private static ValueType GetValidValueByType(ValueType value, Type type)
        {
            if (!IsValueNumeric(value) || !IsTypeNumeric(type))
            {
                // no need to check in range if value or type is not numeric
                return value;
            }

            try
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = type.GenericTypeArguments[0];
                }

                dynamic currentValue = value;
                dynamic maxValue = (type.GetField("MaxValue", BindingFlags.Public | BindingFlags.Static)).GetValue(null);
                if (currentValue > maxValue)
                {
                    return (ValueType)maxValue;
                }

                return value;
            }
            catch (Exception)
            {
                return value;
            }
        }

        private static bool CanPerformEqualityOperation(Type valueType, Type propertyType)
        {
            // property of type bool can only be compared [ == | != ] to a value of same type
            // property of type DateTime or DateTimeOffset can only be compared to value of type DateTime or DateTimeOffset
            return !((valueType == typeof(bool) && propertyType != valueType)
                     || (valueType == typeof(bool?) && propertyType != valueType)
                     || (propertyType == typeof(bool) && valueType != propertyType)
                     || (propertyType == typeof(bool?) && valueType != propertyType)
                     || ((valueType == typeof(DateTime) || valueType == typeof(DateTimeOffset)) && !(propertyType == typeof(DateTime) || propertyType == typeof(DateTimeOffset)))
                     || ((valueType == typeof(DateTime?) || valueType == typeof(DateTimeOffset?)) && !(propertyType == typeof(DateTime?) || propertyType == typeof(DateTimeOffset?)))
                     || ((propertyType == typeof(DateTime) || propertyType == typeof(DateTimeOffset)) && !(valueType == typeof(DateTime) || valueType == typeof(DateTimeOffset)))
                     || ((propertyType == typeof(DateTime?) || propertyType == typeof(DateTimeOffset?)) && !(valueType == typeof(DateTime?) || valueType == typeof(DateTimeOffset?))));
        }

        private static bool CanPerformComparisonOperation(Type valueType, Type propertyType)
        {
            // Comparison [ > | >= | < | <= ] is not a valid operation for boolean type
            // property of type DateTime or DateTimeOffset can only be compared to value of type DateTime or DateTimeOffset
            return !((valueType == typeof(bool))
                     || (valueType == typeof(bool?))
                     || (propertyType == typeof(bool))
                     || (propertyType == typeof(bool?))
                     || ((valueType == typeof(DateTime) || valueType == typeof(DateTimeOffset)) && !(propertyType == typeof(DateTime) || propertyType == typeof(DateTimeOffset)))
                     || ((valueType == typeof(DateTime?) || valueType == typeof(DateTimeOffset?)) && !(propertyType == typeof(DateTime?) || propertyType == typeof(DateTimeOffset?)))
                     || ((propertyType == typeof(DateTime) || propertyType == typeof(DateTimeOffset)) && !(valueType == typeof(DateTime) || valueType == typeof(DateTimeOffset)))
                     || ((propertyType == typeof(DateTime?) || propertyType == typeof(DateTimeOffset?)) && !(valueType == typeof(DateTime?) || valueType == typeof(DateTimeOffset?))));
        }

        private static UnaryExpression CreateConstantExpression(ValueType value, Type propertyType)
        {
            // DateTimeOffset does not coerce to DateTime..
            if (value.GetType() == typeof(DateTimeOffset) && (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?)))
            {
                return Expression.Convert(Expression.Constant(((DateTimeOffset)value).UtcDateTime), propertyType);
            }

            return Expression.Convert(Expression.Constant(GetValidValueByType(value, propertyType)), propertyType);
        }
        #endregion

        private static bool IsTypeNumeric(Type type)
        {
            HashSet<Type> numericTypes = new() { typeof(int), typeof(double), typeof(decimal), typeof(long), typeof(short), typeof(sbyte), typeof(byte), typeof(ulong), typeof(ushort), typeof(uint), typeof(float) };
            return numericTypes.Contains(type) || numericTypes.Contains(Nullable.GetUnderlyingType(type));
        }

        private static bool IsValueNumeric(object value)
        {
            return value is byte or short or int or long or sbyte or ushort or uint or ulong or decimal or double or float;
        }

        private static object GetMaxValueIfOutOfRange(object value, Type type)
        {
            if (!IsValueNumeric(value) || !IsTypeNumeric(type))
            {
                // no need to check in range if value or type is not numeric
                return value;
            }

            try
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = type.GenericTypeArguments[0];
                }

                dynamic currentValue = value;
                dynamic maxValue = (type.GetField("MaxValue", BindingFlags.Public | BindingFlags.Static)).GetValue(null);
                if (currentValue > maxValue)
                {
                    return (ValueType)maxValue;
                }

                return value;
            }
            catch (Exception)
            {
                return value;
            }
        }

        private static MemberExpression CreateMemberExpression(Type type, Expression referenceExp, string propertyName)
        {
            string[] properties = propertyName.Split(new[] { '.' }, 2);
            if (properties.Length == 1)
            {
                var property = type.GetProperty(properties[0]);
                if (property == null)
                {
                    return default;
                }

                return Expression.Property(referenceExp, property);
            }
            else
            {
                var referenceProperty = type.GetProperty(properties[0]);
                if (referenceProperty == null)
                {
                    return default;
                }

                var referencePropertyExp = Expression.Property(referenceExp, referenceProperty);
                return CreateMemberExpression(referenceProperty.PropertyType, referencePropertyExp, properties[1]);
            }
        }

        private static Type GetPropertyType(Type type, string propertyName)
        {
            string[] properties = propertyName.Split(new[] { '.' }, 2);
            if (properties.Length == 1)
            {
                var property = type.GetProperty(properties[0]);
                if (property == null)
                {
                    return default;
                }

                return property.PropertyType;
            }
            else
            {
                var referenceProperty = type.GetProperty(properties[0]);
                if (referenceProperty == null)
                {
                    return default;
                }

                return GetPropertyType(referenceProperty.PropertyType, properties[1]);
            }
        }
    }
}
