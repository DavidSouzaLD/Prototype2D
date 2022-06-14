using UnityEngine;

public class Combat2D : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform attackPosition;

    [Header("Damage")]
    [SerializeField] private LayerMask damageableLayers;
    [SerializeField] private float radiusAttack, minDamage, maxDamage;

    public void MeleeAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPosition.position, radiusAttack, damageableLayers);

        if (hits.Length > 0)
        {
            // have a enemy
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].GetComponent<IDamageable<float>>() != null)
                {
                    float damage = Random.Range(minDamage, maxDamage);
                    hits[i].GetComponent<IDamageable<float>>().TakeDamage(damage);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPosition.position, radiusAttack);
        }
    }
}