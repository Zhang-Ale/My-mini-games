namespace CustomUnityLibrary
{
    using UnityEngine;

    /// <summary>
    /// Plays the provided Particle System when the GameObject is destroyed
    /// </summary>
    public class ParticleSystemOnDestroy : MonoBehaviour
    {
        [Tooltip("The particle system which will be trigged when game object is destroyed")]
        [SerializeField]
        private ParticleSystem particleSystemPrefab;

        private bool applicationClosing;

        void OnDestroy()
        {
            if (applicationClosing)
            {
                return;
            }
            var renderer = GetComponent<Renderer>();
            if (!renderer || renderer.isVisible)
            {
                var particleSystem = Instantiate(particleSystemPrefab, transform.position, Quaternion.identity) as ParticleSystem;
                GameObjectUtility.ChildCloneToContainer(particleSystem.gameObject);
                Destroy(particleSystem.gameObject, particleSystem.duration);
            }
        }

        void OnApplicationQuit()
        {
            applicationClosing = true;
        }
    }
}