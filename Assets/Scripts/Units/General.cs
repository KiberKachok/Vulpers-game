using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : Unit, IDam
{
    [Header("Показатели")]
    public float Damage = 30;
    public float DamageUpgrade = 40;
    public float AttackTime = 0.4f;

    [HideInInspector]
    public bool isAttacking = false;

    [HideInInspector]
    public bool isUpped = false;

    Coroutine attackProcess;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Hex hex in underHex.neighbours)
        {
            if (hex && hex.aboveUnit)
            {
                if(hex.aboveUnit is IDam)
                {
                    (hex.aboveUnit as IDam).upDamage(DamageUpgrade);
                }
            }
        }
    }

    public void Attack(Unit unit)
    {
        if (!isAttacking)
        {
            attackProcess = StartCoroutine(AttackProcess(unit));
        }
    }

    IEnumerator AttackProcess(Unit unit)
    {
        isAttacking = true;

        yield return new WaitForSeconds(AttackTime);
        unit.TakeDamage(Damage);

        isAttacking = false;
    }

    public void Attack(Structure structure)
    {
        if (!isAttacking)
        {
            attackProcess = StartCoroutine(AttackProcess(structure));
        }
    }

    IEnumerator AttackProcess(Structure structure)
    {
        isAttacking = true;

        yield return new WaitForSeconds(AttackTime);
        structure.TakeDamage(Damage);

        isAttacking = false;
    }

    public void upDamage(float damage)
    {
        StartCoroutine(AttackUp(damage));
    }

    IEnumerator AttackUp(float damage)
    {
        if (!isUpped)
        {
            isUpped = true;
            Damage += damage;

            yield return new WaitForSeconds(1f);

            Damage -= damage;
            isUpped = false;
        }
    }
}
