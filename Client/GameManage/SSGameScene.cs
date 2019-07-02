using System.Collections.Generic;
using UnityEngine;

public class SSGameScene : MonoBehaviour
{
    [System.Serializable]
    public class SceneData
    {
        /// <summary>
        /// 摄像机运动控制脚本
        /// </summary>
        internal SSCameraMove cameraMove;
        internal void SetAimTarget(Transform aimTarget)
        {
            if (cameraMove != null)
            {
                cameraMove.SetAimTarget(aimTarget);
            }
        }

        internal void SetIsMoveCamera(bool isMove)
        {
            if (cameraMove != null)
            {
                cameraMove.SetIsMoveCamera(isMove);
            }
        }

        /// <summary>
        /// 声音播放器
        /// </summary>
        public SSAudioPlayer audioPlayer;
        internal void PlayAudio()
        {
            if (audioPlayer != null)
            {
                audioPlayer.Play(SSAudioPlayer.Mode.Loop);
            }
        }
    }
    public SceneData m_SceneData;

    /// <summary>
    /// 玩家球拍数据信息
    /// </summary>
    [System.Serializable]
    public class PaddleData
    {
        public float minPos = -10f;
        public float maxPos = 10f;
        public float moveSpeed = 1f;
        internal void SetMoveSpeed(float val)
        {
            moveSpeed = val;
        }
    }
    public PaddleData m_PaddleData;

    /// <summary>
    /// 曲棍球控制数据
    /// </summary>
    public class BallData
    {
        public float moveSpeed = 40f;
        internal void SetMoveSpeed(float val)
        {
            moveSpeed = val;
        }
    }
    /// <summary>
    /// 曲棍球控制数据
    /// </summary>
    public BallData m_BallData = new BallData();
    
    /// <summary>
    /// 游戏道具管理
    /// </summary>
    SSDaoJuManage m_SSDaoJuManage;

    /// <summary>
    /// 初始化
    /// </summary>
    internal void Init()
    {
        InitWeiDangMesh();
        InitSSGameNanDu();
        InitDaoJuManage();
        CreatePlayerPaddle(SSGlobalData.PlayerEnum.PlayerOne);
        CreatePlayerPaddle(SSGlobalData.PlayerEnum.PlayerTwo);
        CreateGameBall();
        PlayAudio();
    }
    
    /// <summary>
    /// 设置镜头跟踪的曲棍球
    /// </summary>
    internal void SetCameraAimTarget(Transform aimTarget)
    {
        if (m_SceneData != null)
        {
            m_SceneData.SetAimTarget(aimTarget);
        }
    }

    /// <summary>
    /// 设置是否移动镜头
    /// </summary>
    internal void SetIsMoveCamera(bool isMove)
    {
        if (m_SceneData != null)
        {
            m_SceneData.SetIsMoveCamera(isMove);
        }
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    void PlayAudio()
    {
        if (m_SceneData != null)
        {
            m_SceneData.PlayAudio();
        }
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
            //SSDebug.Log("CreatePlayerPaddle................. indexPlayer == " + indexPlayer);
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
            //SSDebug.Log("CreateGameBall......................................................");
            GameObject obj = (GameObject)Instantiate(gmDataPrefab);
            SSBall com = obj.GetComponent<SSBall>();
            if (com != null)
            {
                m_SSBall = com;
                com.Init();
            }
            SetCameraAimTarget(obj.transform);
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

    SSGameNanDu m_SSGameNanDu;
    /// <summary>
    /// 初始化游戏难度控制
    /// </summary>
    void InitSSGameNanDu()
    {
        m_SSGameNanDu = GetComponent<SSGameNanDu>();
        if (m_SSGameNanDu != null)
        {
            m_SSGameNanDu.Init();
        }
    }

    /// <summary>
    /// 开始检测游戏难度
    /// </summary>
    internal void StartCheckGameNanDu()
    {
        if (m_SSGameNanDu != null)
        {
            m_SSGameNanDu.StartLoopCheckNextGameNanDu();
        }
    }

    /// <summary>
    /// 停止游戏难度检测
    /// </summary>
    void StopCheckGameNanDu()
    {
        if (m_SSGameNanDu != null)
        {
            m_SSGameNanDu.OnGameOver();
        }
    }
    
    /// <summary>
    /// 获取曲棍球的运动速度
    /// </summary>
    internal void UpdateBallSpeed()
    {
        if (m_SSGameNanDu != null)
        {
            m_SSGameNanDu.UpdateBallSpeed();
        }
    }

    /// <summary>
    /// 重置曲棍球的速度
    /// </summary>
    internal void ResetBallSpeed()
    {
        if (m_SSGameNanDu != null)
        {
            m_SSGameNanDu.ResetBallSpeed();
        }
    }

    /// <summary>
    /// 设置游戏难度控制数据信息
    /// </summary>
    internal void SetGameNanDu(float ballSpeed, float paddleSpeed)
    {
        //SSDebug.Log("SetGameNanDu -> ballSpeed ======= " + ballSpeed + ", paddleSpeed ======= " + paddleSpeed);
        if (m_PaddleData != null)
        {
            //设置球拍的运动速度
            m_PaddleData.SetMoveSpeed(paddleSpeed);
        }

        if (m_BallData != null)
        {
            m_BallData.SetMoveSpeed(ballSpeed);
        }
    }

    /// <summary>
    /// 游戏场地范围材质管理组件
    /// </summary>
    SSWeiDangMesh m_SSWeiDangMesh;
    /// <summary>
    /// 初始化游戏场地范围材质管理组件
    /// </summary>
    void InitWeiDangMesh()
    {
        m_SSWeiDangMesh = GetComponent<SSWeiDangMesh>();
    }

    /// <summary>
    /// 设置游戏场地范围材质
    /// </summary>
    internal void SetWeiDangMesh(int index)
    {
        if (m_SSWeiDangMesh != null)
        {
            m_SSWeiDangMesh.SetMeshMaterial(index);
        }
    }

    /// <summary>
    /// 当游戏开始
    /// </summary>
    internal void OnGameStart()
    {
        //开始创建道具
        //StartCreateDaoJu();
    }

    /// <summary>
    /// 游戏结束时进入该函数
    /// </summary>
    internal void OnGameOver()
    {
        //停止创建道具
        StopCreateDaoJu();
        //停止游戏难度检测
        StopCheckGameNanDu();
        SetWeiDangMesh(0);
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
