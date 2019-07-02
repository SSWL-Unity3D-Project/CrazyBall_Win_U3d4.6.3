using System.Collections;
using UnityEngine;

public class SSGameNanDu : MonoBehaviour
{
    public enum NanDuEnum
    {
        /// <summary>
        /// 阶段控制
        /// </summary>
        JieDuan = 0,
        /// <summary>
        /// 球拍控制
        /// </summary>
        Paddle = 1,
    }
    /// <summary>
    /// 游戏难度控制
    /// </summary>
    internal NanDuEnum m_NanDuEnum = NanDuEnum.Paddle;

    /// <summary>
    /// 游戏难度数据
    /// </summary>
    [System.Serializable]
    public class NanDuData
    {
        /// <summary>
        /// 每个阶段的时长(秒)
        /// </summary>
        public float Time = 20f;
        /// <summary>
        /// 曲棍球的运动速度
        /// </summary>
        public float BallSpeed = 40f;
        /// <summary>
        /// 球拍运动速度
        /// </summary>
        public float QiuPaiSpeed = 40f;
        /// <summary>
        /// 设置游戏难度
        /// </summary>
        internal void SetGameNanDu()
        {
            if (SSGameMange.GetInstance() != null
                && SSGameMange.GetInstance().m_SSGameScene != null)
            {
                SSGameMange.GetInstance().m_SSGameScene.SetGameNanDu(BallSpeed, QiuPaiSpeed);
            }
        }
    }
    public NanDuData[] m_NanDuDtArray = new NanDuData[3];

    [System.Serializable]
    public class NanDuPaddleData
    {
        //球速度变化：每次发球有固定初速度（可调），挡板与球接触一次加一次速度（可调），当球速到达一定值时将不再加速（可调）。
        /// <summary>
        /// 球拍速度
        /// </summary>
        public float qiuPaiSpeed = 60f;
        /// <summary>
        /// 挡板与曲棍球接触一次,曲棍球的速度增加一次
        /// </summary>
        public float addQiuPaiSpeed = 30f;
        /// <summary>
        /// 球拍的最大速度
        /// </summary>
        public float maxQiuPaiSpeed = 120f;
        /// <summary>
        /// 球拍的当前速度
        /// </summary>
        float speedQiuPaiCur = 0f;
        /// <summary>
        /// 初始发球速度
        /// </summary>
        public float ballSpeed = 90f;
        /// <summary>
        /// 挡板与曲棍球接触一次,曲棍球的速度增加一次
        /// </summary>
        public float addBallSpeed = 30f;
        /// <summary>
        /// 曲棍球的最大速度
        /// </summary>
        public float maxBallSpeed = 120f;
        /// <summary>
        /// 曲棍球的当前速度
        /// </summary>
        float speedBallCur = 0f;
        internal void UpdateBallSpeed()
        {
            if (speedBallCur == 0f)
            {
                speedBallCur = ballSpeed;
            }
            else
            {
                if (speedBallCur < maxBallSpeed)
                {
                    speedBallCur += addBallSpeed;
                    if (speedBallCur > maxBallSpeed)
                    {
                        speedBallCur = maxBallSpeed;
                    }
                }
            }

            if (speedQiuPaiCur == 0f)
            {
                speedQiuPaiCur = qiuPaiSpeed;
            }
            else
            {
                if (speedQiuPaiCur < maxQiuPaiSpeed)
                {
                    speedQiuPaiCur += addQiuPaiSpeed;
                    if (speedQiuPaiCur > maxQiuPaiSpeed)
                    {
                        speedQiuPaiCur = maxQiuPaiSpeed;
                    }
                }
            }

            if (SSGameMange.GetInstance() != null
                && SSGameMange.GetInstance().m_SSGameScene != null)
            {
                SSGameMange.GetInstance().m_SSGameScene.SetGameNanDu(speedBallCur, speedQiuPaiCur);
            }
        }

        internal void Reset()
        {
            speedBallCur = 0f;
            speedQiuPaiCur = 0f;
        }
    }
    /// <summary>
    /// 通过球拍每接触一次曲棍球时增加球速来控制难度的数据
    /// </summary>
    public NanDuPaddleData m_NanDuPaddleData;
    /// <summary>
    /// 游戏难度阶段索引
    /// </summary>
    int m_IndexNanDu = 0;

    internal void Init()
    {
        m_IndexNanDu = 0;
        IsLoopCheck = false;
    }

    void ResetInfo()
    {
        m_IndexNanDu = 0;
        IsLoopCheck = false;
    }

    /// <summary>
    /// 配置游戏难度
    /// </summary>
    void SetGameNanDu(int index)
    {
        if (index < 0 || index >= m_NanDuDtArray.Length)
        {
            return;
        }

        if (m_NanDuDtArray[index] != null)
        {
            m_NanDuDtArray[index].SetGameNanDu();
        }
    }

    bool IsLoopCheck = false;
    internal void StartLoopCheckNextGameNanDu()
    {
        if (m_NanDuEnum != NanDuEnum.JieDuan)
        {
            return;
        }

        if (IsLoopCheck == true)
        {
            return;
        }
        m_IndexNanDu = 0;
        IsLoopCheck = true;
        StartCoroutine(LoopCheckNextGameNanDu());
    }

    IEnumerator LoopCheckNextGameNanDu()
    {
        int loopCount = 0;
        float time = 1f;
        do
        {
            if (SSGameMange.GetInstance().m_SSGameUI.GetIsCreateStartFireBall() == true)
            {
                //创建了开始发球界面
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            //SSDebug.Log("LoopCheckNextGameNanDu -> loopCount ========== " + loopCount);
            if (loopCount == 0)
            {
                if (SSGameMange.GetInstance() != null
                    && SSGameMange.GetInstance().m_SSGameScene != null)
                {
                    SSGameMange.GetInstance().m_SSGameScene.SetWeiDangMesh(m_IndexNanDu);
                }
                
                SetGameNanDu(m_IndexNanDu);
                if (m_IndexNanDu == m_NanDuDtArray.Length - 1)
                {
                    //游戏已经到达最后一个阶段
                    ResetInfo();
                    yield break;
                }
            }

            yield return new WaitForSeconds(time);
            loopCount++;
            bool isGoToNext = GetIsGoToNextNanDu(m_NanDuDtArray[m_IndexNanDu], loopCount * time);
            if (isGoToNext == true)
            {
                //进入下一阶段
                m_IndexNanDu++;
                loopCount = 0;
            }
        }
        while (IsLoopCheck == true);
    }

    bool GetIsGoToNextNanDu(NanDuData nanDu, float time)
    {
        if (nanDu == null)
        {
            return false;
        }
        return nanDu.Time <= time ? true : false;
    }

    /// <summary>
    /// 当游戏结束时停止游戏难度的控制逻辑
    /// </summary>
    internal void OnGameOver()
    {
        switch(m_NanDuEnum)
        {
            case NanDuEnum.JieDuan:
                {
                    if (IsLoopCheck == true)
                    {
                        ResetInfo();
                    }
                    break;
                }
            case NanDuEnum.Paddle:
                {
                    if (m_NanDuPaddleData != null)
                    {
                        m_NanDuPaddleData.Reset();
                    }
                    break;
                }
        }
    }

    /// <summary>
    /// 获取曲棍球的运动速度
    /// </summary>
    internal void UpdateBallSpeed()
    {
        if (m_NanDuPaddleData != null)
        {
            m_NanDuPaddleData.UpdateBallSpeed();
        }
    }

    /// <summary>
    /// 重置曲棍球的速度
    /// </summary>
    internal void ResetBallSpeed()
    {
        if (m_NanDuPaddleData != null)
        {
            m_NanDuPaddleData.Reset();
        }
    }
}
