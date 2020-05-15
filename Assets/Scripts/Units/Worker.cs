using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Unit
{
    [Header("Показатели")]
    public float repairTick = 1;
    public float repairPerTick = 1;

    Coroutine walking = null;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        //walking = StartCoroutine(Go());
        StartCoroutine(Repair());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Repair()
    {
        yield return null;
        while (true)
        {
            yield return new WaitForSeconds(repairTick);

            foreach (Hex hex in underHex.neighbours)
            {
                if(hex && hex.aboveStructure && hex.aboveStructure.team == team)
                {
                    Debug.Log(hex.name);
                    hex.aboveStructure.currentHp = hex.aboveStructure.currentHp + repairPerTick;
                }
            }
        }
    }
}
