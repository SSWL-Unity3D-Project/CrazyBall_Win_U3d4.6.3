using UnityEngine;

[RequireComponent(typeof(SSGameNumUI))]
public class SSPlayerFenShu : SSGameMono
{
    SSGlobalData.PlayerEnum m_IndexPlayer;
    public SSGameNumUI m_SSGameNumUI;

    internal void Init(SSGlobalData.PlayerEnum indexPlayer)
    {
        m_IndexPlayer = indexPlayer;
        ShowPlayerFenShu();
    }

    internal void ShowPlayerFenShu()
    {
        int fenShu = SSGlobalData.GetInstance().GetPlayerFenShu(m_IndexPlayer);
        SSDebug.Log("ShowPlayerFenShu -> fenShu ========= " + fenShu + ", indexPlayer == " + m_IndexPlayer);
        if (m_SSGameNumUI != null)
        {
            m_SSGameNumUI.ShowNumUI(fenShu);
        }
    }
}
