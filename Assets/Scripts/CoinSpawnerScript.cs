using UnityEngine;

public class CoinSpawnerScript : MonoBehaviour
{
    public GameObject coin;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            Debug.Log("Key was pressed!! spawning coin");
            spawnCoin();
        }
    }

    private void spawnCoin(){
        bool coinSpawned = false;
        int attempts = 0;
        while(!coinSpawned & attempts<100){
            attempts++; //to prevent infinite loop

            //to create vector with random coordinates within the arena
            float xCoord = Random.Range(-7.76f,7.76f); //hard-coded the boundaries of the arena
            float yCoord = Random.Range(-3.88f,3.88f);
            Vector2 spawnPos = new Vector2(xCoord, yCoord);

            //check if there is a collidable object in the area
            Collider2D hit = Physics2D.OverlapCircle(spawnPos, 0.6f); //hit == null if there is no collision
            if(hit==null){
                Instantiate(coin, spawnPos,Quaternion.identity );
                coinSpawned = true;
            }
        }

    }
}
