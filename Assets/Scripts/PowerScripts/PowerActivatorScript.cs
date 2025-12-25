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
    public MonoBehaviour power1;


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
            power1.enabled = true;          // Start power
            Debug.Log("Switched to POWER mode");
        }
        else
        {
            // Switch back to Movement mode
            currentState = ControlState.Movement;
            movementScript.enabled = true;  // Start movement
            power1.enabled = false;         // Stop power
            Debug.Log("Switched to MOVEMENT mode");
        }
    }

    
}
