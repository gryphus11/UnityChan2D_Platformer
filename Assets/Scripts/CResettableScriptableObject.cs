using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CResettableScriptableObject : ScriptableObject
{
    [SerializeField]
    private bool _isDontUnloadUnusedAsset = false;

#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CallBeforePlayScene()
    {
        // 플레이 전에 SaveProject 를 추천합니다.
        //UnityEditor.AssetDatabase.SaveAssets();
    }
#endif

    protected virtual void OnEnable()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode == true)
        {
            UnityEditor.EditorApplication.playModeStateChanged += x =>
            {
                if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode == false)
                {
                    Debug.Log("Play Mode : " + x);
                    Resources.UnloadAsset(this);
                }
            };
        }
#endif
        // 빌드후 런타임시 Scriptable Object 를 참조하지 않는 Scene 이 있어도 초기값으로 되돌리지 않음.
        if (_isDontUnloadUnusedAsset == true)
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }
        else
        {
            hideFlags = HideFlags.None;
        }
    }
}
