using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameEvent { }
public static  class EventManager 
{
    private static Dictionary<Type , Action<GameEvent> > dict = new Dictionary<Type, Action<GameEvent> >();
    private static Dictionary<Delegate , Action<GameEvent> > dictLookup = new Dictionary<Delegate, Action<GameEvent> >();

    /// <summary>
    /// �ھ�Event����,�W�[Event(���Ƥ��i�K�[)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="evt"></param>
    public static void AddEvents<T>(Action<T> evt) where T : GameEvent
    {
        if (!dictLookup.ContainsKey(evt))
        {
            Action<GameEvent> newAction = (e) => evt((T)e);
            dictLookup[evt] = newAction;

            if (dict.TryGetValue(typeof(T), out Action<GameEvent> internalAction))
                dict[typeof(T)] = internalAction += newAction;
            else
                dict[typeof(T)] = newAction;
        }
    }
    /// <summary>
    /// ����Event
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="evt"></param>
    public static void RemoveListener<T>(Action<T> evt) where T : GameEvent
    {
        if (dictLookup.TryGetValue(evt, out var action))
        {
            if (dict.TryGetValue(typeof(T), out var tempAction))
            {
                tempAction -= action;
                if (tempAction == null)
                    dict.Remove(typeof(T));
                else
                    dict[typeof(T)] = tempAction;
            }
            dictLookup.Remove(evt);
        }
    }
    /// <summary>
    /// �M���Ҧ�Events
    /// </summary>
    public static void ClearEvents()
    {
        dict.Clear();
        dictLookup.Clear();
    }
    /// <summary>
    /// �s��
    /// </summary>
    /// <param name="evt"></param>
    public static void Broadcast(GameEvent evt)
    {
        if (dict.TryGetValue(evt.GetType(), out Action<GameEvent> action))
            action.Invoke(evt);    
    }
}
