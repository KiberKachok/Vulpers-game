using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Magic;

public class Village : Structure
{
    public float Gold = 0;
    public float Iron = 0;
    public float Food = 0;
    public float Wood = 0;

    //от 1 до 10
    public float valueIron = 6;
    public float valueFood = 6;
    public float valueWood = 6;

    public float hungerTick = 1f;

    public float goldTick = 1f;
    public float goldPerTick = 10f;

    Coroutine hungerProcess;
    Coroutine goldProcess;
    Coroutine spawnProcess;

    public Hex spawnHex;

    public VillageState state;

    public float WorkerTime = 4.5f;
    public float WarriorTime = 6;
    public float ArcherTime = 11;
    public float CaravanTime = 15;
    public float ScoutTime = 7;

    [SerializeField]
    GameObject WorkerPref;

    [SerializeField]
    GameObject WarriorPref;

    [SerializeField]
    GameObject ArcherPref;

    [SerializeField]
    GameObject CaravanPref;

    [SerializeField]
    GameObject ScoutPref;
    public float BuildTime = 2;

    public List<Structure> structures = new List<Structure>();

    [HideInInspector]
    public bool isCreating = false;

    [SerializeField]
    GameObject minePref;

    [SerializeField]
    GameObject farmPref;

    [SerializeField]
    GameObject sawmillPref;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        if (state != VillageState.Ruined)
        {
            Activate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Hex HEXTOBILD;
    public ResourceType TIP;

    [ContextMenu("Build")]
    public void qqq()
    {
        StartCoroutine(Build(HEXTOBILD, TIP));
    }

    public void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Spawn(UnitType.Worker);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Spawn(UnitType.Warrior);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Spawn(UnitType.Scout);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Spawn(UnitType.Archer);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Spawn(UnitType.Caravan);
        }
    }

    IEnumerator Build(Hex hex, ResourceType type)
    {
        GameObject objToSpawn = farmPref;

        switch (type)
        {
            case ResourceType.Farm:
                objToSpawn = farmPref;
                break;

            case ResourceType.Mine:
                objToSpawn = minePref;
                break;

            case ResourceType.Sawmill:
                objToSpawn = sawmillPref;
                break;
        }

        GameObject res = Instantiate(objToSpawn, hex.transform.position - new Vector3(0, 0.4f, 0), Quaternion.identity);
        hex.aboveStructure = res.GetComponent<Structure>();
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForFixedUpdate();
            res.transform.position += new Vector3(0, 0.4f / 100, 0);
            yield return new WaitForSeconds(BuildTime / 100f);
        }

        //res.transform.position = hex.transform.position;
        Structure structRes = res.GetComponent<Structure>();
        structRes.underHex = hex;
        switch (type)
        {
            case ResourceType.Farm:
                structRes.GetComponent<Farm>().village = this;
                break;

            case ResourceType.Mine:
                structRes.GetComponent<Mine>().village = this;
                break;

            case ResourceType.Sawmill:
                structRes.GetComponent<Sawmill>().village = this;
                break;
        }
        structRes.Align();
        structures.Add(structRes);
        (structRes as IRes).Activate();
    }

    public void Spawn(UnitType type)
    {
        if (!isCreating)
        {
            spawnProcess = StartCoroutine(CreateUnit(type));
        }
    }

    IEnumerator CreateUnit(UnitType type)
    {
        isCreating = true;
        float timeToWait = 0.1f;
        GameObject objToSpawn = WorkerPref;
        switch (type)
        {
            case UnitType.Archer:
                timeToWait = ArcherTime / 100f;
                objToSpawn = ArcherPref;
                break;

            case UnitType.Caravan:
                timeToWait = CaravanTime / 100f;
                objToSpawn = CaravanPref;
                break;

            case UnitType.Scout:
                timeToWait = ScoutTime / 100f;
                objToSpawn = ScoutPref;
                break;

            case UnitType.Warrior:
                timeToWait = WarriorTime / 100f;
                objToSpawn = WarriorPref;
                break;

            case UnitType.Worker:
                timeToWait = WorkerTime / 100f;
                objToSpawn = WorkerPref;
                break;
        }
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(timeToWait);
        }
        while (true)
        {
            if (!spawnHex.aboveStructure && !spawnHex.aboveUnit)
            {
                Unit unit = Instantiate(objToSpawn, spawnHex.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity).GetComponent<Unit>();
                unit.team = team;
                unit.Align();
                break;
            }
            else
            {
                yield return null;
            }
        }
        isCreating = false;
    }

    //Начать работу
    [ContextMenu("Активировать")]
    public void Activate()
    {
        //Gold = 0;
        //Iron = 0;
        Food = 100;
        //Wood = 0;
        foreach (Structure structure in structures)
        {
            if(structure)
            {
                (structure as IRes).Activate();
                structure.team = team;
            }
        }

        hungerProcess = StartCoroutine(Hunger());
        goldProcess = StartCoroutine(Charge());
    }

    //Опустошить Город
    [ContextMenu("Деактивировать")]
    public void Deactivate()
    {
        Gold = 0;
        Iron = 0;
        Food = 0;
        Wood = 0;
        foreach (Structure structure in structures)
        {
            if (structure is IRes)
            {
                (structure as IRes).Deactivate();
            }
        }
        StopCoroutine(hungerProcess);
        StopCoroutine(goldProcess);
        state = VillageState.Ruined;
        Debug.Log("Город " + name + " разрушен");
    }

    IEnumerator Charge()
    {
        while (true)
        {
            yield return new WaitForSeconds(goldTick);

            Gold += goldPerTick;
            Debug.Log("В городе " + name + " уплачен налог в размере " + goldPerTick);
        }
    }


    IEnumerator Hunger()
    {
        while (true)
        {
            yield return new WaitForSeconds(hungerTick);

            Food--;
            if (Food <= 0)
            {
                Deactivate();
            }
        }
    }

    void Ruin()
    {
        foreach (Structure structure in structures)
        {
            if (structure is IRes)
            {
                (structure as IRes).Deactivate();
            }
        }

        state = VillageState.Ruined;
    }

    public void BuildRecource(ResourceType type)
    {
        Structure structure;

        switch (type)
        {
            case ResourceType.Farm:
                break;

            case ResourceType.Sawmill:
                break;

            case ResourceType.Mine:
                break;
        }
    }
}
