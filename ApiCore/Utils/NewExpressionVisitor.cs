namespace System.Linq.Expressions
{
    public class NewExpressionVisitor:ExpressionVisitor
    {
        /// <summary>
        /// 建立新表达式
        /// </summary>
        public ParameterExpression _NewParameter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        public NewExpressionVisitor(ParameterExpression param)
        {
            _NewParameter = param;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>

        public Expression Replace(Expression exp)
        {
            return this.Visit(exp);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return this._NewParameter;
        }
    }
}
