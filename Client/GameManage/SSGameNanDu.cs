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
    }
    public NanDuData[] m_NanDuDtArray = new NanDuData[3];

    internal void Init()
    {

    }
}
