using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class KillText : MonoBehaviour
{
    public TextMeshProUGUI _score; 
    public int _killEnemyNumber;
    public TriggerEnemy Enemy1, Enemy2, Enemy3; 
    bool _killed;

    void Update()
    {
        if(Enemy1._kill /*|| Enemy2._kill || Enemy3._kill*/)
        {
            
            _killed = true; 
            if(_killed)
            {
                _killEnemyNumber += 1; 
                _score.text = _killEnemyNumber.ToString();
                _killed = false; 
            }
        }
    }
}
