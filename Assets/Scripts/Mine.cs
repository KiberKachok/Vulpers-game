﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Magic;

public class Mine : Structure, IRes
{
    //public ResourceState state;
    public Village village;

    Coroutine mine;

    public float mineTick = 3f;
    public float minePerTick = 1f;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        //if (village.state != VillageState.Ruined)
        //{
        //    Activate();
        //}
        //else
        //{
        //    Deactivate();
        //}
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
        Ore ore = underHex as Ore;

        while (true)
        {
            yield return new WaitForSeconds(mineTick);

            if (ore.Iron > 0)
            {
                ore.Iron -= minePerTick;
                village.Iron += minePerTick;
                Debug.Log("В городе " + village.name + " +" + minePerTick + " железо от шахты: " + name);
            }

            village.Iron += minePerTick;
            Debug.Log("В городе " + village.name + " +" + minePerTick + " железо от шахты: " + name);
        }
    }
}