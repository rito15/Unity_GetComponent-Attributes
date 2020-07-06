using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Rito.Attributes
{
    // 2020. 03. 20. 작성
    // 2020. 03. 20. 기능 테스트 완료
    // 2020. 03. 30. GetComponentInChild 추가

    public static class GetComponentExtension
    {
        #region Get or Add Component

        /// <summary> 게임 오브젝트에서 지정된 타입의 컴포넌트를 찾고, 없으면 생성하여 추가한 뒤 리턴한다. </summary>
        public static T GetOrAddComponent<T>(this GameObject @this) where T : Component
        {
            T target = @this.GetComponent<T>();

            if (target == null)
                target = @this.AddComponent<T>();

            return target;
        }

        /// <summary> 게임 오브젝트에서 지정된 타입의 컴포넌트를 찾고, 없으면 생성하여 추가한 뒤 리턴한다. </summary>
        public static Component GetOrAddComponent(this GameObject @this, Type type)
        {
            Component target = @this.GetComponent(type);

            if (target == null)
                target = @this.AddComponent(type);

            return target;
        }

        #endregion // ==========================================================

        #region Get or Add Component in Children

        /// <summary> 
        /// <para/> 자신 및 자식 게임오브젝트들에서 해당 타입의 컴포넌트를 찾고,
        /// <para/> [1] 해당 컴포넌트를 찾지 못하면 지정된 이름의 자식 게임오브젝트에 생성 및 추가한다.
        /// <para/> [2] 지정된 이름의 자식 게임오브젝트가 없으면, 해당 이름으로 자식 게임오브젝트를 생성한 뒤 컴포넌트를 추가한다.
        /// </summary>
        public static T GetOrAddComponentInChildren<T>(this GameObject @this, in string childObjectName) where T : Component
        {
            T target = @this.GetComponentInChildren<T>();

            if (target != null)
                return target;

            // 1. 못찾은 경우 : 지정된 이름의 자식 게임오브젝트 찾기
            Transform targetChild = @this.transform.Find(childObjectName);

            if (targetChild != null)
                return targetChild.gameObject.AddComponent<T>();

            // 2. 해당 이름의 자식 게임오브젝트도 없는 경우 : 자식 게임오브젝트를 생성하고 컴포넌트 추가
            GameObject newChild = new GameObject(childObjectName);
            newChild.transform.SetParent(@this.transform);
            target = newChild.AddComponent<T>();

            return target;
        }

        /// <summary> 
        /// <para/> 자신 및 자식 게임오브젝트들에서 해당 타입의 컴포넌트를 찾고,
        /// <para/> [1] 해당 컴포넌트를 찾지 못하면 지정된 이름의 자식 게임오브젝트에 생성 및 추가한다.
        /// <para/> [2] 지정된 이름의 자식 게임오브젝트가 없으면, 해당 이름으로 자식 게임오브젝트를 생성한 뒤 컴포넌트를 추가한다.
        /// </summary>
        public static Component GetOrAddComponentInChildren(this GameObject @this, Type type, string childObjectName)
        {
            Component target = @this.GetComponentInChildren(type);

            if (target != null)
                return target;

            // 1. 못찾은 경우 : 지정된 이름의 자식 게임오브젝트 찾기
            Transform targetChild = @this.transform.Find(childObjectName);

            if (targetChild != null)
                return targetChild.gameObject.AddComponent(type);

            // 2. 해당 이름의 자식 게임오브젝트도 없는 경우 : 자식 게임오브젝트를 생성하고 컴포넌트 추가
            GameObject newChild = new GameObject(childObjectName);
            newChild.transform.SetParent(@this.transform);
            target = newChild.AddComponent(type);

            return target;
        }

        #endregion // ==========================================================

        #region Get or Add Component in Parent

        /// <summary> 
        /// <para/> 자신 및 부모 게임오브젝트들에서 해당 타입의 컴포넌트를 찾고,
        /// <para/> [1] 해당 컴포넌트를 찾지 못하면 지정된 이름의 부모 게임오브젝트에 생성 및 추가한다.
        /// <para/> [2] 지정된 이름의 부모 게임오브젝트가 없으면, 바로 위의 부모 게임오브젝트에 컴포넌트를 추가한다.
        /// <para/> * 부모 게임오브젝트가 없을 경우(본인이 루트인 경우), [1]과 [2]는 동작하지 않으며 null을 리턴한다.
        /// </summary>
        public static T GetOrAddComponentInParent<T>(this GameObject @this, in string parentObjectName) where T : Component
        {
            T target = @this.GetComponentInParent<T>();

            // 1. 자신 또는 부모들에서 해당 컴포넌트를 찾은 경우 : 바로 리턴
            if (target != null)
                return target;

            // 2. 못찾은 경우 : 게임 오브젝트가 루트인지, 부모를 갖고 있는지 확인
            Transform parent = @this.transform.parent;

            // 3. 루트인 경우 : null 리턴
            if (parent == null)
                return null;

            // 4. 부모가 존재하는 경우 : 매개변수로 지정한 이름의 부모를 찾아 컴포넌트 추가 후 리턴
            while(parent != null)
            {
                if(parent.gameObject.name.Equals(parentObjectName))
                    return parent.gameObject.AddComponent<T>();

                parent = parent.parent;
            }

            // 5. 해당 이름의 부모가 없는 경우 : null 리턴
            return null;
        }

        /// <summary> 
        /// <para/> 자신 및 부모 게임오브젝트들에서 해당 타입의 컴포넌트를 찾고,
        /// <para/> [1] 해당 컴포넌트를 찾지 못하면 지정된 이름의 부모 게임오브젝트에 생성 및 추가한다.
        /// <para/> [2] 지정된 이름의 부모 게임오브젝트가 없으면, 바로 위의 부모 게임오브젝트에 컴포넌트를 추가한다.
        /// <para/> * 부모 게임오브젝트가 없을 경우(본인이 루트인 경우), [1]과 [2]는 동작하지 않으며 null을 리턴한다.
        /// </summary>
        public static Component GetOrAddComponentInParent(this GameObject @this, Type type, string parentObjectName)
        {
            Component target = @this.GetComponentInParent(type);

            // 1. 자신 또는 부모들에서 해당 컴포넌트를 찾은 경우 : 바로 리턴
            if (target != null)
                return target;

            // 2. 못찾은 경우 : 게임 오브젝트가 루트인지, 부모를 갖고 있는지 확인
            Transform parent = @this.transform.parent;

            // 3. 루트인 경우 : null 리턴
            if (parent == null)
                return null;

            // 4. 부모가 존재하는 경우 : 매개변수로 지정한 이름의 부모를 찾아 컴포넌트 추가 후 리턴
            while (parent != null)
            {
                if (parent.gameObject.name.Equals(parentObjectName))
                    return parent.gameObject.AddComponent(type);

                parent = parent.parent;
            }

            // 5. 해당 이름의 부모가 없는 경우 : null 리턴
            return null;
        }

        #endregion // ==========================================================

        #region In Child

        /// <summary> 
        /// <para/> 특정 이름의 자식 게임오브젝트들에서 해당 타입의 컴포넌트를 찾아 리턴한다.
        /// <para/> * 해당 이름의 자식 게임오브젝트가 존재하지 않을 경우, 아무런 동작을 하지 않는다.
        /// </summary>
        public static T GetComponentInChild<T>(this GameObject @this, string childObjectName) where T : Component
        {
            // 지정된 이름의 자식 게임오브젝트 찾기
            Transform targetChild = @this.transform.Find(childObjectName);
            if (targetChild == null)
                return null;

            var targetComponent = targetChild.gameObject.GetComponent<T>();

            if (targetComponent != null)
                return targetComponent;

            else
                return null;
        }

        /// <summary> 
        /// <para/> 지정한 이름의 자식 게임오브젝트들에서 해당 타입의 컴포넌트를 찾아 리턴한다.
        /// <para/> * 해당 자식 게임오브젝트에 해당 컴포넌트가 존재하지 않을 경우, 추가하여 리턴한다.
        /// <para/> * 해당 이름의 자식 게임오브젝트가 존재하지 않을 경우, 해당 이름으로 자식 게임오브젝트를 생성하고 컴포넌트를 추가하여 리턴한다.
        /// </summary>
        public static T GetOrAddComponentInChild<T>(this GameObject @this, string childObjectName) where T : Component
        {
            // 지정된 이름의 자식 게임오브젝트 찾거나 새롭게 생성
            Transform targetChild = @this.transform.Find(childObjectName);
            if (targetChild == null)
            {
                GameObject childGO = new GameObject(childObjectName);
                targetChild = childGO.transform;
                targetChild.SetParent(@this.transform);
            }

            // 자식에서 컴포넌트 탐색하거나 생성
            var targetComponent = targetChild.gameObject.GetComponent<T>();

            if (targetComponent == null)
                targetComponent = targetChild.gameObject.AddComponent<T>();

            return targetComponent;
        }

        #endregion // ==========================================================

        #region Children, Parent Only

        /// <summary> 자신을 제외한 자식 게임오브젝트들에서 해당 타입의 컴포넌트를 찾아 리턴
        /// </summary>
        public static T GetComponentInChildrenOnly<T>(this GameObject @this) where T : Component
        {
            Transform tr = @this.transform;
            if (tr.childCount == 0)
                return null;

            T target;
            for (int i = 0; i < tr.childCount; i++)
            {
                target = tr.GetChild(i).GetComponentInChildren<T>();
                if (target != null)
                    return target;
            }

            return null;
        }

        /// <summary> 자신을 제외한 자식 게임오브젝트들에서 해당 타입의 컴포넌트들을 모두 찾아 리턴
        /// </summary>
        public static T[] GetComponentsInChildrenOnly<T>(this GameObject @this) where T : Component
        {
            Transform tr = @this.transform;
            if (tr.childCount == 0)
                return null;

            List<T> childComponentList = new List<T>();
            T[] targets;
            for (int i = 0; i < tr.childCount; i++)
            {
                targets = tr.GetChild(i).GetComponentsInChildren<T>();
                if (targets != null && targets.Length != 0)
                    foreach (var target in targets)
                    {
                        childComponentList.Add(target);
                    }
            }

            return childComponentList.ToArray();
        }

        /// <summary> 자신을 제외한 부모 게임오브젝트들에서 해당 타입의 컴포넌트를 찾아 리턴
        /// </summary>
        public static T GetComponentInParentOnly<T>(this GameObject @this) where T : Component
        {
            Transform tr = @this.transform;

            // 부모가 없어 루트인 경우
            if (tr.parent == null)
                return null;


            List<T> parentComponentList = new List<T>();
            T targetComponent;

            while (tr.parent != null)
            {
                tr = tr.parent;

                targetComponent = tr.GetComponent<T>();
                if (tr != null)
                    return targetComponent;
            }

            return null;
        }

        /// <summary> 자신을 제외한 부모 게임오브젝트들에서 해당 타입의 컴포넌트들을 모두 찾아 리턴
        /// </summary>
        public static T[] GetComponentsInParentOnly<T>(this GameObject @this) where T : Component
        {
            Transform tr = @this.transform;

            // 부모가 없어 루트인 경우
            if (tr.parent == null)
                return null;


            List<T> parentComponentList = new List<T>();
            T[] targets;

            while (tr.parent != null)
            {
                tr = tr.parent;

                targets = tr.GetComponents<T>();
                if (targets != null && targets.Length != 0)
                    foreach (var target in targets)
                    {
                        parentComponentList.Add(target);
                    }
            }

            return parentComponentList.ToArray();
        }

        #endregion // ==========================================================
    }
}