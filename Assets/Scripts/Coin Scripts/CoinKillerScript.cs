using UnityEngine;

public class CoinKillerScript : MonoBehaviour
{
    private int ID;
    SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //play coin sound
        sr = GetComponent<SpriteRenderer>();
        ID = Random.Range(1, 3);//powerID's are 1 for the bomb and 2 for the invincibility.(for now)
        switch (ID)
        {
            case 1:
                sr.color = Color.grey;
                break;
            case 2:
                sr.color = Color.blue;
                break;
        }
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D other){
    
        if(other.CompareTag("Player")){
            //check if player already has a power. If not, destroy this object and give him a power
            PowerActivatorScript player = other.GetComponent<PowerActivatorScript>();
           
           
            if (player.hasPower == false)
            {
                
                
                player.powerID = ID;
                player.hasPower= true; 

                
                Destroy(gameObject);
            }
        }
    }
}
