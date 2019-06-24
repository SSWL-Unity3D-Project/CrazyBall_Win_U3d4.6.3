using UnityEngine;

public class SSDaoJuJiaFen : MonoBehaviour
{
    /// <summary>
    /// 加分展示
    /// </summary>
    public SSGameNumUI m_SSGameNumUI;
    /// <summary>
    /// 显示数字UI
    /// </summary>
    internal void ShowNumUI(int val)
    {
        if (m_SSGameNumUI != null)
        {
            m_SSGameNumUI.ShowNumUI(val);
        }
    }
}
