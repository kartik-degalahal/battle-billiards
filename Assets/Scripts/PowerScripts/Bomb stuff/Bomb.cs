using UnityEngine;
using System.Collections.Generic;

public class Bomb : MonoBehaviour
{
    public float blastRadius;
    public float maxDamage;
    public float knockBackScaling;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //play some satisfying beep sound to arm bomb?
    }

    // Update is called once per frame
    void Update()
    {
        //count turns, explode on ? turns
        //explode if player says to explode??
    }

    public void OnTriggerEnter2D(Collider2D other){
    
        if(other.CompareTag("Player")){
            Debug.Log("BOOM!");
            explode();
    }
    }
    public void explode(){
        //for all players
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players){
            //calculate distance between player and ball
            float distance = Vector3.Distance(transform.position, player.transform.position);

            //deal damage if distance < blastRadius
            if(distance < blastRadius){
                //damage = maxDamage*(blastRadius-distance)
                //give knockback, using same formula
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                Vector2 dir = (player.transform.position - transform.position).normalized; //to get direcetion of knockback
                rb.AddForce(knockBackScaling*(blastRadius-distance)*dir, ForceMode2D.Impulse);
            }
        }

        
        
        //explosion animation and sound
        //bomb dissapears
        Destroy(gameObject);
    }
}
