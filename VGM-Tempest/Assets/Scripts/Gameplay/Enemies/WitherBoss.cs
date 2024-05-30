using UnityEngine;

public class WitherBoss : Boss
{
    private Animator anim;

    public override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        anim.Play("TakeDamage");
    }
}
