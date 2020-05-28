using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Magic;

public class GameController : MonoBehaviour
{
    [SerializeField]
    Camera _camera;

    [SerializeField]
    private GameObject fogObj;

    public Unit focusedUnit = null;

    [SerializeField]
    Village focusedVillage = null;

    [SerializeField]
    GameObject HighlightPref;

    public Dictionary<Vector2, Hex> HexDict = new Dictionary<Vector2, Hex>();
    Coroutine highlightingProcess;

    [ContextMenu("Clean")]
    public void DictCleaner()
    {
        HexDict = new Dictionary<Vector2, Hex>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ToNeighbours();
        highlightingProcess = StartCoroutine(Highlighting());
    }

    [SerializeField]
    ShareTab shareTab;

    [SerializeField]
    TradeTab tradeTab;

    //[SerializeField]
    //VillageTab villageTab;

    IEnumerator Highlighting()
    {
        List<GameObject> highlightHexs = new List<GameObject>();
        Unit newFocused = null;
        Hex newFocusedUnderhex = null;
        while (true)
        {
            if (focusedUnit != newFocused)
            {
                foreach (GameObject i in highlightHexs.GetRange(0, highlightHexs.Count))
                {
                    if (i)
                    {
                        highlightHexs.Remove(i);
                        Destroy(i);
                    }
                }

                if (focusedUnit)
                {
                    if (!focusedUnit.isMoving && highlightHexs.Count == 0)
                    {
                        foreach (Hex i in focusedUnit.underHex.neighbours)
                        {
                            if (i && !i.aboveUnit && (!i.aboveStructure || i.aboveStructure as Field))
                            {
                                highlightHexs.Add(Instantiate(HighlightPref, i.transform.position + new Vector3(0, 0.01f, 0), Quaternion.identity));
                            }
                        }
                    }
                }
            }
            else
            {
                if (focusedUnit)
                {
                    if (newFocusedUnderhex != focusedUnit.underHex)
                    {
                        foreach (GameObject i in highlightHexs.GetRange(0, highlightHexs.Count))
                        {
                            if (i)
                            {
                                highlightHexs.Remove(i);
                                Destroy(i);
                            }
                        }

                        if (focusedUnit)
                        {
                            if (!focusedUnit.isMoving && highlightHexs.Count == 0)
                            {
                                foreach (Hex i in focusedUnit.underHex.neighbours)
                                {
                                    if (i && !i.aboveUnit && (!i.aboveStructure || i.aboveStructure as Field))
                                    {
                                        highlightHexs.Add(Instantiate(HighlightPref, i.transform.position + new Vector3(0, 0.01f, 0), Quaternion.identity));
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (GameObject i in highlightHexs.GetRange(0, highlightHexs.Count))
                    {
                        if (i)
                        {
                            highlightHexs.Remove(i);
                            Destroy(i);
                        }
                    }
                }
            }

            newFocused = focusedUnit;
            if (focusedUnit && !focusedUnit.isMoving)
            {
                newFocusedUnderhex = focusedUnit.underHex;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void Stringer(string str)
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && (!EventSystem.current.IsPointerOverGameObject()))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                switch (hit.collider.gameObject.tag)
                {
                    case "Units":
                        {
                            Unit raycastUnit = hit.collider.gameObject.GetComponent<Unit>();
                            Debug.Log(raycastUnit.name + " Сторона: " + raycastUnit.team + "  HP: " + raycastUnit.currentHp);

                            if (raycastUnit.team == Team.Our)
                            {
                                if (focusedUnit == raycastUnit)
                                {
                                    focusedUnit = null;
                                }
                                else
                                {
                                    focusedUnit = raycastUnit;
                                }
                            }
                            else if (raycastUnit.team == Team.Enemy)
                            {
                                if (focusedUnit as Warrior)
                                {
                                    if (focusedUnit.underHex.neighbours.Contains(raycastUnit.underHex))
                                    {
                                        (focusedUnit as Warrior).Attack(raycastUnit);
                                    }
                                }
                                else if (focusedUnit as Scout)
                                {
                                    if (focusedUnit.underHex.neighbours.Contains(raycastUnit.underHex))
                                    {
                                        (focusedUnit as Scout).Attack(raycastUnit);
                                    }
                                }
                                else if (focusedUnit as General)
                                {
                                    if (focusedUnit.underHex.neighbours.Contains(raycastUnit.underHex))
                                    {
                                        (focusedUnit as General).Attack(raycastUnit);
                                    }
                                }
                                else if(focusedUnit as Archer)
                                {
                                    if (Pathfinder.neighboursReturner(focusedUnit.underHex, 2).Contains(raycastUnit.underHex))
                                    {
                                        (focusedUnit as Archer).Attack(raycastUnit);
                                    }
                                }
                            }

                            //villageTab.StopVillageGUI();
                        }
                        break;

                    case "Ground":
                        {
                            Hex raycastHex = hit.collider.gameObject.GetComponent<Hex>();
                            //Debug.Log("Хекс: " + raycastHex.name);

                            if (focusedUnit)
                            {
                                if (focusedUnit.underHex.neighbours.Contains(raycastHex))
                                {
                                    focusedUnit.Move(raycastHex);
                                }
                                else
                                {
                                    focusedUnit = null;
                                }
                            }

                            //villageTab.StopVillageGUI();
                        }
                        break;
                    
                    //Если нажали на здание
                    case "Struct":
                        {
                            Structure raycastStructure = hit.collider.gameObject.GetComponent<Structure>();
                            if(raycastStructure as Village)
                            {
                                Debug.Log(raycastStructure.name + " Сторона: " + raycastStructure.team + " Состояние: " + (raycastStructure as Village).state + " HP: " + raycastStructure.currentHp);
                            }
                            else
                            {
                                Debug.Log(raycastStructure.name + " Сторона: " + raycastStructure.team + " HP: " + raycastStructure.currentHp);
                            }
                            
                            if (focusedUnit)
                            {
                                //villageTab.StopVillageGUI();

                                //Если в радиусе юнита - то:
                                if (focusedUnit.underHex.neighbours.Contains(raycastStructure.underHex))
                                {
                                    if (focusedUnit as Worker)
                                    {
                                        if (raycastStructure as Village)
                                        {
                                            Village rayVillage = raycastStructure as Village;

                                            if(rayVillage.state == VillageState.Ruined)
                                            {
                                                rayVillage.state = VillageState.Active;
                                                rayVillage.team = focusedUnit.team;
                                                rayVillage.Activate();
                                                focusedUnit.underHex = null;
                                                Destroy(focusedUnit.gameObject);
                                                focusedUnit = null;
                                            }
                                        }
                                    }
                                    else if (focusedUnit as Warrior)
                                    {
                                        if (raycastStructure.team != Team.Our)
                                        {
                                            (focusedUnit as Warrior).Attack(raycastStructure);
                                        }
                                    }
                                    else if (focusedUnit as Scout)
                                    {
                                        if (raycastStructure.team != Team.Our)
                                        {
                                            (focusedUnit as Scout).Attack(raycastStructure);
                                        }
                                    }
                                    else if (focusedUnit as General)
                                    {
                                        if (raycastStructure.team != Team.Our)
                                        {
                                            (focusedUnit as General).Attack(raycastStructure);
                                        }
                                    }
                                    else if (focusedUnit as Archer)
                                    {
                                        if (raycastStructure.team != Team.Our)
                                        {
                                            (focusedUnit as Archer).Attack(raycastStructure);
                                        }
                                    }


                                    //Про торговца тут
                                    else if (focusedUnit as Caravan)
                                    {
                                        if(raycastStructure as Village && (raycastStructure as Village).state == VillageState.Active)
                                        {
                                            if (raycastStructure.team == Team.Our)
                                            {
                                                shareTab.StartShare(focusedUnit as Caravan, raycastStructure as Village);
                                            }
                                            else if(raycastStructure.team == Team.Neutral)
                                            {
                                                tradeTab.StartTrade(focusedUnit as Caravan, raycastStructure as Village);
                                            }
                                        }
                                    }
                                }
                                else if (focusedUnit as Archer)
                                {
                                    if (raycastStructure.team != Team.Our)
                                    {
                                        if (Pathfinder.neighboursReturner(focusedUnit.underHex, 2).Contains(raycastStructure.underHex))
                                        {
                                            (focusedUnit as Archer).Attack(raycastStructure);
                                        }
                                    }
                                }
                                //Иначе сбрасываем
                                else
                                {
                                    focusedUnit = null;
                                }
                            }
                            else
                            {
                                //ЧТО ТО СВЯЗАННОЕ С ПРОКАЧКОЙ ГОРОДА :D
                                //пришло время её написать, но мне нравится этот смайл, не буду удалять

                                if (raycastStructure as Village && raycastStructure.team == Team.Our)
                                {
                                    focusedVillage = raycastStructure as Village;

                                    //villageTab.StopVillageGUI();
                                    //villageTab.StartVillageGUI(focusedVillage);
                                }
                            }

                            
                        }
                        break;
                }
            }
            else
            {
                focusedUnit = null;
            }
        }
    }


    //РЕДАКТОР КАРТЫ
    [ContextMenu("Выровнять - глобально")]
    public void ToNeighbours()
    {
        foreach (Hex hex in HexDict.Values)
        {
            hex.AddHex();
            hex.neighbours = new Hex[6];
        }

        Vector2[] y0 = new Vector2[6] { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(-1, 1) };
        Vector2[] y1 = new Vector2[6] { new Vector2(1, 1), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1) };

        foreach (Hex hex in HexDict.Values)
        {
            for (int i = 0; i < 6; i++)
            {
                Vector2 j = hex.Coords + (y1[i] * Mathf.Abs(hex.Coords.y % 2) + (y0[i] * (1 - Mathf.Abs(hex.Coords.y % 2))));
                if (HexDict.ContainsKey(j))
                {
                    hex.neighbours[i] = HexDict[j];
                }
            }
        }
    }
}
