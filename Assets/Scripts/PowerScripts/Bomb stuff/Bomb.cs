using UnityEngine;
using System.Collections.Generic;


public class Bomb : MonoBehaviour
{
    public GameObject explosionPrefab; 

    [Range(0f, 1f)] public float armingVolume = 0.5f;
    [Range(0f, 1f)] public float explosionVolume = 1.0f;
    public AudioClip armingClip;
    public AudioClip explosionClip;

    public float blastRadius;
    public float maxDamage;
    public float knockBackScaling;
    public LayerMask playerLayer;

    public int turnsRemaining = 4;
    private Player initialOwner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialOwner = TurnManager.Instance.currentPlayer;
        //play some satisfying beep sound to arm bomb?
    }

    // Update is called once per frame
    void Update()
    {
        //explode if someone comes in proximity to the bomb
        checkProximity();

        //count turns, explode on ? turns
        checkTurnChange();
        
        //explode if player says to explode??
    }
    private bool isExploding = false;

    void checkProximity(){
        if(isExploding) return;
        //check if anyone is hit within range
        Collider2D hit = Physics2D.OverlapCircle(transform.position, blastRadius, playerLayer);
        if(hit!=null){
            //prevent owner from blowing up on first throw
            if(TurnManager.Instance.currentPlayer == initialOwner && turnsRemaining == 4) return;
            if (armingClip != null) {
                AudioSource.PlayClipAtPoint(armingClip, transform.position, armingVolume);
            }
            // Start the delayed explosion
            StartCoroutine(DelayedExplosion(0.5f));
        }
    }
    System.Collections.IEnumerator DelayedExplosion(float delay) {
    isExploding = true;
    
    // Optional: add a 'fast beep' sound or visual effect here 
    // to warn the player it's about to blow!
    
    yield return new WaitForSeconds(delay);
    explode();
}

    void checkTurnChange(){
        if(TurnManager.Instance.currentPlayer!=initialOwner){
            turnsRemaining--;
            initialOwner = TurnManager.Instance.currentPlayer;//to prevent this loop from happening infinitely in 1 turn
            if (turnsRemaining<=0){
                explode();
            }

        }
    }
    

    void explode() {
        // 1. Only find objects within the actual blast zone using Physics
        Collider2D[] objectsInBlast = Physics2D.OverlapCircleAll(transform.position, blastRadius, playerLayer);
        foreach (Collider2D hit in objectsInBlast) {
            // 2. Check if the object is a Player (using Tag or Component)
            if (hit.CompareTag("Player")) {
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

                if (rb != null) {
                    float distance = Vector3.Distance(transform.position, hit.transform.position);
                    
                    // 3. Direction logic with a safety check for Zero
                    Vector2 dir = (hit.transform.position - transform.position).normalized;
                    if (dir == Vector2.zero) dir = Random.insideUnitCircle.normalized;

                    // 4. formula
                    float forceStrength = knockBackScaling * (blastRadius - distance);
                    
                    rb.AddForce(dir * forceStrength, ForceMode2D.Impulse);

                    Debug.Log("Hitting: " + hit.name + " with force: " + forceStrength);
                    rb.AddForce(dir * forceStrength, ForceMode2D.Impulse);
                }
            }
        }
        //explosion effect and sound
        if (explosionClip != null) {
            AudioSource.PlayClipAtPoint(explosionClip, transform.position, explosionVolume);
        }
        if (explosionPrefab != null) {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        // Destroying the bomb
        Destroy(gameObject);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
        
        


