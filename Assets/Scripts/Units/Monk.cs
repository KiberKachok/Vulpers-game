using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Monk : Unit
{
    Coroutine walking = null;

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
            yield return new WaitForSeconds(Random.Range(1, 1.2f));

            List<Hex> variants = new List<Hex>();

            foreach(Hex hex in underHex.neighbours)
            {
                if (hex && (hex.aboveStructure == null || hex.aboveStructure as Field))
                {
                    variants.Add(hex);
                }
            }

            if (variants.Count != 0)
            {
                Move(variants[Random.Range(Mathf.FloorToInt(0), Mathf.FloorToInt(variants.Count()))]);
            }
        }
    }
}
