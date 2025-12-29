using UnityEngine;

public class Mortar : MonoBehaviour
{
    public PowerActivatorScript activator;

    //mortar target
    public GameObject targetPrefab;
    public GameObject bomb;
    private GameObject target;

    private LineRenderer line;
    private Rigidbody2D rb;
    private Vector3 initTargetVector;
    private Vector3 targetVector;
    
    //variables
    public float distanceMultiplier = 3f;
    public float dragLimit = 10f;

    public Player ballOwner;
    private bool isDragging = false;
    public bool isAttacking = false;

    //public AudioSource throwAudio;

    //for gradient of the dragline
    public Gradient powerGradient;

    //  Flag to prevent multiple shots in one turn 
    private bool hasShotThisTurn = false;

    void OnEnable()
{
    hasShotThisTurn = false;
    isDragging = false; // Safety reset
    Debug.Log("Mortar Ready for new shot!");
}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("this script started");
        //getting reference to rigid body
       rb = GetComponent<Rigidbody2D>();
       //getting reference to linerenderer
       line = GetComponent<LineRenderer>();


       //ensure line renderer is based on 2 points and disabled
       line.positionCount = 2;
       line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //  Reset the shot lock if the turn switches back to this player 
        if (TurnManager.Instance.currentPlayer != ballOwner)
        {
            hasShotThisTurn = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (TurnManager.Instance != null && TurnManager.Instance.currentPlayer == ballOwner && !hasShotThisTurn)
            {
                //ScreenToWorldPoint returns vector in same Z as the camera, so we need to add forward so that the 
                //point is visible on the camera
                Vector3 startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward;

                if (Vector2.Distance(startPos, transform.position) < 1.5f)
                {
                    isDragging = true;
                    line.SetPosition(0, startPos); //set starting point for the line
                    line.SetPosition(1, startPos);
                    line.enabled = true; // make the line visible
                }

                //create a new target object at the location of the ball
                initTargetVector = transform.position;
                target = Instantiate(targetPrefab, initTargetVector, Quaternion.identity);
            }
        }
        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward;
            Vector3 lineVector = endPos - line.GetPosition(0); //tracking mouseposition


            //if drag is more than the limit, modify length of vector.
            if (lineVector.magnitude > dragLimit)
            {
                line.SetPosition(1, line.GetPosition(0) + lineVector.normalized * dragLimit);
                targetVector = initTargetVector + -distanceMultiplier * lineVector.normalized * dragLimit;
            }
            else
            {
                line.SetPosition(1, endPos);
                targetVector = initTargetVector + -distanceMultiplier * lineVector;
            }

            //to move the target object
            if (target != null)
            {
                target.transform.position = targetVector;
            }


            //to change the color of the drag line based on how much it is dragged
            //using gradients
            float powerRatio = Mathf.Clamp01(lineVector.magnitude / dragLimit);
            Color currentColor = powerGradient.Evaluate(powerRatio); //uses gradient set in unity
            line.startColor = currentColor;
            line.endColor = currentColor;
        }
        if (Input.GetMouseButtonUp(0) && isDragging)
        {

            //if target is still inside player, will consider that as cancelling the shot
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < 1f)
            {
                isDragging = false;
                line.enabled = false; //remove the line
                Destroy(target);//remove target object
            }
            else
            {
                isDragging = false;

                //  Lock the player from shooting again until the turn switches
                hasShotThisTurn = true;

                line.enabled = false; //remove the line]

                //throwAudio.play(); //to play throwing audio (will prolly add after throw animation)

                Instantiate(bomb, target.transform.position, Quaternion.identity); //add bomb object

                Destroy(target);//remove target object
                /*
                if (TurnManager.Instance != null)
                {
                    TurnManager.Instance.NotifyShotFired();
                }
                */
                activator.hasPower = false;
                activator.ToggleControl();
            }
        }
    }

    public void Disabler()
    {
        isDragging = false;
        line.enabled = false; //remove the line
        Destroy(target);
     }


}
