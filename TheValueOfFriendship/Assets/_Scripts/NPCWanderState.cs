using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class WanderState : INPCState
{
    public INPCState DoState(NPCSearch_ClassBased npc)
    {
        if (npc.navAgent == null)
        {
            npc.nextLocation = npc.transform.position;
            npc.navAgent = npc.GetComponent<NavMeshAgent>();
        }

        DoWander(npc);
        if (npc.startFollow)
            return npc.followState;
        else
            return npc.wanderState;
    }

    private void DoWander(NPCSearch_ClassBased npc)
    {
        //if close choose next location
        if (npc.navAgent.remainingDistance < 1f)
        {
            Vector3 random = Random.insideUnitSphere * npc.wanderDistance;
            random.z = 0f;
            npc.nextLocation = npc.navAgent.transform.position + random;

            if (NavMesh.SamplePosition(npc.nextLocation, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                npc.nextLocation = hit.position;
                npc.navAgent.SetDestination(npc.nextLocation);
            }
        }
    }
}