using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour
{
    public GameObject originObject;
    [SerializeField]
    Sprite m_sprite;

    [SerializeField]
    bool delicateBubbles = false;

    [SerializeField]
    int lifeCounter = 3;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // trigger animations of sprite

        if (col.gameObject.tag == "Player" || delicateBubbles)
        {
            if (--lifeCounter <= 0)
            {
                BubbleManager.instance.RemoveBubble(this);
            }
        }
    }



}
