using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBox : MonoBehaviour
{
    public int triggeredCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (triggeredCount == 2)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Collector" || other.gameObject.tag == "Nut")
        {
            if(other.gameObject.GetComponent<Collectable>().collectableType == Collectable.CollectableType.PackagedNut)
            {
                if (!gameObject.transform.parent.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.activeSelf)
                {
                    Collect.Instance.collectables.Remove(other.gameObject);
                    Destroy(other.gameObject);
                    GameObject go = ObjectPooler.Instance.SpawnForGameObject("ConfettiDirectionalRainbow", new Vector3(gameObject.transform.parent.transform.GetChild(1).gameObject.transform.position.x, gameObject.transform.parent.transform.GetChild(1).gameObject.transform.position.y+ 0.25f, gameObject.transform.parent.transform.GetChild(1).gameObject.transform.position.z), gameObject.transform.rotation, gameObject.transform.parent.transform.GetChild(1).gameObject.transform);
                    Destroy(go, 3);
                    gameObject.transform.parent.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    triggeredCount++;
                    Collect.Instance.packagedNutCount++;
                }
                else
                {
                    Collect.Instance.collectables.Remove(other.gameObject);
                    Destroy(other.gameObject);
                    GameObject go = ObjectPooler.Instance.SpawnForGameObject("ConfettiDirectionalRainbow", new Vector3(gameObject.transform.parent.transform.GetChild(2).gameObject.transform.position.x, gameObject.transform.parent.transform.GetChild(2).gameObject.transform.position.y + 0.25f, gameObject.transform.parent.transform.GetChild(2).gameObject.transform.position.z), gameObject.transform.rotation, gameObject.transform.parent.transform.GetChild(2).gameObject.transform);
                    Destroy(go, 3);
                    gameObject.transform.parent.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    triggeredCount++;
                    Collect.Instance.packagedNutCount++;
                }
            }
        }
    }
}
