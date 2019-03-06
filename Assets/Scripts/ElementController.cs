using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementController : MonoBehaviour
{

    public GameObject Controller;

    private Renderer rend;
    private GameController GameController;
    private Color previousColor;
    private List<string> Elements = new List<string>() { "Spock", "Lizard", "Rock", "Paper", "Scissor" };

    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        previousColor = rend.material.color;
        GameController = Controller.GetComponent<GameController>();
    }

    void OnMouseEnter()
    {
        rend.material.color = Color.yellow;
        previousColor = Color.yellow;
    }
    void OnMouseExit()
    {
        rend.material.color = Color.white;
        previousColor = Color.white;
    }
    void OnMouseDown()
    {
        //Mouse can be clicked only if Coroutine is not running
        if (!GameController.isRunning)
        {
            // Passing the selected element by user to GameController
            GameController.Game(Elements.IndexOf(gameObject.name));
            rend.material.color = Color.red;
        }
    }
}
