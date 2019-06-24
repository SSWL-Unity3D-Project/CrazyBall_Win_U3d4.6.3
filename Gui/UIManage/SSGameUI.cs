using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSGameUI : SSGameMono
{
    [System.Serializable]
    public class GameUIData
    {
        /// <summary>
        /// UI界面中心锚点
        /// </summary>
        public Transform PanelCenterTr;
        /// <summary>
        /// 玩家的曲棍球进入对方后所得分数
        /// </summary>
        public int JiaFenVal = 10;
        /// <summary>
        /// 等待对方开始游戏UI管理
        /// </summary>
        SSDengDaiDuiFang m_SSDengDaiDuiFang;
        SSGameUI m_SSGameUI;
        internal void Init(SSGameUI com)
        {
            m_SSGameUI = com;
            if (PanelCenterTr != null)
            {
                m_SSDengDaiDuiFang = PanelCenterTr.gameObject.AddComponent<SSDengDaiDuiFang>();
            }
        }

        bool[] IsClickStartBtArray = new bool[SSGlobalData.MAX_PLAYER];
        internal void SetIsClickStartBt(SSGlobalData.PlayerEnum indexPlayer, bool isClick)
        {
            if (_IsGameStart == true)
            {
                return;
            }

            int index = (int)indexPlayer;
            if (index < 0 || index >= IsClickStartBtArray.Length)
            {
                return;
            }
            IsClickStartBtArray[index] = isClick;

            bool isAllClickStart = true;
            for (int i = 0; i < IsClickStartBtArray.Length; i++)
            {
                if (IsClickStartBtArray[i] == false)
                {
                    isAllClickStart = false;
                    break;
                }
            }

            if (isAllClickStart == true)
            {
                _IsGameStart = true;
                if (m_SSDengDaiDuiFang != null)
                {
                    //删除等待对方UI
                    m_SSDengDaiDuiFang.RemoveDengDaiDuiFang();
                }

                if (m_SSGameUI != null)
                {
                    //创建玩家发球倒计时
                    m_SSGameUI.CreateStartGameGo();
                }

                if (SSGameMange.GetInstance() != null && SSGameMange.GetInstance().m_SSGameScene != null)
                {
                    //开始创建道具
                    SSGameMange.GetInstance().m_SSGameScene.StartCreateDaoJu();
                }
            }
            else
            {
                if (m_SSDengDaiDuiFang != null)
                {
                    m_SSDengDaiDuiFang.CreateDengDaiDuiFangUI(indexPlayer, PanelCenterTr);
                }
            }
        }

        internal void Reset()
        {
            _IsGameStart = false;
            for (int i = 0; i < IsClickStartBtArray.Length; i++)
            {
                IsClickStartBtArray[i] = false;
            }
        }

        /// <summary>
        /// 是否开始游戏.
        /// 当游戏结束之后需要重置该属性.
        /// </summary>
        bool _IsGameStart = false;
        internal bool IsGameStart
        {
            get { return _IsGameStart; }
        }
    }
    public GameUIData m_GameUIData;

    /// <summary>
    /// 初始化
    /// </summary>
    internal void Init()
    {
        if (m_GameUIData == null)
        {
            m_GameUIData = new GameUIData();
        }

        if (m_GameUIData != null)
        {
            m_GameUIData.Init(this);
        }
        InputEventCtrl.GetInstance().ClickStartBtOneEvent += ClickStartBtOneEvent;
        InputEventCtrl.GetInstance().ClickStartBtTwoEvent += ClickStartBtTwoEvent;
    }

    internal void ResetInfo()
    {
        if (m_GameUIData != null)
        {
            m_GameUIData.Reset();
        }
    }

    private void ClickStartBtOneEvent(InputEventCtrl.ButtonState val)
    {
        OnClickStartBt(SSGlobalData.PlayerEnum.PlayerOne, val);
    }

    private void ClickStartBtTwoEvent(InputEventCtrl.ButtonState val)
    {
        OnClickStartBt(SSGlobalData.PlayerEnum.PlayerTwo, val);
    }

    void OnClickStartBt(SSGlobalData.PlayerEnum indexPlayer, InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.UP)
        {
            return;
        }
        m_GameUIData.SetIsClickStartBt(indexPlayer, true);
    }

    bool IsCreateStartGameGo = false;
    /// <summary>
    /// 创建游戏倒计时
    /// </summary>
    void CreateStartGameGo()
    {
        if (IsCreateStartGameGo == true)
        {
            return;
        }

        string prefabPath = "GUI/DaoJiShi/StartGame/StartGameGo";
        GameObject gmDataPrefab = (GameObject)Resources.Load(prefabPath);
        if (gmDataPrefab != null)
        {
            SSDebug.Log("CreateStartGameGo......................................................");
            GameObject obj = (GameObject)Instantiate(gmDataPrefab, m_GameUIData.PanelCenterTr);
            SSStartGameGo com = obj.GetComponent<SSStartGameGo>();
            if (com != null)
            {
                IsCreateStartGameGo = true;
                com.Init();
            }
        }
        else
        {
            SSDebug.LogWarning("CreateStartGameGo -> gmDataPrefab was null! prefabPath == " + prefabPath);
        }
    }

    internal void RemoveStartGameGo()
    {
        IsCreateStartGameGo = false;
        CreatePlayerFenShuUI();
    }

    /// <summary>
    /// 是否创建开始发球界面
    /// </summary>
    internal bool IsCreateStartFireBall = false;
    /// <summary>
    /// 创建开始发球界面
    /// </summary>
    internal void CreateStartFireBall(SSGlobalData.PlayerEnum indexPlayer)
    {
        int index = (int)indexPlayer + 1;
        if (index < 1)
        {
            return;
        }

        string prefabPath = "GUI/DaoJiShi/StartGame/StartFireBallP" + index;
        GameObject gmDataPrefab = (GameObject)Resources.Load(prefabPath);
        if (gmDataPrefab != null)
        {
            SSDebug.Log("CreateStartFireBall......................................................");
            GameObject obj = (GameObject)Instantiate(gmDataPrefab, m_GameUIData.PanelCenterTr);
            SSStartGameGo com = obj.GetComponent<SSStartGameGo>();
            if (com != null)
            {
                IsCreateStartFireBall = true;
                com.Init();
            }
        }
        else
        {
            SSDebug.LogWarning("CreateStartFireBall -> gmDataPrefab was null! prefabPath == " + prefabPath);
        }
    }

    internal void RemoveStartFireBall()
    {
        IsCreateStartFireBall = false;
    }

    /// <summary>
    /// 是否创建游戏倒计时
    /// </summary>
    bool IsCreateDaoJiShi = false;
    /// <summary>
    /// 创建游戏倒计时
    /// </summary>
    internal void CreateGameDaoJiShi()
    {
        if (IsCreateDaoJiShi == true)
        {
            return;
        }

        string prefabPath = "GUI/DaoJiShi/DaoJiShi";
        GameObject gmDataPrefab = (GameObject)Resources.Load(prefabPath);
        if (gmDataPrefab != null)
        {
            SSDebug.Log("CreateGameDaoJiShi......................................................");
            GameObject obj = (GameObject)Instantiate(gmDataPrefab, m_GameUIData.PanelCenterTr);
            SSGameDaoJiShi com = obj.GetComponent<SSGameDaoJiShi>();
            if (com != null)
            {
                IsCreateDaoJiShi = true;
                com.Init();
            }
        }
        else
        {
            SSDebug.LogWarning("CreateGameDaoJiShi -> gmDataPrefab was null! prefabPath == " + prefabPath);
        }
    }

    /// <summary>
    /// 删除游戏倒计时
    /// </summary>
    internal void RemoveGameDaoJiShi()
    {
        IsCreateDaoJiShi = false;
    }
    
    /// <summary>
    /// 当玩家没有接住曲棍球时,给赢得玩家创建加分UI界面
    /// </summary>
    internal void CreateJiaFenUI(SSGlobalData.PlayerEnum indexPlayer)
    {
        int index = (int)indexPlayer + 1;
        if (index < 1)
        {
            return;
        }

        string prefabPath = "GUI/JiaFen/JiaFenP" + index;
        GameObject gmDataPrefab = (GameObject)Resources.Load(prefabPath);
        if (gmDataPrefab != null)
        {
            SSDebug.Log("CreateJiaFenUI......................................................");
            Instantiate(gmDataPrefab, m_GameUIData.PanelCenterTr);
            SSGlobalData.GetInstance().AddPlayerFenShu(indexPlayer, m_GameUIData.JiaFenVal);
        }
        else
        {
            SSDebug.LogWarning("CreateJiaFenUI -> gmDataPrefab was null! prefabPath == " + prefabPath);
        }
    }
    
    /// <summary>
    /// 当玩家击出的住曲棍球碰上道具时,给该玩家创建道具加分UI界面
    /// </summary>
    internal void CreateDaoJuJiaFenUI(SSGlobalData.PlayerEnum indexPlayer, int fenShu)
    {
        int index = (int)indexPlayer + 1;
        if (index < 1)
        {
            return;
        }

        string prefabPath = "GUI/JiaFen/DaoJuJiaFenP" + index;
        GameObject gmDataPrefab = (GameObject)Resources.Load(prefabPath);
        if (gmDataPrefab != null)
        {
            SSDebug.Log("CreateDaoJuJiaFenUI......................................................");
            GameObject obj = (GameObject)Instantiate(gmDataPrefab, m_GameUIData.PanelCenterTr);
            SSDaoJuJiaFen com = obj.GetComponent<SSDaoJuJiaFen>();
            if (com != null)
            {
                com.ShowNumUI(fenShu);
            }
        }
        else
        {
            SSDebug.LogWarning("CreateDaoJuJiaFenUI -> gmDataPrefab was null! prefabPath == " + prefabPath);
        }
    }

    SSPlayerFenShu[] m_SSPlayerFenShu = new SSPlayerFenShu[SSGlobalData.MAX_PLAYER];
    /// <summary>
    /// 创建玩家分数UI界面
    /// </summary>
    void CreatePlayerFenShuUI()
    {
        for (int i = 0; i < SSGlobalData.MAX_PLAYER; i++)
        {
            CreatePlayerFenShuUI((SSGlobalData.PlayerEnum)i);
        }
    }

    /// <summary>
    /// 创建玩家分数UI界面
    /// </summary>
    void CreatePlayerFenShuUI(SSGlobalData.PlayerEnum indexPlayer)
    {
        int index = (int)indexPlayer + 1;
        if (index < 1 || index > SSGlobalData.MAX_PLAYER)
        {
            return;
        }

        if (m_SSPlayerFenShu[index - 1] != null)
        {
            return;
        }

        string prefabPath = "GUI/FenShu/FenShuP" + index;
        GameObject gmDataPrefab = (GameObject)Resources.Load(prefabPath);
        if (gmDataPrefab != null)
        {
            SSDebug.Log("CreatePlayerFenShuUI......................................................");
            GameObject obj = (GameObject)Instantiate(gmDataPrefab, m_GameUIData.PanelCenterTr);
            SSPlayerFenShu com = obj.GetComponent<SSPlayerFenShu>();
            if (com != null)
            {
                m_SSPlayerFenShu[index - 1] = com;
                com.Init(indexPlayer);
            }
        }
        else
        {
            SSDebug.LogWarning("CreatePlayerFenShuUI -> gmDataPrefab was null! prefabPath == " + prefabPath);
        }
    }

    /// <summary>
    /// 删除玩家分数界面
    /// </summary>
    internal void RemovePlayerFenShu()
    {
        for (int i = 0; i < SSGlobalData.MAX_PLAYER; i++)
        {
            RemovePlayerFenShu((SSGlobalData.PlayerEnum)i);
        }
    }

    /// <summary>
    /// 删除玩家分数界面
    /// </summary>
    void RemovePlayerFenShu(SSGlobalData.PlayerEnum indexPlayer)
    {
        int index = (int)indexPlayer;
        if (index < 0 || index >= SSGlobalData.MAX_PLAYER)
        {
            return;
        }

        if (m_SSPlayerFenShu[index] != null)
        {
            m_SSPlayerFenShu[index].RemoveSelf();
            m_SSPlayerFenShu[index] = null;
        }
    }

    /// <summary>
    /// 展示玩家分数
    /// </summary>
    internal void ShowPlayerFenShu(SSGlobalData.PlayerEnum indexPlayer)
    {
        int index = (int)indexPlayer;
        if (index < 0 || index >= SSGlobalData.MAX_PLAYER)
        {
            return;
        }

        if (m_SSPlayerFenShu[index] != null)
        {
            m_SSPlayerFenShu[index].ShowPlayerFenShu();
        }
    }

    /// <summary>
    /// 游戏比赛结果
    /// </summary>
    List<GameObject> m_GameResult = new List<GameObject>();
    /// <summary>
    /// 添加游戏比赛结果
    /// </summary>
    void AddGameResult(GameObject obj)
    {
        if (obj != null && m_GameResult.Contains(obj) == false)
        {
            m_GameResult.Add(obj);
        }
    }

    /// <summary>
    /// 清楚游戏比赛结果
    /// </summary>
    internal void ClearGameResult()
    {
        if (m_GameResult != null && m_GameResult.Count > 0)
        {
            GameObject[] objArray = m_GameResult.ToArray();
            m_GameResult.Clear();
            for (int i = 0; i < objArray.Length; i++)
            {
                if (objArray[i] != null)
                {
                    Destroy(objArray[i]);
                }
            }
        }
    }

    /// <summary>
    /// 展示玩家游戏比赛结果
    /// </summary>
    internal void ShowPlayerGameRaceResult()
    {
        int fenShuP1 = SSGlobalData.GetInstance().GetPlayerFenShu(SSGlobalData.PlayerEnum.PlayerOne);
        int fenShuP2 = SSGlobalData.GetInstance().GetPlayerFenShu(SSGlobalData.PlayerEnum.PlayerTwo);
        SSGlobalData.GameRaceResult resultP1 = SSGlobalData.GameRaceResult.PingJu;
        SSGlobalData.GameRaceResult resultP2 = SSGlobalData.GameRaceResult.PingJu;
        SSDebug.Log("ShowPlayerGameRaceResult -> fenShuP1 == " + fenShuP1);
        SSDebug.Log("ShowPlayerGameRaceResult -> fenShuP2 == " + fenShuP2);

        if (fenShuP1 == fenShuP2)
        {
            //平局
            resultP1 = resultP2 = SSGlobalData.GameRaceResult.PingJu;
        }
        else if (fenShuP1 > fenShuP2)
        {
            resultP1 = SSGlobalData.GameRaceResult.Victory;
            resultP2 = SSGlobalData.GameRaceResult.Failure;
        }
        else if (fenShuP1 < fenShuP2)
        {
            resultP1 = SSGlobalData.GameRaceResult.Failure;
            resultP2 = SSGlobalData.GameRaceResult.Victory;
        }

        CreateGameResult(SSGlobalData.PlayerEnum.PlayerOne, resultP1);
        CreateGameResult(SSGlobalData.PlayerEnum.PlayerTwo, resultP2);
        StartCoroutine(DelayRemoveGameResult());
    }

    IEnumerator DelayRemoveGameResult()
    {
        yield return new WaitForSeconds(4f);
        //删除游戏比赛结果界面
        ClearGameResult();

        //删除玩家分数界面
        SSGameMange.GetInstance().m_SSGameUI.RemovePlayerFenShu();
        //重置数据信息
        SSGlobalData.GetInstance().ResetInfo();
    }

    /// <summary>
    /// 创建游戏比赛结果
    /// </summary>
    void CreateGameResult(SSGlobalData.PlayerEnum indexPlayer, SSGlobalData.GameRaceResult result)
    {
        if (m_GameResult == null)
        {
            return;
        }

        GameObject obj = null;
        switch (result)
        {
            case SSGlobalData.GameRaceResult.Victory:
                {
                    obj = CreateGameVictoryResult(indexPlayer);
                    break;
                }
            case SSGlobalData.GameRaceResult.Failure:
                {
                    obj = CreateGameFailureResult(indexPlayer);
                    break;
                }
            case SSGlobalData.GameRaceResult.PingJu:
                {
                    obj = CreateGamePingJuResult(indexPlayer);
                    break;
                }
        }

        if (obj != null)
        {
            AddGameResult(obj);
        }
    }

    /// <summary>
    /// 创建胜利界面
    /// </summary>
    GameObject CreateGameVictoryResult(SSGlobalData.PlayerEnum indexPlayer)
    {
        GameObject obj = null;
        int index = (int)indexPlayer + 1;
        if (index < 1 || index > SSGlobalData.MAX_PLAYER)
        {
            return obj;
        }

        string prefabPath = "GUI/FenShu/HuoShengP" + index;
        GameObject gmDataPrefab = (GameObject)Resources.Load(prefabPath);
        if (gmDataPrefab != null)
        {
            SSDebug.Log("CreateGameVictoryResult......................................................");
            obj = (GameObject)Instantiate(gmDataPrefab, m_GameUIData.PanelCenterTr);
        }
        else
        {
            SSDebug.LogWarning("CreateGameVictoryResult -> gmDataPrefab was null! prefabPath == " + prefabPath);
        }
        return obj;
    }

    /// <summary>
    /// 创建失败界面
    /// </summary>
    GameObject CreateGameFailureResult(SSGlobalData.PlayerEnum indexPlayer)
    {
        GameObject obj = null;
        int index = (int)indexPlayer + 1;
        if (index < 1 || index > SSGlobalData.MAX_PLAYER)
        {
            return obj;
        }

        string prefabPath = "GUI/FenShu/ShiBaiP" + index;
        GameObject gmDataPrefab = (GameObject)Resources.Load(prefabPath);
        if (gmDataPrefab != null)
        {
            SSDebug.Log("CreateGameFailureResult......................................................");
            obj = (GameObject)Instantiate(gmDataPrefab, m_GameUIData.PanelCenterTr);
        }
        else
        {
            SSDebug.LogWarning("CreateGameFailureResult -> gmDataPrefab was null! prefabPath == " + prefabPath);
        }
        return obj;
    }

    /// <summary>
    /// 创建平局界面
    /// </summary>
    GameObject CreateGamePingJuResult(SSGlobalData.PlayerEnum indexPlayer)
    {
        GameObject obj = null;
        int index = (int)indexPlayer + 1;
        if (index < 1 || index > SSGlobalData.MAX_PLAYER)
        {
            return obj;
        }

        string prefabPath = "GUI/FenShu/PingJuP" + index;
        GameObject gmDataPrefab = (GameObject)Resources.Load(prefabPath);
        if (gmDataPrefab != null)
        {
            SSDebug.Log("CreateGamePingJuResult......................................................");
            obj = (GameObject)Instantiate(gmDataPrefab, m_GameUIData.PanelCenterTr);
        }
        else
        {
            SSDebug.LogWarning("CreateGamePingJuResult -> gmDataPrefab was null! prefabPath == " + prefabPath);
        }
        return obj;
    }
}
