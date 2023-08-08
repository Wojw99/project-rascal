using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSignalizer : MonoBehaviour
{
    #region Singleton

    public static EventSignalizer instance;

    private void Awake() {
        instance = this;
    }

    private EventSignalizer() {

    }

    #endregion

    private string currentSignal = "";

    // public event EventHandler<OnSignalChangedEventArgs> OnSignalChanged;
    public event SignalEventDelegate OnSignalChanged;
    
    public string CurrentSignal { 
        set {
            currentSignal = value; 
            OnSignalChanged?.Invoke(value);
        } 
    }

    public void Clear() {
        currentSignal = "";
    }
}

public delegate void SignalEventDelegate(string signal);

// public class OnSignalChangedEventArgs : EventArgs {
//     public string newSignal;

//     public OnSignalChangedEventArgs(string newSignal)
//     {
//         this.newSignal = newSignal;
//     }
// }
