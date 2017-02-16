using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NestExtensions
{
    public static class ElasticPathExtensions
    {
        private static readonly Dictionary<string, string> _ExpressionPathStringCache = new Dictionary<string, string>();

        public static string ToPathString<T>(this Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof (expression));
            }

            string key = $"{typeof (T)}_{expression}";

            if (_ExpressionPathStringCache.ContainsKey(key))
            {
                return _ExpressionPathStringCache[key];
            }

            Expression pathExpression = expression.Body;

            if (pathExpression is UnaryExpression)
            {
                var convertExpression = pathExpression as UnaryExpression;
                pathExpression = convertExpression.Operand;
            }

            var pathString = GetPathString(pathExpression);
            _ExpressionPathStringCache[key] = pathString;

            return pathString;
        }

        private static string GetPathString(Expression pathExpression)
        {
            List<string> members = new List<string>();

            var methodCallExpression = pathExpression as MethodCallExpression;
            var memberExpression = pathExpression as MemberExpression;

            while (memberExpression != null || (methodCallExpression != null && methodCallExpression.Method.Name == "First"))
            {
                if (methodCallExpression != null)
                {
                    var arguementExpression = methodCallExpression.Arguments.First();
                    pathExpression = arguementExpression;
                }
                else
                {
                    var memberName = FirstCharacterToLower(memberExpression.Member.Name);

                    members.Add(memberName);
                    pathExpression = memberExpression.Expression;
                }

                methodCallExpression = pathExpression as MethodCallExpression;
                memberExpression = pathExpression as MemberExpression;
            }

            var pathString = string.Join(".", members.Reverse<string>().ToArray());
            return pathString;
        }

        private static string FirstCharacterToLower(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }
    }
}
