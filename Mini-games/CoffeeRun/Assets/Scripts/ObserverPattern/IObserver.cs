using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    //subject uses this method to communicate with the observer
    public void OnNotify(PlayerActions action); 
}
