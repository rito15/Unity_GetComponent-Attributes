/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;

namespace Rito
{
    // 2020. 03. 18. 작성
    // 2020. 03. 20. 필드 - Array 대상 구현 및 테스트 완료
    // 2020. 03. 20. 필드 - List 대상 구현 및 테스트 완료
    // 2020. 03. 21. 필드 - GetOrAddComponent 구현 및 테스트 완료
    // 2020. 03. 21. 프로퍼티 대상으로 구현 완료
    // 2020. 03. 23. 리팩토링 : 필드, 프로퍼티를 MemberInfo로 합치고 구현 및 테스트 완료

    // TODO : 모든 테스트케이스 검증

    // TODO : 에디터를 실행하면 알아서 이 싱글톤 오브젝트가 만들어지게 하기 ★

    public partial class GetComponentController : MonoBehaviour
    {
        #region Singleton - Public

        /// <summary> 싱글톤 인스턴스 Getter </summary>
        public static GetComponentController Instance
        {
            get
            {
                if (instance == null)    // 체크 1 : 인스턴스가 없는 경우
                    CheckExsistence();

                return instance;
            }
        }

        /// <summary> 싱글톤 인스턴스의 또다른 이름 </summary>
        public static GetComponentController Sin => Instance;
        /// <summary> 싱글톤 인스턴스의 또다른 이름 </summary>
        public static GetComponentController Ins => Instance;
        /// <summary> 싱글톤 인스턴스의 또다른 이름 </summary>
        public static GetComponentController I => Instance;

        /// <summary>
        /// 싱글톤을 그저 생성하기 위한 메소드
        /// </summary>
        public void Call() { }

        /// <summary>
        /// 싱글톤을 그저 생성하기 위한 정적 메소드
        /// </summary>
        public static void Call_()
        {
            if (instance == null)
                CheckExsistence();
        }

        #endregion // ==================================================================

        #region Singleton - Private

        // 싱글톤 인스턴스
        private static GetComponentController instance;

        // 싱글톤 인스턴스 존재 여부 확인 (체크 2)
        private static void CheckExsistence()
        {
            // 싱글톤 검색
            instance = FindObjectOfType<GetComponentController>();

            // 인스턴스 가진 오브젝트가 존재하지 않을 경우, 빈 오브젝트를 임의로 생성하여 인스턴스 할당
            if (instance == null)
            {
                // 빈 게임 오브젝트 생성
                GameObject container = new GameObject("GetComponentController Singleton Container");

                // 게임 오브젝트에 클래스 컴포넌트 추가 후 인스턴스 할당
                instance = container.AddComponent<GetComponentController>();
            }
        }

        /// <summary> 
        /// [Awake()에서 호출]
        /// <para/> 싱글톤 스크립트를 미리 오브젝트에 담아 사용하는 경우를 위한 로직
        /// </summary>
        private void CheckInstance()
        {
            // 싱글톤 인스턴스가 존재하지 않았을 경우, 본인으로 초기화
            if (instance == null)
                instance = this;

            // 싱글톤 인스턴스가 존재하는데, 본인이 아닐 경우, 스스로를 파괴
            if (instance != null && instance != this)
                Destroy(this);
        }

        #endregion // ==================================================================

        private void Awake()
        {
            CheckInstance();

            RunAttributeAction(EventFlow.Awake);
        }

        private void Start()
        {
            RunAttributeAction(EventFlow.Start);
        }

        /// <summary> 원하는 유니티 기본 이벤트 타이밍에 GetComponent 동작 </summary>
        private void RunAttributeAction(EventFlow flow)
        {
            // 모든 활성 컴포넌트 찾기
            var allComponents = FindObjectsOfType<Component>();

            foreach (var component in allComponents)
            {
                RunGetComponentActions(component, flow);
            }
        }

        /// <summary> 필드, 프로퍼티 대상으로 GetComponent, GetOrAddCOmponent 수행 </summary>
        private void RunGetComponentActions(Component component, EventFlow flow)
        {
            // 1. 모든 필드 찾기 -> NonPublic은 타입 찾을 때 인식이 안되니 미리 패스
            var fInfos = component.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);

            // 2. Component의 자식 타입인 참조형 필드만 골라내기 (is Component)
            // 3. GetComponent류 애트리뷰트가 있는 필드만 골라내기
            // 4. 입력된 이벤트 타이밍이 일치하는 필드만 골라내기
            var targetFInfos =
                from fieldInfo in fInfos
                where fieldInfo.GetCustomAttribute<GetComponentAttributeBase>()?.Flow == flow &&
                      (fieldInfo.FieldType.Ex_IsChildOrEqualsTo(typeof(Component)) ||
                       fieldInfo.FieldType.Ex_IsArrayAndChildOf(typeof(Component)) ||
                       (fieldInfo.FieldType.Ex_IsListType())
                      )
                select fieldInfo;

            // 프로퍼티에 똑같이 수행
            var pInfos = component.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var targetPInfos =
                from pInfo in pInfos
                where pInfo.SetMethod != null && // 프로퍼티에 Setter가 없는 경우 X
                      pInfo.GetCustomAttribute<GetComponentAttributeBase>()?.Flow == flow &&
                      (pInfo.PropertyType.Ex_IsChildOrEqualsTo(typeof(Component)) ||
                       pInfo.PropertyType.Ex_IsArrayAndChildOf(typeof(Component)) ||
                       (pInfo.PropertyType.Ex_IsListType())
                      )
                select pInfo;

            // 멤버 리스트로 병합
            List<MemberInfo> memberInfos = targetFInfos.ToList<MemberInfo>();
            memberInfos.AddRange(pInfos.ToList<MemberInfo>());

            // 5. GetComponent 종류별로 해주기
            foreach (var memberInfo in memberInfos)
            {
                var customAttribute = memberInfo.GetCustomAttribute<GetComponentAttributeBase>();
                var memberType = memberInfo.Ex_GetMemberType();
                string methodName = "";

                // 5-1. GetComponent, GetOrAddComponent - Element
                if (memberType.Ex_IsChildOrEqualsTo(typeof(Component)))
                {
                    // GetComponent, GetOrAdd
                    GetComponentToElementMember(memberInfo, memberType, component);
                }
                // Array or List
                else
                {
                    switch (customAttribute)
                    {
                        case GetComponents getComs:
                            methodName = "GetComponents";
                            break;

                        case GetComponentsInParent getComsParent:
                            methodName = "GetComponentsInParent";
                            break;

                        case GetComponentsInChildren getComsChildren:
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
        private void GetComponentToElementMember(in MemberInfo memberInfo, in Type memberType, in Component component)
        {
            var customAttribute = memberInfo.GetCustomAttribute<GetComponentAttributeBase>();
            GameObject go = component.gameObject;

            string methodName = "";
            Type[] methodParams = new Type[0];      // GetOrAdd에서 사용 : 메소드 파라미터
            object[] realParams = new object[0];    // GetOrAdd에서 사용 : 실질 파라미터
            string goaTargetString = "";            // GetOrAdd 애트리뷰트에서 가져오는 타겟 부모 또는 자식 이름 스트링

            switch (customAttribute)
            {
                case GetComponent g:
                    memberInfo.Ex_SetValue(component, go.GetComponent(memberType));
                    break;

                case GetComponentInParent g:
                    memberInfo.Ex_SetValue(component, go.GetComponentInParent(memberType));
                    break;

                case GetComponentInChildren g:
                    memberInfo.Ex_SetValue(component, go.GetComponentInChildren(memberType));
                    break;


                case GetOrAddComponent goa:
                    methodName = "GetOrAddComponent";
                    methodParams = new Type[] { typeof(GameObject), typeof(Type) };
                    realParams = new object[] { component.gameObject, memberType };
                    break;

                case GetOrAddComponentInChildren goa:
                    methodName = "GetOrAddComponentInChildren";
                    goaTargetString = (customAttribute as GetOrAddComponentInChildren).ChildObjectName;
                    methodParams = new Type[] { typeof(GameObject), typeof(Type), typeof(string) };
                    realParams = new object[] { component.gameObject, memberType, goaTargetString };
                    break;

                case GetOrAddComponentInParent goa:
                    methodName = "GetOrAddComponentInParent";
                    goaTargetString = (customAttribute as GetOrAddComponentInParent).ParentObjectName;
                    methodParams = new Type[] { typeof(GameObject), typeof(Type), typeof(string) };
                    realParams = new object[] { component.gameObject, memberType, goaTargetString };
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
        private void GetComponentToArrayMember(in MemberInfo memberInfo, in Type memberType, in Component component,
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
        private void GetComponentToListMember(in MemberInfo memberInfo, in Type memberType, in Component component,
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
 */