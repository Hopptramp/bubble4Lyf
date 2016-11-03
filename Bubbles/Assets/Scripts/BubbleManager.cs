using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BubbleManager : MonoBehaviour
{
    private static BubbleManager _instance;
    public static BubbleManager instance { get { return _instance; } }

    [SerializeField] GameObject bubblePrefab;
    List<Bubble> bubbleList;

    [SerializeField] int massSpawnNo = 10;

    // Use this for initialization
    void Start()
    {
        _instance = this;
        bubbleList = new List<Bubble>();
    }

    public void InstantiateBubble(GameObject origin, Vector2 pos, Vector2 size, float magnitude, Vector2 direction)
    {
        GameObject bubbleOBJ = Instantiate(bubblePrefab, pos, Quaternion.identity) as GameObject;
        bubbleOBJ.transform.localScale = size;


        Bubble bubble = bubbleOBJ.GetComponent<Bubble>();
        bubble.originObject = origin;

        Rigidbody2D rb = bubbleOBJ.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * magnitude, ForceMode2D.Impulse);

        bubbleList.Add(bubble);

    }

    IEnumerator SpawnMass()
    {
        for(int i = 0; i < massSpawnNo; ++i)
        {
            float size = Random.Range(0.5f, 2);
            InstantiateBubble(gameObject, new Vector2(Random.Range(-10, 10), Random.Range(-5, 5)), new Vector2(size, size), Random.Range(0.5f, 2f), new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)));

            yield return null;
        }
    }

    public void RemoveBubble(Bubble bubble)
    {
        bubbleList.Remove(bubble);
        Destroy(bubble.gameObject);
    }

    public void RemoveAllBubbles()
    {
        for(int i = 0; i < bubbleList.Count; ++i)
        {
            Destroy(bubbleList[i].gameObject);
        }

        bubbleList.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            InstantiateBubble(gameObject, Vector2.zero, Vector2.one, 5, Vector2.up);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SpawnMass());
        }
    }
}
