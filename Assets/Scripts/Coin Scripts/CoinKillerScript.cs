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
            Debug.Log("Coin died");
            Destroy(gameObject);
    }
    }
}
