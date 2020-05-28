using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CaravanAI : MonoBehaviour
{
    public float Tick = 3;

    Caravan caravan;
                    //x - золото, y - дерево, z - железо, w - еда "-" забрать деньги у города
    public Dictionary<Village, Vector4> Commands = new Dictionary<Village, Vector4>();

    // Start is called before the first frame update
    void Start()
    {
        caravan = GetComponent<Caravan>();
    }

    public void GoTo(Structure structure)
    {
        List<Hex> path = Pathfinder.Path(caravan.underHex, structure.underHex);

        caravan.Move(path[0]);
    }

    private void OnMouseDown()
    {
        Debug.Log(Commands.Count());
    }

    public IEnumerator Go()
    {
        while (true)
        {
            if(Commands.Count > 0)
            {
                //Village village = Commands.ElementAt(0).Key as Village;
                KeyValuePair<Village, Vector4> village = Commands.ElementAt(0);
                if (caravan.underHex.neighbours.Contains(village.Key.underHex))
                {
                    float gold = village.Value.x > 0 ? Mathf.Clamp(Mathf.Abs(village.Value.x), 0, village.Key.Gold) : -Mathf.Clamp(Mathf.Abs(village.Value.x), 0, village.Key.Gold);
                    float wood = village.Value.y > 0 ? Mathf.Clamp(Mathf.Abs(village.Value.x), 0, village.Key.Gold) : -Mathf.Clamp(Mathf.Abs(village.Value.y), 0, village.Key.Wood);
                    float iron = village.Value.z > 0 ? Mathf.Clamp(Mathf.Abs(village.Value.x), 0, village.Key.Gold) : -Mathf.Clamp(Mathf.Abs(village.Value.z), 0, village.Key.Iron);
                    float food = village.Value.w > 0 ? Mathf.Clamp(Mathf.Abs(village.Value.x), 0, village.Key.Gold) : -Mathf.Clamp(Mathf.Abs(village.Value.w), 0, village.Key.Food);

                    village.Key.Gold += gold;
                    village.Key.Wood += wood;
                    village.Key.Iron += iron;
                    village.Key.Food += food;

                    caravan.Gold -= gold;
                    caravan.Wood -= wood;
                    caravan.Iron -= iron;
                    caravan.Food -= food;

                    Commands.Remove(Commands.ElementAt(0).Key);
                }
                else
                {
                    List<Hex> path = Pathfinder.PathForUnits(caravan.underHex, village.Key.underHex);
                    yield return StartCoroutine(caravan.MoveCommand(path[0]));
                    yield return new WaitForSeconds(0);
                }
            }
            else
            {
                yield break;
            }
        }
    }
}
