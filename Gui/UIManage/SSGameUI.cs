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

        bool[] IsClickStartBtArray = new bool[2];
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
            GameObject obj = (GameObject)Instantiate(gmDataPrefab);
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
}
