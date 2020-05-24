using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Magic;

public class Farm : Structure, IRes
{
    [Header("Показатели")]
    public int allowedFields;
    public float GrowKoef = 1;
    public float CheckTick = 5;

    [Space(10)]
    public Village village;

    [Space(10)]
    [SerializeField]
    GameObject fieldPref;

    Coroutine growCow;

    [Space(10)]
    public List<Field> fields = new List<Field>();
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
        growCow = StartCoroutine(CheckAround());

        foreach (Field field in fields)
        {
            field.Activate();
        }
    }

    [ContextMenu("Деактивировать")]
    public void Deactivate()
    {
        StopCoroutine(growCow);

        foreach(Field field in fields)
        {
            field.Deactivate();
        }
    }

    public void Grow()
    {
        village.Food += GrowKoef;
        //Debug.Log("В городе " + village.name + " +1 еда от фермы: " + name);
    }

    IEnumerator CheckAround()
    {
        yield return null;
        while (true)
        {
            //List<Hex> hexsAround = underHex.neighbours.ToList();
            //List<Hex> ableAround = new List<Hex>();

            //foreach (Hex hex in underHex.neighbours)
            //{
            //    if (!hex.aboveStructure && !hex.aboveUnit)
            //    {
            //        ableAround.Add(hex);
            //    }
            //}

            //for (int i = 0; i < (allowedFields - fields.Count); i++)
            //{
            //}

            foreach (Hex hex in underHex.neighbours)
            {
                if(!hex.aboveStructure && !hex.aboveUnit && fields.Count < allowedFields)
                {
                    Field field = Instantiate(fieldPref, hex.transform).GetComponent<Field>();
                    fields.Add(field);
                    field.farm = this;
                    field.Align();
                }
            }

            yield return new WaitForSeconds(CheckTick);
        }
    }

}
