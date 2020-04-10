using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.Attributes
{
    /// <summary> 
    /// <para/> 날짜 : 2020-04-10 PM 4:57:10
    /// <para/> 설명 : Find 종류의 메소드 구현
    /// <para/> 
    /// </summary>
    public static class FindHelper
    {
        /// <summary> 현재 씬 내에서 타겟 타입의 컴포넌트를 찾아 리턴
        /// <para/> * 씬 내에 해당 컴포넌트가 존재하지 않을 경우 null 리턴
        /// </summary>
        public static T Find<T>() where T : Component
        {
            return Object.FindObjectOfType<T>();
        }

        /// <summary> 현재 씬 내에서 타겟 타입의 컴포넌트를 모두 찾아 리턴
        /// <para/> * 씬 내에 해당 컴포넌트가 하나도 존재하지 않을 경우 null 리턴
        /// </summary>
        public static T[] FindAll<T>() where T : Component
        {
            return Object.FindObjectsOfType<T>();
        }

        /// <summary> 현재 씬 내에서 타겟 타입의 컴포넌트를 찾아 리턴
        /// <para/> * 씬 내에 해당 컴포넌트가 존재하지 않을 경우,
        /// <para/> 지정한 이름(NewGoName)으로 게임 오브젝트를 생성한 뒤 컴포넌트 추가
        /// </summary>
        public static T FindOrAdd<T>(string newName) where T : Component
        {
            if (newName.Length == 0)
            {
                newName = typeof(T).ToString() + " Container";
                int dotIndex = newName.LastIndexOf('.');

                if(dotIndex > -1)
                    newName = newName.Substring(dotIndex + 1);
            }

            T target = Object.FindObjectOfType<T>();

            if (target == null)
            {
                GameObject go = new GameObject(newName);
                target = go.AddComponent<T>();
            }

            return target;
        }

        /// <summary> 현재 씬 내에서 지정한 이름의 게임 오브젝트를 찾아, 타겟 멤버 타입의 컴포넌트를 찾아 리턴
        /// <para/> * 지정한 이름의 게임 오브젝트가 존재하지 않거나, 해당 컴포넌트가 존재하지 않을 경우 null 리턴
        /// </summary>
        public static T FindByName<T>(string name) where T : Component
        {
            GameObject targetGO = GameObject.Find(name);
            if (targetGO == null) return null;

            T target = targetGO.GetComponent<T>();

            return target;
        }

        /// <summary> 현재 씬 내에서 지정한 이름의 게임 오브젝트를 찾아, 타겟 멤버 타입의 컴포넌트를 찾아 리턴
        /// <para/> * 지정한 이름의 게임 오브젝트가 존재하지 않을 경우, 씬 내에 새롭게 생성
        /// <para/> * 지정한 이름의 게임 오브젝트 내에 해당 컴포넌트가 존재하지 않을 경우, 컴포넌트를 새롭게 추가
        /// </summary>
        public static T FindByNameOrAdd<T>(string name) where T : Component
        {
            GameObject targetGO = GameObject.Find(name);
            if (targetGO == null)
            {
                if (name.Length == 0)
                {
                    name = typeof(T).ToString() + " Container";
                    int dotIndex = name.LastIndexOf('.');

                    if (dotIndex > -1)
                        name = name.Substring(dotIndex + 1);
                }

                targetGO = new GameObject(name);
            }

            T target = targetGO.GetComponent<T>();
            if (target == null)
                target = targetGO.AddComponent<T>();

            return target;
        }
    }
}
