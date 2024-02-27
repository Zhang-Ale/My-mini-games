using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCFollowState : INPCState
{

    public INPCState DoState(NPCSearch_ClassBased npc)
    {
        if (npc.navAgent == null)
        {
            npc.nextLocation = npc.transform.position;
            npc.navAgent = npc.GetComponent<NavMeshAgent>();
            npc.navAgent.updateRotation = false;
            npc.navAgent.updateUpAxis = false;
        }

        Follow(npc);
        return npc.followState;
    }
    private void Follow(NPCSearch_ClassBased npc)
    {
        npc.target = GameObject.Find("Player").transform;
        npc.navAgent.SetDestination(npc.target.position);

        if (npc.navAgent.remainingDistance < 1f)
        {
            Vector3 random = Random.insideUnitSphere * npc.wanderDistance;
            random.z = 0f;
            npc.nextLocation = npc.navAgent.transform.position + random;

            if (NavMesh.SamplePosition(npc.target.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                npc.navAgent.speed = 0;
                npc.SpawnFriend(npc.target); 
            }
        }
    }
}
