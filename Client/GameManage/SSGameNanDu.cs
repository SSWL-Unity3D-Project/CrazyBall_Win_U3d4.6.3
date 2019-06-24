using System.Collections;
using UnityEngine;

public class SSGameNanDu : MonoBehaviour
{
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
            //SSDebug.Log("LoopCheckNextGameNanDu -> loopCount ========== " + loopCount);
            if (loopCount == 0)
            {
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
        if (IsLoopCheck == true)
        {
            ResetInfo();
        }
    }
}
