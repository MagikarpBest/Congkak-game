using System;

public static class EventBus 
{
    // Pit UI
    public static Action<int> OnPitClicked;
    public static Action OnBoardUpdated;
    public static Action<bool> OnTurnUpdated;

    // Game Manager
    public static Action OnBoardManagerReady;
}
