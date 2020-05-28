using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Magic;

public class EmperorAI : MonoBehaviour
{
    int workerCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Strateg());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Strateg()
    {
        while (true)
        {
            List<Village> inter = FindObjectsOfType<Village>().ToList();

            List<Village> ruined = inter.GetRange(0, inter.Count);

            foreach (Village i in ruined.GetRange(0, ruined.Count))
            {
                if (i.state == VillageState.Active)
                {
                    ruined.Remove(i);
                }
            }
            List<Village> villages = inter.GetRange(0, inter.Count);

            foreach(Village i in villages.GetRange(0, villages.Count))
            {
                if (i.Gold > i.UnitValues[UnitType.Worker].x && i.Iron > i.UnitValues[UnitType.Worker].y)
                {

                }
                else
                {
                    villages.Remove(i);
                }
            }

            if(villages.Count > 0 && ruined.Count > 0)
            {
                Village nVillage = null;
                Village nRuin = null;

                float minDist = 0;

                foreach(Village i in villages)
                {
                    foreach(Village j in ruined)
                    {
                        if(Vector3.Distance(i.transform.position, j.transform.position) < minDist)
                        {
                            minDist = Vector3.Distance(i.transform.position, j.transform.position);
                        }

                        nVillage = i;
                        nRuin = j;
                    }
                }

                workerCount++;
                string nameOfWorker = "worker" + workerCount.ToString();
                yield return StartCoroutine(nVillage.CreateUnit(Pathfinder.PathForUnits(nVillage.underHex, nRuin.underHex)[0], UnitType.Worker, nameOfWorker));
                GameObject worker = GameObject.Find(nameOfWorker);
                WorkerAI workAi = worker.AddComponent<WorkerAI>();

                workAi.Points.Add(nRuin.underHex);
                yield return StartCoroutine(workAi.Go());
            }

            yield return new WaitForSeconds(5f);
        }
    }
}
