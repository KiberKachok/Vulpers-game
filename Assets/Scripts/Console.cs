using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Console : MonoBehaviour
{
    [SerializeField]
    GameController gameController;

    TMP_InputField _console;
    GameObject _textArea;

    public void OnSubmit(string msg)
    {
        if (Input.GetKey(KeyCode.Return))
        {
            List<string> cmd = new List<string>();

            cmd = msg.Split(' ').ToList();

            switch (cmd[0])
            {
                case "show":
                    {
                        switch (cmd[1])
                        {
                            case "hex":
                            case "hexs":
                                {
                                    string listOfHexs = "Количество хексов: " + gameController.HexDict.Count().ToString() + '\n';

                                    foreach (KeyValuePair<Vector2, Hex> k in gameController.HexDict)
                                    {
                                        listOfHexs += k.Value.name + " " + k.Key +  '\n';
                                    }

                                    Debug.Log(listOfHexs);
                                }
                                break;
                        }
                    }
                    break;
            }
        }
        _console.text = "";
    }

    void Awake()
    {
        _textArea = transform.GetChild(0).gameObject;
        _console = GetComponent<TMP_InputField>();
        Hide();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_textArea.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hide();
        }
    }

    void Show()
    {
        _console.text = "";
        _textArea.SetActive(true);
    }

    void Hide()
    {
        _console.text = "";
        _textArea.SetActive(false);
    }
}
