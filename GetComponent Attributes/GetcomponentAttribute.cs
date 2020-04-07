
namespace Rito.Attributes
{
    // 2020. 03. 18. 작성
    // 2020. 03. 20. GetOrAdd 추가
    // 2020. 03. 30. GetComponentInAChild 추가
    // 2020. 04. 07. 싱글톤 -> Start() 직전에 자동 호출되는 정적 메소드화,
    //               애트리뷰트 파라미터에서 EventFlow 제거, AllowOverwrite 추가
    // 2020. 04. 07. GetOrAddComponentInAChild 추가

    #region Base

    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public abstract class GetComponentBaseAttribute : System.Attribute
    {
        /// <summary> 이미 기존에 할당한 경우에도 새롭게 GetComponent를 수행하여 덮어쓸지 여부 </summary>
        public bool AllowOverwrite { get; }

        public GetComponentBaseAttribute() => AllowOverwrite = false;
        public GetComponentBaseAttribute(bool a) => AllowOverwrite = a;
    }

    #endregion // ==========================================================

    #region Get Component, Components

    /// <summary> 게임오브젝트에서 해당 타입의 컴포넌트를 찾아 지정된 필드 또는 프로퍼티에 초기화한다. 
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentAttribute : GetComponentBaseAttribute
    {
        public GetComponentAttribute() : base() { }
        public GetComponentAttribute(bool a) : base(a) { }
    }

    /// <summary> 게임오브젝트에서 해당 타입의 컴포넌트들을 찾아 지정된 필드 또는 프로퍼티에 초기화한다.
    /// <para/> 대상 : Array, List, Dictionary
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentsAttribute : GetComponentBaseAttribute
    {
        public GetComponentsAttribute() : base() { }
        public GetComponentsAttribute(bool a) : base(a) { }
    }

    /// <summary> 자신 및 자식 게임오브젝트에서 해당 타입의 컴포넌트를 찾아 지정된 필드 또는 프로퍼티에 초기화한다. 
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentInChildrenAttribute : GetComponentBaseAttribute
    {
        public GetComponentInChildrenAttribute() : base() { }
        public GetComponentInChildrenAttribute(bool a) : base(a) { }
    }

    /// <summary> 자신 및 자식 게임오브젝트에서 해당 타입의 컴포넌트들을 찾아 지정된 필드 또는 프로퍼티에 초기화한다.
    /// <para/> 대상 : Array, List, Dictionary
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentsInChildrenAttribute : GetComponentBaseAttribute
    {
        public GetComponentsInChildrenAttribute() : base() { }
        public GetComponentsInChildrenAttribute(bool a) : base(a) { }
    }

    /// <summary> 자신 및 부모 게임오브젝트에서 해당 타입의 컴포넌트를 찾아 지정된 필드 또는 프로퍼티에 초기화한다. 
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentInParentAttribute : GetComponentBaseAttribute
    {
        public GetComponentInParentAttribute() : base() { }
        public GetComponentInParentAttribute(bool a) : base(a) { }
    }

    /// <summary> 자신 및 부모 게임오브젝트에서 해당 타입의 컴포넌트들을 찾아 지정된 필드 또는 프로퍼티에 초기화한다.
    /// <para/> 대상 : Array, List, Dictionary
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentsInParentAttribute : GetComponentBaseAttribute
    {
        public GetComponentsInParentAttribute() : base() { }
        public GetComponentsInParentAttribute(bool a) : base(a) { }
    }

    /// <summary> 지정한 이름의 자식 게임오브젝트에서 해당 타입의 컴포넌트를 찾아 지정된 필드 또는 프로퍼티에 초기화한다. 
    /// <para/> * 해당 이름의 자식 게임오브젝트가 존재하지 않을 경우, 아무런 동작을 하지 않는다.
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetComponentInAChildAttribute : GetComponentBaseAttribute
    {
        /// <summary> 대상 자식 게임오브젝트의 이름 </summary>
        public string ChildObjectName { get; }

        public GetComponentInAChildAttribute(string childName) : base()
            => ChildObjectName = childName;

        public GetComponentInAChildAttribute(bool a, string childName) : base(a)
            => ChildObjectName = childName;

        public GetComponentInAChildAttribute(string childName, bool a) : base(a)
            => ChildObjectName = childName;
    }

    #endregion

    #region Get Or Add Component

    /// <summary> 게임오브젝트에서 해당 타입의 컴포넌트를 찾고, 없으면 생성 및 추가한다. 
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
    /// <para/> [1] 해당 컴포넌트를 찾지 못하면 지정된 이름의 자식 게임오브젝트에 생성 및 추가한다.
    /// <para/> [2] 지정된 이름의 자식 게임오브젝트가 없으면, 해당 이름으로 자식 게임오브젝트를 생성한 뒤 컴포넌트를 추가한다.
    /// <para/> * 대상 : 필드, 프로퍼티
    /// <para/> * AllowOverwrite : 이미 참조가 할당되어 있어도 GetComponent를 수행하여 덮어쓸지 여부
    /// </summary>
    public class GetOrAddComponentInAChildAttribute : GetComponentBaseAttribute
    {
        /// <summary> 대상 자식 게임오브젝트의 이름 </summary>
        public string ChildObjectName { get; }

        public GetOrAddComponentInAChildAttribute(string childName) : base()
            => ChildObjectName = childName;
        public GetOrAddComponentInAChildAttribute(bool a, string childName) : base(a)
            => ChildObjectName = childName;
        public GetOrAddComponentInAChildAttribute(string childName, bool a) : base(a)
            => ChildObjectName = childName;
    }

    /// <summary> 
    /// <para/> 자신 및 자식 게임오브젝트들에서 해당 타입의 컴포넌트를 찾고,
    /// <para/> [1] 해당 컴포넌트를 찾지 못하면 지정된 이름의 자식 게임오브젝트에 생성 및 추가한다.
    /// <para/> [2] 지정된 이름의 자식 게임오브젝트가 없으면, 해당 이름으로 자식 게임오브젝트를 생성한 뒤 컴포넌트를 추가한다.
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
    /// <para/> [1] 해당 컴포넌트를 찾지 못하면 지정된 이름의 부모 게임오브젝트에 생성 및 추가한다.
    /// <para/> [2] 지정된 이름의 부모 게임오브젝트가 없으면, 바로 위의 부모 게임오브젝트에 컴포넌트를 추가한다.
    /// <para/> * 부모 게임오브젝트가 없을 경우(본인이 루트인 경우), [1]과 [2]는 동작하지 않는다.
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
}