namespace LevelGenerator2D
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;

    [Serializable]
    public class Spawner : MonoBehaviour
    {
        /// <summary>
        /// Delegate for spawner spawning
        /// </summary>
        public delegate void Spawning(GameObject[] spawns);

        /// <summary>
        /// Event called when spawnable spawns
        /// </summary>
        public static event Spawning OnSpawn;

        private const float DefaultWeight = 1.0f;
        private const float MinSpawnChance = 0.0f;
        private const float MaxSpawnChance = 1.0f;
        private static readonly Color OutlineColor = Color.yellow;

        [Tooltip("Chances that any GameObject will spawn")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float spawnChance = 1.0f;

        [Tooltip("List of GameObjects which can be spawned")]
        [SerializeField]
        private List<GameObject> spawnables = new List<GameObject>();

        [Tooltip("Spawn on start, opposed to waiting for spawn to be called manually")]
        [SerializeField]
        private bool spawnOnStart = false;

        [Tooltip("Minimum number of GameObjects this spawner will spawn")]
        [SerializeField]
        [Range(1, 25)]
        private int minSpawns = 1;

        [Tooltip("Maximum number of GameObjects this spawner will spawn")]
        [SerializeField]
        [Range(1, 25)]
        private int maxSpawns = 1;

        [Tooltip("Particle effect used when spawning")]
        [SerializeField]
        private ParticleSystem visualEffect;

        [Tooltip("Weights associated with the spawnable Game Objects")]
        [SerializeField]
        private List<float> weights = new List<float>();

        private Vector2 maxSpawnableSize = Vector2.zero;

        void Start()
        {
            if (spawnOnStart)
            {
                Spawn();
            }
        }

        void OnDrawGizmosSelected()
        {
            OutlineLargestMaximumSize();
        }

        /// <summary>
        /// Gets the minimum number of GameObjects this spawner will spawn
        /// </summary>
        /// <returns>Number of game objects</returns>
        public int GetMinSpawns()
        {
            return minSpawns;
        }

        /// <summary>
        /// Sets the minimum number of GameObjects that this spawner will spawn
        /// </summary>
        /// <param name="minSpawns">Number of game objects</param>
        public void SetMinSpawns(int minSpawns)
        {
            this.minSpawns = minSpawns;
        }

        /// <summary>
        /// gets the maximum number of game objects that this spawner will spawn
        /// </summary>
        /// <returns>Max number of gameobjects</returns>
        public int GetMaxSpawns()
        {
            return maxSpawns;
        }

        /// <summary>
        /// Sets the maximum number of gameobjects that will be spawned
        /// </summary>
        /// <param name="maxSpawns">Maximum number of gameobjects</param>
        public void SetMaxSpawns(int maxSpawns)
        {
            this.maxSpawns = maxSpawns;
        }

        /// <summary>
        /// Gets whether or not the spawner will spawn on start
        /// </summary>
        /// <returns>If spawns on start</returns>
        public bool GetSpawnOnStart()
        {
            return spawnOnStart;
        }

        /// <summary>
        /// Set whether or not the spawner should spawn on start
        /// </summary>
        /// <param name="spawnOnStart">Whether or not to spawn on start</param>
        public void SetSpawnOnStart(bool spawnOnStart)
        {
            this.spawnOnStart = spawnOnStart;
        }

        /// <summary>
        /// Gets the chance a GameObject will be spawned
        /// </summary>
        /// <returns>Chance of spawn</returns>
        public float GetSpawnChance()
        {
            return spawnChance;
        }

        /// <summary>
        /// Sets the chance of a GameObject spawning
        /// </summary>
        /// <param name="spawnChance">Spawn chance</param>
        public void SetSpawnChance(float spawnChance)
        {
            this.spawnChance = spawnChance;
        }

        /// <summary>
        /// Gets the spawnable game objects for this spawner
        /// </summary>
        /// <returns>Spawnable game objects</returns>
        public List<GameObject> GetSpawnables()
        {
            return spawnables;
        }

        /// <summary>
        /// Gets the weights for the spawnable game objects
        /// </summary>
        /// <returns>Weights</returns>
        public List<float> GetWeights()
        {
            return weights;
        }

        /// <summary>
        /// Adds a spawnable Game Object
        /// </summary>
        /// <param name="weight">Rate of game object's spawning</param>
        public void AddSpawnable(GameObject spawnable)
        {
            if (spawnables.Contains(spawnable))
            {
                Debug.LogWarning(gameObject + " already contains " + spawnable.gameObject + ", so it has not been added.", spawnable.gameObject);
                return;
            }
            spawnables.Add(spawnable);
            weights.Add(DefaultWeight);
        }

        /// <summary>
        /// Removes a spawnable game object from the options
        /// </summary>
        /// <param name="spawnable">Spawnable to remove</param>
        public void RemoveSpawnable(GameObject spawnable)
        {
            if (!spawnables.Contains(spawnable))
            {
                Debug.LogError("Attempting to remove " + spawnable + ", but " + gameObject + " does not contain it!", gameObject);
            }
            int index = spawnables.IndexOf(spawnable);
            spawnables.RemoveAt(index);
            weights.RemoveAt(index);
        }

        /// <summary>
        /// Sets the weight for a spawnable game object
        /// </summary>
        /// <param name="spawnable">Spawnable to set weight for</param>
        /// <param name="weight">Weight to set</param>
        public void SetWeight(GameObject spawnable, float weight)
        {
            if (!spawnables.Contains(spawnable))
            {
                Debug.LogError("Attemping to set the weight for " + spawnable + ", but it does not exist in " + gameObject + "!", gameObject);
            }
            int index = spawnables.IndexOf(spawnable);
            weights[index] = weight;
        }

        /// <summary>
        /// Spawns random game objects based on their weights
        /// </summary>
        /// <returns>GameObjects which spawned</returns>
        public GameObject[] Spawn()
        {
            var spawnedGameObjects = new List<GameObject>();
            int spawnCount = UnityEngine.Random.Range(GetMinSpawns(), GetMaxSpawns() + 1);
            for (int i = 0; i < spawnCount; i++)
            {
                float spawnChanceValue = UnityEngine.Random.Range(MinSpawnChance, MaxSpawnChance);
                if (spawnChanceValue > spawnChance)
                {
                    continue;
                }
                float randomValue = UnityEngine.Random.Range(0.0f, weights.Sum());
                float accumulatedValue = 0.0f;
                for (int j = 0; j < weights.Count; j++)
                {
                    accumulatedValue += weights[j];
                    if (randomValue < accumulatedValue)
                    {
                        var spawnablePrefab = spawnables[j];
                        var spawn = Instantiate(spawnablePrefab, transform.position, Quaternion.identity) as GameObject;
                        spawn.transform.SetParent(transform.parent);
                        spawnedGameObjects.Add(spawn);
                        break;
                    }
                }
            }
            if (OnSpawn != null)
            {
                OnSpawn(spawnedGameObjects.ToArray());
            }
            return spawnedGameObjects.ToArray();
        }

        /// <summary>
        /// Sets the size for the spawnable outline
        /// </summary>
        public void SetMaxSpawnableSize()
        {
            var maxSize = Vector2.one;
            foreach (var spawnable in spawnables)
            {
                var spriteRenderer = spawnable.GetComponent<SpriteRenderer>();
                if (spriteRenderer)
                {
                    Vector2 spriteSize = spriteRenderer.bounds.size;
                    maxSize = Vector2.Max(spriteSize, maxSize);
                }
                var collider = spawnable.GetComponent<Collider2D>();
                if (collider)
                {
                    var colliderSize = collider.bounds.size;
                    maxSize = Vector2.Max(colliderSize, maxSize);
                }
            }
            maxSpawnableSize = maxSize;
#if UNITY_EDITOR
            SceneView.RepaintAll();
#endif
        }

        /// <summary>
        /// Draws an outline for the largest gameobject's x and largest gameobject's y
        /// </summary>
        private void OutlineLargestMaximumSize()
        {
            Gizmos.color = OutlineColor;
            Gizmos.DrawWireCube(transform.position, maxSpawnableSize);
        }
    }
}