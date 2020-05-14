using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout : Unit, IDam
{
    Coroutine attackProcess;

    public bool isAttacking = false;

    public float AttackTime = 0.4f;
    public float Damage = 30;

    public float goldPerAttack;

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
            if (unit as Caravan)
            {
                Caravan caravan = unit as Caravan;

                float moneyToAdd = goldPerAttack;


                if (caravan.Gold < goldPerAttack)
                {
                    moneyToAdd = caravan.Gold;
                }

                caravan.Gold -= moneyToAdd;
                FindNearestVillage(team).Gold += moneyToAdd;
                Debug.Log("УКРАЛ ЗОЛОТО!!!");
            }
            //Ну типа еще караван добавить
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
            if(structure as Village)
            {
                Village village = structure as Village;

                float moneyToAdd = goldPerAttack;


                if (village.Gold < goldPerAttack)
                {
                    moneyToAdd = village.Gold;
                }

                village.Gold -= moneyToAdd;
                FindNearestVillage(team).Gold += moneyToAdd;
                Debug.Log("УКРАЛ ЗОЛОТО!!!");
            }
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
