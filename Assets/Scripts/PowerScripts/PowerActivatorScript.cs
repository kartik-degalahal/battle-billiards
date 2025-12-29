using UnityEngine;

public class PowerActivatorScript : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        //check if we have a coin
        //when we have a coin, dont collide with coins --> done in CoinKillerScript
        //if we collide with a coin, "pick it up" --> done in CoinKillerScript
        if(allotPower){
            //allot random power

            //temporarily only alloting 1 power

            hasPower = true;
            allotPower = false;
        }

        //to toggle between power and movement
        if (Input.GetKeyDown(KeyCode.F) && hasPower)
        {
            ToggleControl();
        }
        //if power is used, hasPower == false
    }
    public void ToggleControl()
    {
        if (currentState == ControlState.Movement)
        {
            // Switch to Power mode
            currentState = ControlState.Power;
            movementScript.enabled = false; // Stop movement
            switch (powerID)
            {
                case 1:
                    power1.enabled = true;
                    Debug.Log("Explosive Acquired");// Start power1
                    break;
                case 2:
                    BallHealth health = GetComponent<BallHealth>();
                    health.isinvincible= true;
                    Debug.Log("Invincibility Acquired");
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

            Debug.Log("Switched to MOVEMENT mode");
        }
    }

    
}
