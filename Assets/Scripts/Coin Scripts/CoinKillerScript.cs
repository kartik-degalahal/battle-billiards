using UnityEngine;

public class CoinKillerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //play coin sound
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D other){
    
        if(other.CompareTag("Player")){
            //check if player already has a power. If not, destroy this object and give him a power
            PowerActivatorScript player = other.GetComponent<PowerActivatorScript>();
            if(!player.hasPower){
                Debug.Log("Coin died");
                player.allotPower = true;
                Destroy(gameObject);
            }
    }
    }
}
