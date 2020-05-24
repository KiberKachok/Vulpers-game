using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Monk : Unit
{
    [Header("Показатели")]
    public float meditatingTime = 10;
    public float meditatingChance = 10;
    public float goTick = 2;

    [Space(10)]
    public float healthTick = 2;
    public float healthPerTick = 10;

    Coroutine walking = null;
    Coroutine meditating = null;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        walking = StartCoroutine(Go());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Go()
    {
        while (true)
        {
            yield return new WaitForSeconds(goTick);

            List<Hex> variants = new List<Hex>();

            foreach(Hex hex in underHex.neighbours)
            {
                if (hex && (hex.aboveStructure == null || hex.aboveStructure as Field))
                {
                    variants.Add(hex);
                }
            }

            if(Random.Range(0, 100f) < meditatingChance)
            {
                //Debug.Log("Я, " + name + " сел медитировать.");
                meditating = StartCoroutine(Meditating());
                StopCoroutine(walking);
            }
            else
            {
                if (variants.Count != 0)
                {
                    Move(variants[Random.Range(Mathf.FloorToInt(0), Mathf.FloorToInt(variants.Count()))]);
                }
            }
        }
    }

    IEnumerator Meditating()
    {
        float time = meditatingTime;
        yield return new WaitForSeconds(1.2f);

        for (float i = 0; i < time; i += healthTick)
        {
            foreach (Hex hex in underHex.neighbours)
            {
                if (hex && hex.aboveUnit && hex.aboveUnit.team == Magic.Team.Our)
                {
                    hex.aboveUnit.currentHp = Mathf.Clamp(hex.aboveUnit.currentHp + healthPerTick, 0, hex.aboveUnit.maxHp);
                }
            }

            yield return new WaitForSeconds(healthTick);
        }

        walking = StartCoroutine(Go());
    }
}

