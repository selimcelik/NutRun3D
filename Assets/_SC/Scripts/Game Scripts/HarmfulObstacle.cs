using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HarmfulObstacle : MonoBehaviour
{
    public enum HarmfulObstacleType
    {
        Fire,
        Punch,
        Hand
    }
    public HarmfulObstacleType harmfulObstacleType;

    private bool canMove = true;

    public bool leftSide, rightSide = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(harmfulObstacleType == HarmfulObstacleType.Punch && canMove)
        {
            StartCoroutine(punchMovement());
            canMove = false;
        }
        if(harmfulObstacleType == HarmfulObstacleType.Hand && canMove)
        {
            StartCoroutine(handMovement());
            canMove = false;
        }
    }

    IEnumerator handMovement()
    {
        if (rightSide)
        {
            gameObject.transform.DOMoveX(0.8f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            gameObject.transform.DOMoveX(1.4f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(handMovement());
        }
        if (leftSide)
        {
            gameObject.transform.DOMoveX(-0.8f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            gameObject.transform.DOMoveX(-1.4f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(handMovement());
        }
    }

    IEnumerator punchMovement()
    {
        if (rightSide)
        {
            gameObject.transform.DOMoveX(0.85f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            gameObject.transform.DOMoveX(1.4f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(punchMovement());
        }
        if (leftSide)
        {
            gameObject.transform.DOMoveX(-0.95f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            gameObject.transform.DOMoveX(-1.4f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(punchMovement());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Collector")
        {
            Collect.Instance.collectables.Remove(other.gameObject);
            Destroy(other.gameObject);
        }
    }
}
