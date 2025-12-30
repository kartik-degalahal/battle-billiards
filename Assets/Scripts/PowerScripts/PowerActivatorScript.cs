using UnityEngine;
using UnityEngine.UI;

public class PowerActivatorScript : MonoBehaviour
{
    [Header("UI References")]
    public GameObject uiPanel;      // The main container for the power icon
    public Image powerIconDisplay;  // The image that changes sprites
    public GameObject highlight;    // The outline/glow object

    [Header("Power Sprites")]
    public Sprite spritePower1;     // Drag your Mortar sprite here
    public Sprite spritePower2;     // Drag your Invincibility sprite here

    //to help swap
    public enum ControlState { Movement, Power };
    public ControlState currentState = ControlState.Movement;

    public bool hasPower; //to check if the person has a power or not
    public bool allotPower; //becomes true only when there is a pending power to be alloted
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //all the powers that we can activate
    public MonoBehaviour movementScript;
    public Mortar power1;
    public int powerID = 0;

    void Start()
    {
        hasPower = false;
        allotPower = false;

        //to ensure that the UI is hidden at start
        if(uiPanel != null) uiPanel.SetActive(false); 
        if(highlight != null) highlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //check if we have a coin
        //when we have a coin, dont collide with coins --> done in CoinKillerScript
        //if we collide with a coin, "pick it up" --> done in CoinKillerScript

        //to toggle between power and movement
        if (Input.GetKeyDown(KeyCode.F) && hasPower) // i think if you add the check here it should work <--------
        {
            ToggleControl();
        }
        //if power is used, hasPower == false

        if(hasPower){
            uiPanel.SetActive(true);
            switch(powerID){
                case 1:
                    powerIconDisplay.sprite = spritePower1;
                    break;
                case 2:
                    powerIconDisplay.sprite = spritePower2;
                    break;
            }
        }
    }
    public void ToggleControl()
    {
        if (currentState == ControlState.Movement)
        {
            // Switch to Power mode
            currentState = ControlState.Power;
            movementScript.enabled = false; // Stop movement
            highlight.SetActive(true); //highlight power UI
            switch (powerID)
            {
                case 1:
                    power1.enabled = true;
                    Debug.Log("Explosive Acquired");// Start power1
                    //powerIconDisplay.sprite = spritePower1;
                    break;
                case 2:
                    BallHealth health = GetComponent<BallHealth>();
                    health.isinvincible= true;
                    Debug.Log("Invincibility Acquired");
                    //powerIconDisplay.sprite = spritePower2;
                    break;
            }
            
            Debug.Log("Switched to POWER mode");
        }
        else
        {
            // Switch back to Movement mode
            currentState = ControlState.Movement;
            movementScript.enabled = true;  // Start movement
            power1.enabled = false;         // Stop power1
            power1.Disabler();
            highlight.SetActive(false); // remove highlight on power UI
            Debug.Log("Switched to MOVEMENT mode");
        }

    }

public void ResetPower()
{
    // --- ADD THIS: Stop physics momentum ---
    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        rb.linearVelocity = Vector2.zero; // Resets speed to zero
        rb.angularVelocity = 0f;          // Stops any spinning
    }

    // Reset Logic
    hasPower = false;
    allotPower = false;
    powerID = 0; 
    
    // Reset State to Movement
    currentState = ControlState.Movement;
    movementScript.enabled = true;

    // --- ADD THIS: Ensure power scripts are actually off ---
    if (power1 != null) 
    {
        power1.enabled = false;
        power1.Disabler(); // Call your cleanup method
    }

    // Reset UI
    if (uiPanel != null) uiPanel.SetActive(false);
    if (highlight != null) highlight.SetActive(false);
    
    Debug.Log("Power used: Physics cleared and UI hidden.");
}
    
}
