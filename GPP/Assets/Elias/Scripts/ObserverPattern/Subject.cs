using System.Collections.Generic;
using UnityEngine;

public abstract class Subject : MonoBehaviour
{
    private List<IObserver> _Observers = new List<IObserver>();

    public void AddObserver(IObserver observer)
    {
        _Observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer) 
    {
        _Observers.Remove(observer);
    }

    public void RemoveAllObservers()
    {
        _Observers.Clear();
    }

    protected void NotifyObservers(string action)
    {
        _Observers.ForEach((observer) =>
        {
            observer.OnNotify(action);
        });
    }
}
