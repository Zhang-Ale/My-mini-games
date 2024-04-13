using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public int MaxHealth = 100;
    public int Damage = 10;
    public int CriticalDamage = 25; 
    public GameObject EnemyPrefab; 
}
