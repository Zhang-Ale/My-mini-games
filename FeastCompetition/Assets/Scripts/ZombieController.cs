using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using System.Collections;
public class ZombieController : MonoBehaviour
{
    public NavMeshAgent _agent;
    public GameObject brain1Prefab, brain2Prefab, brain3Prefab, brain4Prefab;
    private GameObject _destination;
    private GameObject[] _destinations;
    private bool _isSeeking;
    ScoreManager SM;
    public GameObject canvas;
    public LayerMask brainLM; 
    public GameObject zombie;
    public GameObject grave;
    public float smellSense;
    public GameObject minimapIcon;
    public ParticleSystem onCollectParticle;
    public ParticleSystem onAsleepParticle;
    public bool _asleep;
    public GameObject triggerArea;

    BrainSpawnManager brainSpawner1, brainSpawner2, brainSpawner3, brainSpawner4;

    public void SetNextDestination()
    {
        int index = Random.Range(0, _destinations.Length);
        _destination = _destinations[index];
        float randomX = _destination.transform.position.x + Random.Range(-1, 1);
        float randomZ = _destination.transform.position.z + Random.Range(-1, 1);
        Vector3 closeDest = new Vector3(randomX, _destination.transform.position.y, randomZ); 
        _agent.destination = closeDest;           
    }

    private void Start()
    {
        _destinations = GameObject.FindGameObjectsWithTag("Destination");
        brainSpawner1 = GameObject.Find("area1").GetComponent<BrainSpawnManager>();
        brainSpawner2 = GameObject.Find("area2").GetComponent<BrainSpawnManager>();
        brainSpawner3 = GameObject.Find("area3").GetComponent<BrainSpawnManager>();
        brainSpawner4 = GameObject.Find("area4").GetComponent<BrainSpawnManager>();
        SM = GameObject.Find("Canvas").GetComponent<ScoreManager>();
        SetNextDestination(); 
    }

    private void FixedUpdate()
    {      
        Collider[] brainHit = Physics.OverlapSphere(transform.position, smellSense, brainLM);
        if (brainHit != null)
        { 
            _isSeeking = true;
            foreach (Collider brain in brainHit)
            {
                _agent.destination = brain.transform.position;
            }
        }
        else
        {
            _isSeeking = false;
        }

        if (!_isSeeking)
        {
            int index = Random.Range(0, _destinations.Length);
            _destination = _destinations[index];
            var distanceToDestination = Vector3.Distance(transform.position, _destination.transform.position);
            if (distanceToDestination <= .5f)
            {
                SetNextDestination();
            }
        }

        if (_asleep)
        {
            StartCoroutine(WakeUp());
        }
        else
        {
            StopCoroutine(WakeUp());
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(_isSeeking)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
        }
        else
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.1f);
        }
        // Draw a disc displaying the smell sense range for the zombie   
        Gizmos.DrawSphere(transform.position, smellSense);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BrainArea1(Clone)")
        {         
            SM.AddPointToEnemy();
            onCollectParticle.Play();
            Destroy(other.gameObject);
            _asleep = true;
            brainSpawner1.StartCoroutine("SpawnOneBrain");
        }

        if (other.gameObject.name == "BrainArea2(Clone)")
        {
            SM.AddPointToEnemy();
            onCollectParticle.Play();
            Destroy(other.gameObject);
            _asleep = true;
            brainSpawner2.StartCoroutine("SpawnOneBrain");
        }

        if (other.gameObject.name == "BrainArea3(Clone)")
        {
            SM.AddPointToEnemy();
            onCollectParticle.Play();
            Destroy(other.gameObject);
            _asleep = true;
            brainSpawner3.StartCoroutine("SpawnOneBrain");
        }

        if (other.gameObject.name == "BrainArea4(Clone)")
        {
            SM.AddPointToEnemy();
            onCollectParticle.Play();
            Destroy(other.gameObject);
            _asleep = true;
            brainSpawner4.StartCoroutine("SpawnOneBrain");
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

    IEnumerator WakeUp()
    {
        _agent.speed = 0;
        yield return new WaitForSeconds(2.5f);
        MinimapIconMovement MIM = GetComponentInParent<MinimapIconMovement>();
        MIM.changeIcon = true; 
        grave.SetActive(true);
        zombie.SetActive(false);
        onAsleepParticle.gameObject.SetActive(true);
        onAsleepParticle.Play();
        canvas.SetActive(true);
        canvas.GetComponent<WakeUpCount>().enabled = true;
        canvas.GetComponent<WakeUpCount>().timeCount = 27;
        yield return new WaitForSeconds(20);
        onAsleepParticle.Stop();
        onAsleepParticle.gameObject.SetActive(false);
        canvas.SetActive(false);
        canvas.GetComponent<WakeUpCount>().enabled = false; 
        grave.SetActive(false);
        zombie.SetActive(true);
        MIM.changeIcon = false;
        _agent.speed = 2;
        _isSeeking = false;
        SetNextDestination();
        _asleep = false;
    }
}
