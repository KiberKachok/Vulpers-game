using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Unit, IDam
{
    Coroutine attackProcess;

    public bool isAttacking = false;

    public float AttackTime = 0.4f;
    public float Damage = 30;
    public bool isUpped = false;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(Unit unit)
    {
        if (!isAttacking)
        {
            Debug.Log(name + " атаковал: " + unit.name);
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
            Debug.Log(name + " атаковал: " + structure.name);
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
