using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;

namespace Rito.Attributes
{
    // 2020. 03. 18. 작성
    // 2020. 03. 20. 필드 - Array 대상 구현 및 테스트 완료
    // 2020. 03. 20. 필드 - List 대상 구현 및 테스트 완료
    // 2020. 03. 21. 필드 - GetOrAddComponent 구현 및 테스트 완료
    // 2020. 03. 21. 프로퍼티 대상으로 구현 완료
    // 2020. 03. 23. 리팩토링 : 필드, 프로퍼티를 MemberInfo로 합치고 구현 및 테스트 완료
    // 2020. 03. 30. GetComponentInAChild 구현 및 테스트 완료
    // 2020. 04. 06. 싱글톤 오브젝트 자동 생성 구현 완료
    // 2020. 04. 07. 싱글톤 -> 로드 시 자동 호출되는 정적 메소드로 변경(OnEnable() ~ Start() 사이 호출)
    // 2020. 04. 08. 씬 재시작 시에도 기능이 동작하도록 추가
    // 2020. 04. 10. 필드 탐색 범위 NonPublic 추가 : SerializeField를 함께 사용한 private, protected 필드에 적용 가능

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

        /// <summary> 필드, 프로퍼티 대상으로 GetComponent, GetOrAddCOmponent 수행 </summary>
        private static void RunGetComponentActions(Component component)
        {
            // 1. 모든 필드 찾기
            var fInfos = component.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            // 2. Component의 자식 타입인 참조형 필드만 골라내기 (is Component)
            // 3. GetComponent류 애트리뷰트가 있는 필드만 골라내기
            // 4. 입력된 이벤트 타이밍이 일치하는 필드만 골라내기
            var targetFInfos =
                from fInfo in fInfos
                where fInfo.GetCustomAttribute<GetComponentBaseAttribute>() != null &&
                      (fInfo.GetCustomAttribute<GetComponentBaseAttribute>().AllowOverwrite == true ||
                       fInfo.GetValue(component) == null || (fInfo.GetValue(component) as Component) == null
                      )
                      &&
                      (fInfo.FieldType.Ex_IsChildOrEqualsTo(typeof(Component)) ||
                       fInfo.FieldType.Ex_IsArrayAndChildOf(typeof(Component)) ||
                       (fInfo.FieldType.Ex_IsListType())
                      )
                select fInfo;

            // 프로퍼티에 똑같이 수행
            var pInfos = component.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            var targetPInfos =
                from pInfo in pInfos
                where pInfo.SetMethod != null && pInfo.GetMethod != null && 
                      pInfo.GetCustomAttribute<GetComponentBaseAttribute>() != null &&
                      (pInfo.GetCustomAttribute<GetComponentBaseAttribute>().AllowOverwrite == true ||
                       pInfo.GetValue(component) == null || (pInfo.GetValue(component) as Component) == null
                      )
                      &&
                      (pInfo.PropertyType.Ex_IsChildOrEqualsTo(typeof(Component)) ||
                       pInfo.PropertyType.Ex_IsArrayAndChildOf(typeof(Component)) ||
                       (pInfo.PropertyType.Ex_IsListType())
                      )
                select pInfo;

            // 멤버 리스트로 병합
            List<MemberInfo> memberInfos = targetFInfos.ToList<MemberInfo>();
            memberInfos.AddRange(targetPInfos.ToList<MemberInfo>());

            // 5. GetComponent 종류별로 해주기
            foreach (var memberInfo in memberInfos)
            {
                var customAttribute = memberInfo.GetCustomAttribute<GetComponentBaseAttribute>();
                var memberType = memberInfo.Ex_GetMemberType();

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
                    }

                    // 5-2. GetComponents - Array
                    if (memberType.Ex_IsArrayAndChildOf(typeof(Component)))
                    {
                        GetComponentToArrayMember(memberInfo, memberType, component, methodName);
                    }

                    // 5-3. GetComponents - List<T>
                    else if (memberType.Ex_IsListAndChildOf(typeof(Component)))
                    {
                        GetComponentToListMember(memberInfo, memberType, component, methodName);
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
            string goaTargetString = "";            // GetOrAdd 애트리뷰트에서 가져오는 타겟 부모 또는 자식 이름 스트링

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


                case GetComponentInAChildAttribute gcic:
                    var childName = (customAttribute as GetComponentInAChildAttribute).ChildObjectName;
                    var method = typeof(GetComponentExtension)
                        .GetMethod("GetComponentInAChild", new Type[] { typeof(GameObject), typeof(string) });
                    var gMethod = method.MakeGenericMethod(memberType);
                    var returnedComponent = gMethod.Invoke(go, new object[] { go, childName });

                    memberInfo.Ex_SetValue(component, returnedComponent);
                    break;

                case GetOrAddComponentInAChildAttribute goaic:
                    var childName_ = (customAttribute as GetOrAddComponentInAChildAttribute).ChildObjectName;
                    var method_ = typeof(GetComponentExtension)
                        .GetMethod("GetOrAddComponentInAChild", new Type[] { typeof(GameObject), typeof(string) });
                    var gMethod_ = method_.MakeGenericMethod(memberType);
                    var returnedComponent_ = gMethod_.Invoke(go, new object[] { go, childName_ });

                    memberInfo.Ex_SetValue(component, returnedComponent_);
                    break;


                case GetOrAddComponentAttribute goa:
                    methodName = "GetOrAddComponent";
                    methodParams = new Type[] { typeof(GameObject), typeof(Type) };
                    realParams = new object[] { go, memberType };
                    break;

                case GetOrAddComponentInChildrenAttribute goa:
                    methodName = "GetOrAddComponentInChildren";
                    goaTargetString = (customAttribute as GetOrAddComponentInChildrenAttribute).ChildObjectName;
                    methodParams = new Type[] { typeof(GameObject), typeof(Type), typeof(string) };
                    realParams = new object[] { go, memberType, goaTargetString };
                    break;

                case GetOrAddComponentInParentAttribute goa:
                    methodName = "GetOrAddComponentInParent";
                    goaTargetString = (customAttribute as GetOrAddComponentInParentAttribute).ParentObjectName;
                    methodParams = new Type[] { typeof(GameObject), typeof(Type), typeof(string) };
                    realParams = new object[] { go, memberType, goaTargetString };
                    break;
            }

            // Get Or Add
            if (methodName.Length > 0)
            {
                var getOrAddMethod = typeof(GetComponentExtension).GetMethod(methodName, methodParams);
                var returnedComponent = getOrAddMethod.Invoke(component.gameObject, realParams);
                memberInfo.Ex_SetValue(component, returnedComponent);
            }
        }

        /// <summary> 배열 타입 필드, 프로퍼티에 대해 GetComponent 수행</summary>
        private static void GetComponentToArrayMember(in MemberInfo memberInfo, in Type memberType, in Component component,
            in string methodName)
        {
            Type elementType = memberType.GetElementType();
            GameObject go = component.gameObject;

            if (methodName.Length > 0)
            {
                // GetCompo~<타입> 메소드를 타입 지정하여 가져오기
                MethodInfo getComsMethod = typeof(GameObject).GetMethod(methodName, new Type[0])
                            .MakeGenericMethod(elementType);

                // 게임오브젝트로부터 해당 타입의 컴포넌트들 가져오기
                var targetComponentsObj = getComsMethod.Invoke(go, null);

                // 배열로 변환
                Array targetComponentsArr = targetComponentsObj as Array;

                // 타겟 필드에 할당
                if (targetComponentsArr != null)
                    memberInfo.Ex_SetValue(component, targetComponentsArr);
            }
        }

        /// <summary> 리스트 타입 필드, 프로퍼티에 대해 GetComponent 수행</summary>
        private static void GetComponentToListMember(in MemberInfo memberInfo, in Type memberType, in Component component,
            in string methodName)
        {
            Type genericType = memberType.GetGenericArguments()[0];
            GameObject go = component.gameObject;

            if (methodName.Length > 0)
            {
                // GetCompo~<타입> 메소드를 타입 지정하여 가져오기
                MethodInfo getComsMethod = typeof(GameObject).GetMethod(methodName, new Type[0])
                            .MakeGenericMethod(genericType);

                // 게임오브젝트로부터 해당 타입의 컴포넌트들 가져오기
                var targetComponentsObj = getComsMethod.Invoke(go, null);

                // 배열로 변환
                Array targetComponentsArr = targetComponentsObj as Array;

                if (targetComponentsArr != null)
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

        #endregion // ==========================================================
    }
}