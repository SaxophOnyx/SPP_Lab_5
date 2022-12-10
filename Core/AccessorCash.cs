using Core.Exceptions;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Core
{
    public class AccessorCash
    {
        private ConcurrentDictionary<CompositeKey<Type, string>, Func<object, string, object>> _accessors;


        public AccessorCash()
        {
            _accessors = new();
        }


        public object AddOrUse(object target, string name)
        {
            var targetType = target.GetType();
            var key = new CompositeKey<Type, string>(targetType, name);

            var func = _accessors.GetOrAdd(key, (key) => CreateAccessor(target, name));
            return func.Invoke(target, name);
        }

        private static Func<object, string, object> CreateAccessor(object target, string name)
        {
            Type targetType = target.GetType();

            ParameterExpression paramAsObject = Expression.Parameter(typeof(object), "paramAsObject");
            ParameterExpression nameParam = Expression.Parameter(typeof(string), "nameParam");
            UnaryExpression converted = Expression.Convert(paramAsObject, targetType);
            MemberExpression propertyExpr;

            try
            {
                propertyExpr = Expression.PropertyOrField(converted, name);
            }
            catch (ArgumentException)
            {
                throw new UnknownParamException();
            }

            Expression castedPropertyExpr = Expression.Convert(propertyExpr, typeof(object));

            var lambda = Expression.Lambda<Func<object, string, object>>(castedPropertyExpr, new ParameterExpression[] { paramAsObject, nameParam });
            var accessor = lambda.Compile();

            return accessor;
        }
    }
}
