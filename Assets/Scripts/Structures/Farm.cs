using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Magic;

public class Farm : Structure, IRes
{
    [Header("Показатели")]
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
        Debug.Log("В городе " + village.name + " +1 еда от фермы: " + name);
    }

    IEnumerator CheckAround()
    {
        yield return null;
        while (true)
        {
            foreach (Hex hex in underHex.neighbours)
            {
                if(!hex.aboveStructure && !hex.aboveUnit)
                {
                    Field field = Instantiate(fieldPref, hex.transform).GetComponent<Field>();
                    field.farm = this;
                    field.Align();
                }
            }

            yield return new WaitForSeconds(CheckTick);
        }
    }

}
