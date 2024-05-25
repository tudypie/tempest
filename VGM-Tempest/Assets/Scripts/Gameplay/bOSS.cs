using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float maxHp = 100;
    [SerializeField] private float hp;

    private void Awake()
    {
        hp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            Destroy(gameObject);
            GameManager.Instance.NextLevel();
        }
    }
}
