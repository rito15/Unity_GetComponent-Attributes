
namespace Rito.Attributes
{
    #region Dev Log
/*
     2020. 03. 18. 작성
     2020. 03. 20. GetOrAdd 추가
     2020. 03. 30. GetComponentInAChild 추가
     2020. 04. 07. 싱글톤 -> Start() 직전에 자동 호출되는 정적 메소드화,
                   애트리뷰트 파라미터에서 EventFlow 제거, AllowOverwrite 추가
     2020. 04. 07. GetOrAddComponentInAChild 추가
     2020. 04. 12. AttributeUsage - bool 파라미터들 명시
*/
    #endregion // ==========================================================

    #region Base

    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public abstract class GetComponentBaseAttribute : System.Attribute
    {
        /// <summary> 이미 기존에 할당한 경우에도 새롭게 GetComponent를 수행하여 덮어쓸지 여부 </summary>
        public bool AllowOverwrite { get; }

        public GetComponentBaseAttribute() => AllowOverwrite = false;
        public GetComponentBaseAttribute(bool a) => AllowOverwrite = a;
    }

    #endregion // ==========================================================

    #region Get Component, Components

    /// <summary> 게임오브젝트에서 해당 타입의 컴포넌트를 찾아 지정된 필드 또는 프로퍼티에 초기화합니다. 
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentAttribute : GetComponentBaseAttribute
    {
        public GetComponentAttribute() : base() { }
        public GetComponentAttribute(bool a) : base(a) { }
    }

    /// <summary> 게임오브젝트에서 해당 타입의 컴포넌트들을 찾아 지정된 필드 또는 프로퍼티에 초기화합니다.
    /// <para/> 대상 : Array, List, Dictionary
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentsAttribute : GetComponentBaseAttribute
    {
        public GetComponentsAttribute() : base() { }
        public GetComponentsAttribute(bool a) : base(a) { }
    }

    /// <summary> 자신 및 자식 게임오브젝트에서 해당 타입의 컴포넌트를 찾아 지정된 필드 또는 프로퍼티에 초기화합니다. 
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentInChildrenAttribute : GetComponentBaseAttribute
    {
        public GetComponentInChildrenAttribute() : base() { }
        public GetComponentInChildrenAttribute(bool a) : base(a) { }
    }

    /// <summary> 자신 및 자식 게임오브젝트에서 해당 타입의 컴포넌트들을 찾아 지정된 필드 또는 프로퍼티에 초기화합니다.
    /// <para/> 대상 : Array, List, Dictionary
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentsInChildrenAttribute : GetComponentBaseAttribute
    {
        public GetComponentsInChildrenAttribute() : base() { }
        public GetComponentsInChildrenAttribute(bool a) : base(a) { }
    }

    /// <summary> 자신 및 부모 게임오브젝트에서 해당 타입의 컴포넌트를 찾아 지정된 필드 또는 프로퍼티에 초기화합니다. 
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentInParentAttribute : GetComponentBaseAttribute
    {
        public GetComponentInParentAttribute() : base() { }
        public GetComponentInParentAttribute(bool a) : base(a) { }
    }

    /// <summary> 자신 및 부모 게임오브젝트에서 해당 타입의 컴포넌트들을 찾아 지정된 필드 또는 프로퍼티에 초기화합니다.
    /// <para/> 대상 : Array, List, Dictionary
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentsInParentAttribute : GetComponentBaseAttribute
    {
        public GetComponentsInParentAttribute() : base() { }
        public GetComponentsInParentAttribute(bool a) : base(a) { }
    }

    /// <summary> 지정한 이름의 자식 게임오브젝트에서 해당 타입의 컴포넌트를 찾아 지정된 필드 또는 프로퍼티에 초기화합니다. 
    /// <para/> * 해당 이름의 자식 게임오브젝트가 존재하지 않을 경우, 아무런 동작을 하지 않습니다.
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentInChildAttribute : GetComponentBaseAttribute
    {
        /// <summary> 대상 자식 게임오브젝트의 이름 </summary>
        public string ChildObjectName { get; }

        public GetComponentInChildAttribute(string childName) : base()
            => ChildObjectName = childName;

        public GetComponentInChildAttribute(bool a, string childName) : base(a)
            => ChildObjectName = childName;

        public GetComponentInChildAttribute(string childName, bool a) : base(a)
            => ChildObjectName = childName;
    }

    #endregion

    #region Get Or Add Component

    /// <summary> 게임오브젝트에서 해당 타입의 컴포넌트를 찾고, 없으면 생성 및 추가합니다. 
    /// <para/> * 대상 : 필드, 프로퍼티
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetOrAddComponentAttribute : GetComponentBaseAttribute
    {
        public GetOrAddComponentAttribute() : base() { }
        public GetOrAddComponentAttribute(bool a) : base(a) { }
    }

    /// <summary> 
    /// <para/> 지정한 이름의 자식 오브젝트에서 해당 타입의 컴포넌트를 찾고,
    /// <para/> [1] 해당 컴포넌트를 찾지 못하면 지정된 이름의 자식 게임오브젝트에 생성 및 추가합니다.
    /// <para/> [2] 지정된 이름의 자식 게임오브젝트가 없으면, 해당 이름으로 자식 게임오브젝트를 생성한 뒤 컴포넌트를 추가합니다.
    /// <para/> * 대상 : 필드, 프로퍼티
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetOrAddComponentInChildAttribute : GetComponentBaseAttribute
    {
        /// <summary> 대상 자식 게임오브젝트의 이름 </summary>
        public string ChildObjectName { get; }

        public GetOrAddComponentInChildAttribute(string childName) : base()
            => ChildObjectName = childName;
        public GetOrAddComponentInChildAttribute(bool a, string childName) : base(a)
            => ChildObjectName = childName;
        public GetOrAddComponentInChildAttribute(string childName, bool a) : base(a)
            => ChildObjectName = childName;
    }

    /// <summary> 
    /// <para/> 자신 및 자식 게임오브젝트들에서 해당 타입의 컴포넌트를 찾고,
    /// <para/> [1] 해당 컴포넌트를 찾지 못하면 지정된 이름의 자식 게임오브젝트에 생성 및 추가합니다.
    /// <para/> [2] 지정된 이름의 자식 게임오브젝트가 없으면, 해당 이름으로 자식 게임오브젝트를 생성한 뒤 컴포넌트를 추가합니다.
    /// <para/> * 대상 : 필드, 프로퍼티
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetOrAddComponentInChildrenAttribute : GetComponentBaseAttribute
    {
        /// <summary> 대상 자식 게임오브젝트의 이름 </summary>
        public string ChildObjectName { get; }

        public GetOrAddComponentInChildrenAttribute(string childName) : base()
            => ChildObjectName = childName;
        public GetOrAddComponentInChildrenAttribute(bool a, string childName) : base(a)
            => ChildObjectName = childName;
        public GetOrAddComponentInChildrenAttribute(string childName, bool a) : base(a)
            => ChildObjectName = childName;
    }

    /// <summary> 
    /// <para/> 자신 및 부모 게임오브젝트들에서 해당 타입의 컴포넌트를 찾고,
    /// <para/> [1] 해당 컴포넌트를 찾지 못하면 지정된 이름의 부모 게임오브젝트에 생성 및 추가합니다.
    /// <para/> [2] 지정된 이름의 부모 게임오브젝트가 없으면, 바로 위의 부모 게임오브젝트에 컴포넌트를 추가합니다.
    /// <para/> * 부모 게임오브젝트가 없을 경우(본인이 루트인 경우), [1]과 [2]는 동작하지 않습니다.
    /// <para/> * 대상 : 필드, 프로퍼티
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetOrAddComponentInParentAttribute : GetComponentBaseAttribute
    {
        /// <summary> 대상 부모 게임오브젝트의 이름 </summary>
        public string ParentObjectName { get; }

        public GetOrAddComponentInParentAttribute(string parentName) : base()
            => ParentObjectName = parentName;
        public GetOrAddComponentInParentAttribute(bool a, string parentName) : base(a)
            => ParentObjectName = parentName;
        public GetOrAddComponentInParentAttribute(string parentName, bool a) : base(a)
            => ParentObjectName = parentName;
    }

    #endregion // ==========================================================

    #region GetComponent(s) Children, Parent - Only

    /// <summary> 자신을 제외한 자식 게임오브젝트들에서 해당 타입의 컴포넌트를 찾아 지정된 필드 또는 프로퍼티에 초기화합니다. 
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentInChildrenOnlyAttribute : GetComponentBaseAttribute
    {
        public GetComponentInChildrenOnlyAttribute() : base() { }
        public GetComponentInChildrenOnlyAttribute(bool a) : base(a) { }
    }

    /// <summary> 자신을 제외한 부모 게임오브젝트들에서 해당 타입의 컴포넌트를 찾아 지정된 필드 또는 프로퍼티에 초기화합니다. 
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentInParentOnlyAttribute : GetComponentBaseAttribute
    {
        public GetComponentInParentOnlyAttribute() : base() { }
        public GetComponentInParentOnlyAttribute(bool a) : base(a) { }
    }

    /// <summary> 자신을 제외한 자식 게임오브젝트들에서 해당 타입의 컴포넌트들을 찾아 지정된 필드 또는 프로퍼티에 초기화합니다. 
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentsInChildrenOnlyAttribute : GetComponentBaseAttribute
    {
        public GetComponentsInChildrenOnlyAttribute() : base() { }
        public GetComponentsInChildrenOnlyAttribute(bool a) : base(a) { }
    }

    /// <summary> 자신을 제외한 부모 게임오브젝트들에서 해당 타입의 컴포넌트들을 찾아 지정된 필드 또는 프로퍼티에 초기화합니다. 
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentsInParentOnlyAttribute : GetComponentBaseAttribute
    {
        public GetComponentsInParentOnlyAttribute() : base() { }
        public GetComponentsInParentOnlyAttribute(bool a) : base(a) { }
    }

    #endregion // ==========================================================

    #region Find

    /// <summary> 현재 씬 내에서 타겟 타입의 컴포넌트를 찾아와 초기화
    /// <para/> * 씬 내에 해당 컴포넌트가 하나도 존재하지 않을 경우 아무런 동작을 수행하지 않습니다.
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 기능을 수행하여 덮어쓸지 여부
    /// </summary>
    public class FindAttribute : GetComponentBaseAttribute
    {
        public FindAttribute(bool a = false) : base(a) { }
    }

    /// <summary> 현재 씬 내에서 타겟 타입의 컴포넌트를 모두 찾아와 초기화
    /// <para/> * 씬 내에 해당 컴포넌트가 하나도 존재하지 않을 경우 아무런 동작을 수행하지 않습니다.
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 기능을 수행하여 덮어쓸지 여부
    /// </summary>
    public class FindAllAttribute : GetComponentBaseAttribute
    {
        public FindAllAttribute(bool a = false) : base(a) { }
    }

    /// <summary> 현재 씬 내에서 타겟 타입의 컴포넌트를 찾아와 초기화
    /// <para/> * 씬 내에 해당 컴포넌트가 하나도 존재하지 않을 경우, 지정한 이름(NewGoName)으로 게임 오브젝트를 생성한 뒤 컴포넌트를 추가합니다.
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 기능을 수행하여 덮어쓸지 여부
    /// </summary>
    public class FindOrAddAttribute : GetComponentBaseAttribute
    {
        public string NewGoName { get; }

        public FindOrAddAttribute(string newGoName, bool a = false) : base(a) => NewGoName = newGoName;
        public FindOrAddAttribute(bool a, string newGoName) : base(a) => NewGoName = newGoName;
    }

    /// <summary> 현재 씬 내에서 지정한 이름(TargetGoName)의 게임 오브젝트를 찾아, 타겟 멤버 타입의 컴포넌트를 찾아와 초기화
    /// <para/> * 지정한 이름의 게임 오브젝트가 존재하지 않거나, 해당 컴포넌트가 존재하지 않을 경우 아무런 동작을 수행하지 않습니다.
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 기능을 수행하여 덮어쓸지 여부
    /// </summary>
    public class FindByNameAttribute : GetComponentBaseAttribute
    {
        public string TargetGoName { get; }

        public FindByNameAttribute(string targetName, bool a = false) : base(a) => TargetGoName = targetName;
        public FindByNameAttribute(bool a, string targetName) : base(a) => TargetGoName = targetName;
    }

    /// <summary> 현재 씬 내에서 지정한 이름(TargetGoName)의 게임 오브젝트를 찾아, 타겟 멤버 타입의 컴포넌트를 찾아와 초기화합니다.
    /// <para/> * 지정한 이름의 게임 오브젝트가 존재하지 않을 경우, 씬 내에 새롭게 생성합니다.
    /// <para/> * 지정한 이름의 게임 오브젝트 내에 해당 컴포넌트가 존재하지 않을 경우, 컴포넌트를 새롭게 추가합니다.
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 기능을 수행하여 덮어쓸지 여부
    /// </summary>
    public class FindByNameOrAddAttribute : GetComponentBaseAttribute
    {
        public string TargetGoName { get; }

        public FindByNameOrAddAttribute(string targetName, bool a = false) : base(a) => TargetGoName = targetName;
        public FindByNameOrAddAttribute(bool a, string targetName) : base(a) => TargetGoName = targetName;
    }


    #endregion // ==========================================================
}