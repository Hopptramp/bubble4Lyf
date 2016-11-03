using UnityEngine;
using System.Collections;

[System.Serializable]
public class ControlState
{
    public bool FIRE = false;
    public bool DETACH = false;   
    public Vector2 MOVEMENT = Vector2.zero;
    public Vector2 AIM = Vector2.up;
    
}

public class PlayerMovement : MonoBehaviour {

    [SerializeField]
    public GameObject attacherPrefab;
    private GameObject attacherInstance;
    private Attacher attacherScript;
    [SerializeField]
    private Rigidbody2D selfRB;
    [SerializeField]
    private bool useDebugInput;
    public float controlSpeed = 5;
    
    private ControlState currControls = new ControlState();
    private ControlState prevControls = new ControlState();
    public Vector2 aimingDirection { get; private set; }

    public void SetControlState(ControlState _new)
    {
        prevControls = currControls;
        currControls = _new;        
    }

	// Use this for initialization
	void Awake () {
	
	}

    public void Reset() { selfRB.velocity = Vector3.zero; SetControlState(new ControlState()); }
    public void Reset(Vector2 _spawnPos) { selfRB.velocity = Vector3.zero; SetControlState(new ControlState()); transform.position = _spawnPos; }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = useDebugInput ? Input.GetAxis("Horizontal") : currControls.MOVEMENT.x;
        x *= controlSpeed * Time.deltaTime;
        selfRB.AddForce(new Vector2(x, 0));
        aimingDirection = (useDebugInput ? GetMouseAim() : currControls.AIM).normalized;

        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }

    }
    void Fire()
    {
        DeleteAttacher();
        attacherInstance = (GameObject)Instantiate(attacherPrefab, transform.position, Quaternion.identity);
        attacherScript = attacherInstance.GetComponent<Attacher>();
        attacherScript.parentScript = this;
        attacherScript.direction = aimingDirection;
    }
    public void DeleteAttacher()
    {
        if (attacherInstance != null)
            Destroy(attacherInstance);
        attacherInstance = null;
        attacherScript = null;
    }

    Vector2 GetMouseAim()
    {
        Vector2 temp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2(temp.x - transform.position.x, temp.y - transform.position.y);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, (new Vector3(transform.position.x + aimingDirection.x * 5, transform.position.y + aimingDirection.y * 5, 0)));
    }
}
