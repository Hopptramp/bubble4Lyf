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
	
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        transform.Translate(direction*moveSpeed * Time.fixedDeltaTime);
        distanceTravelled += direction.magnitude * moveSpeed * Time.fixedDeltaTime;
        if (distanceTravelled >= maxDistance)
            parentScript.DeleteAttacher();
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag != "Player")
        {
            attached = true;
        }
    }
}
