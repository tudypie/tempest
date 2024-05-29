using UnityEngine;

public class Boss : MonoBehaviour
{
    public float maxHp = 100;
    public float hp;
    public int scorePointsOnKill = 100;

    private GameManager gameManager;

    public virtual void Awake()
    {
        hp = maxHp;
    }

    public virtual void Start()
    {
        gameManager = GameManager.Instance;
    }

    public virtual void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            if (gameManager.TwoPlayersInGame) {
                gameManager.scoreManager.AddScore(0, scorePointsOnKill / 2);
                gameManager.scoreManager.AddScore(1, scorePointsOnKill / 2);
            } else {
                gameManager.scoreManager.AddScore(0, scorePointsOnKill);
            }
            gameManager.NextLevel();
            Destroy(gameObject);
        }
    }
}
