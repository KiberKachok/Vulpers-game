using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Magic;
public class VillageAI : MonoBehaviour
{
    public float Tick = 3;
    Village village;

    // Start is called before the first frame update
    void Start()
    {
        village = GetComponent<Village>();
        StartCoroutine(FoodRuler());
        StartCoroutine(WoodRuler());
        StartCoroutine(MineRuler());

        Time.timeScale = 6;

    }

    public IEnumerator FoodRuler()
    {
        while (true)
        {
            if(village.HungerCoef < 0 && village.state == VillageState.Active)
            {
                if(village.Wood >= village.StructValues[ResourceType.Farm].y)
                {
                    if (village.Gold >= village.StructValues[ResourceType.Farm].x)
                    {
                        //построить ферму
                        List<Hex> ableHexs = Pathfinder.neighboursReturner(village.underHex, 4).Except(Pathfinder.neighboursReturner(village.underHex, 2)).ToList();

                        foreach (Hex i in ableHexs.GetRange(0, ableHexs.Count()))
                        {
                            if(i.aboveStructure || i.aboveUnit)
                            {
                                ableHexs.Remove(i);
                            }
                        }

                        if (ableHexs.Count > 0)
                        {
                            yield return StartCoroutine(village.Build(ableHexs[Mathf.FloorToInt(Random.Range(0, ableHexs.Count))], ResourceType.Farm));
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    yield return StartCoroutine(Ask(new Vector4(village.StructValues[ResourceType.Farm].x, village.StructValues[ResourceType.Farm].y, 0, 0)));
                }
            }

            yield return new WaitForSeconds(Tick);
        }
    }

    public IEnumerator WoodRuler()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            //Debug.Log("hm");
            if (village.HungerCoef > 0 && village.SawmillCount == 0 && village.state == VillageState.Active)
            {
                if (village.Wood >= village.StructValues[ResourceType.Sawmill].y)
                {
                    if (village.Gold >= village.StructValues[ResourceType.Sawmill].x)
                    {

                        List<Hex> ableHexs = Pathfinder.neighboursReturner(village.underHex, 4);/*.Except(Pathfinder.neighboursReturner(village.underHex, 2)).ToList();*/
                        //Debug.Log(string.Join(",", ableHexs));
                        foreach (Hex i in ableHexs.GetRange(0, ableHexs.Count()))
                        {
                            if (i.aboveStructure || i.aboveUnit)
                            {
                                ableHexs.Remove(i);
                            }
                            else
                            {
                                int forestCount = 0;

                                foreach (Hex j in i.neighbours)
                                {
                                    if (j && j.aboveStructure as Forest)
                                    {
                                        forestCount++;
                                    }
                                }

                                if (forestCount == 0)
                                {
                                    ableHexs.Remove(i);
                                }
                            }
                        }

                        int ask = Mathf.FloorToInt(Random.Range(0, ableHexs.Count));
                        //Debug.Log(ask);
                        if(ableHexs.Count > 0)
                        {
                            StartCoroutine(village.Build(ableHexs[Mathf.FloorToInt(Random.Range(0, ableHexs.Count))], ResourceType.Sawmill));
                        }
                    }
                }
                else
                {
                    yield return StartCoroutine(Ask(new Vector4(village.StructValues[ResourceType.Sawmill].x, village.StructValues[ResourceType.Sawmill].y, 0, 0)));
                }
            }

            yield return new WaitForSeconds(Tick);
        }
    }

    public IEnumerator MineRuler()
    {
        while (true)
        {
            if (village.HungerCoef > 0 && village.SawmillCount >= 1 && village.MineCount < 1 && village.state == VillageState.Active)
            {
                if (village.Wood >= village.StructValues[ResourceType.Mine].y)
                {
                    if (village.Gold >= village.StructValues[ResourceType.Mine].x)
                    {
                        //построить ферму
                        List<Hex> ableHexs = Pathfinder.neighboursReturner(village.underHex, 4).Except(Pathfinder.neighboursReturner(village.underHex, 2)).ToList();

                        foreach (Hex i in ableHexs.GetRange(0, ableHexs.Count()))
                        {
                            if (i.aboveStructure && i.aboveUnit)
                            {
                                ableHexs.Remove(i);
                            }
                            //else
                            //{
                            //    int forestCount = 0;

                            //    foreach (Hex j in i.neighbours)
                            //    {
                            //        if (j && j.aboveStructure as Forest)
                            //        {
                            //            forestCount++;
                            //        }
                            //    }

                            //    if (forestCount == 0)
                            //    {
                            //        ableHexs.Remove(i);
                            //    }
                            //}
                        }

                        if (ableHexs.Count > 0)
                        {
                            StartCoroutine(village.Build(ableHexs[Mathf.FloorToInt(Random.Range(0, ableHexs.Count))], ResourceType.Mine));
                        }
                    }
                }
                else
                {
                    yield return StartCoroutine(Ask(new Vector4(village.StructValues[ResourceType.Mine].x, village.StructValues[ResourceType.Mine].y, 0, 0)));
                }
            }

            yield return new WaitForSeconds(Tick);
        }
    }

    public IEnumerator Ask(Vector4 res)
    {
        List<Village> villages = FindObjectsOfType<Village>().ToList();

        foreach (Village i in villages.GetRange(0, villages.Count))
        {
            if (i == village || i.team != Team.Enemy || i.SawmillCount == 0 || i.state != VillageState.Active || i.Gold < res.x || i.Wood < res.y || i.Iron < res.z || i.Food < res.w)
            {
                villages.Remove(i);
            }
        }

        if (villages.Count > 0)
        {
            float distanceBetween = Mathf.Infinity;
            float dist = 0;

            Village nearestVillage = null;

            foreach (Village i in villages)
            {
                dist = Vector3.Distance(i.transform.position, transform.position);

                if (dist < distanceBetween)
                {
                    nearestVillage = i;
                    distanceBetween = dist;
                }
            }

            List<Caravan> caravans = FindObjectsOfType<Caravan>().ToList();
            Caravan nearestCaravan = null;
            float distanceBetweenCaravans = Mathf.Infinity;
            float distCaravans = 0;

            foreach (Caravan i in caravans)
            {
                distCaravans = Vector3.Distance(i.transform.position, nearestVillage.transform.position);

                if (distCaravans < distanceBetweenCaravans && i.team == Team.Enemy && i.GetComponent<CaravanAI>() && i.GetComponent<CaravanAI>().Commands.Count == 0)
                {
                    nearestCaravan = i;
                    distanceBetweenCaravans = distCaravans;
                }
            }

            if (!nearestCaravan && nearestVillage.Gold >= nearestVillage.UnitValues[UnitType.Caravan].x && nearestVillage.Iron >= nearestVillage.UnitValues[UnitType.Caravan].y)
            {
                yield return StartCoroutine(nearestVillage.CreateUnit(Pathfinder.PathForUnits(nearestVillage.underHex, village.underHex)[0], UnitType.Caravan));
            }

            //caravans = FindObjectsOfType<Caravan>().ToList();
            //nearestCaravan = null;
            //distanceBetweenCaravans = Mathf.Infinity;
            //distCaravans = 0;

            //foreach (Caravan i in caravans)
            //{
            //    distCaravans = Vector3.Distance(i.transform.position, nearestVillage.transform.position);

            //    if (distCaravans < distanceBetweenCaravans && i.team == Team.Enemy && i.GetComponent<CaravanAI>() && i.GetComponent<CaravanAI>().Commands.Count == 0)
            //    {
            //        nearestCaravan = i;
            //        distanceBetweenCaravans = distCaravans;
            //    }
            //}

            if (nearestCaravan)
            {
                CaravanAI nearestCaravanAI = nearestCaravan.GetComponent<CaravanAI>();

                nearestCaravanAI.Commands = new Dictionary<Village, Vector4>
                {
                    {nearestVillage, new Vector4(-res.x, -res.y, -res.z, -res.w)},
                    {village, new Vector4(res.x, res.y, res.z, res.w)}
                };

                yield return StartCoroutine(nearestCaravanAI.Go());
            }
        }
        else
        {
            
        }
    }

    //public IEnumerator ForTheEmperor()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(10f);
    //    }
    //}
}
