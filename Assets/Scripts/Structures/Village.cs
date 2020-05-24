using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Magic;

public class Village : Structure
{
    public VillageState state;

    [Header("Ресурсы")]
    public float Gold = 0;
    public float Iron = 0;
    public float Food = 0;
    public float Wood = 0;

    //от 1 до 10
    [Header("Цены")]
    public float valueIron = 6;
    public float valueFood = 6;
    public float valueWood = 6;

    [Header("Еда")]
    public float hungerTick = 1f;

    [Header("Налоги")]
    public float goldTick = 1f;
    public float goldPerTick = 10f;

    [Header("Апгрейд")]
    //public int levelChargeUp = 1;
    //private int[] chargeUpMap = new int[5] {10, 12, 16, 20, 25};

    public int levelSawmill = 1;
    private int[] upSawmill = new int[5] { 1, 2, 3, 4, 5 };



    [ContextMenu("Апгрейд Лесопилки")]
    public void SawmillUpgrade()
    {
        Gold -= UpgradeCost[levelSawmill - 1];

        if (levelSawmill < 5)
        {
            levelSawmill++;

            foreach (Structure i in structures)
            {
                if (i as Sawmill)
                {
                    (i as Sawmill).cutPerTick = upSawmill[levelSawmill - 1];
                }
            }
        }

        Debug.Log("Апгрейд Лесопилки");
    }

    public int levelMine = 1;
    private int[] upMine = new int[5] { 1, 2, 3, 4, 5 };

    [ContextMenu("Апгрейд Шахты")]
    public void MineUpgrade()
    {
        Gold -= UpgradeCost[levelMine - 1];

        if (levelMine < 5)
        {
            levelMine++;

            foreach (Structure i in structures)
            {
                if (i as Mine)
                {
                    (i as Mine).minePerTick = upMine[levelMine - 1];
                }
            }
        }

        Debug.Log("Апгрейд Шахты");
    }

    public int levelFarm = 1;
    private int[] upFarm = new int[5] { 2, 3, 4, 5, 6 };

    [ContextMenu("Апгрейд Фермы")]
    public void FarmUpgrade()
    {
        Gold -= UpgradeCost[levelFarm - 1];

        if (levelFarm < 5)
        {
            levelFarm++;

            foreach (Structure i in structures)
            {
                if (i as Farm)
                {
                    (i as Farm).allowedFields = upFarm[levelFarm - 1];
                }
            }
        }

        Debug.Log("Апгрейд Фермы");
    }



    [Header("Описание")]
    public string Name;
    [TextArea()]
    public string Description;

    [Header("Время на создание юнитов")]
    public float WorkerTime = 4.5f;
    public float WarriorTime = 6;
    public float ArcherTime = 11;
    public float CaravanTime = 15;
    public float ScoutTime = 7;

    public int[] UpgradeCost = new int[4] { 10, 16, 22, 30 };

    public int levelOf(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Farm:
                return levelFarm;

            case ResourceType.Mine:
                return levelMine;

            case ResourceType.Sawmill:
                return levelSawmill;
        }
        return 0;
    }

    public int CostOfUpgrade(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Farm:
                return UpgradeCost[levelFarm - 1];

            case ResourceType.Mine:
                Debug.Log(levelMine - 1);
                return UpgradeCost[levelMine - 1];

            case ResourceType.Sawmill:
                return UpgradeCost[levelSawmill - 1];
        }
        return 0;
    }

    public Dictionary<UnitType, Vector2> UnitValues = new Dictionary<UnitType, Vector2>
    {
                                  // x - Золото - железо
        {UnitType.Worker, new Vector2(11, 11)},
        {UnitType.Warrior, new Vector2(12, 12)},
        {UnitType.Scout, new Vector2(13, 13)},
        {UnitType.Archer, new Vector2(14, 14)},
        {UnitType.Caravan, new Vector2(15, 15)}
    };

    public Dictionary<ResourceType, Vector2> StructValues = new Dictionary<ResourceType, Vector2>
    {
                                  // x - Золото - дерево
        {ResourceType.Farm, new Vector2(11, 11)},
        {ResourceType.Mine, new Vector2(12, 10)},
        {ResourceType.Sawmill, new Vector2(13, 10)},
    };

    [Header("Префабы юнитов")]
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

    [Header("Префабы построек")]
    public float BuildTime = 2;

    [SerializeField]
    GameObject minePref;

    [SerializeField]
    GameObject farmPref;

    [SerializeField]
    GameObject sawmillPref;

    public Hex spawnHex;

    public List<Structure> structures = new List<Structure>();

    Coroutine hungerProcess;
    Coroutine goldProcess;
    Coroutine spawnProcess;

    [HideInInspector]
    public bool isCreating = false;

    [Header("Панель")]
    [SerializeField]
    GameObject canvasIo;
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI WoodText;
    public TextMeshProUGUI IronText;
    public TextMeshProUGUI FoodText;
    bool isFocusing;
    bool isFocusingSpawn;
    UnitType typeOfUnit;
    public bool isUsedBySpawner;
    public void Focus()
    {
        isFocusing = true;
        canvasIo.SetActive(true);
    }

    public void unFocus()
    {
        isFocusing = false;
        canvasIo.SetActive(false);
    }

    public void FocusSpawn(UnitType type)
    {
        isFocusingSpawn = true;
        canvasIo.SetActive(true);

        typeOfUnit = type;
    }

    public void unFocusSpawn()
    {
        isFocusingSpawn = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        unFocus();

        base.Start();

        if (state != VillageState.Ruined)
        {
            Activate();
        }
        else
        {
            levelSawmill = 1;
            levelMine = 1;
            levelFarm = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFocusing)
        {
            if (isFocusingSpawn)
            {
                GoldText.text = Gold.ToString() + " AAA ";
                WoodText.text = Wood.ToString();
                IronText.text = Iron.ToString() + " AAA ";
                FoodText.text = Food.ToString();
            }
            else
            {
                GoldText.text = Gold.ToString();
                WoodText.text = Wood.ToString();
                IronText.text = Iron.ToString();
                FoodText.text = Food.ToString();
            }
        }
    }

    public IEnumerator Build(Hex hex, ResourceType type)
    {
        if(!hex.aboveStructure && !hex.aboveUnit)
        {
            Gold -= StructValues[type].x;
            Wood -= StructValues[type].y;

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
                    (structRes as Farm).allowedFields = upFarm[levelFarm - 1];
                    structRes.GetComponent<Farm>().village = this;
                    break;

                case ResourceType.Mine:
                    (structRes as Mine).minePerTick = upMine[levelMine - 1];
                    structRes.GetComponent<Mine>().village = this;
                    break;

                case ResourceType.Sawmill:
                    (structRes as Sawmill).cutPerTick = upSawmill[levelMine - 1];
                    structRes.GetComponent<Sawmill>().village = this;
                    break;
            }

            structRes.team = team;
            structRes.Align();
            structures.Add(structRes);
            (structRes as IRes).Activate();
        }
    }

    public void Spawn(Hex hex, UnitType type)
    {
        spawnProcess = StartCoroutine(CreateUnit(hex, type));
    }

    IEnumerator CreateUnit(Hex hex, UnitType type)
    {
        if(!hex.aboveStructure && !hex.aboveUnit && Gold >= UnitValues[type].x && Iron >= UnitValues[type].y)
        {
            Gold -= UnitValues[type].x;
            Iron -= UnitValues[type].y;

            isCreating = true;

            hex.aboveUnit = Instantiate(WorkerPref, hex.transform.position + new Vector3(0, -10f, 0), Quaternion.identity).GetComponent<Unit>();

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

            Destroy(hex.aboveUnit);
            hex.aboveUnit = null;
            Unit unit = Instantiate(objToSpawn, hex.transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity).GetComponent<Unit>();
            unit.team = team;
            unit.Align();

            isCreating = false;
        }
    }

    //Начать работу
    [ContextMenu("Активировать")]
    public void Activate()
    {
        //Gold = 0;
        //Iron = 0;
        Food = 100;
        //Wood = 0;
        foreach (Structure i in structures)
        {
            if(i)
            {
                (i as IRes).Activate();
                i.team = team;
                if (i as Sawmill)
                {
                    //levelSawmill = 1;
                    (i as Sawmill).cutPerTick = upSawmill[levelSawmill - 1];
                }

                if (i as Mine)
                {
                    //levelMine = 1;
                    (i as Mine).minePerTick = upMine[levelMine - 1];
                }

                if (i as Farm)
                {
                    //levelFarm = 1;
                    (i as Farm).allowedFields = upFarm[levelFarm - 1];
                }
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

        levelSawmill = 1;
        levelMine = 1;
        levelFarm = 1;


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
            //Debug.Log("В городе " + name + " уплачен налог в размере " + goldPerTick);
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

    private void OnMouseEnter()
    {
        Focus();
    }

    private void OnMouseExit()
    {
        if (!isUsedBySpawner)
        {
            unFocus();
        }
    }
}
