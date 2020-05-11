using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Dictionary<Vector2, Hex> HexDict = new Dictionary<Vector2, Hex>();

    // Start is called before the first frame update
    void Start()
    {
        ToNeighbours();
    }

    // Update is called once per frame
    void Update()
    {
        
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
