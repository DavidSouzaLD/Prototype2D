using UnityEngine;

public class BoxCheck : BaseCheck
{
    [Header("Settings:")]
    [SerializeField] private Vector2 size;
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
        return Physics2D.OverlapBoxAll(transform.position, size, 0f, layerMask);
    }

    private void OnDrawGizmos()
    {
        if (size != Vector2.zero)
        {
            if (IsActive())
            {
                Gizmos.color = active;
                Gizmos.DrawWireCube(transform.position, size);
            }
            else
            {
                Gizmos.color = desactive;
                Gizmos.DrawWireCube(transform.position, size);
            }
        }
    }
}