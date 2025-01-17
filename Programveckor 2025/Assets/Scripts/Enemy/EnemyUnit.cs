using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyUnit : UnitBase
{
    Animator anim;

    // Has the enemy been spared or not?
    [HideInInspector] public bool hasBeenSpared;

    Rigidbody2D rb;
    [SerializeField] float sparedLifeRetreatSpeed;

    [SerializeField] float armor;

    [SerializeField] bool finalBoss;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    // Attack logic
    public override void Attack()
    {
        // Only attack if it hasn't been spared, to avoid bugs where enemies attack after being spared
        if (!hasBeenSpared)
        {
            
            

            // Assign a random attack to the player
            AssignNewAttack(attacks[Random.Range(0, attacks.Count)]);

            if (currentAttack.armorIncrease > 0)
            {
                StartCoroutine(AttackAnimation("Defending"));

                armor += currentAttack.armorIncrease;
            }
            else
            {
                if (currentAttack.selfDamage)
                {
                    TakeDamage(currentAttack.damage * 0.5f);
                }

                StartCoroutine(AttackAnimation("Attacking"));

                // Set "currentTarget" to a random player unit
                UnitBase currentTarget;
                currentTarget = CombatManager.Instance.playersInCombat[Random.Range(0, CombatManager.Instance.playersInCombat.Count)];

                // Make the current target take damage equal to the damage of the current attack
                currentTarget.TakeDamage(currentAttack.damage);
            }              
        }      
    }

    IEnumerator AttackAnimation(string animBoolName)
    {
        anim.SetBool(animBoolName, true);

        yield return new WaitForSeconds(anim.GetNextAnimatorStateInfo(0).length);

        anim.SetBool(animBoolName, false);
    }

    // Check if the unit is clicked
    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CombatManager.Instance.SetCurrentTarget(this);
        }
    }

    // On death remove enemy from lists, give xp to the player and add to the "badness" count
    public override void Die()
    {
        if (finalBoss)
        {
            StartCoroutine(LoadEnding());
        }

        CombatManager.Instance.RemoveEnemyFromList(this);
        CombatManager.Instance.SetCurrentTarget(null);

        Karma.Instance.BadDecision(1);

        Experience.Instance.GainExperience(5, 8);

        if (!finalBoss)
        {
            Destroy(gameObject, .5f);
        }
        
    }

    IEnumerator LoadEnding()
    {
        yield return new WaitForSeconds(2f);

        if (Karma.Instance.karma <= -Karma.Instance.maximumEvilness)
        {
            SceneManager.LoadScene(3);
        }
        else
        {
            SceneManager.LoadScene(4);
        }
        
    }

    public override void TakeDamage(float amount)
    {
        Mathf.Clamp(amount -= armor, 0, Mathf.Infinity);
        if(armor > 0)
        {
            armor--;
        }     

        health -= amount;

        StartCoroutine(SmoothHealthBar());

        if (health <= 0)
        {
            Die();
        }
    }

    // Caleld when you spare the life of an enemty
    public void OnSpared()
    {
        Destroy(gameObject);
        Destroy(CombatManager.Instance.currentTargetMarker);

        Karma.Instance.GoodDecision(1);
    }
}
