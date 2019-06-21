using System.Collections.Generic;
using UnityEngine;

public class SSGameScene : MonoBehaviour
{
    /// <summary>
    /// 玩家球拍数据信息
    /// </summary>
    [System.Serializable]
    public class PaddleData
    {
        public float minPos = -10f;
        public float maxPos = 10f;
        public float moveSpeed = 1f;
    }
    public PaddleData m_PaddleData;

    /// <summary>
    /// 游戏道具管理
    /// </summary>
    SSDaoJuManage m_SSDaoJuManage;

    /// <summary>
    /// 初始化
    /// </summary>
    internal void Init()
    {
        InitDaoJuManage();
        CreatePlayerPaddle(SSGlobalData.PlayerEnum.PlayerOne);
        CreatePlayerPaddle(SSGlobalData.PlayerEnum.PlayerTwo);
        CreateGameBall();
    }
    
    /// <summary>
    /// 玩家球拍组件
    /// </summary>
    List<SSPlayerPaddle> m_PlayerPaddleList = new List<SSPlayerPaddle>();
    /// <summary>
    /// 设置玩家球拍组件
    /// </summary>
    void SetPlayerPaddle(SSPlayerPaddle paddle)
    {
        if (m_PlayerPaddleList == null)
        {
            return;
        }

        if (paddle != null && m_PlayerPaddleList.Contains(paddle) == false)
        {
            m_PlayerPaddleList.Add(paddle);
        }
    }

    /// <summary>
    /// 获取玩家球拍组件
    /// </summary>
    internal SSPlayerPaddle GetPlayerPaddle(SSGlobalData.PlayerEnum indexPlayer)
    {
        SSPlayerPaddle result = m_PlayerPaddleList.Find((dt) => {
            return dt.IndexPlayer.Equals(indexPlayer);
        });
        return result;
    }

    /// <summary>
    /// 创建玩家的球拍
    /// </summary>
    void CreatePlayerPaddle(SSGlobalData.PlayerEnum indexPlayer)
    {
        if (indexPlayer == SSGlobalData.PlayerEnum.Null)
        {
            return;
        }

        int index = (int)indexPlayer + 1;
        string prefabPath = "Player/Paddle_" + index;
        GameObject gmDataPrefab = (GameObject)Resources.Load(prefabPath);
        if (gmDataPrefab != null)
        {
            SSDebug.Log("CreatePlayerPaddle................. indexPlayer == " + indexPlayer);
            GameObject obj = (GameObject)Instantiate(gmDataPrefab);
            SSPlayerPaddle com = obj.GetComponent<SSPlayerPaddle>();
            if (com != null)
            {
                com.Init(m_PaddleData, indexPlayer);
                SetPlayerPaddle(com);
            }
        }
        else
        {
            SSDebug.LogWarning("CreatePlayerPaddle -> gmDataPrefab was null! prefabPath == " + prefabPath);
        }
    }

    /// <summary>
    /// 曲棍球组件
    /// </summary>
    internal SSBall m_SSBall;
    /// <summary>
    /// 创建曲棍球
    /// </summary>
    void CreateGameBall()
    {
        string prefabPath = "Ball/Ball";
        GameObject gmDataPrefab = (GameObject)Resources.Load(prefabPath);
        if (gmDataPrefab != null)
        {
            SSDebug.Log("CreateGameBall......................................................");
            GameObject obj = (GameObject)Instantiate(gmDataPrefab);
            SSBall com = obj.GetComponent<SSBall>();
            if (com != null)
            {
                m_SSBall = com;
                com.Init();
            }
        }
        else
        {
            SSDebug.LogWarning("CreateGameBall -> gmDataPrefab was null! prefabPath == " + prefabPath);
        }
    }

    void InitDaoJuManage()
    {
        m_SSDaoJuManage = GetComponent<SSDaoJuManage>();
        if (m_SSDaoJuManage != null)
        {
            m_SSDaoJuManage.Init();
        }
    }

    /// <summary>
    /// 开始创建道具
    /// </summary>
    internal void StartCreateDaoJu()
    {
        if (m_SSDaoJuManage != null)
        {
            m_SSDaoJuManage.StartCreateDaoJu();
        }
    }

    /// <summary>
    /// 停止创建道具
    /// </summary>
    internal void StopCreateDaoJu()
    {
        if (m_SSDaoJuManage != null)
        {
            m_SSDaoJuManage.StopCreateDaoJu();
        }
    }

    /// <summary>
    /// 删除道具
    /// </summary>
    internal void RemoveDaoJu(GameObject obj)
    {
        if (m_SSDaoJuManage != null)
        {
            m_SSDaoJuManage.RemoveDaoJuFromList(obj);
        }
    }

    /// <summary>
    /// 响应事件.
    /// </summary>
    public delegate void EventHandel();
    /// <summary>
    /// 发球事件
    /// </summary>
    public event EventHandel OnFireBallEvent;
    public void PlayerFireBallEvent()
    {
        if (OnFireBallEvent != null)
        {
            OnFireBallEvent();
        }
    }
}
