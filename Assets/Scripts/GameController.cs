using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region UIElements
    public Button Restart;
    public Text RoundResult;
    public Text PlayerScoreText;
    private int PlayerScore = 0;
    public Text OpponentScoreText;
    private int OpponentScore = 0;
    #endregion UIElements

    [HideInInspector]
    public bool isRunning = false;
    public GameObject Opponent;
    public float flipDuration;

    //Indexes are required to calculate winner of each round 
    private readonly List<string> Elements = new List<string>() { "Spock", "Lizard", "Rock", "Paper", "Scissor" };
    private int OppMove;
   

    void Start()
    {
        Restart.onClick.AddListener(RestartGame);
    }

    public void Game(int PlayerElement)
    {
        StartCoroutine(RoundResultCoroutine(PlayerElement));
    }

    //Coroutine running every round
    IEnumerator RoundResultCoroutine(int PlayerElement)
    {
        //Since Range with integers are not maximally inclusive
        OppMove = Random.Range(0, 5);

        //Activate child Opponent played
        Opponent.transform.GetChild(OppMove).transform.gameObject.SetActive(true);

        float t = 0;
        //isRunning is used in order to prevent user inteference during each round is held.

        isRunning = true;

        while (t < flipDuration)
        {
            t += Time.deltaTime;
            Opponent.transform.eulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 180, 0), t / flipDuration);
            yield return null;
        }

        /*
         * Player has equal probability of winning or losing since there are odd number of elements.
         * From the rules we can infer with the indexing of Spock = 0, Lizard = 1, Rock = 2, Paper = 3, Scissor = 4
         * if the absolute difference between Player and opponent element is even, then element with smaller index wins
         * if it is odd, the element with bigger index wins. Because as we can see from the graph and imagine it as a directed graph
         * an element always wins against the next element and loses against the previous element. Also it wins against next element of its directed 
         * neighbor. This mathematical solution is implemented in order to eliminate keeping track of all possible Rules between two chosen elements.
         * Also this aproach would work with any N = #elements given that N is odd. N should be odd to have same probability for each element in order to
         * have a fair game.
         */
        if (PlayerElement == OppMove)
        {
            RoundResult.text = "It is a tie";
            RoundResult.GetComponent<Text>().color = Color.white;
        }
        //Having even absolute difference
        else if (Mathf.Abs(OppMove - PlayerElement) % 2 == 0)
        {
            if (PlayerElement > OppMove)
            {
                UpdateScore("Opponent", PlayerElement, OppMove);
            }
            else
            {
                UpdateScore("Player", PlayerElement, OppMove);
            }
        }
        //Having odd absolute difference
        else
        {
            if (PlayerElement > OppMove)
            {
                UpdateScore("Player", PlayerElement, OppMove);
            }
            else
            {
                UpdateScore("Opponent", PlayerElement, OppMove);
            }
        }

        yield return new WaitForSeconds(flipDuration * 2);
        RoundResult.text = " ";
        t = 0;

        while (t < flipDuration)
        {
            t += Time.deltaTime;
            Opponent.transform.eulerAngles = Vector3.Lerp(new Vector3(0, 180, 0), new Vector3(0, 0, 0), t / flipDuration);
            yield return null;
        }

        Opponent.transform.GetChild(OppMove).transform.gameObject.SetActive(false);
        isRunning = false;
    }

    // Method used for updating score and modifying round result text
    private void UpdateScore(string winner, int PlayerMove, int OpponentMove)
    {
        if (winner == "Player")
        {
            PlayerScore++;
            PlayerScoreText.text = PlayerScore.ToString();
            RoundResult.text = Elements[PlayerMove] + " wins against " + Elements[OpponentMove];
            RoundResult.GetComponent<Text>().color = Color.green;
        }
        else if (winner == "Opponent")
        {
            OpponentScore++;
            OpponentScoreText.text = OpponentScore.ToString();
            RoundResult.text = Elements[PlayerMove] + " loses against " + Elements[OpponentMove];
            RoundResult.GetComponent<Text>().color = Color.red;
        }
    }

    private void RestartGame()
    {
        if (!isRunning)
        {
            PlayerScore = 0;
            PlayerScoreText.text = PlayerScore.ToString();
            OpponentScore = 0;
            OpponentScoreText.text = OpponentScore.ToString();
        }
    }
}
