using UnityEngine;

public 
class 
FootSolver : MonoBehaviour
{
    public Transform body;
    public FootSolver otherFoot; 
    public float footSpacing = 0.2f;
    public float stepDistance = 0.5f;
    public float stepHeight = 0.2f;
    public float speed = 5f;
    public LayerMask terrain;

    private Vector3 currentPosition;  
    private Vector3 oldPosition;     
    private Vector3 newPosition;      
    private float lerp;              

    public 
    bool 
    IsStepping {
        get { return lerp < 1f; }
    }

    void 
    Start(){
        currentPosition = transform.position;
        oldPosition = transform.position;
        newPosition = transform.position;
    }

    void 
    Update(){
        transform.position = currentPosition;

        if (!IsStepping && (otherFoot == null || !otherFoot.IsStepping)) {
            Vector3 rayOrigin = body.position + (body.right * footSpacing);
            Ray ray = new Ray(rayOrigin, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit info, 10f, terrain)){
                if (Vector3.Distance(newPosition, info.point) > stepDistance){
                    lerp = 0f;
                    newPosition = info.point;
                }
            }
        }

        if (lerp < 1f) {
            Vector3 footPosition = Vector3.Lerp(oldPosition, newPosition, lerp);

            footPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = footPosition;

            lerp += Time.deltaTime * speed;
        }else{

            oldPosition = newPosition;
        }
    }

    void 
    OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(newPosition, 0.05f);

    }
}
