using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace MPP_WeakDelegate
{
    /// <summary>
    /// Class for generate WeakDelegate using generation Dynamic method by ILGenerator
    /// </summary>
    public class WeakDelegateModified
    {        
        #region Fields

        public Type delegateType;
        public WeakReference targetRef;
        private MethodInfo listenerMethodInfo;
        private Delegate weak;
        public Delegate Weak
        {
            get
            {
                return weak;
            }
        }

        public WeakReference TargetRef
        {
            get
            {
                return targetRef;
            }
        }

        #endregion

        public WeakDelegateModified(Delegate listenerHandler)
        {            
            this.listenerMethodInfo = listenerHandler.Method;
            this.targetRef = new WeakReference(listenerHandler.Target);
            delegateType = listenerHandler.GetType();
            initProxyDelegate();                        
        }
        
        private void initProxyDelegate()
        {
            Type[] dynamicMethodArgsTypes = getMethodArgs();            

            DynamicMethod dynamicMethod = new DynamicMethod("DynamicDelegate", 
                listenerMethodInfo.ReturnType, dynamicMethodArgsTypes, listenerMethodInfo.DeclaringType.Module);

            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            dynamicMethod.InitLocals = true;
            Label listenerDelegateIsNotNull = ilGenerator.DefineLabel();            
            LocalBuilder declareLocal = ilGenerator.DeclareLocal(targetRef.Target.GetType());

            ParameterInfo[] parameterInfos = listenerMethodInfo.GetParameters();
            for (Int16 i = 0; i < parameterInfos.Length; i++)
            {
                ParameterInfo parameterInfo = parameterInfos[i];
                if (!parameterInfo.IsDefined(typeof(OutAttribute), true)) continue;
                Type elementType = parameterInfo.ParameterType.GetElementType();
                ilGenerator.Emit(OpCodes.Ldarg, i + 1);
                ilGenerator.Emit(OpCodes.Initobj, elementType);
            }
            // clear out parameters

            ilGenerator.Emit(OpCodes.Ldarg_0);
            var targetRefField = typeof(WeakDelegateModified).GetField("targetRef");
            ilGenerator.Emit(OpCodes.Ldfld, targetRefField);
            ilGenerator.Emit(OpCodes.Callvirt, typeof(WeakReference).GetMethod("get_Target"));
            ilGenerator.Emit(OpCodes.Ldnull);
            ilGenerator.Emit(OpCodes.Ceq);
            ilGenerator.Emit(OpCodes.Brfalse_S, listenerDelegateIsNotNull);
            //if(targetRef.Target == null)

            ilGenerator.Emit(OpCodes.Ret);
            //return
            
            ilGenerator.MarkLabel(listenerDelegateIsNotNull);
            // LABEL listenerDelegateIsNotNull

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldfld, targetRefField);
            for (Int16 i = 1; i < dynamicMethodArgsTypes.Length; i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg_S, i);
            }
            
            ilGenerator.Emit(OpCodes.Callvirt, listenerMethodInfo);
            ilGenerator.Emit(OpCodes.Ret);
            //return target.Invoke(arg1, arg2, .....);       
                                 
            weak = dynamicMethod.CreateDelegate(delegateType, this);                
        }

        private Type[] getMethodArgs()
        {
            ParameterInfo[] methodParams = listenerMethodInfo.GetParameters();
            Type[] resultMethodArgs = new Type[methodParams.Length + 1];            
            resultMethodArgs[0] = typeof(WeakDelegateModified);
            for (int i = 0; i < methodParams.Length; i++)
            {
                resultMethodArgs[i + 1] = methodParams[i].ParameterType;
            }
            return resultMethodArgs; 
        }
    }
}
