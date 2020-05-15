using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : Structure
{
    [Header("Показатели")]
    public float Food = 0;
    public float growTick = 1f;
    public float growPerTick = 20;

    [Space(10)]
    public Farm farm;

    Coroutine growProcess;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        growProcess = StartCoroutine(Grow());
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("Активировать")]
    public void Activate()
    {
        if (growProcess == null)
        {
            growProcess = StartCoroutine(Grow());
        }
    }

    [ContextMenu("Деактивировать")]
    public void Deactivate()
    {
        StopCoroutine(growProcess);
    }

    IEnumerator Grow()
    {
        while (true)
        {
            yield return new WaitForSeconds(growTick);

            Food += growPerTick;

            if(Food >= 100)
            {
                Food = 0;
                farm.Grow();
            }
        }
    }
}
