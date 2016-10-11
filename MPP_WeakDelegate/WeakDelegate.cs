using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace MPP_WeakDelegate
{
    public class WeakDelegate
    {
        private WeakReference weakRef;
        private WeakReference listenerMethodInfoRef;
        private MethodInfo listenerMethodInfo;        
        private Delegate weak;
        public Delegate Weak 
        { 
            get
            {
                return weak;
            }           
        }

        public WeakDelegate(Delegate listenerHandler)
        {
            this.listenerMethodInfoRef = new WeakReference(listenerHandler.Method);
            this.listenerMethodInfo = (MethodInfo)listenerMethodInfoRef.Target;
            this.weakRef = new WeakReference(listenerHandler.Target);
            initProxyDelegate();
        }

        private void initProxyDelegate()
        {
            ConstantExpression targetObject = Expression.Constant(weakRef.Target, weakRef.Target.GetType());            
            ConstantExpression nullObject = Expression.Constant(null);
            Expression isTargetObjectNotNull = Expression.NotEqual(targetObject, nullObject);
            ParameterExpression[] targetMethodArgsExpressions = getTargetMethodArgsExpressions();
            Expression callTargetHandler = Expression.Call(targetObject, listenerMethodInfo, targetMethodArgsExpressions);
            Expression checkTargetObjectNotNullAndCallTargetHandler = Expression.IfThen(isTargetObjectNotNull, callTargetHandler);
            LambdaExpression labmdaExpression = Expression.Lambda(checkTargetObjectNotNullAndCallTargetHandler, targetMethodArgsExpressions);
            weak = labmdaExpression.Compile();
        }

        private ParameterExpression[] getTargetMethodArgsExpressions()
        {
            ParameterInfo[] methodParams = listenerMethodInfo.GetParameters();
            ParameterExpression[] argsExpressions = new ParameterExpression[methodParams.Length];
            for(int i = 0; i < argsExpressions.Length; i++)
            {
                argsExpressions[i] = Expression.Parameter(methodParams[i].ParameterType, methodParams[i].Name);
            }
            return argsExpressions;
        }      
    }
}
