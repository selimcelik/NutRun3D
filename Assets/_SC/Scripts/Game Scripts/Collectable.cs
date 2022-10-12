using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum CollectableType
    {
        NormalNut,
        SplitedNut,
        ChocolatedNut,
        ChocolatedAndNutCelled,
        PackagedNut,
    }
    public CollectableType collectableType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.tag == "Collector" && other.gameObject.tag == "Nut")
        {
            other.gameObject.GetComponent<SphereCollider>().isTrigger = true;
            Collect.Instance.collectables.Add(other.gameObject);
            other.gameObject.transform.parent = gameObject.transform.parent.transform;
            other.gameObject.transform.localPosition = new Vector3(0, 0, -0.5f + Collect.Instance.zAxisIndex);
            other.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            other.gameObject.GetComponent<Collectable>().enabled = true;
            Collect.Instance.collectScore += 10;
            //gameObject.transform.parent.transform.GetChild(0).gameObject.SetActive(true);
            Collect.Instance.rescaleFromOtherScript();
        }
    }
}
