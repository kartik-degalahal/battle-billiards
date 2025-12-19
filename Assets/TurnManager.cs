using UnityEngine;
using TMPro;
public class TurnManager:MonoBehaviour
{
    public TextMeshProUGUI turnText;
    void Start()
    {
        if (turnText != null)
        {
            turnText.text = "Player 1, It's Your Turn!";
        }
    }
}
