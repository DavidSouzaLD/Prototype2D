using UnityEngine;

public class HealthTest : MonoBehaviour, IDamageable<float>
{
    public float Health;

    public void TakeDamage(float _damage)
    {
        Health -= _damage;

        if (Health <= 0)
        {
            Destroy(gameObject);
        }

        Debug.Log("Damaged!");
    }
}
