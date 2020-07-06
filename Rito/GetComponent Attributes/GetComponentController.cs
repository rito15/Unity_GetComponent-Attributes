using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;

namespace Rito.Attributes
{
    #region Development Log
/*
     2020. 03. 18. 작성
     2020. 03. 20. 필드 - Array 대상 구현 및 테스트 완료
     2020. 03. 20. 필드 - List 대상 구현 및 테스트 완료
     2020. 03. 21. 필드 - GetOrAddComponent 구현 및 테스트 완료
     2020. 03. 21. 프로퍼티 대상으로 구현 완료
     2020. 03. 23. 리팩토링 : 필드, 프로퍼티를 MemberInfo로 합치고 구현 및 테스트 완료
     2020. 03. 30. GetComponentInChild 구현 및 테스트 완료
     2020. 04. 06. 싱글톤 오브젝트 자동 생성 구현 완료
     2020. 04. 07. 싱글톤 -> 로드 시 자동 호출되는 정적 메소드로 변경(OnEnable() ~ Start() 사이 호출)
     2020. 04. 08. 씬 재시작 시에도 기능이 동작하도록 추가
     2020. 04. 10. 필드 탐색 범위 NonPublic 추가 : private, protected 필드에도 모두 적용 가능
     2020. 04. 12. 에러 타입 3종류로 세분화(컴포넌트가 아닌 경우, 배열이나 리스트인 경우/아닌 경우)
*/
    #endregion // ==========================================================

    public static class GetComponentController
    {
        /// <summary> OnEnable() ~ Start() 사이에 실행 </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void OnRuntimeLoad()
        {
            RunAttributeAction();

            // 씬 재시작 시 호출하도록 추가
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (a, b) => RunAttributeAction();
        }
        
        private static void RunAttributeAction()
        {
            // 모든 활성 컴포넌트 찾기
            var allComponents = UnityEngine.Object.FindObjectsOfType<Component>();

            foreach (var component in allComponents)
            {
                RunGetComponentActions(component);
            }
        }

        /// <summary> 원하는 타이밍에 특정 컴포넌트 내에서 실행 </summary>
        public static void Run(in Component com)
        {
            RunGetComponentActions(com);
        }

        /// <summary> 필드, 프로퍼티 대상으로 GetComponent, GetOrAddCOmponent 수행 </summary>
        private static void RunGetComponentActions(Component component)
        {
            Type thisComponentType = component.GetType();
            Type thisComponentBaseType = thisComponentType.BaseType;

            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            BindingFlags nonPublicFlags = BindingFlags.Instance | BindingFlags.NonPublic;

            // 모든 필드, 프로퍼티 찾기
            var fInfos = thisComponentType.GetFields(flags).ToList();
            var pInfos = thisComponentType.GetProperties(flags).ToList();

            // 부모 타입이 존재하는 경우 : private 필드, 프로퍼티 추가
            while (thisComponentBaseType != null
                && !thisComponentBaseType.Equals(typeof(MonoBehaviour))
                && !thisComponentBaseType.Equals(typeof(Behaviour))
                && !thisComponentBaseType.Equals(typeof(Component))
                )
            {
                var baseFInfos = thisComponentBaseType.GetFields(nonPublicFlags);
                foreach (var fInfo in baseFInfos)
                {
                    if (fInfo.IsPrivate)
                        fInfos.Add(fInfo);
                }

                var basePInfos = thisComponentBaseType.GetProperties(nonPublicFlags);
                foreach (var pInfo in basePInfos)
                {
                    if (pInfo.GetMethod != null && pInfo.GetMethod.IsPrivate &&
                        pInfo.SetMethod != null && pInfo.SetMethod.IsPrivate)
                        pInfos.Add(pInfo);
                }

                thisComponentBaseType = thisComponentBaseType.BaseType;
            }

            // 솎아내기
            var targetFInfos =
                from fInfo in fInfos
                where fInfo.GetCustomAttribute<GetComponentBaseAttribute>() != null &&
                      (fInfo.GetCustomAttribute<GetComponentBaseAttribute>().AllowOverwrite == true ||
                       fInfo.GetValue(component) == null || (fInfo.GetValue(component) as Component) == null
                      )
                select fInfo;

            var targetPInfos =
                from pInfo in pInfos
                where pInfo.SetMethod != null && pInfo.GetMethod != null && 
                      pInfo.GetCustomAttribute<GetComponentBaseAttribute>() != null &&
                      (pInfo.GetCustomAttribute<GetComponentBaseAttribute>().AllowOverwrite == true ||
                       pInfo.GetValue(component) == null || (pInfo.GetValue(component) as Component) == null
                      )
                select pInfo;

            // 멤버 리스트로 병합
            List<MemberInfo> memberInfos = targetFInfos.ToList<MemberInfo>();
            memberInfos.AddRange(targetPInfos);

            // GetComponent 종류별로 수행
            foreach (var memberInfo in memberInfos)
            {
                var memberType = memberInfo.Ex_GetMemberType();
                Type componentType = typeof(Component);

                // 컴포넌트 타입이 아닌 멤버에 애트리뷰트 사용한 경우 예외 처리
                if (!
                    (memberType.Ex_IsChildOrEqualsTo(componentType) ||
                     memberType.Ex_IsArrayAndChildOf(componentType) ||
                     memberType.Ex_IsListAndChildOf(componentType))
                  )
                {
                    ErrorLog(memberInfo, component, memberType, "Target Member Must Be Component-Inheriting Type.");
                    continue;
                }

                //


                var customAttribute = memberInfo.GetCustomAttribute<GetComponentBaseAttribute>();

                // 5-1. GetComponent, GetOrAddComponent - Element
                if (memberType.Ex_IsChildOrEqualsTo(typeof(Component)))
                {
                    // GetComponent, GetOrAdd
                    GetComponentToElementMember(memberInfo, memberType, component);
                }
                // Array or List
                else
                {
                    string methodName = "";

                    switch (customAttribute)
                    {
                        case GetComponentsAttribute getComs:
                            methodName = "GetComponents";
                            break;

                        case GetComponentsInParentAttribute getComsParent:
                            methodName = "GetComponentsInParent";
                            break;

                        case GetComponentsInChildrenAttribute getComsChildren:
                            methodName = "GetComponentsInChildren";
                            break;

                        // Only
                        case GetComponentsInChildrenOnlyAttribute childrenOnly:
                            methodName = "GetComponentsInChildrenOnly";
                            break;

                        case GetComponentsInParentOnlyAttribute parentOnly:
                            methodName = "GetComponentsInParentOnly";
                            break;

                        // Find
                        case FindAllAttribute fa:
                            methodName = "FindAll";
                            break;
                    }

                    // 5-2. GetComponents - Array
                    if (memberType.Ex_IsArrayAndChildOf(typeof(Component)))
                    {
                        GetComponentToArrayOrList(memberInfo, memberType, component, methodName, true);
                    }

                    // 5-3. GetComponents - List<T>
                    else if (memberType.Ex_IsListAndChildOf(typeof(Component)))
                    {
                        GetComponentToArrayOrList(memberInfo, memberType, component, methodName, false);
                    }

                    // Error
                    else
                    {
                        ErrorLog(memberInfo, component, memberType);
                    }
                } 
            } // foreach
        } // Method

        #region Small Methods

        /// <summary> 배열이나 제네릭이 아닌 보통 타입의 필드, 프로퍼티에 대해
        /// <para/> GetComponent 또는 GetOrAddComponent 수행</summary>
        private static void GetComponentToElementMember(in MemberInfo memberInfo, in Type memberType, in Component component)
        {
            var customAttribute = memberInfo.GetCustomAttribute<GetComponentBaseAttribute>();
            GameObject go = component.gameObject;

            string methodName = "";
            Type[] methodParams = new Type[0];      // GetOrAdd에서 사용 : 메소드 파라미터
            object[] realParams = new object[0];    // GetOrAdd에서 사용 : 실질 파라미터
            string targetString = "";            // GetOrAdd 애트리뷰트에서 가져오는 타겟 부모 또는 자식 이름 스트링

            // Extension Method
            string paramString = "";
            MethodInfo method;
            object returnedComponent;
            string extMethodName;

            switch (customAttribute)
            {
                case GetComponentAttribute g:
                    memberInfo.Ex_SetValue(component, go.GetComponent(memberType));
                    break;

                case GetComponentInParentAttribute g:
                    memberInfo.Ex_SetValue(component, go.GetComponentInParent(memberType));
                    break;

                case GetComponentInChildrenAttribute g:
                    memberInfo.Ex_SetValue(component, go.GetComponentInChildren(memberType));
                    break;


                case GetOrAddComponentAttribute goa:
                    methodName = "GetOrAddComponent";
                    methodParams = new Type[] { typeof(GameObject), typeof(Type) };
                    realParams = new object[] { go, memberType };
                    break;

                case GetOrAddComponentInChildrenAttribute goa:
                    methodName = "GetOrAddComponentInChildren";
                    targetString = (customAttribute as GetOrAddComponentInChildrenAttribute).ChildObjectName;
                    methodParams = new Type[] { typeof(GameObject), typeof(Type), typeof(string) };
                    realParams = new object[] { go, memberType, targetString };
                    break;

                case GetOrAddComponentInParentAttribute goa:
                    methodName = "GetOrAddComponentInParent";
                    targetString = (customAttribute as GetOrAddComponentInParentAttribute).ParentObjectName;
                    methodParams = new Type[] { typeof(GameObject), typeof(Type), typeof(string) };
                    realParams = new object[] { go, memberType, targetString };
                    break;


                // 확장 메소드 - 추가 파라미터가 존재하여 as를 써야 함 - 일일이 다 작성
                case GetComponentInChildAttribute gcic:
                    paramString = (customAttribute as GetComponentInChildAttribute).ChildObjectName;
                    method = typeof(GetComponentExtension)
                        .GetMethod("GetComponentInChild", new Type[] { typeof(GameObject), typeof(string) })
                        .MakeGenericMethod(memberType);
                    returnedComponent = method.Invoke(go, new object[] { go, paramString });

                    memberInfo.Ex_SetValue(component, returnedComponent);
                    break;

                case GetOrAddComponentInChildAttribute goaic:
                    paramString = (customAttribute as GetOrAddComponentInChildAttribute).ChildObjectName;
                    method = typeof(GetComponentExtension)
                        .GetMethod("GetOrAddComponentInChild", new Type[] { typeof(GameObject), typeof(string) })
                        .MakeGenericMethod(memberType);
                    returnedComponent = method.Invoke(go, new object[] { go, paramString });

                    memberInfo.Ex_SetValue(component, returnedComponent);
                    break;


                // 확장 메소드 - 추가 파라미터가 없어서 as를 안써도 되는 경우들
                case GetComponentInChildrenOnlyAttribute gco:
                case GetComponentInParentOnlyAttribute gpo:
                    extMethodName = customAttribute.GetType().ToString().Replace("Attribute", "");
                    int dotIndex = extMethodName.LastIndexOf('.');

                    if(dotIndex > -1)
                        extMethodName = extMethodName.Substring(dotIndex + 1);

                    method = typeof(GetComponentExtension)
                        .GetMethod(extMethodName, new Type[] { typeof(GameObject) })
                        .MakeGenericMethod(memberType);
                    returnedComponent = method.Invoke(go, new object[] { go });

                    memberInfo.Ex_SetValue(component, returnedComponent);
                    break;


                // 정적 메소드 - Find
                case FindAttribute fih:
                    method = typeof(FindHelper)
                        .GetMethod("Find")
                        .MakeGenericMethod(memberType);
                    returnedComponent = method.Invoke(null, null);

                    memberInfo.Ex_SetValue(component, returnedComponent);
                    break;

                case FindOrAddAttribute foa:
                    paramString = (customAttribute as FindOrAddAttribute).NewGoName;

                    method = typeof(FindHelper)
                        .GetMethod("FindOrAdd", new Type[] { typeof(string) })
                        .MakeGenericMethod(memberType);
                    returnedComponent = method.Invoke(null, new object[] { paramString });

                    memberInfo.Ex_SetValue(component, returnedComponent);
                    break;

                case FindByNameAttribute fbn:
                    paramString = (customAttribute as FindByNameAttribute).TargetGoName;

                    method = typeof(FindHelper)
                        .GetMethod("FindByName", new Type[] { typeof(string) })
                        .MakeGenericMethod(memberType);
                    returnedComponent = method.Invoke(null, new object[] { paramString });

                    memberInfo.Ex_SetValue(component, returnedComponent);
                    break;

                case FindByNameOrAddAttribute fbnoa:
                    paramString = (customAttribute as FindByNameOrAddAttribute).TargetGoName;

                    method = typeof(FindHelper)
                        .GetMethod("FindByNameOrAdd", new Type[] { typeof(string) })
                        .MakeGenericMethod(memberType);
                    returnedComponent = method.Invoke(null, new object[] { paramString });

                    memberInfo.Ex_SetValue(component, returnedComponent);
                    break;

                default:
                    ErrorLog(memberInfo, component, memberType, "Target Member Must Be Array or List.");
                    break;
            }

            // Extensions - Get Or Add, Only
            if (methodName.Length > 0)
            {
                var extMethod = typeof(GetComponentExtension).GetMethod(methodName, methodParams);
                returnedComponent = extMethod.Invoke(component.gameObject, realParams);
                memberInfo.Ex_SetValue(component, returnedComponent);
            }
        }

        // isArray : true(Array), false(List)
        /// <summary> 배열 타입 필드, 프로퍼티에 대해 GetComponent 수행</summary>
        private static void GetComponentToArrayOrList(in MemberInfo memberInfo, in Type memberType, in Component component,
            in string methodName, in bool isArray)
        {
            if (methodName.Length == 0)
            {
                ErrorLog(memberInfo, component, memberType, "Target Member Must Not Be Array or List.");
                return;
            }

            Type elementType = isArray ? memberType.GetElementType() : memberType.GetGenericArguments()[0];

            GameObject go = component.gameObject;
            object caller = go;
            object[] realParams = null;

            // 1. GetCompo~<타입> 메소드를 타입 지정하여 가져오기
            MethodInfo getComsMethod = typeof(GameObject).GetMethod(methodName, new Type[0]);

            // 2. 못찾으면 확장 메소드에서 탐색
            if (getComsMethod == null)
            {
                getComsMethod = typeof(GetComponentExtension).GetMethod(methodName, new Type[] { typeof(GameObject) });
                realParams = new object[] { go };
            }

            // 3. 또 못찾으면 FIndHelper에서 탐색
            if (getComsMethod == null)
            {
                getComsMethod = typeof(FindHelper).GetMethod(methodName, new Type[0]);
                caller = null;
                realParams = null;
            }

            MethodInfo genericMethod = getComsMethod.MakeGenericMethod(elementType);

            // 메소드 실행
            var targetComponentsObj = genericMethod.Invoke(caller, realParams);

            // 배열로 변환
            Array targetComponentsArr = targetComponentsObj as Array;

            // ERROR
            if (targetComponentsArr == null)
            {
                ErrorLog(memberInfo, component, memberType);
                return;
            }
            // 타겟 필드에 할당
            else
            {
                // 1. 배열인 경우
                if (isArray)
                    memberInfo.Ex_SetValue(component, targetComponentsArr);
                // 2. 리스트인 경우
                else
                {
                    // 새로운 리스트를 인스턴스화하여 생성
                    object newList = Activator.CreateInstance(memberType);

                    // Add 메소드 가져오기
                    MethodInfo AddToListMethod = memberType.GetMethod("Add");

                    // 리스트에 배열 요소들 초기화
                    foreach (var item in targetComponentsArr)
                    {
                        AddToListMethod.Invoke(newList, new object[] { item });
                    }

                    // 타겟 필드에 리스트 참조 할당
                    memberInfo.Ex_SetValue(component, newList);
                }
            }
        }

        /// <summary> 잘못된 애트리뷰트 사용에 대해 콘솔 경고 메시지 출력 </summary>
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void ErrorLog(in MemberInfo memberInfo, in Component component, in Type memberType, in string msg = "")
        {
            Debug.LogWarning($"[Rito.GetComponentAttributes] Wrong Attribute Usage :\n{(msg.Length > 0 ? msg + "\n" : "")}\n" +
                        $"GameObject : {component.gameObject.name}, \nComponent : {component.GetType()}, \n" +
                        $"Member Name : {memberInfo.Name}, \nMember Type : {memberType}\n\n\n");
        }

        #endregion // ==========================================================
    } // class End

    /// <summary> Acronate(GetComponentController) </summary>
    public static class GCA
    {
        public static void Run(in Component com) => GetComponentController.Run(com);
    }
}