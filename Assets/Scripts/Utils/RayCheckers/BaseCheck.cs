using UnityEngine;

public class BaseCheck : MonoBehaviour
{
    [SerializeField] protected Color active = Color.green;
    [SerializeField] protected Color desactive = Color.red;

    public virtual bool IsActive()
    {
        return false;
    }

    public virtual Collider2D[] GetColliders()
    {
        return null;
    }
}
