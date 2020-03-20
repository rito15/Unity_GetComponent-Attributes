using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito
{
    // 2020. 03. 18. 작성
    // 2020. 03. 20. GetOrAdd 추가

    #region Base

    interface IGetComponent { }

    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public abstract class GetComponentAttributeBase : System.Attribute, IGetComponent
    {
        public EventFlow Flow { get; }

        public GetComponentAttributeBase() => Flow = EventFlow.Awake;
        public GetComponentAttributeBase(EventFlow t) => Flow = t;
    }

    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public abstract class GetOrAddComponentAttributeBase : System.Attribute, IGetComponent
    {
        public EventFlow Flow { get; }

        public GetOrAddComponentAttributeBase() => Flow = EventFlow.Awake;
        public GetOrAddComponentAttributeBase(EventFlow t) => Flow = t;
    }

    #endregion // ==========================================================

    #region Get Component, Components

    /// <summary> 게임오브젝트에서 해당 타입의 컴포넌트를 찾아 지정된 필드 또는 프로퍼티에 초기화한다. 
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetComponent : GetComponentAttributeBase
    {
        public GetComponent() : base() { }
        public GetComponent(EventFlow t) : base(t) { }
    }

    /// <summary> 게임오브젝트에서 해당 타입의 컴포넌트들을 찾아 지정된 필드 또는 프로퍼티에 초기화한다.
    /// <para/> 대상 : Array, List, Dictionary
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetComponents : GetComponentAttributeBase
    {
        public GetComponents() : base() { }
        public GetComponents(EventFlow t) : base(t) { }
    }

    /// <summary> 자신 및 자식 게임오브젝트에서 해당 타입의 컴포넌트를 찾아 지정된 필드 또는 프로퍼티에 초기화한다. 
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetComponentInChildren : GetComponentAttributeBase
    {
        public GetComponentInChildren() : base() { }
        public GetComponentInChildren(EventFlow t) : base(t) { }
    }

    /// <summary> 자신 및 자식 게임오브젝트에서 해당 타입의 컴포넌트들을 찾아 지정된 필드 또는 프로퍼티에 초기화한다.
    /// <para/> 대상 : Array, List, Dictionary
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetComponentsInChildren : GetComponentAttributeBase
    {
        public GetComponentsInChildren() : base() { }
        public GetComponentsInChildren(EventFlow t) : base(t) { }
    }

    /// <summary> 자신 및 부모 게임오브젝트에서 해당 타입의 컴포넌트를 찾아 지정된 필드 또는 프로퍼티에 초기화한다. 
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetComponentInParent : GetComponentAttributeBase
    {
        public GetComponentInParent() : base() { }
        public GetComponentInParent(EventFlow t) : base(t) { }
    }

    /// <summary> 자신 및 부모 게임오브젝트에서 해당 타입의 컴포넌트들을 찾아 지정된 필드 또는 프로퍼티에 초기화한다.
    /// <para/> 대상 : Array, List, Dictionary
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetComponentsInParent : GetComponentAttributeBase
    {
        public GetComponentsInParent() : base() { }
        public GetComponentsInParent(EventFlow t) : base(t) { }
    }

    #endregion

    #region Get Or Add Component

    /// <summary> 게임오브젝트에서 해당 타입의 컴포넌트를 찾고, 없으면 생성 및 추가한다. 
    /// <para/> * 대상 : 필드, 프로퍼티
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetOrAddComponent : GetOrAddComponentAttributeBase
    {
        public GetOrAddComponent() : base() { }
        public GetOrAddComponent(EventFlow t) : base(t) { }
    }

    /// <summary> 
    /// <para/> 자신 및 자식 게임오브젝트들에서 해당 타입의 컴포넌트를 찾고,
    /// <para/> [1] 해당 컴포넌트를 찾지 못하면 지정된 이름의 자식 게임오브젝트에 생성 및 추가한다.
    /// <para/> [2] 지정된 이름의 자식 게임오브젝트가 없으면, 해당 이름으로 자식 게임오브젝트를 생성한 뒤 컴포넌트를 추가한다.
    /// <para/> * 대상 : 필드, 프로퍼티
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetOrAddComponentInChildren : GetOrAddComponentAttributeBase
    {
        /// <summary> 대상 자식 게임오브젝트의 이름 </summary>
        public string ChildObjectName { get; }

        public GetOrAddComponentInChildren(string childName) : base() => ChildObjectName = childName;
        public GetOrAddComponentInChildren(EventFlow t, string childName) : base(t) => ChildObjectName = childName;
        public GetOrAddComponentInChildren(string childName, EventFlow t) : base(t) => ChildObjectName = childName;
    }

    /// <summary> 
    /// <para/> 자신 및 부모 게임오브젝트들에서 해당 타입의 컴포넌트를 찾고,
    /// <para/> [1] 해당 컴포넌트를 찾지 못하면 지정된 이름의 부모 게임오브젝트에 생성 및 추가한다.
    /// <para/> [2] 지정된 이름의 부모 게임오브젝트가 없으면, 바로 위의 부모 게임오브젝트에 컴포넌트를 추가한다.
    /// <para/> * 부모 게임오브젝트가 없을 경우(본인이 루트인 경우), [1]과 [2]는 동작하지 않는다.
    /// <para/> * 대상 : 필드, 프로퍼티
    /// <para/> * EventFlow : 초기화할 타이밍(Awake(기본), Start)
    /// </summary>
    public class GetOrAddComponentInParent : GetOrAddComponentAttributeBase
    {
        /// <summary> 대상 부모 게임오브젝트의 이름 </summary>
        public string ParentObjectName { get; }

        public GetOrAddComponentInParent(string childName) : base() => ParentObjectName = childName;
        public GetOrAddComponentInParent(EventFlow t, string childName) : base(t) => ParentObjectName = childName;
        public GetOrAddComponentInParent(string childName, EventFlow t) : base(t) => ParentObjectName = childName;
    }

    #endregion // ==========================================================

    /// <summary> GetComponent되는 타이밍 결정 </summary>
    public enum EventFlow
    {
        Awake, Start
    }
}