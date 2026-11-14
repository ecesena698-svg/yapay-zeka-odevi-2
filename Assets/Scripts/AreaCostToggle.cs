using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Unity.AI.Navigation;

[RequireComponent(typeof(NavMeshAgent))]
public class AreaCostToggle : MonoBehaviour
{
    public NavMeshLink jumpLink;
    public string mudAreaName = "Camur";
    public string waterAreaName = "SigSu";
    public float[] mudCosts = { 1f, 2f, 5f, 10f };
    public float waterCost = 2f; // Sýð su maliyeti

    NavMeshAgent agent;
    InputAction cycleMud, toggleLink;
    int mudArea, waterArea, costIndex;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        cycleMud = InputSystem.actions.FindAction("CycleMudCost");
        toggleLink = InputSystem.actions.FindAction("ToggleLink");
        mudArea = NavMesh.GetAreaFromName(mudAreaName);
        waterArea = NavMesh.GetAreaFromName(waterAreaName);
    }

    void Start() => ApplyCosts();

    void Update()
    {
        if (cycleMud != null && cycleMud.WasPressedThisFrame())
        {
            costIndex = (costIndex + 1) % mudCosts.Length;
            ApplyCosts();
        }
        if (toggleLink != null && toggleLink.WasPressedThisFrame() && jumpLink != null)
            jumpLink.activated = !jumpLink.activated;
    }

    void ApplyCosts()
    {
        if (mudArea >= 0) agent.SetAreaCost(mudArea, mudCosts[costIndex]);
        if (waterArea >= 0) agent.SetAreaCost(waterArea, waterCost);
    }

    void OnGUI()
    {
        string link = (jumpLink != null && jumpLink.activated) ? "açýk" : "kapalý";
        GUI.Label(new Rect(10, 60, 600, 25), $"[M] Çamur: {mudCosts[costIndex]} | Sýð Su: {waterCost} | [J] Baðlantý: {link} | [K] Kapý");
    }
}