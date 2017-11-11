using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager {

    public delegate void EventCallback(params object[] paramsContainer);

    private static EventCallback[] _allEvents;

    public static void AddEventListener(Events eventNum, EventCallback listener)
    {
        if (_allEvents == null) _allEvents = new EventCallback[(int)Events.NONE];

        _allEvents[(int)eventNum] += listener;
    }

    public static void RemoveEventListener(Events eventNum, EventCallback listener)
    {
        if (_allEvents != null)
            _allEvents[(int)eventNum] -= listener;
    }

    public static void DispatchEvent(Events eventNum)
    {
        DispatchEvent(eventNum, null);
    }

    public static void DispatchEvent(Events eventNum, params object[] paramsContainer)
    {
        if (_allEvents != null && _allEvents[(int)eventNum] != null)
            _allEvents[(int)eventNum](paramsContainer);
    }

    public static void ClearAllEvents()
    {
        _allEvents = null;
    }
}

public enum Events
{
    /// <summary>Dispatched when the attack animation ends.</summary>
    ON_ATTACK_EXIT,

    /// <summary>Dispatched when the ultimate attack animation ends.</summary>
    ON_ULTIMATE_EXIT,

    /// <summary>Dispatched when punch hits enemy.</summary>
    ON_HIT,

    /// <summary>Dispatched when a change in life has happened.</summary>
    ON_LIFE_UPDATE,

    NONE
}
