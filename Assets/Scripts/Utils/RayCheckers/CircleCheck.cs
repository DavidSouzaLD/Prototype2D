using UnityEngine;

public class CircleCheck : BaseCheck
{
    [Header("Settings:")]
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layerMask;

    public override bool IsActive()
    {
        Collider2D[] cols = GetColliders();

        if (cols.Length > 0)
        {
            foreach (Collider2D col in cols)
            {
                if (col != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override Collider2D[] GetColliders()
    {
        return Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
    }

    private void OnDrawGizmos()
    {
        if (radius != 0)
        {
            if (IsActive())
            {
                Gizmos.color = active;
                Gizmos.DrawWireSphere(transform.position, radius);
            }
            else
            {
                Gizmos.color = desactive;
                Gizmos.DrawWireSphere(transform.position, radius);
            }
        }
    }
}