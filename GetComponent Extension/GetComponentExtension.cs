using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Rito
{
    // 2020. 03. 20. 작성
    // 2020. 03. 20. 기능 테스트 완료

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
        public static Component GetOrAddComponent(this GameObject @this, Type p_type)
        {
            Component target = @this.GetComponent(p_type);

            if (target == null)
                target = @this.AddComponent(p_type);

            return target;
        }

        #endregion // ==========================================================

        #region Get or Add Component in Children

        /// <summary> 
        /// <para/> 자신 및 자식 게임오브젝트들에서 해당 타입의 컴포넌트를 찾고,
        /// <para/> [1] 해당 컴포넌트를 찾지 못하면 지정된 이름의 자식 게임오브젝트에 생성 및 추가한다.
        /// <para/> [2] 지정된 이름의 자식 게임오브젝트가 없으면, 해당 이름으로 자식 게임오브젝트를 생성한 뒤 컴포넌트를 추가한다.
        /// </summary>
        public static T GetOrAddComponentInChildren<T>(this GameObject @this, in string p_childObjectName) where T : Component
        {
            T target = @this.GetComponentInChildren<T>();

            if (target != null)
                return target;

            // 1. 못찾은 경우 : 지정된 이름의 자식 게임오브젝트 찾기
            Transform targetChild = @this.transform.Find(p_childObjectName);

            if (targetChild != null)
                return targetChild.gameObject.AddComponent<T>();

            // 2. 해당 이름의 자식 게임오브젝트도 없는 경우 : 자식 게임오브젝트를 생성하고 컴포넌트 추가
            GameObject newChild = new GameObject(p_childObjectName);
            newChild.transform.SetParent(@this.transform);
            target = newChild.AddComponent<T>();

            return target;
        }

        /// <summary> 
        /// <para/> 자신 및 자식 게임오브젝트들에서 해당 타입의 컴포넌트를 찾고,
        /// <para/> [1] 해당 컴포넌트를 찾지 못하면 지정된 이름의 자식 게임오브젝트에 생성 및 추가한다.
        /// <para/> [2] 지정된 이름의 자식 게임오브젝트가 없으면, 해당 이름으로 자식 게임오브젝트를 생성한 뒤 컴포넌트를 추가한다.
        /// </summary>
        public static Component GetOrAddComponentInChildren(this GameObject @this, Type p_type, in string p_childObjectName)
        {
            Component target = @this.GetComponentInChildren(p_type);

            if (target != null)
                return target;

            // 1. 못찾은 경우 : 지정된 이름의 자식 게임오브젝트 찾기
            Transform targetChild = @this.transform.Find(p_childObjectName);

            if (targetChild != null)
                return targetChild.gameObject.AddComponent(p_type);

            // 2. 해당 이름의 자식 게임오브젝트도 없는 경우 : 자식 게임오브젝트를 생성하고 컴포넌트 추가
            GameObject newChild = new GameObject(p_childObjectName);
            newChild.transform.SetParent(@this.transform);
            target = newChild.AddComponent(p_type);

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
        public static T GetOrAddComponentInParent<T>(this GameObject @this, in string p_parentObjectName) where T : Component
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
                if(parent.gameObject.name.Equals(p_parentObjectName))
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
        public static Component GetOrAddComponentInParent(this GameObject @this, Type p_type, in string p_parentObjectName)
        {
            Component target = @this.GetComponentInParent(p_type);

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
                if (parent.gameObject.name.Equals(p_parentObjectName))
                    return parent.gameObject.AddComponent(p_type);

                parent = parent.parent;
            }

            // 5. 해당 이름의 부모가 없는 경우 : null 리턴
            return null;
        }

        #endregion // ==========================================================
    }
}