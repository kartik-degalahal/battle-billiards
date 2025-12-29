using UnityEngine;
using TMPro;

public enum Player { Player1, Player2 }

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    public TextMeshProUGUI turnText;
    public Player currentPlayer = Player.Player1;
    public GameObject player1Object; // Assign RedBall here in Inspector
    public GameObject player2Object; // Assign BlueBall here in Inspector
    private GameObject currentplayerobj;

    private bool hasShotBeenFired = false; 

    void Awake() { Instance = this; }
    void Start() { UpdateUI(); }

    public void NotifyShotFired()
    {
        hasShotBeenFired = true;
    }

    void Update()
    {
        
        if (hasShotBeenFired)
        {
            if (AreBallsStopped())
            {
                hasShotBeenFired = false;
                EndTurn();
            }
        }
    }

    bool AreBallsStopped()
    {
        Rigidbody2D[] allBalls = FindObjectsOfType<Rigidbody2D>();
        foreach (Rigidbody2D rb in allBalls)
        {
          
            if (rb.linearVelocity.magnitude > 0.15f)
            {
                return false;
            }
        }
        return true; 
    }

    public void EndTurn()
    {
        currentPlayer = (currentPlayer == Player.Player1) ? Player.Player2 : Player.Player1;
        if (currentPlayer == Player.Player2)
        {
            currentplayerobj = player2Object;
        }
        else { currentplayerobj = player1Object; }
        currentplayerobj.GetComponent<BallLauncher>().isAttacking=false;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (turnText != null)
        {
            turnText.text = (currentPlayer == Player.Player1) ? "Red's Turn" : "Blue's Turn";
            turnText.color = (currentPlayer == Player.Player1) ? Color.red : Color.blue;
        }
    }
}