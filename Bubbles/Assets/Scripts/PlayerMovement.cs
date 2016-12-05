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
    private Tether m_tether;

    [SerializeField]
    private Rigidbody2D selfRB;
    [SerializeField]
    private bool useDebugInput;
    public float controlSpeed = 5;
    
    private ControlState currControls = new ControlState();
    private ControlState prevControls = new ControlState();
    public Vector2 aimingDirection { get; private set; }

    private LineRenderer lr;
    [SerializeField]
    private AnimationCurve reelCurve;
    public float reelSpeed;
    private float reelCounter = 0;

    public void SetControlState(ControlState _new)
    {
        prevControls = currControls;
        currControls = _new;        
    }

	// Use this for initialization
	void Awake ()
    {
        lr = GetComponent<LineRenderer>();
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
            if (m_tether != null)
            {
                if (!m_tether.tetherActive)
                    Fire();
            }
            else
                Fire();

        }
        if (Input.GetButton("Fire1"))
        {
            m_tether.tetherLength-= reelCurve.Evaluate(reelCounter) * reelSpeed * Time.fixedDeltaTime;
            reelCounter = Time.fixedDeltaTime;
        }
        if (Input.GetButtonUp("Fire1"))
            reelCounter = 0;

        if (Input.GetButtonDown("Fire2"))
        {
            Detach();
        }
        if (m_tether != null)
        {
            m_tether.RecalcForce(Time.fixedDeltaTime);
        }
    }
        
    void LateUpdate()
    {
        if (m_tether != null)
        {
            if (m_tether.tO.attachedTo == null)
                Detach();
        }
        if (m_tether != null && m_tether.tetherActive)
        {
            lr.enabled = true;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, m_tether.tO.dummyObject.transform.position);
        }
        else
            lr.enabled = false;

        
    }

    public void Detach()
    {
        if (m_tether != null)
            m_tether.Disconnect(); 
    }
    public void Attach(RaycastHit2D _hit)
    {
       
        m_tether = new Tether();
        m_tether.SetAttachment(_hit, selfRB, Vector2.Distance(_hit.point, transform.position));        
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
