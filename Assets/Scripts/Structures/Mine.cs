using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Magic;

public class Mine : Structure, IRes
{
    [Header("Показатели")]
    public float mineTick = 3f;
    public float minePerTick = 1f;

    public bool isAboveOre;

    [Space(10)]
    public Village village;

    Coroutine mine;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("Активировать")]
    public void Activate()
    {
        mine = StartCoroutine(MineProcess());
    }

    [ContextMenu("Деактивировать")]
    public void Deactivate()
    {
        StopCoroutine(mine);
    }

    IEnumerator MineProcess()
    {
        if (underHex as Ore)
        {
            isAboveOre = true;
        }

        while (true)
        {
            yield return new WaitForSeconds(mineTick);

            village.Iron += minePerTick + minePerTick * (isAboveOre ? 1 : 0);
            //Debug.Log("В городе " + village.name + " +" + minePerTick + " железо от шахты: " + name);
        }
    }
}
