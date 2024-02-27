using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    void OnNotify(GameObject GameObj, Action actionType);
}
//Either the one on top, or the one below
public abstract class Observer : MonoBehaviour
{
    public abstract void OnNotify(GameObject GameObj, Action actionType);
}

public enum Action
{
    OnPlayerShoot,
    OnEnemyDestroy, 
    OnPowerUpCollect
}

public class Observable : MonoBehaviour
{
    //a collection of all the observers of this subject
    protected List<IObserver> _observers = new List<IObserver>();

    //add the observer to the list
    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    //remove the observer from the list
    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    //notify all observers
    public void Notify(GameObject GameObj, Action actionType)
    {
        foreach (IObserver observer in _observers)
        {
            observer.OnNotify(GameObj, actionType);
        }
    }
}
