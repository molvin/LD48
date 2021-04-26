using GameStructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;

public abstract class TickAgent : MonoBehaviour
{
    public ParticleSystem DamageEffect;
    public ParticleSystem DeathEffect;
    public ParticleSystem PoisonedEffect;

    public string DeathSound;
    public string HurtSound;

    [HideInInspector]
    public Animator Animator;
    [HideInInspector]
    public Vector2Int GridPos;
    [HideInInspector]
    public bool IsAlive = true;
    [HideInInspector]
    public int CurrentHealth;
    [HideInInspector]
    public int MaxHealth;


    public int initiative = 0;
    public AbilitySystem AbilitySystem;

    protected GridGenerator Grid => GameStateManager.Instance.GetGridManager();

    public abstract void Initialize(LDBlock data);
    public abstract void Tick(int Frame, bool Scrum);

    protected void OnDamageTaken(int Health)
    {
        if (Health == CurrentHealth)
        {
            return;
        }

        if (Health < CurrentHealth)
        {
            AudioSystem.Play(HurtSound);
            if (AbilitySystem.HasActiveTag(TypeTag.Poison))
            {
                StartCoroutine(PlayEffect(PoisonedEffect));
            }
            else
            {
                StartCoroutine(PlayEffect(DamageEffect));
            }
        }

        if (Health <= 0)
        {
            IsAlive = false;
            Grid.setOccupied((Vector3Int)GridPos, false);
            Animator.SetBool("Death", true);
            AudioSystem.Play(DeathSound);
            StartCoroutine(PlayEffect(DeathEffect));
        }

        CurrentHealth = Health;
    }

    protected void UpdateMaxHealth(int MaxValue)
    {
        MaxHealth = MaxValue;
    }

    private IEnumerator PlayEffect(ParticleSystem ParticleEffect)
    {
        if (ParticleEffect != null)
        {
            ParticleSystem Particle = Instantiate(ParticleEffect, transform.position, transform.rotation);
            Particle.Play();
            while (Particle.isPlaying)
            {
                yield return null;
            }
            Destroy(Particle.gameObject);
        }
    }
}
