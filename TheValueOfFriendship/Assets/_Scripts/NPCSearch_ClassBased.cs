using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCSearch_ClassBased : MonoBehaviour
{
    [SerializeField]
    private string currentStateName;
    private INPCState currentState;

    public WanderState wanderState = new WanderState();
    public NPCFollowState followState = new NPCFollowState();
    public bool startFollow;
    public GameObject[] friend;
    public GameObject newObject;

    public NavMeshAgent navAgent;
    public int ran;
    public float random;
    public Vector3 nextLocation;
    public Transform target;
    public float wanderDistance = 10f;
    public float pickUpDistance = 25f;

    //public static List<GameObject> pickUps = new List<GameObject>();
    //public static List<GameObject> critters = new List<GameObject>();

    private void OnEnable()
    {
        currentState = wanderState;
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.DoState(this);
        currentStateName = currentState.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            startFollow = true;
        }
    }

    public void SpawnFriend(Transform parent)
    {
        ran = Random.Range(0, friend.Length);
        GameObject friendToSpawn = friend[ran];
        //random = Random.Range(friendToSpawn.stayValueMin, friendToSpawn.stayValueMax);
        newObject = Instantiate(friendToSpawn, transform.position, Quaternion.identity);
        
        Destroy(this.gameObject); 
    }
}