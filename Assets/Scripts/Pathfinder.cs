using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
