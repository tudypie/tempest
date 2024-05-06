using UnityEngine;
using UnityEngine.UI;

public class bOSS : MonoBehaviour
{
    [SerializeField] private float maxHp = 100;
    [SerializeField] private float hp;
    [SerializeField] private Image healthBar;

    private void Awake()
    {
        hp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        healthBar.fillAmount = hp / maxHp;

        if (hp < 0)
        {
            Destroy(gameObject);
        }
    }
}
