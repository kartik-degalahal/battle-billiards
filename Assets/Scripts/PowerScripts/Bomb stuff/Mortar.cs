using UnityEngine;

public class Mortar : MonoBehaviour
{

    //mortar target
    public GameObject target;
    public GameObject bomb;

    private LineRenderer line;
    private Rigidbody2D rb;
    private Vector3 initTargetVector;
    private Vector3 targetVector;
    
    //variables
    public float distanceMultiplier = 3f;
    public float dragLimit = 10f;

    //for gradient of the dragline
    public Gradient powerGradient;


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

        if(Input.GetMouseButtonDown(0)){
            //ScreenToWorldPoint returns vector in same Z as the camera, so we need to add forward so that the 
            //point is visible on the camera
            Vector3 startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition)+ Vector3.forward;
            line.SetPosition(0,startPos); //set starting point for the line
            line.SetPosition(1,startPos);
            line.enabled = true; // make the line visible

            //create a new target object at the location of the ball
            initTargetVector = transform.position;
            target = Instantiate(target, initTargetVector,Quaternion.identity );
        }
        if(Input.GetMouseButton(0)){
            Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition)+ Vector3.forward;
            Vector3 lineVector = endPos-line.GetPosition(0); //tracking mouseposition


            //if drag is more than the limit, modify length of vector.
            if(lineVector.magnitude> dragLimit){
                line.SetPosition(1,line.GetPosition(0)+lineVector.normalized*dragLimit);
                targetVector = initTargetVector + -distanceMultiplier*lineVector.normalized*dragLimit;
            }
            else{
                line.SetPosition(1,endPos);
                targetVector = initTargetVector + -distanceMultiplier*lineVector;
            }

            //to move the target object
            target.transform.position = targetVector;


            //to change the color of the drag line based on how much it is dragged

            /*
            //combine green and red using the ratio of vector magnitude and drag limit
            float powerRatio = Mathf.Clamp01(lineVector.magnitude/dragLimit);
            Color mixColor = Color.Lerp(Color.green, Color.red, powerRatio);
            line.startColor = mixColor;
            line.endColor = mixColor;
            */

            //using gradients
            float powerRatio = Mathf.Clamp01(lineVector.magnitude/dragLimit);
            Color currentColor = powerGradient.Evaluate(powerRatio); //uses gradient set in unity
            line.startColor = currentColor;
            line.endColor = currentColor;
        }
        if(Input.GetMouseButtonUp(0)){
            line.enabled = false; //remove the line
            Instantiate(bomb,target.transform.position, Quaternion.identity ); //add bomb object

            Destroy(target);//remove target object
            /*
            This is from ball launcher script
            
            //adding force based on startPos and EndPos
            Vector3 lineVector = line.GetPosition(1)-line.GetPosition(0);
            rb.AddForce(-forceMultiplier*lineVector, ForceMode2D.Impulse);
            */
        }
    }
}
