using UnityEngine;
using System.Collections;

[System.Serializable]
public class Tether {

    public bool tetherActive = false;
    public TetheredObject tO = new TetheredObject();
    public float tetherLength;
    private Rigidbody2D playerRB;    
        

    public void Disconnect()
    {
        tetherActive = false;
        ClearTO();
    }

    public void SetAttachment(RaycastHit2D _hit, Rigidbody2D _rb, float _length)
    {
        Debug.Log("Tether made");
        playerRB = _rb;
        tetherActive = true;
        tetherLength = _length;
        CreateNewTO(_hit);
    }

    void ClearTO()
    {
        tO.attachedTo = null;
        if (tO.dummyObject != null)
        {
            GameObject.Destroy(tO.dummyObject);
            tO.dummyObject = null;
        }
    }
    void CreateNewTO(RaycastHit2D _hit)
    {
        ClearTO();
        tO.attachedTo = _hit.collider.gameObject;
        tO.dummyObject = new GameObject("DUMMY");
        tO.dummyObject.transform.position = _hit.point;
        tO.dummyObject.transform.SetParent(tO.attachedTo.transform);
    }

    // Call from parent update
    public void RecalcForce (float _dt)
    {
	    if (tetherActive)
        {
            Vector3 tetherpoint = tO.dummyObject.transform.position;
            Vector3 vel = playerRB.velocity;
            Vector3 testPos = playerRB.transform.position;
            vel += Physics.gravity * _dt;
            vel -= (playerRB.drag * vel) * _dt;
            testPos += vel * _dt;
            if ((testPos - tetherpoint).magnitude >tetherLength)
            {
                testPos = tetherpoint + (testPos - tetherpoint).normalized * tetherLength;
                vel = (testPos - playerRB.transform.position) / _dt;
                playerRB.velocity = vel;
            }
        }
	}
}

public struct TetheredObject
{
    public GameObject attachedTo;
    public GameObject dummyObject;
}
