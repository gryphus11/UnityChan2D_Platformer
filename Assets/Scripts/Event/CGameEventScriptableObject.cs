using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObject/Create GameEvent")]
public class CGameEventScriptableObject : CResettableScriptableObject
{
    private List<CGameEventListener> listeners = new List<CGameEventListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; --i)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(CGameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(CGameEventListener listener)
    {
        listeners.Remove(listener);
    }
}
