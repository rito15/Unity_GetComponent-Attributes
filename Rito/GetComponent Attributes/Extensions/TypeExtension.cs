using System;
using System.Collections.Generic;

namespace Rito.Attributes
{

    // 2020. 03. 23. 작성
    public static class TypeExtension
    {
        /// <summary>
        /// <para/> 타겟 타입의 자식이거나 동일한지 검사
        /// </summary>
        public static bool Ex_IsChildOrEqualsTo(this Type @this, in Type target)
        {
            return @this.IsSubclassOf(target) || @this.IsEquivalentTo(target);
        }

        /// <summary>
        /// <para/> 타겟 배열 타입의 자식이거나 동일한지 검사
        /// <para/> * target은 배열 또는 배열의 엘리먼트 타입으로 지정 가능
        /// </summary>
        public static bool Ex_IsArrayAndChildOf(this Type @this, in Type target)
        {
            // 1. 배열 타입이 아니면 false 리턴
            if (@this.HasElementType == false) return false;

            // 2. 지정한 타겟 타입과 동일하거나 자식인지 검사
            // 2-1. 타겟을 배열 타입으로 넣은 경우
            if (target.HasElementType)
            {
                if (@this.IsEquivalentTo(target))
                    return true;

                if (@this.GetElementType().IsSubclassOf(target.GetElementType()))
                    return true;
            }
            // 2-2. 타겟을 엘리먼트 타입으로 넣은 경우
            else
            {
                if (@this.GetElementType().IsEquivalentTo(target))
                    return true;

                if (@this.GetElementType().IsSubclassOf(target))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// <para/> 이 타입이 List&lt;타입&gt; 꼴인지 검사
        /// </summary>
        public static bool Ex_IsListType(this Type @this)
        {
            return @this.IsGenericType && @this.GetGenericTypeDefinition() == typeof(List<>);
        }

        /// <summary>
        /// <para/> 이 타입이 List&lt;타입&gt; 꼴인지 검사
        /// <para/> + 지정한 제네릭 타입이 target 타입과 같거나, target의 자식인지 검사
        /// </summary>
        public static bool Ex_IsListAndChildOf(this Type @this, in Type target)
        {
            if (@this.IsGenericType == false) return false;
            if (@this.GetGenericTypeDefinition() != typeof(List<>)) return false;

            if (@this.GetGenericArguments()[0].IsEquivalentTo(target)) return true;
            if (@this.GetGenericArguments()[0].IsSubclassOf(target)) return true;

            return false;
        }
    }
}