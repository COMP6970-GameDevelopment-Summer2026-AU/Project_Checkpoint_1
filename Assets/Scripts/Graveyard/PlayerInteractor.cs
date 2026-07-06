// PlayerInteractor.cs — finds the nearest Harvestable in range each frame,
// shows an interaction prompt (Module 7 UI), and harvests it when the player
// presses E. The prompt text is set by the GraveyardManager's HUD.

using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction")]
    public float interactRange = 2.5f;
    public LayerMask harvestableMask = ~0;

    [Tooltip("Seconds per harvest swing (movement is locked and re-hits are blocked).")]
    public float swingTime = 0.7f;

    Harvestable current;
    ThirdPersonController controller;
    KeeperAnimator keeperAnimator;
    float swingTimer;

    void Awake()
    {
        controller = GetComponent<ThirdPersonController>();
        keeperAnimator = GetComponent<KeeperAnimator>();
    }

    void Update()
    {
        // Count down the current swing and lock movement while it plays.
        if (swingTimer > 0f) swingTimer -= Time.deltaTime;
        if (controller != null) controller.harvesting = swingTimer > 0f;

        if (GraveyardManager.Instance == null || !GraveyardManager.Instance.IsPlaying)
        {
            GraveyardManager.Instance?.HidePrompt();
            return;
        }

        current = FindNearest();

        if (current != null)
        {
            GraveyardManager.Instance.ShowPrompt(
                $"Press [E] to {current.Verb()} {current.DisplayName()}");

            if (swingTimer <= 0f && GKInput.InteractPressed())
            {
                swingTimer = swingTime;
                keeperAnimator?.TriggerHarvest(current.type);   // per-type animation (M7.3)
                current.Harvest();
            }
        }
        else
        {
            GraveyardManager.Instance.HidePrompt();
        }
    }

    Harvestable FindNearest()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position, interactRange, harvestableMask, QueryTriggerInteraction.Collide);

        Harvestable best = null;
        float bestDist = float.MaxValue;

        foreach (var col in hits)
        {
            var h = col.GetComponentInParent<Harvestable>();
            if (h == null || h.IsDepleted) continue;

            float d = (h.transform.position - transform.position).sqrMagnitude;
            if (d < bestDist) { bestDist = d; best = h; }
        }
        return best;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
