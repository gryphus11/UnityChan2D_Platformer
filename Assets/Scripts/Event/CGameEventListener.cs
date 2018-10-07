using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CGameEventListener : MonoBehaviour {

    public CGameEventScriptableObject events;
    public UnityEvent response;

    private void OnEnable()
    {
        events.RegisterListener(this);
    }

    private void OnDisable()
    {
        events.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        response.Invoke();
    }
}
