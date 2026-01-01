using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public TextMeshProUGUI winMessageText;
    public GameObject game_End;
    public TextMeshProUGUI turnText;
    public bool isinvincible;

    // Drag the "Fill" image (the green one) into this slot in the Inspector
    public Image healthFill;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Detects when the ball hits something
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Ignore Walls
        if (collision.gameObject.CompareTag("Wall")) return;

        // 2. Try to find the BallLauncher on the OTHER object we hit
        BallLauncher otherLauncher = collision.gameObject.GetComponent<BallLauncher>();

        if (otherLauncher != null && otherLauncher.isAttacking)
        {
            // Calculate damage
            float impactForce = collision.relativeVelocity.magnitude;

            if (impactForce > 1.5f)
            {
                TakeDamage(impactForce * 1f);

                // 3. Reset the attacker so it doesn't do "double damage" 
                // if it bounces and hits the same ball twice in one frame
                otherLauncher.isAttacking = false;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (isinvincible) { return; }
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update the bar visually
        if (healthFill != null)
        {
            healthFill.fillAmount = currentHealth / maxHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        string winner = (gameObject.name == "RedBall") ? "BLUE PLAYER" : "RED PLAYER";
        if (winMessageText != null)
        {
            winMessageText.text = winner + " WINS!";
            game_End.SetActive(true);
        }
        Debug.Log(gameObject.name + " destroyed!");
        Destroy(gameObject);
        turnText.text = "";
        Time.timeScale = 0;
        
    }
}