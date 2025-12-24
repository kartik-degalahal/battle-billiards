using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision){
        //if collision is with a coin
        if(collision.gameObject.tag == "Coin"){
            //enable random power.
            Debug.Log("Power acquired!!");
        }
    }
}
