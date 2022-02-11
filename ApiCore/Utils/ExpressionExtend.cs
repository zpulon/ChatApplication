namespace System.Linq.Expressions
{
    /// <summary>
    /// 合并表达式 And Or  Not扩展
    /// </summary>
    public static class ExpressionExtend
    {
        /// <summary>
        /// 合并表达式 expr1 AND expr2
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        public static Expression<Func<T,bool>> And<T>(this Expression<Func<T,bool>> expr1,Expression<Func<T,bool>> expr2)
        {
            if(expr1 == null)
            {
                return expr2;
            }else if(expr2 == null)
            {
                return expr1;
            }
            ParameterExpression newParameter = Expression.Parameter(typeof(T), "x");
            NewExpressionVisitor newExpressionVisitor = new NewExpressionVisitor(newParameter);
            var left = newExpressionVisitor.Replace(expr1.Body);
            var right = newExpressionVisitor.Replace(expr2.Body);
            var body = Expression.And(left, right);
            return Expression.Lambda<Func<T, bool>>(body, newParameter);
        }
        /// <summary>
        /// 并表达式 expr1 Or expr2
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>

        public static Expression<Func<T,bool>> Or<T>(this  Expression<Func<T,bool>> expr1,Expression<Func<T,bool>> expr2)
        {
            if(expr1 == null)
            {
                return expr2;
            }else if(expr2 == null)
            {
                return expr1;
            }
            ParameterExpression newParameter = Expression.Parameter(typeof(T), "x");
            NewExpressionVisitor newExpressionVisitor = new NewExpressionVisitor(newParameter);
            var left = newExpressionVisitor.Replace(expr1.Body);
            var right = newExpressionVisitor.Replace(expr2.Body);
            var body = Expression.Or(left, right);
            return Expression.Lambda<Func<T, bool>>(body, newParameter);
        }


        /// <summary>
        /// 表达式取反
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static Expression<Func<T,bool>> Not<T>(this Expression<Func<T,bool>> expr)
        {
            if (expr == null) return null;
            var candidateExpr = expr.Parameters[0];
            var body = Expression.Not(expr.Body);
            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

    }
}
