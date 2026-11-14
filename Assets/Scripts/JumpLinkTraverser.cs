using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class JumpLinkTraverser : MonoBehaviour
{
    public float arcHeight = 1.5f;
    public float duration = 0.6f;

    NavMeshAgent agent;
    bool jumping;
    float t;
    Vector3 a, b;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
    }

    void Update()
    {
        if (agent.isOnOffMeshLink && !jumping)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            a = transform.position;
            b = data.endPos + Vector3.up * agent.baseOffset;
            jumping = true;
            t = 0f;
        }
        if (!jumping) return;

        t += Time.deltaTime / duration;
        float k = Mathf.Clamp01(t);
        transform.position = Vector3.Lerp(a, b, k) + Vector3.up * (4f * arcHeight * k * (1f - k));

        if (t >= 1f)
        {
            agent.CompleteOffMeshLink();
            jumping = false;
        }
    }
}