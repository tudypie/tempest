using UnityEngine;

public class DoomBoss : Boss
{
    [SerializeField] private int totalAnimations;
    [SerializeField] private int animationIndex;

    private Animator anim;
    private SpriteRenderer sr;

    public override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        animationIndex = (int)(hp / (0.2 * maxHp)) + 1;
        anim.Play("Idle" + animationIndex.ToString());
        sr.color = Color.red;
        Invoke(nameof(HideHitVisual), 0.07f);
    }

    private void HideHitVisual() => sr.color = Color.white;
}
