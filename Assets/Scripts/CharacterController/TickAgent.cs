using GameStructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameplayAbilitySystem;

public abstract class TickAgent : MonoBehaviour
{
    public ParticleSystem DamageEffect;
    public ParticleSystem DeathEffect;

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
            StartCoroutine(PlayEffect(DamageEffect));
        }

        if (Health <= 0)
        {
            IsAlive = false;
            Grid.setOccupied((Vector3Int)GridPos, false);
            Animator.SetBool("Death", true);
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
        ParticleSystem Particle = Instantiate(ParticleEffect, transform.position, transform.rotation);
        Particle.Play();
        while (Particle.isPlaying)
        {
            yield return null;
        }
        Destroy(Particle.gameObject);
    }
}
