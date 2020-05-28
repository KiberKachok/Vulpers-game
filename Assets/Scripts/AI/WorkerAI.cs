using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Magic;

public class WorkerAI : MonoBehaviour
{
    public float Tick = 3;

    Worker worker;

    public List<Hex> Points = new List<Hex>();

    // Start is called before the first frame update
    void Start()
    {
        worker = GetComponent<Worker>();
    }

    public void GoTo(Structure structure)
    {
        List<Hex> path = Pathfinder.Path(worker.underHex, structure.underHex);

        worker.Move(path[0]);
    }

    private void OnMouseDown()
    {
        Debug.Log(Points.Count());
    }

    public IEnumerator Go()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            if (Points.Count > 0)
            {
                List<Hex> path = Pathfinder.PathForUnits(worker.underHex, Points[0]);
                //Debug.Log(worker.name + " " + path.Count + " " + string.Join(",", path));
                if (path.Count > 0)
                {
                    if (path[0].aboveStructure && path[0].aboveStructure as Village)
                    {
                        Village village = path[0].aboveStructure as Village;
                        if (village.state == VillageState.Ruined)
                        {
                            village.state = VillageState.Active;
                            village.team = worker.team;
                            village.Activate();
                            village.gameObject.AddComponent<VillageAI>();
                            worker.underHex = null;
                            Destroy(gameObject);
                            yield break;
                        }
                        else
                        {
                            yield break;
                        }
                    }
                    else
                    {
                        yield return StartCoroutine(worker.MoveCommand(path[0]));

                    }

                    yield return new WaitForSeconds(0);
                }
                else
                {
                    yield break;
                }

            }
            else
            {
                yield break;
            }
        }
    }
}
