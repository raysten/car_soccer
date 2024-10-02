using System;
using TMPro;
using UnityEngine;

public class UiDebug : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;
    
    private static UiDebug _instance;

    // Public static property to access the instance
    public static UiDebug Instance
    {
        get
        {
            // If the instance is null, find the singleton instance in the scene
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<UiDebug>();

                // If there is no instance in the scene, create one
                if (_instance == null)
                {
                    var singletonObject = new GameObject("DebugUiSingleton");
                    _instance = singletonObject.AddComponent<UiDebug>();
                }
            }

            return _instance;
        }
    }

    // Optional: Prevent multiple instances by destroying duplicates
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
        else
        {
            _instance = this; // Set the instance to this
            DontDestroyOnLoad(gameObject); // Make this instance persistent
        }
    }

    public void WriteText(string message)
    {
        _text.SetText(message);
    }
}