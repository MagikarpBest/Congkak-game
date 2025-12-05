using System;
using UnityEngine;

public static class EventBus 
{
    // Pit UI
    public static Action<int> OnPitClicked;
    public static Action OnBoardUpdated;

    // Game Manager
    public static Action OnBoardManagerReady;
}
