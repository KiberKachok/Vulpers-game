using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Magic;

public class SpawnController : MonoBehaviour
{
    GameController gameController;

    [SerializeField]
    GameObject HighlightAroundVillagePref;
    [SerializeField]
    GameObject HighlightUnderCursor;

    GameObject HighlightUC = null;
    List<GameObject> highlights = new List<GameObject>();

    LineRenderer lnrd;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DragUpgrade(string str)
    {
        gameController.focusedUnit = null;

        switch (str)
        {
            case "Sawmill":
                StartCoroutine(DraggingUpgrade(ResourceType.Sawmill));
                break;

            case "Mine":
                StartCoroutine(DraggingUpgrade(ResourceType.Mine));
                break;

            case "Farm":
                StartCoroutine(DraggingUpgrade(ResourceType.Farm));
                break;
        }
    }

    IEnumerator DraggingUpgrade(ResourceType type)
    {
        Village nearestVillage = null;
        GameObject HighlightHex = Instantiate(HighlightUnderCursor);
        HighlightHex.transform.rotation = Quaternion.identity;

        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            LayerMask layer = 1 << 8;

            List<Village> newVillages = Pathfinder.FindVillagesOfTeams(Team.Our);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
            {
                Hex focusedHex = hit.transform.gameObject.GetComponent<Hex>();

                if (focusedHex.aboveStructure && focusedHex.aboveStructure as Village)
                {
                    Village village = focusedHex.aboveStructure as Village;
                    if (village.levelOf(type) < 5 && village.Gold >= village.CostOfUpgrade(type))
                    {
                        village.GoldText.text = "<color=\"green\"><b>" + village.GoldText.text + "</b></color>";
                    }
                    else
                    {
                        village.GoldText.text = "<color=\"red\"><b>" + village.GoldText.text + "</b></color>";
                    }
                }

                HighlightHex.transform.position = focusedHex.transform.position + new Vector3(0, 0.002f, 0);

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    break;
                }

                if (Input.GetKeyDown(KeyCode.Mouse0) && focusedHex.aboveStructure && focusedHex.aboveStructure as Village)
                {
                    Village village = focusedHex.aboveStructure as Village;

                    if (/*nearestVillage.underHex.neighbours.Contains(focusedHex)*/true)
                    {
                        if (village.Gold >= village.CostOfUpgrade(type) && village.levelOf(type) < 5)
                        {
                            switch (type)
                            {
                                case ResourceType.Farm:
                                    village.FarmUpgrade();
                                    break;

                                case ResourceType.Mine:
                                    village.MineUpgrade();
                                    break;

                                case ResourceType.Sawmill:
                                    village.SawmillUpgrade();
                                    break;
                            }
                        }
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            yield return null;
        }

        Destroy(HighlightHex);
    }

    public void DragUnit(string str)
    {
        gameController.focusedUnit = null;

        foreach (GameObject i in highlights)
        {
            Destroy(i);
        }

        switch (str)
        {
            case "Archer":
                StartCoroutine(DraggingUnit(UnitType.Archer));
                break;

            case "Caravan":
                StartCoroutine(DraggingUnit(UnitType.Caravan));
                break;

            case "Scout":
                StartCoroutine(DraggingUnit(UnitType.Scout));
                break;

            case "Warrior":
                StartCoroutine(DraggingUnit(UnitType.Warrior));
                break;

            case "Worker":
                StartCoroutine(DraggingUnit(UnitType.Worker));
                break;
        }
    }

    IEnumerator DraggingUnit(UnitType type)
    {
        Dictionary<Village, List<GameObject>> Highs = new Dictionary<Village, List<GameObject>>();
        Village nearestVillage = null;
        GameObject HighlightHex = Instantiate(HighlightUnderCursor);
        HighlightHex.transform.rotation = Quaternion.identity;

        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            LayerMask layer = 1 << 8;

            List<Village> newVillages = Pathfinder.FindVillagesOfTeams(Team.Our);

            foreach (KeyValuePair<Village, List<GameObject>> i in Highs)
            {
                if(i.Key.team != Team.Our)
                {
                    foreach (GameObject j in i.Value)
                    {
                        Destroy(j);
                    }
                    Highs.Remove(i.Key);
                }
            }

            foreach (Village i in newVillages)
            {
                if (!Highs.ContainsKey(i))
                {
                    List<GameObject> newArea = new List<GameObject>();

                    foreach(Hex j in i.underHex.neighbours)
                    {
                        newArea.Add(Instantiate(HighlightAroundVillagePref, j.transform.position + new Vector3(0, 0.001f, 0), Quaternion.identity));
                    }

                    i.Focus();
                    i.isUsedBySpawner = true;
                    Highs.Add(i, newArea);
                }
            }

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
            {
                Hex focusedHex = hit.transform.gameObject.GetComponent<Hex>();
                nearestVillage = Pathfinder.findNearestVillage(focusedHex.transform.position, Highs.Keys.ToList());

                if (nearestVillage.underHex.neighbours.Contains(focusedHex))
                {
                    if(nearestVillage.Gold >= nearestVillage.UnitValues[type].x)
                    {
                        nearestVillage.GoldText.text = "<color=\"green\"><b>" + nearestVillage.GoldText.text + "</b></color>";
                    }
                    else
                    {
                        nearestVillage.GoldText.text = "<color=\"red\"><b>" + nearestVillage.GoldText.text + "</b></color>";
                    }
                    if (nearestVillage.Iron >= nearestVillage.UnitValues[type].y)
                    {
                        nearestVillage.IronText.text = "<color=\"green\"><b>" + nearestVillage.IronText.text + "</b></color>";
                    }
                    else
                    {
                        nearestVillage.IronText.text = "<color=\"red\"><b>" + nearestVillage.IronText.text + "</b></color>";
                    }
                }

                HighlightHex.transform.position = focusedHex.transform.position + new Vector3(0, 0.002f, 0);

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    break;
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (nearestVillage.underHex.neighbours.Contains(focusedHex))
                    {
                        if (nearestVillage.Gold >= nearestVillage.UnitValues[type].x && nearestVillage.Iron >= nearestVillage.UnitValues[type].y)
                        {
                            nearestVillage.Spawn(focusedHex, type);
                        }
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            yield return null;
        }

        Dictionary<Village, List<GameObject>> HighsCopy = new Dictionary<Village, List<GameObject>>(Highs);

        foreach (KeyValuePair<Village, List<GameObject>> i in HighsCopy)
        {
            foreach (GameObject j in i.Value)
            {
                Destroy(j);
            }

            i.Key.unFocus();
            i.Key.isUsedBySpawner = false;
            Highs.Remove(i.Key);
        }

        Destroy(HighlightHex);
    }

    public void DragStruct(string str)
    {
        gameController.focusedUnit = null;

        //foreach (GameObject i in highlights)
        //{
        //    Destroy(i);
        //}

        switch (str)
        {
            case "Sawmill":
                StartCoroutine(DraggingStruct(ResourceType.Sawmill));
                break;

            case "Mine":
                StartCoroutine(DraggingStruct(ResourceType.Mine));
                break;

            case "Farm":
                StartCoroutine(DraggingStruct(ResourceType.Farm));
                break;
        }
    }

    IEnumerator DraggingStruct(ResourceType type)
    {
        Dictionary<Village, List<GameObject>> Highs = new Dictionary<Village, List<GameObject>>();
        Village nearestVillage = null;
        GameObject HighlightHex = Instantiate(HighlightUnderCursor);
        HighlightHex.transform.rotation = Quaternion.identity;

        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            LayerMask layer = 1 << 8;

            List<Village> newVillages = Pathfinder.FindVillagesOfTeams(Team.Our);

            foreach (KeyValuePair<Village, List<GameObject>> i in Highs)
            {
                if (i.Key.team != Team.Our)
                {
                    Highs.Remove(i.Key);
                }
            }

            foreach (Village i in newVillages)
            {
                if (!Highs.ContainsKey(i))
                {
                    List<GameObject> newArea = new List<GameObject>();

                    i.Focus();
                    i.isUsedBySpawner = true;
                    Highs.Add(i, newArea);
                }
            }

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
            {
                Hex focusedHex = hit.transform.gameObject.GetComponent<Hex>();
                nearestVillage = Pathfinder.findNearestVillage(focusedHex.transform.position, Highs.Keys.ToList());

                if (!nearestVillage.underHex.neighbours.Contains(focusedHex))
                {
                    if (nearestVillage.Gold >= nearestVillage.StructValues[type].x)
                    {
                        nearestVillage.GoldText.text = "<color=\"green\"><b>" + nearestVillage.GoldText.text + "</b></color>";
                    }
                    else
                    {
                        nearestVillage.GoldText.text = "<color=\"red\"><b>" + nearestVillage.GoldText.text + "</b></color>";
                    }
                    if (nearestVillage.Wood >= nearestVillage.StructValues[type].y)
                    {
                        nearestVillage.WoodText.text = "<color=\"green\"><b>" + nearestVillage.WoodText.text + "</b></color>";
                    }
                    else
                    {
                        nearestVillage.WoodText.text = "<color=\"red\"><b>" + nearestVillage.WoodText.text + "</b></color>";
                    }
                }

                HighlightHex.transform.position = focusedHex.transform.position + new Vector3(0, 0.002f, 0);

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    break;
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (/*nearestVillage.underHex.neighbours.Contains(focusedHex)*/true)
                    {
                        if (nearestVillage.Gold >= nearestVillage.StructValues[type].x && nearestVillage.Wood >= nearestVillage.StructValues[type].y)
                        {
                            nearestVillage.StartCoroutine(nearestVillage.Build(focusedHex, type));
                        }
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            yield return null;
        }

        Dictionary<Village, List<GameObject>> HighsCopy = new Dictionary<Village, List<GameObject>>(Highs);

        foreach (KeyValuePair<Village, List<GameObject>> i in HighsCopy)
        {
            //foreach (GameObject j in i.Value)
            //{
            //    Destroy(j);
            //}

            i.Key.unFocus();
            i.Key.isUsedBySpawner = false;
            Highs.Remove(i.Key);
        }

        Destroy(HighlightHex);
    }
}
