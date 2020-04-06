using UnityEngine;
using UnityEditor;

/// <summary> 
/// <para/> 플레이 모드가 시작될 때 싱글톤 오브젝트를 생성해주는 클래스
/// <para/> ---------------------------------------------------------------
/// <para/> * 사용법(싱글톤 클래스 블록 내부에 작성)
/// <para/> .
/// <para/> [UnityEditor.InitializeOnEnterPlayMode]
/// <para/> public static void OnEnterPlayMode() => SingletonAutoLoader.Run&lt;싱글톤클래스타입&gt;();
/// </summary>
public static class SingletonAutoLoader
{
    // 사용법
    // [UnityEditor.InitializeOnEnterPlayMode]
    // public static void OnEnterPlayMode() => SingletonAutoLoader.Run<싱글톤클래스타입>();

    public static void Run<SingletonType>() where SingletonType : Component
    {
        EditorApplication.update += CreateSingletonInstance<SingletonType>;
    }

    private static void CreateSingletonInstance<SingletonType>() where SingletonType : Component
    {
        if (Application.isPlaying && Object.FindObjectOfType<SingletonType>() == null)
        {
            new GameObject("[Singleton] " + typeof(SingletonType).Name).AddComponent<SingletonType>();
            Debug.Log($"Create Singleton Instance : {typeof(SingletonType).Name}");
        }

        EditorApplication.update -= CreateSingletonInstance<SingletonType>;
    }
}