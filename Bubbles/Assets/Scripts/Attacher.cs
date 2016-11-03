using UnityEngine;
using System.Collections;

public class Attacher : MonoBehaviour {

    [HideInInspector]
    public PlayerMovement parentScript;
    public bool attached { get; private set; }
    public Vector2 direction = Vector3.up;
    public float moveSpeed = 10;
    public float maxDistance = 20;
    private float distanceTravelled = 0;
    public float size = 0.15f;
	
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, size, direction, direction.magnitude * moveSpeed * Time.fixedDeltaTime);
        if (hit)
        {
            if (hit.collider.tag != "Player")
            {
                //Call attacher in parent here;
                parentScript.DeleteAttacher();
            }
        }
        transform.Translate(direction * moveSpeed * Time.fixedDeltaTime);
        distanceTravelled += direction.magnitude * moveSpeed * Time.fixedDeltaTime;
        if (distanceTravelled >= maxDistance)
            parentScript.DeleteAttacher();
    }
        
    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, size);
    }
}
