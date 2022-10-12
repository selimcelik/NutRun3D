using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using ManagerActorFramework;

public class Collect : Actor<LevelManager>
{
    public static Collect Instance;

    public List<GameObject> collectables = new List<GameObject>();

    public int collectScore = 0;

    public float zAxisIndex = 0;

    public int packagedNutCount = 0;
    public int levelEndMoneyMountain = 0;
    public bool canMoneyCreate = false;
    public bool canCameraChange = false;

    protected override void MB_Awake()
    {
        Instance = this;
        packagedNutCount = 0;
        levelEndMoneyMountain = 0;
        collectScore = 0;
    }

    // Update is called once per frame
    protected override void MB_Update()
    {
        if(!Move.Instance.levelFailed && !Move.Instance.levelFinish && Move.Instance.levelStart)
        {
            if (collectables.Count > 0 && collectables.Count < 2)
            {
                collectables[0].transform.DOMoveX(gameObject.transform.position.x, .2f);
                collectables[0].transform.DOMoveZ(gameObject.transform.position.z + .5f, .2f);
            }
            if (collectables.Count > 1)
            {
                collectables[0].transform.DOMoveX(gameObject.transform.position.x, .2f);
                collectables[0].transform.DOMoveZ(gameObject.transform.position.z + .5f, .2f);
                for (int i = 0; i < collectables.Count - 1; i++)
                {
                    //collectables[i].transform.DOMoveY(0.5f, 0.2f);
                    collectables[i + 1].transform.DOMoveX(collectables[i].transform.position.x, .2f);
                    collectables[i + 1].transform.DOMoveZ(collectables[i].transform.position.z + .5f, .2f);
                }

            }

        }

        if (GameManager.Instance.mountainFinish)
        {
            Push(ManagerEvents.FinishLevel, true);
            GameManager.Instance.mountainFinish = false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Nut")
        {
            collectables.Add(hit.gameObject);
            //hit.gameObject.layer = 7;
            hit.gameObject.GetComponent<SphereCollider>().isTrigger = true;
            hit.gameObject.GetComponent<Collectable>().enabled = true;
            hit.gameObject.transform.parent = gameObject.transform;
            hit.gameObject.tag = "Collector";
            hit.gameObject.transform.localPosition = new Vector3(0, 0, -0.5f + zAxisIndex);
            collectScore += 10;
            //gameObject.transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(Rescale());
        }

        if(hit.gameObject.tag == "finishLine")
        {
            canCameraChange = true;
            //gameObject.transform.DOMoveZ(96.16f, 7);
            hit.gameObject.SetActive(false);
        }

        if(hit.gameObject.tag == "Mountain")
        {
            Move.Instance.levelFinish = true;
            levelEndMoneyMountain = packagedNutCount + gameObject.transform.childCount;
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                collectables.Remove(gameObject.transform.GetChild(i).gameObject);
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
            canMoneyCreate = true;
        }
    }
    public void rescaleFromOtherScript()
    {
        for (int i = collectables.Count - 1; i >= 0; i--)
        {
            //collectables[i].transform.DOMoveY(0.5f, 0.2f);
            collectables[i].tag = "Collector";
        }
        StartCoroutine(Rescale());
    }

    IEnumerator Rescale()
    {
        zAxisIndex -= .6f;
        for (int i = collectables.Count - 1; i >= 0; i--)
        {
            collectables[i].transform.DOScale(3f, 0.2f);
            yield return new WaitForSeconds(0.01f);
            collectables[i].transform.DOScale(1f, 0.2f);
        }
        yield return new WaitForSeconds(0.25f);


        //gameObject.transform.GetChild(0).gameObject.SetActive(false);


    }


}
