using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏中UI数字信息控制组件.
/// </summary>
public class SSGameNumUI : SSGameMono
{
    [System.Serializable]
    public class FixedUiPosData
    {
        /// <summary>
        /// 数字UI精灵组件的父级.
        /// </summary>
        public Transform UISpriteParent;
        /// <summary>
        /// 是否修改UI信息的x轴坐标.
        /// </summary>
        public bool IsFixPosX = false;
        /// <summary>
        /// UI坐标x轴偏移量.
        /// m_PosXArray[0]   --- 最小数据的x轴坐标.
        /// m_PosXArray[max] --- 最大数据的x轴坐标.
        /// </summary>
        public int[] m_PosXArray;
    }
    /// <summary>
    /// 修改UI坐标数据信息.
    /// </summary>
    public FixedUiPosData m_FixedUiPosDt;
    [System.Serializable]
    public class NumImageData
    {
        /// <summary>
        /// 数字UI精灵组件.
        /// m_NumUIArray[0]   - 最低位.
        /// m_NumUIArray[max] - 最高位.
        /// </summary>
        public Image[] m_NumUIArray;
        /// <summary>
        /// 数字图集,m_SpritArray[0] 数字0,m_SpritArray[9] 数字9
        /// </summary>
        public Sprite[] m_SpritArray = new Sprite[10];

        public void ShowNum(int indexNum, int num)
        {
            if (indexNum < 0 || indexNum >= m_NumUIArray.Length)
            {
                return;
            }

            if (num < 0 || num >= m_SpritArray.Length)
            {
                return;
            }

            if (m_NumUIArray[indexNum] != null && m_SpritArray[num] != null)
            {
                if (m_NumUIArray[indexNum].gameObject.activeInHierarchy == false)
                {
                    m_NumUIArray[indexNum].gameObject.SetActive(true);
                }
                m_NumUIArray[indexNum].sprite = m_SpritArray[num];
            }
        }

        public void HiddeNum(int indexNum)
        {
            if (indexNum < 0 || indexNum >= m_NumUIArray.Length)
            {
                return;
            }

            if (m_NumUIArray[indexNum] != null)
            {
                m_NumUIArray[indexNum].gameObject.SetActive(false);
            }
        }
    }
    public NumImageData m_NumImageData;
    /// <summary>
    /// 是否隐藏高位数字的0.
    /// </summary>
    public bool IsHiddenGaoWeiZero = true;
    bool IsInit = false;
    /// <summary>
    /// 初始化.
    /// </summary>
    void Init()
    {
        if (IsInit == true)
        {
            return;
        }
        IsInit = true;
    }

    /// <summary>
    /// 显示UI数量信息.
    /// </summary>
    internal void ShowNumUI(int num)
    {
        if (IsInit == false)
        {
            Init();
        }

        if (num < 0)
        {
            return;
        }

        string numStr = num.ToString();
        if (numStr.Length > m_NumImageData.m_NumUIArray.Length)
        {
            num = (int)Mathf.Pow(10, m_NumImageData.m_NumUIArray.Length) - 1;
            numStr = num.ToString();
        }

        if (m_FixedUiPosDt != null && m_FixedUiPosDt.IsFixPosX == true)
        {
            if (m_FixedUiPosDt.UISpriteParent != null)
            {
                int len = numStr.Length;
                if (m_FixedUiPosDt.m_PosXArray.Length >= len)
                {
                    //动态修改UI数据的父级坐标.
                    Vector3 posTmp = m_FixedUiPosDt.UISpriteParent.localPosition;
                    posTmp.x = m_FixedUiPosDt.m_PosXArray[len - 1];
                    m_FixedUiPosDt.UISpriteParent.localPosition = posTmp;
                }
            }
        }
        
        int max = m_NumImageData.m_NumUIArray.Length;
        int numVal = num;
        int valTmp = 0;
        int powVal = 0;
        for (int i = max - 1; i >= 0; i--)
        {
            if (i >= numStr.Length && IsHiddenGaoWeiZero)
            {
                //隐藏数据高位的0.
                m_NumImageData.HiddeNum(i);
            }
            else
            {
                powVal = (int)Mathf.Pow(10, i);
                valTmp = numVal / powVal;
                //SSDebug.Log("ShowNumUI -> valTmp ====== " + valTmp);
                m_NumImageData.ShowNum(i, valTmp);
                numVal -= valTmp * powVal;
            }
        }
    }

    internal void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}