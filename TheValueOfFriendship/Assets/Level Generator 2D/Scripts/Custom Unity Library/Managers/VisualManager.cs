namespace CustomUnityLibrary
{
    using UnityEngine;

    public class VisualManager : ManagerBehaviour<VisualManager>
    {
        public void Play(ParticleSystem particleSystem, Vector3 position)
        {
            var viewport = Camera.main.GetViewport();
            if (viewport.Contains(position))
            {
                var particleSystemInstance = Instantiate(particleSystem, position, Quaternion.identity) as ParticleSystem;
                GameObjectUtility.ChildCloneToContainer(particleSystemInstance.gameObject);
                if (!particleSystemInstance.isPlaying)
                {
                    particleSystemInstance.Play();
                }
                Destroy(particleSystemInstance.gameObject, particleSystemInstance.duration);
            }
        }
    }
}