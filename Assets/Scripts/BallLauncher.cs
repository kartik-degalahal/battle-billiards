using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    private LineRenderer line;
    private Rigidbody2D rb;
    
    //variables
    public float forceMultiplier = 3f;
    public float dragLimit = 10f;

    //for gradient of the dragline
    public Gradient powerGradient;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        }
        if(Input.GetMouseButton(0)){
            Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition)+ Vector3.forward;
            Vector3 lineVector = endPos-line.GetPosition(0);

            //if drag is more than the limit, modify length of vector.
            if(lineVector.magnitude> dragLimit){
                line.SetPosition(1,line.GetPosition(0)+lineVector.normalized*dragLimit);
            }
            else{
                line.SetPosition(1,endPos);
            }

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
            //adding force based on startPos and EndPos
            Vector3 lineVector = line.GetPosition(1)-line.GetPosition(0);
            rb.AddForce(-forceMultiplier*lineVector, ForceMode2D.Impulse);
        }
    }
}
