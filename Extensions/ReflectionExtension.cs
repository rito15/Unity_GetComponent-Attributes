using System.Collections.Generic;
using System.Reflection;
using System;

// 2020. 03. 23. 작성
public static class ReflectionExtension
{
    /// <summary>
    /// <para/> FieldInfo 또는 PropertyInfo의 타입 가져오기
    /// <para/> 
    /// </summary>
    public static Type Ex_GetMemberType(this MemberInfo @this)
    {
        if (@this.MemberType.Equals(MemberTypes.Field))
            return (@this as FieldInfo)?.FieldType;

        else if (@this.MemberType.Equals(MemberTypes.Property))
            return (@this as PropertyInfo)?.PropertyType;

        else
            return null;
    }

    /// <summary>
    /// <para/> 필드 또는 프로퍼티의 값 가져오기
    /// <para/> 
    /// </summary>
    public static object Ex_GetValue(this MemberInfo @this, in object target)
    {
        if (@this.MemberType.Equals(MemberTypes.Field))
            return (@this as FieldInfo)?.GetValue(target);

        else if (@this.MemberType.Equals(MemberTypes.Property))
            return (@this as PropertyInfo)?.GetValue(target);

        else
            return null;
    }

    /// <summary>
    /// <para/> 필드 또는 프로퍼티에 값 할당하기
    /// <para/> 
    /// </summary>
    public static void Ex_SetValue(this MemberInfo @this, in object target, in object value)
    {
        if (@this.MemberType.Equals(MemberTypes.Field))
            (@this as FieldInfo)?.SetValue(target, value);

        else if (@this.MemberType.Equals(MemberTypes.Property))
        {
            PropertyInfo pInfo = @this as PropertyInfo;

            if(pInfo.SetMethod != null)
                pInfo?.SetValue(target, value);
        }
    }
}
