using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    OnPowerUpCollect,
    OnShotgunPowerUpCollect
}

public class Observable : MonoBehaviour
{
    protected int moveSpeed = 30;
    [SerializeField] protected bool poweredUp;
    [SerializeField] protected bool shotgunPoweredUp;
    protected float powerUpDuration = 5;
    protected Menu menu;
    protected Spawner spawner;
    protected CharacterController CC;
    protected Rigidbody RB;
    protected NavMeshAgent agent;
    protected PUSpawner pus;
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
    protected void InstantiateParticle(GameObject part)
    {
        Instantiate(part, transform.position, Quaternion.identity);
    }

    protected void SetUp()
    {
        spawner = FindObjectOfType<Spawner>();
        menu = FindObjectOfType<Menu>();
        poweredUp = false;
        CC = GetComponent<CharacterController>();
        RB = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        pus = GameObject.FindGameObjectWithTag("PUS").GetComponent<PUSpawner>();
    }
}
