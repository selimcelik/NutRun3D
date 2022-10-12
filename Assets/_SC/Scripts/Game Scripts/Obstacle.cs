using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType
    {
        Breaker,
        Chocolate,
        CellMachine,
        Packer,
    }

    public ObstacleType obstacleType;

    private bool canMove = true;

    public GameObject plus;
    private int nutCounter = 0;

    private bool firstTouch = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(obstacleType == ObstacleType.Breaker && canMove)
        {
            StartCoroutine(breakerMove());
            canMove = false;
        }

        if(obstacleType == ObstacleType.Packer && canMove)
        {
            StartCoroutine(packerMove());
            canMove = false;
        }
    }

    IEnumerator packerMove()
    {
        gameObject.transform.DOMoveY(.5f, 0.15f);
        yield return new WaitForSeconds(0.15f);
        gameObject.transform.DOMoveY(1f, 0.15f);
        yield return new WaitForSeconds(0.15f);
        StartCoroutine(packerMove());
    }

    IEnumerator breakerMove()
    {
        gameObject.transform.DOMoveY(0.325f, 0.15f);
        yield return new WaitForSeconds(0.15f);
        gameObject.transform.DOMoveY(0.725f, 0.15f);
        yield return new WaitForSeconds(0.15f);
        StartCoroutine(breakerMove());

    }

    private void OnTriggerEnter(Collider other)
    {
        if (obstacleType == ObstacleType.Breaker)
        {
            if (other.gameObject.tag == "Collector" || other.gameObject.tag == "Nut")
            {
                if (other.gameObject.GetComponent<Collectable>().collectableType == Collectable.CollectableType.NormalNut)
                {
                    GameObject go = ObjectPooler.Instance.SpawnForGameObject("Hazelnut", other.gameObject.transform.position, other.gameObject.transform.rotation, other.gameObject.transform.parent);
                    Collect.Instance.collectables[Collect.Instance.collectables.IndexOf(other.gameObject)] = go;
                    other.gameObject.GetComponent<SphereCollider>().enabled = false;
                    other.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    Destroy(other.gameObject, 1);

                    Rigidbody[] rb = other.gameObject.GetComponentsInChildren<Rigidbody>();
                    for (int i = 0; i < rb.Length; i++)
                    {
                        rb[i].gameObject.transform.parent = null;
                        rb[i].gameObject.transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y * 2, transform.localScale.z * 2);
                        rb[i].isKinematic = false;
                        Destroy(rb[i].gameObject, 5);
                    }

                    nutCounter++;
                    plus.SetActive(true);
                    plus.transform.GetChild(0).GetComponent<TextMesh>().text = nutCounter.ToString() + "$";
                    StartCoroutine(plusOrMinusFalser());
                    go.gameObject.tag = "Collector";
                }

            }
        }
            if (obstacleType == ObstacleType.Chocolate)
            {
                if(other.gameObject.tag == "Collector" || other.gameObject.tag == "Nut")
                {
                    if(other.gameObject.GetComponent<Collectable>().collectableType == Collectable.CollectableType.SplitedNut)
                    {
                        GameObject go = ObjectPooler.Instance.SpawnForGameObject("Chocolate_Nut", other.gameObject.transform.position, other.gameObject.transform.rotation, other.gameObject.transform.parent);
                        Collect.Instance.collectables[Collect.Instance.collectables.IndexOf(other.gameObject)] = go;
                        other.gameObject.GetComponent<SphereCollider>().enabled = false;
                        other.gameObject.GetComponent<MeshRenderer>().enabled = false;
                        Destroy(other.gameObject, 1);
                        nutCounter++;
                        plus.SetActive(true);
                        plus.transform.GetChild(0).GetComponent<TextMesh>().text = nutCounter.ToString() + "$";
                        StartCoroutine(plusOrMinusFalser());
                        go.gameObject.tag = "Collector";

                    }

                }
                

            }

            if(obstacleType == ObstacleType.CellMachine)
            {
                if(other.gameObject.tag == "Collector" || other.gameObject.tag == "Nut")
                {
                    if(other.gameObject.GetComponent<Collectable>().collectableType == Collectable.CollectableType.ChocolatedNut)
                    {
                        GameObject go = ObjectPooler.Instance.SpawnForGameObject("Hazelnut_Chocolate", other.gameObject.transform.position, other.gameObject.transform.rotation, other.gameObject.transform.parent);
                        Collect.Instance.collectables[Collect.Instance.collectables.IndexOf(other.gameObject)] = go;
                        other.gameObject.GetComponent<SphereCollider>().enabled = false;
                        other.gameObject.GetComponent<MeshRenderer>().enabled = false;
                        Destroy(other.gameObject, 1);
                        nutCounter++;
                        plus.SetActive(true);
                        plus.transform.GetChild(0).GetComponent<TextMesh>().text = nutCounter.ToString() + "$";
                        StartCoroutine(plusOrMinusFalser());
                        go.gameObject.tag = "Collector";

                    }

                }
            }

            if(obstacleType == ObstacleType.Packer)
            {
                if(other.gameObject.tag == "Collector" || other.gameObject.tag =="Nut")
                {
                    if(other.gameObject.GetComponent<Collectable>().collectableType == Collectable.CollectableType.ChocolatedAndNutCelled)
                    {
                        GameObject go = ObjectPooler.Instance.SpawnForGameObject("Packaget_Nut", other.gameObject.transform.position, other.gameObject.transform.rotation, other.gameObject.transform.parent);
                        Collect.Instance.collectables[Collect.Instance.collectables.IndexOf(other.gameObject)] = go;
                        other.gameObject.GetComponent<SphereCollider>().enabled = false;
                        other.gameObject.GetComponent<MeshRenderer>().enabled = false;
                        Destroy(other.gameObject, 1);
                        nutCounter++;
                        plus.SetActive(true);
                        plus.transform.GetChild(0).GetComponent<TextMesh>().text = nutCounter.ToString() + "$";
                        StartCoroutine(plusOrMinusFalser());
                        go.gameObject.tag = "Collector";

                    }

                }
            }

    }

    IEnumerator plusOrMinusFalser()
    {

        yield return new WaitForSeconds(5f);
        if (plus.activeSelf)
        {
            plus.SetActive(false);
        }

        //Collect.Instance.rescaleFromOtherScript();
    }
}
