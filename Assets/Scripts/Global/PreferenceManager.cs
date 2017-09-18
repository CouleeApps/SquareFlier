using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PreferenceManager : MonoBehaviour
{
    public static PreferenceManager CurrentManager;

    public bool ShowTimer = false;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        CurrentManager = this;
    }

    void Update()
    {
        
    }
}
