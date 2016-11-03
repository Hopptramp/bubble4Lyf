using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BubbleManager : MonoBehaviour
{
    private static BubbleManager _instance;
    public static BubbleManager instance { get { return _instance; } }

    [SerializeField] bool isDebug = false;
    [SerializeField] GameObject bubblePrefab;
    private GameObject bubbleParent;
    List<Bubble> bubbleList;

    [SerializeField] int massSpawnNo = 10;
    [SerializeField] float spawnWidth = 10;
    [SerializeField] float spawnSizeMin = 0.5f;
    [SerializeField] float spawnSizeMax = 2f;
    [SerializeField] float spawnForceMagnitude = 2;
    [SerializeField] float spawnResetDelay = 5;
    [SerializeField] Transform spawnPoint;

    // Use this for initialization
    void Start()
    {
        _instance = this;
        bubbleList = new List<Bubble>();
        bubbleParent = new GameObject("Bubble Parent");
    }

    public void InstantiateBubble(GameObject origin, Vector2 pos, Vector2 size, float magnitude, Vector2 direction)
    {
        GameObject bubbleOBJ = Instantiate(bubblePrefab, pos, Quaternion.identity) as GameObject;
        bubbleOBJ.transform.localScale = size;


        Bubble bubble = bubbleOBJ.GetComponent<Bubble>();
        bubble.originObject = origin;

        Rigidbody2D rb = bubbleOBJ.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * magnitude, ForceMode2D.Impulse);
        rb.angularVelocity = Random.Range(-35, 35);

        bubbleOBJ.transform.parent = bubbleParent.transform;
        bubbleList.Add(bubble);
    }

    IEnumerator SpawnMass()
    {
        for(int i = 0; i < massSpawnNo; ++i)
        {
            float size = Random.Range(spawnSizeMin, spawnSizeMax);
            InstantiateBubble(gameObject, spawnPoint.position + new Vector3(Random.Range(-10, 10), Random.Range(-spawnWidth, spawnWidth)), new Vector2(size, size), spawnForceMagnitude, new Vector2(Random.Range(-5, 5), Random.Range(-10, 10)));

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(spawnResetDelay);

        StartCoroutine(SpawnMass());
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
        if (isDebug)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                InstantiateBubble(gameObject, Vector2.zero, Vector2.one, 5, Vector2.up);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(SpawnMass());
            }
        }
    }
}
