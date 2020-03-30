using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito
{
    // 2020. 03. 18. 작성
    // 2020. 03. 20. GetOrAdd 추가
    // 2020. 03. 30. GetComponentInAChild 추가

    #region Base

    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public abstract class GetComponentBaseAttribute : System.Attribute
    {
        public EventFlow Flow { get; }

        public GetComponentBaseAttribute() => Flow = EventFlow.Awake;
        public GetComponentBaseAttribute(EventFlow t) => Flow = t;
    }

    #endregion // ==========================================================

    #region Get Component, Components

    /// <summary> 게임오브젝트에서 해당 타입의 컴포넌트를 찾아 지정된 필드 또는 프로퍼티에 초기화한다. 
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetComponentAttribute : GetComponentBaseAttribute
    {
        public GetComponentAttribute() : base() { }
        public GetComponentAttribute(EventFlow t) : base(t) { }
    }

    /// <summary> 게임오브젝트에서 해당 타입의 컴포넌트들을 찾아 지정된 필드 또는 프로퍼티에 초기화한다.
    /// <para/> 대상 : Array, List, Dictionary
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetComponentsAttribute : GetComponentBaseAttribute
    {
        public GetComponentsAttribute() : base() { }
        public GetComponentsAttribute(EventFlow t) : base(t) { }
    }

    /// <summary> 자신 및 자식 게임오브젝트에서 해당 타입의 컴포넌트를 찾아 지정된 필드 또는 프로퍼티에 초기화한다. 
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetComponentInChildrenAttribute : GetComponentBaseAttribute
    {
        public GetComponentInChildrenAttribute() : base() { }
        public GetComponentInChildrenAttribute(EventFlow t) : base(t) { }
    }

    /// <summary> 자신 및 자식 게임오브젝트에서 해당 타입의 컴포넌트들을 찾아 지정된 필드 또는 프로퍼티에 초기화한다.
    /// <para/> 대상 : Array, List, Dictionary
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetComponentsInChildrenAttribute : GetComponentBaseAttribute
    {
        public GetComponentsInChildrenAttribute() : base() { }
        public GetComponentsInChildrenAttribute(EventFlow t) : base(t) { }
    }

    /// <summary> 자신 및 부모 게임오브젝트에서 해당 타입의 컴포넌트를 찾아 지정된 필드 또는 프로퍼티에 초기화한다. 
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetComponentInParentAttribute : GetComponentBaseAttribute
    {
        public GetComponentInParentAttribute() : base() { }
        public GetComponentInParentAttribute(EventFlow t) : base(t) { }
    }

    /// <summary> 자신 및 부모 게임오브젝트에서 해당 타입의 컴포넌트들을 찾아 지정된 필드 또는 프로퍼티에 초기화한다.
    /// <para/> 대상 : Array, List, Dictionary
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetComponentsInParentAttribute : GetComponentBaseAttribute
    {
        public GetComponentsInParentAttribute() : base() { }
        public GetComponentsInParentAttribute(EventFlow t) : base(t) { }
    }

    /// <summary> 지정한 이름의 자식 게임오브젝트에서 해당 타입의 컴포넌트를 찾아 지정된 필드 또는 프로퍼티에 초기화한다. 
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// <para/> * 해당 이름의 자식 게임오브젝트가 존재하지 않을 경우, 아무런 동작을 하지 않는다.
    /// </summary>
    public class GetComponentInAChildAttribute : GetComponentBaseAttribute
    {
        /// <summary> 대상 자식 게임오브젝트의 이름 </summary>
        public string ChildObjectName { get; }

        public GetComponentInAChildAttribute(string childName) : base()
            => ChildObjectName = childName;

        public GetComponentInAChildAttribute(EventFlow t, string childName) : base(t)
            => ChildObjectName = childName;

        public GetComponentInAChildAttribute(string childName, EventFlow t) : base(t)
            => ChildObjectName = childName;
    }

    #endregion

    #region Get Or Add Component

    /// <summary> 게임오브젝트에서 해당 타입의 컴포넌트를 찾고, 없으면 생성 및 추가한다. 
    /// <para/> * 대상 : 필드, 프로퍼티
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetOrAddComponentAttribute : GetComponentBaseAttribute
    {
        public GetOrAddComponentAttribute() : base() { }
        public GetOrAddComponentAttribute(EventFlow t) : base(t) { }
    }

    /// <summary> 
    /// <para/> 자신 및 자식 게임오브젝트들에서 해당 타입의 컴포넌트를 찾고,
    /// <para/> [1] 해당 컴포넌트를 찾지 못하면 지정된 이름의 자식 게임오브젝트에 생성 및 추가한다.
    /// <para/> [2] 지정된 이름의 자식 게임오브젝트가 없으면, 해당 이름으로 자식 게임오브젝트를 생성한 뒤 컴포넌트를 추가한다.
    /// <para/> * 대상 : 필드, 프로퍼티
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetOrAddComponentInChildrenAttribute : GetComponentBaseAttribute
    {
        /// <summary> 대상 자식 게임오브젝트의 이름 </summary>
        public string ChildObjectName { get; }

        public GetOrAddComponentInChildrenAttribute(string childName) : base()
            => ChildObjectName = childName;
        public GetOrAddComponentInChildrenAttribute(EventFlow t, string childName) : base(t)
            => ChildObjectName = childName;
        public GetOrAddComponentInChildrenAttribute(string childName, EventFlow t) : base(t)
            => ChildObjectName = childName;
    }

    /// <summary> 
    /// <para/> 자신 및 부모 게임오브젝트들에서 해당 타입의 컴포넌트를 찾고,
    /// <para/> [1] 해당 컴포넌트를 찾지 못하면 지정된 이름의 부모 게임오브젝트에 생성 및 추가한다.
    /// <para/> [2] 지정된 이름의 부모 게임오브젝트가 없으면, 바로 위의 부모 게임오브젝트에 컴포넌트를 추가한다.
    /// <para/> * 부모 게임오브젝트가 없을 경우(본인이 루트인 경우), [1]과 [2]는 동작하지 않는다.
    /// <para/> * 대상 : 필드, 프로퍼티
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetOrAddComponentInParentAttribute : GetComponentBaseAttribute
    {
        /// <summary> 대상 부모 게임오브젝트의 이름 </summary>
        public string ParentObjectName { get; }

        public GetOrAddComponentInParentAttribute(string parentName) : base()
            => ParentObjectName = parentName;
        public GetOrAddComponentInParentAttribute(EventFlow t, string parentName) : base(t)
            => ParentObjectName = parentName;
        public GetOrAddComponentInParentAttribute(string parentName, EventFlow t) : base(t)
            => ParentObjectName = parentName;
    }

    #endregion // ==========================================================

    /// <summary> GetComponent되는 타이밍 결정 </summary>
    public enum EventFlow
    {
        Awake, Start
    }
}