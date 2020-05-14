using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Magic;

public class Sawmill : Structure, IRes
{
    //public ResourceState state;
    public Village village;

    Coroutine cut;

    public float cutTick = 3f;
    public float cutByTick = 1f;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        //if(village.state != VillageState.Ruined)
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
        cut = StartCoroutine(Cut());
    }

    [ContextMenu("Деактивировать")]
    public void Deactivate()
    {
        StopCoroutine(cut);
    }

    IEnumerator Cut()
    {
        while (true)
        {
            yield return new WaitForSeconds(cutTick);

            List<Forest> forests = new List<Forest>();

            foreach (Hex hex in underHex.neighbours)
            {
                if (hex.aboveStructure as Forest)
                {
                    forests.Add(hex.aboveStructure as Forest);
                }
            }

            float maxWood = 0;
            Forest forestToCut = null;

            foreach (Forest forest in forests)
            {
                if (forest.Wood > maxWood)
                {
                    maxWood = forest.Wood;
                    forestToCut = forest;
                }
            }

            if (forestToCut != null)
            {
                forestToCut.Wood -= cutByTick;
                village.Wood += cutByTick;
                Debug.Log("В городе " + village.name + " +" + cutByTick + " дерева от лесопилки: " + name);
            }
        }
    }
}
