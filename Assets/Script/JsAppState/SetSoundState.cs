using System;
using UnityEngine;

public class SetSoundState : MonoBehaviour
{
    /// <summary> JS 브릿지 </summary>
    [SerializeField]
    private JsAppStateReceiver jsAppStateReceiver;
    void Start()
    {
        jsAppStateReceiver.AddListener(OnChangeJsState);
    }

    private void OnDestroy()
    {
        jsAppStateReceiver.RemoveListener(OnChangeJsState);
    }

    private void OnChangeJsState(AppState appState)
    {
        switch (appState)
        {
            case AppState.BACKGROUND:
                AudioListener.pause = true;
                break;
            case AppState.FOREGROUND:
                AudioListener.pause = false;
                break;
        }
    }
}