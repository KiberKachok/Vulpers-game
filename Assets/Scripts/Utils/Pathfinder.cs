using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Magic;
using System.Linq;

public class Pathfinder : MonoBehaviour
{
    public Hex start;
    public Hex finish;

    [ContextMenu("Поиск Пути")]
    void aligner()
    {
        Debug.Log(string.Join(",", Path(start, finish)));
    }

    static public List<Hex> Path(Hex start, Hex finish)
    {
        //Точка, и откуда в неё пришли
        Dictionary<Hex, Hex> HexsParents = new Dictionary<Hex, Hex>();

        Queue<Hex> queue = new Queue<Hex>();
        HashSet<Hex> exploredHexs = new HashSet<Hex>();
        queue.Enqueue(start);

        while (queue.Count != 0)
        {
            Hex current = queue.Dequeue();
            if (current == finish)
            {
                break;
            }

            Hex[] neighbours = current.neighbours;

            foreach (Hex hex in neighbours)
            {
                if (hex != null && !exploredHexs.Contains(hex))
                {
                    exploredHexs.Add(hex);
                    HexsParents.Add(hex, current);
                    queue.Enqueue(hex);
                }
            }
        }

        Hex currentDraw = finish;
        List<Hex> path = new List<Hex>();
        while (currentDraw != start)
        {
            if (HexsParents.ContainsKey(currentDraw))
            {
                path.Add(currentDraw);
                currentDraw = HexsParents[currentDraw];
            }
            else
            {
                return new List<Hex>();
            }
        }

        path.Reverse();
        return path;
    }

    public static List<Hex> neighboursReturner(Hex hex, int dimension)
    {
        List<Hex> neighbours = new List<Hex> { hex };

        for (int i = 0; i < dimension; i++)
        {
            List<Hex> interNei = neighbours.GetRange(0, neighbours.Count);
            foreach (Hex j in interNei)
            {
                foreach (Hex k in j.neighbours)
                {
                    if (!neighbours.Contains(k) && k != null)
                    {
                        neighbours.Add(k);
                    }
                }
            }
        }
        return neighbours;
    }

    public static Village findNearestVillage(Vector3 point, List<Village> villages)
    {
        Village village = null;
        float minDist = Mathf.Infinity;

        foreach(Village i in villages)
        {
            if(i.team == Team.Our && Vector3.Distance(i.transform.position, point) < minDist)
            {
                minDist = Vector3.Distance(i.transform.position, point);
                village = i;
            }
        }


        return village;
    }

    public static List<Village> FindVillagesOfTeams(Team team)
    {
        List<Village> villages = FindObjectsOfType<Village>().ToList();

        foreach(Village i in villages.GetRange(0, villages.Count() - 1))
        {
            if(i.team != team)
            {
                villages.Remove(i);
            }
        }

        return villages;
    }
}
