using UnityEngine;
using UnityEngine.AI;

public class VampireController : MonoBehaviour
{
    public BrainSpawnManager brainSpawner1, brainSpawner2, brainSpawner3, brainSpawner4;
    public GameObject brain1Prefab, brain2Prefab, brain3Prefab, brain4Prefab;
    private NavMeshAgent _agent;
    public GameObject targetPoint;
    public GameObject minimapIcon;
    public GameObject brainPrefab;
    public LayerMask layerMask;
    public AudioSource AS; 
    public bool collected; 
    ScoreManager SM;
    public ParticleSystem particleEffect;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        SM = GameObject.Find("Canvas").GetComponent<ScoreManager>();
      }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hasHit = Physics.Raycast(ray, out hit, 100, layerMask);

            if (hasHit)
            {
                _agent.destination = hit.point;
                targetPoint.transform.position = hit.point;
            }
        }

        minimapIcon.transform.position = new Vector3(transform.position.x, minimapIcon.transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "BrainArea1(Clone)")
        {
            collected = true; 
            SM.AddPointToPlayer();
            AS.Play();
            Destroy(other.gameObject);
            particleEffect.Play();
            brainSpawner1.StartCoroutine("SpawnOneBrain");
        }

        if (other.gameObject.name == "BrainArea2(Clone)")
        {
            collected = true;
            SM.AddPointToPlayer();
            AS.Play();
            Destroy(other.gameObject);
            particleEffect.Play();
            brainSpawner2.StartCoroutine("SpawnOneBrain");
        }

        if (other.gameObject.name == "BrainArea3(Clone)")
        {
            collected = true;
            SM.AddPointToPlayer();
            AS.Play();
            Destroy(other.gameObject);
            particleEffect.Play();
            brainSpawner3.StartCoroutine("SpawnOneBrain");
        }

        if (other.gameObject.name == "BrainArea4(Clone)")
        {
            collected = true;
            SM.AddPointToPlayer();
            AS.Play();
            Destroy(other.gameObject);
            particleEffect.Play();
            brainSpawner4.StartCoroutine("SpawnOneBrain");
        }
    }
}
