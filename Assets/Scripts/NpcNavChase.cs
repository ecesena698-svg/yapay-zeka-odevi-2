using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(LineRenderer))]
public class NpcNavChase : MonoBehaviour
{
    public Transform target;
    public float repathInterval = 0.25f;
    public bool slowInMud = true; // Ek görev: Çamurda yavaþlasýn mý? (+10 Puan)

    NavMeshAgent agent;
    LineRenderer line;
    float timer;
    float baseSpeed;
    int mudArea;
    NavMeshPathStatus lastStatus = NavMeshPathStatus.PathComplete;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        line = GetComponent<LineRenderer>();
        line.widthMultiplier = 0.15f;
        baseSpeed = agent.speed;
        mudArea = NavMesh.GetAreaFromName("Camur");
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f && !agent.isOnOffMeshLink && target != null)
        {
            agent.SetDestination(target.position);
            timer = repathInterval;
        }

        if (!agent.pathPending && agent.pathStatus != lastStatus)
        {
            lastStatus = agent.pathStatus;
            Debug.Log($"Yol durumu: {lastStatus}");
        }

        // EK GÖREV (+10 Puan): Çamurda hýzý yarýya düþür
        if (slowInMud && mudArea >= 0)
        {
            int currentArea = GetCurrentArea();
            agent.speed = (currentArea == mudArea) ? baseSpeed * 0.5f : baseSpeed;
        }

        Vector3[] corners = agent.path.corners;
        line.positionCount = corners.Length;
        for (int i = 0; i < corners.Length; i++)
            line.SetPosition(i, corners[i] + Vector3.up * 0.2f);
    }

    int GetCurrentArea()
    {
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            for (int i = 0; i < 32; i++)
                if ((hit.mask & (1 << i)) != 0) return i;
        }
        return 0;
    }

    void OnDisable()
    {
        if (line != null) line.positionCount = 0;
    }
}