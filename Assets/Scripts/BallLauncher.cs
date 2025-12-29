using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    private LineRenderer line;
    private Rigidbody2D rb;

    public float forceMultiplier = 3f;
    public float dragLimit = 10f;
    public Gradient powerGradient;

    public Player ballOwner;
    private bool isDragging = false;
    public bool isAttacking = false;

    //  Flag to prevent multiple shots in one turn 
    private bool hasShotThisTurn = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false;
    }
    void FixedUpdate()
    {
        // Check if the ball is moving very slowly
        if (rb.linearVelocity.magnitude < 0.5f)
        {
            // Force it to a complete stop
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f; // Stop any spinning too
        }
    }
    void Update()
    {
        //  Reset the shot lock if the turn switches back to this player 
        if (TurnManager.Instance.currentPlayer != ballOwner)
        {
            hasShotThisTurn = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //  Added check for hasShotThisTurn 
            if (TurnManager.Instance != null && TurnManager.Instance.currentPlayer == ballOwner && !hasShotThisTurn)
            {
                Vector3 startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward;

                if (Vector2.Distance(startPos, transform.position) < 1.5f)
                {
                    isDragging = true;
                    line.SetPosition(0, startPos);
                    line.SetPosition(1, startPos);
                    line.enabled = true;
                }
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            isAttacking = true;
            Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward;
            Vector3 lineVector = endPos - line.GetPosition(0);

            if (lineVector.magnitude > dragLimit)
            {
                line.SetPosition(1, line.GetPosition(0) + lineVector.normalized * dragLimit);
            }
            else
            {
                line.SetPosition(1, endPos);
            }

            float powerRatio = Mathf.Clamp01(lineVector.magnitude / dragLimit);
            Color currentColor = powerGradient.Evaluate(powerRatio);
            line.startColor = currentColor;
            line.endColor = currentColor;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            
           

            //  Lock the player from shooting again until the turn switches
            hasShotThisTurn = true;

            line.enabled = false;

            Vector3 lineVector = line.GetPosition(1) - line.GetPosition(0);
            rb.AddForce(-forceMultiplier * lineVector, ForceMode2D.Impulse);

            if (TurnManager.Instance != null)
            {
                TurnManager.Instance.NotifyShotFired();
            }
        }
    }
}