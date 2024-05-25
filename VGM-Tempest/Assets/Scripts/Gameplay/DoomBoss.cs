using UnityEngine;

public class DoomBoss : Boss
{
    public int totalAnimations;
    public int animationIndex;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        animationIndex = (int)(hp / 20) + 1;
        anim.Play("Idle" + animationIndex.ToString());
    }
}
