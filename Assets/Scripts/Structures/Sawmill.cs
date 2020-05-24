using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Magic;

public class Sawmill : Structure, IRes
{
    [Header("Показатели")]
    public float cutTick = 3f;
    public float cutPerTick = 1f;

    [Space(10)]
    public Village village;

    Coroutine cut;

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
                forestToCut.Wood -= cutPerTick;
                village.Wood += cutPerTick;
                //Debug.Log("В городе " + village.name + " +" + cutPerTick + " дерева от лесопилки: " + name);
            }
        }
    }
}
