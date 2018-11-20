using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Proposing.API.Domain.Core
{
    public static class ExpressionUtils
    {
        public static PropertyInfo GetProperty<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            var member = GetMemberExpression(expression).Member;
            var property = member as PropertyInfo;
            if (property == null)
            {
                throw new InvalidOperationException(string.Format("Member with Name '{0}' is not a property.", member.Name));
            }
            return property;
        }

        private static MemberExpression GetMemberExpression<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            MemberExpression memberExpression = null;
            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var body = (UnaryExpression)expression.Body;
                memberExpression = body.Operand as MemberExpression;
            }
            else if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (memberExpression == null)
            {
                throw new ArgumentException("Not a member access", nameof(expression));
            }

            return memberExpression;
        }

        public static Action<TEntity, TProperty> ToSetter<TEntity, TProperty>(this Expression<Func<TEntity, TProperty>> propertyExpression)
        {
            PropertyInfo propertyInfo = ExpressionUtils.GetProperty(propertyExpression);

            ParameterExpression instance = Expression.Parameter(typeof(TEntity), "instance");
            ParameterExpression parameter = Expression.Parameter(typeof(TProperty), "param");

            var body = Expression.Call(instance, propertyInfo.GetSetMethod(true), Expression.Convert(parameter, propertyInfo.PropertyType));

            return Expression.Lambda<Action<TEntity, TProperty>>(body, instance, parameter).Compile();
        }

        public static Func<TEntity, TProperty> ToGetter<TEntity, TProperty>(this Expression<Func<TEntity, TProperty>> propertyExpression)
        {
            PropertyInfo propertyInfo = ExpressionUtils.GetProperty(propertyExpression);

            ParameterExpression instance = Expression.Parameter(typeof(TEntity), "instance");

            var body = Expression.Call(instance, propertyInfo.GetGetMethod(true));

            return Expression.Lambda<Func<TEntity, TProperty>>(body, instance).Compile();
        }

        public static Func<TEntity> CreateDefaultConstructor<TEntity>()
        {
            var body = Expression.New(typeof(TEntity));
            var lambda = Expression.Lambda<Func<TEntity>>(body);

            return lambda.Compile();
        }

        public static Func<TEntity> CreateDefaultConstructor<TEntity>(Type type)
        {
            var body = Expression.New(type);
            var lambda = Expression.Lambda<Func<TEntity>>(body);

            return lambda.Compile();
        }
    }
}
