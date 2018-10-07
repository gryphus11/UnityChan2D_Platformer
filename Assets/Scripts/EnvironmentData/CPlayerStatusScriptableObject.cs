using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerStatusScriptableObject : CResettableScriptableObject
{
    public float hp = 1000.0f;

#if UNITY_EDITOR
    private const string PATH = "Assets/Resources/PlayerStatus.asset";
    [UnityEditor.MenuItem("Assets/Create/ScriptableObject/Create Player Status")]
    private static void CreatePlayerData()
    {
        CPlayerStatusScriptableObject playerData = CreateInstance<CPlayerStatusScriptableObject>();

        UnityEditor.AssetDatabase.CreateAsset(playerData, PATH);
        UnityEditor.AssetDatabase.ImportAsset(PATH);
    }
#endif
}
