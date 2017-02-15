using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RequestsForRights.Web.Infrastructure.Enums;
using RequestsForRights.Web.Infrastructure.Utilities.EntitySorter.Interfaces;

namespace RequestsForRights.Web.Infrastructure.Utilities.EntitySorter
{
    internal class EntitySorterBuilder<T>
    {
        private readonly Type _keyType;
        private readonly LambdaExpression _keySelector;

        public EntitySorterBuilder(string propertyName)
        {
            var propertyAccessors =
                GetPropertyAccessors(propertyName);

            _keyType = propertyAccessors.Last().ReturnType;

            var builder = CreateLambdaBuilder(_keyType);

            _keySelector =
                builder.BuildLambda(propertyAccessors);
        }

        private interface ILambdaBuilder
        {
            LambdaExpression BuildLambda(
                IEnumerable<MethodInfo> propertyAccessors);
        }

        public SortDirection Direction { get; set; }

        public IEntitySorter<T> BuildOrderByEntitySorter()
        {
            var typeArgs = new[] { typeof(T), _keyType };

            var sortType =
                typeof(OrderBySorter<,>).MakeGenericType(typeArgs);

            return (IEntitySorter<T>)Activator.CreateInstance(sortType,
                _keySelector, Direction);
        }

        public IEntitySorter<T> BuildThenByEntitySorter(
            IEntitySorter<T> baseSorter)
        {
            var typeArgs = new[] { typeof(T), _keyType };

            var sortType =
                typeof(ThenBySorter<,>).MakeGenericType(typeArgs);

            return (IEntitySorter<T>)Activator.CreateInstance(sortType,
                baseSorter, _keySelector, Direction);
        }

        private static ILambdaBuilder CreateLambdaBuilder(Type keyType)
        {
            var typeArgs = new[] { typeof(T), keyType };

            var builderType =
                typeof(LambdaBuilder<>).MakeGenericType(typeArgs);

            return (ILambdaBuilder)Activator.CreateInstance(builderType);
        }

        private static List<MethodInfo> GetPropertyAccessors(
            string propertyName)
        {
            try
            {
                return GetPropertyAccessorsFromChain(propertyName);
            }
            catch (InvalidOperationException ex)
            {
                var message = propertyName +
                    " could not be parsed. " + ex.Message;
                throw new ArgumentException(message, "propertyName");
            }
        }

        private static List<MethodInfo> GetPropertyAccessorsFromChain(
            string propertyNameChain)
        {
            var propertyAccessors = new List<MethodInfo>();

            var declaringType = typeof(T);

            foreach (var name in propertyNameChain.Split('.'))
            {
                var accessor = GetPropertyAccessor(declaringType, name);

                propertyAccessors.Add(accessor);

                declaringType = accessor.ReturnType;
            }

            return propertyAccessors;
        }

        private static MethodInfo GetPropertyAccessor(Type declaringType,
            string propertyName)
        {
            var prop = GetPropertyByName(declaringType, propertyName);

            return GetPropertyGetter(prop);
        }

        private static PropertyInfo GetPropertyByName(Type declaringType,
            string propertyName)
        {
            var flags = BindingFlags.IgnoreCase |
                BindingFlags.Instance | BindingFlags.Public;

            var prop = declaringType.GetProperty(propertyName, flags);

            if (prop == null)
            {
                var exceptionMessage = string.Format(
                    "{0} does not contain a property named '{1}'.",
                    declaringType, propertyName);

                throw new InvalidOperationException(exceptionMessage);
            }

            return prop;
        }

        private static MethodInfo GetPropertyGetter(PropertyInfo property)
        {
            var propertyAccessor = property.GetGetMethod();

            if (propertyAccessor != null) return propertyAccessor;
            var exceptionMessage = string.Format(
                "The property '{0}' does not contain a getter.",
                property.Name);

            throw new InvalidOperationException(exceptionMessage);
        }

        private sealed class LambdaBuilder<TKey> : ILambdaBuilder
        {
            public LambdaExpression BuildLambda(
                IEnumerable<MethodInfo> propertyAccessors)
            {
                var parameterExpression =
                    Expression.Parameter(typeof(T), "entity");

                var propertyExpression = BuildPropertyExpression(
                    propertyAccessors, parameterExpression);

                return Expression.Lambda<Func<T, TKey>>(
                    propertyExpression, parameterExpression);
            }

            private static Expression BuildPropertyExpression(
                IEnumerable<MethodInfo> propertyAccessors,
                Expression parameterExpression)
            {
                Expression propertyExpression = null;

                foreach (var propertyAccessor in propertyAccessors)
                {
                    var innerExpression =
                        propertyExpression ?? parameterExpression;

                    propertyExpression = Expression.Property(
                        innerExpression, propertyAccessor);
                }

                return propertyExpression;
            }
        }
    }
}