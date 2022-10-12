/*
* Developed by Gökhan KINAY.
* www.gokhankinay.com.tr
*
* Contact,
* info@gokhankinay.com.tr
*/

using UnityEngine;
using ManagerActorFramework;
using System.Collections;
using DG.Tweening;
using Cinemachine;

public class GameManager : Manager<GameManager>
{
    // Info
    public bool IsGameStarted { get; private set; }
    public bool IsGameOver { get; private set; }

    // GameData : Entity
    public GameData gameData { get; private set; }

    public GameObject cam1;
    public GameObject cam2;
    public GameObject player;
    public bool levelEnd = true;

    public bool mountainFinish = false;

    protected override void MB_Start()
    {
        // FPS
        Application.targetFrameRate = 60;

        // Init Game Data
        gameData = GameData.Get();
        if (gameData == null)
        {
            gameData = new GameData();
            bool isSuccess = gameData.Register();
            if (!isSuccess)
            {
                Debug.LogError("GameData Entity register error!");
            }
        }

        // Load game data
        gameData.Load();

        // Init game
        GameStatus_Init();
    }

    protected override void MB_Listen(bool status)
    {
        if (status)
        {
            // Button Clicks
            Instance.Subscribe(ManagerEvents.BtnClick_Play, BtnClick_Play);
            Instance.Subscribe(ManagerEvents.BtnClick_Continue, BtnClick_Continue);
            Instance.Subscribe(ManagerEvents.BtnClick_SuccessBackToMenu, BtnClick_SuccessBackToMenu);
            Instance.Subscribe(ManagerEvents.BtnClick_Retry, BtnClick_Retry);
            Instance.Subscribe(ManagerEvents.BtnClick_FailBackToMenu, BtnClick_FailBackToMenu);

            // Others
            LevelManager.Instance.Subscribe(ManagerEvents.FinishLevel, FinishLevel);
        }
        else
        {
            // Button Clicks
            Instance.Unsubscribe(ManagerEvents.BtnClick_Play, BtnClick_Play);
            Instance.Unsubscribe(ManagerEvents.BtnClick_Continue, BtnClick_Continue);
            Instance.Unsubscribe(ManagerEvents.BtnClick_SuccessBackToMenu, BtnClick_SuccessBackToMenu);
            Instance.Unsubscribe(ManagerEvents.BtnClick_Retry, BtnClick_Retry);
            Instance.Unsubscribe(ManagerEvents.BtnClick_FailBackToMenu, BtnClick_FailBackToMenu);

            // Others
            LevelManager.Instance.Unsubscribe(ManagerEvents.FinishLevel, FinishLevel);
        }
    }

    private void Update()
    {
        if (Collect.Instance.canCameraChange)
        {
            cam1.SetActive(false);
            cam2.SetActive(true);
            if (levelEnd)
            {
                StartCoroutine(movePlayerAndMoney());
                levelEnd = false;
            }
        }
        else
        {
            cam2.SetActive(false);
            cam1.SetActive(true);
        }
    }

    IEnumerator movePlayerAndMoney()
    {
        yield return new WaitUntil(() => Collect.Instance.canMoneyCreate);
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.DORotate(new Vector3(0, 180, 180), 1);
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < Collect.Instance.levelEndMoneyMountain; i++)
        {
            GameObject go = ObjectPooler.Instance.SpawnForGameObject("Money", player.transform.position, new Quaternion(0, 0, 0, 0), LevelManager.Instance.levelActor.gameObject.transform);
            yield return new WaitForSeconds(0.05f);
            player.transform.DOMoveY(player.transform.position.y + 0.25f, 0.1f);
            yield return new WaitForSeconds(0.05f);

            if(i == Collect.Instance.levelEndMoneyMountain - 1)
            {
                mountainFinish = true;
                cam2.GetComponent<CinemachineVirtualCamera>().enabled = false;
            }
        }

    }

    #region Button Clicks

    private void BtnClick_Play(object[] args)
    {
        GameStatus_Start();
    }

    private void BtnClick_Continue(object[] args)
    {
        GameStatus_Restart(false);
    }

    private void BtnClick_SuccessBackToMenu(object[] args)
    {
        GameStatus_Restart(true);
    }

    private void BtnClick_Retry(object[] args)
    {
        GameStatus_Restart(false);
    }

    private void BtnClick_FailBackToMenu(object[] args)
    {
        GameStatus_Restart(true);
    }

    #endregion


    #region Game Status

    private void GameStatus_Init()
    {
        Publish(ManagerEvents.GameStatus_Init, gameData);
    }

    private void GameStatus_Start()
    {
        if (IsGameStarted)
        {
            return;
        }

        IsGameStarted = true;
        IsGameOver = false;

        Publish(ManagerEvents.GameStatus_Start);

        // Analytics
        //AnalyticsManager.Instance.StartLevel(gameData.Level + 1);
    }

    private void GameStatus_GameOver(bool isSuccess)
    {
        if (IsGameOver)
        {
            return;
        }

        IsGameStarted = true;
        IsGameOver = true;

        Publish(ManagerEvents.GameStatus_GameOver, isSuccess);

        if (isSuccess)
        {
            // Analytics
            //AnalyticsManager.Instance.CompletedLevel(gameData.Level + 1);

            // New level
            gameData.UpdateLevel(gameData.Level + 1);
        }
        else
        {
            // Analytics
            //AnalyticsManager.Instance.FailLevel(gameData.Level + 1);
        }
    }

    private void GameStatus_Restart(bool backToMenu)
    {
        if (!IsGameOver)
        {
            return;
        }

        IsGameStarted = false;
        IsGameOver = false;

        Publish(ManagerEvents.GameStatus_Restart);

        // Init game
        GameStatus_Init();

        // Continue
        if (!backToMenu)
        {
            GameStatus_Start();
        }
    }

    #endregion

    #region Other Events

    private void FinishLevel(object[] args)
    {
        bool isSuccess = (bool)args[0];

        GameStatus_GameOver(isSuccess);
    }

    #endregion

}
