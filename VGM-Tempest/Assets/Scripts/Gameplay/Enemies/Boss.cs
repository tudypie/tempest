using UnityEngine;

public class Boss : MonoBehaviour
{
    public float maxHp = 100;
    public float hp;

    public virtual void Awake()
    {
        hp = maxHp;
    }

    public virtual void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            Destroy(gameObject);
            GameManager.Instance.NextLevel();
        }
    }
}
