//#define USE_PC_INPUT_TEST
using UnityEngine;

/// <summary>
/// 监听pc键盘鼠标按键消息组件.
/// </summary>
public class InputEventCtrl : MonoBehaviour
{
	static private InputEventCtrl _Instance = null;
	static public InputEventCtrl GetInstance()
    {
        if (_Instance == null)
        {
            GameObject obj = new GameObject("_InputEvent");
            _Instance = obj.AddComponent<InputEventCtrl>();
            _Instance.InitPlayerDirData();
        }
		return _Instance;
    }
    
    public enum ButtonState
    {
        DOWN = 0,
        UP = 1,
    }

    /// <summary>
    /// 按键响应事件.
    /// </summary>
    public delegate void EventHandel(ButtonState val);

    #region Click Button Envent
    public void OnClickGameStartBt(SSGlobalData.PlayerEnum indexPlayer, ButtonState val)
    {
        switch (indexPlayer)
        {
            case SSGlobalData.PlayerEnum.PlayerOne:
                {
                    ClickStartBtOne(val);
                    break;
                }
            case SSGlobalData.PlayerEnum.PlayerTwo:
                {
                    ClickStartBtTwo(val);
                    break;
                }
        }
    }

    public event EventHandel ClickStartBtOneEvent;
	public void ClickStartBtOne(ButtonState val)
	{
		if(ClickStartBtOneEvent != null)
		{
			ClickStartBtOneEvent( val );
		}
	}
	
	public event EventHandel ClickStartBtTwoEvent;
	public void ClickStartBtTwo(ButtonState val)
	{
		if(ClickStartBtTwoEvent != null)
		{
			ClickStartBtTwoEvent( val );
		}
	}

	public event EventHandel ClickSetEnterBtEvent;
	public void ClickSetEnterBt(ButtonState val)
	{
		if(ClickSetEnterBtEvent != null)
		{
			ClickSetEnterBtEvent( val );
		}
	}
	
	public event EventHandel ClickSetMoveBtEvent;
	public void ClickSetMoveBt(ButtonState val)
	{
		if(ClickSetMoveBtEvent != null)
		{
			ClickSetMoveBtEvent( val );
		}
	}

    /// <summary>
    /// 清除所有玩家的方向信息.
    /// </summary>
    public void ClearAllPlayerDirBtInfo()
    {
        for (int i = 0; i < 2; i++)
        {
            SSGlobalData.PlayerEnum index = (SSGlobalData.PlayerEnum)i;
            OnClickFangXiangLBt(index, ButtonState.UP);
            OnClickFangXiangRBt(index, ButtonState.UP);
        }
    }
    
    /// <summary>
    /// 向左运动.
    /// </summary>
    public void OnClickFangXiangLBt(SSGlobalData.PlayerEnum index, ButtonState val)
    {
        ClickFangXiangLBt(index, val);
    }

    /// <summary>
    /// 向右运动.
    /// </summary>
    public void OnClickFangXiangRBt(SSGlobalData.PlayerEnum index, ButtonState val)
    {
        ClickFangXiangRBt(index, val);
    }

    enum DirBt
    {
        Left = 0,
        Right = 1,
    }
    /**
	 * 方向左响应.
	 */
    public void ClickFangXiangLBt(SSGlobalData.PlayerEnum indexPlayer, ButtonState val)
    {
        int index = (int)indexPlayer;
        if (index < 0 || index >= m_PlayerDirData.Length)
        {
            return;
        }

        int indexDirLeft = (int)DirBt.Left;
        int indexDirRight = (int)DirBt.Right;
        switch (val)
        {
            case ButtonState.DOWN:
                {
                    m_PlayerDirData[index].PlayerFXTmp[indexDirLeft] = 1f;
                    m_PlayerDirData[index].PlayerFX = -1f;
                    break;
                }
            case ButtonState.UP:
                {
                    m_PlayerDirData[index].PlayerFXTmp[indexDirLeft] = 0f;
                    m_PlayerDirData[index].PlayerFX = m_PlayerDirData[index].PlayerFXTmp[indexDirRight] == 0f ? 0f : 1f;
                    break;
                }
        }
	}
	/**
	 * 方向右响应.
	 */
	public void ClickFangXiangRBt(SSGlobalData.PlayerEnum indexPlayer, ButtonState val)
    {
        int index = (int)indexPlayer;
        if (index < 0 || index >= m_PlayerDirData.Length)
        {
            return;
        }

        int indexDirLeft = (int)DirBt.Left;
        int indexDirRight = (int)DirBt.Right;
        switch (val)
        {
            case ButtonState.DOWN:
                {
                    m_PlayerDirData[index].PlayerFXTmp[indexDirRight] = 1f;
                    m_PlayerDirData[index].PlayerFX = 1f;
                    break;
                }
            case ButtonState.UP:
                {
                    m_PlayerDirData[index].PlayerFXTmp[indexDirRight] = 0f;
                    m_PlayerDirData[index].PlayerFX = m_PlayerDirData[index].PlayerFXTmp[indexDirLeft] == 0f ? 0f : -1f;
                    break;
                }
        }
	}

    class PlayerDirData
    {
        internal float[] PlayerFXTmp = new float[SSGlobalData.MAX_PLAYER];
        internal float PlayerFX;
    }
    PlayerDirData[] m_PlayerDirData = new PlayerDirData[SSGlobalData.MAX_PLAYER];

    void InitPlayerDirData()
    {
        for (int i = 0; i < m_PlayerDirData.Length; i++)
        {
            m_PlayerDirData[i] = new PlayerDirData();
        }
    }

    internal float GetPlayerDirVal(SSGlobalData.PlayerEnum indexPlayer)
    {
        int index = (int)indexPlayer;
        if (index < 0 || index >= m_PlayerDirData.Length)
        {
            return 0f;
        }

        float val = 0f;
        if (m_PlayerDirData[index] != null)
        {
            val = m_PlayerDirData[index].PlayerFX;
        }
        return val;
    }
    #endregion

    void Update()
	{
        //StartBt PlayerOne
        if (Input.GetKeyUp(KeyCode.G)) {
            OnClickGameStartBt( SSGlobalData.PlayerEnum.PlayerOne, ButtonState.UP );
		}
		
		if (Input.GetKeyDown(KeyCode.G)) {
            OnClickGameStartBt( SSGlobalData.PlayerEnum.PlayerOne, ButtonState.DOWN );
		}
		
		//StartBt PlayerTwo
		if (Input.GetKeyUp(KeyCode.K)) {
            OnClickGameStartBt( SSGlobalData.PlayerEnum.PlayerTwo, ButtonState.UP );
		}
		
		if (Input.GetKeyDown(KeyCode.K)) {
            OnClickGameStartBt( SSGlobalData.PlayerEnum.PlayerTwo, ButtonState.DOWN );
        }

        //默认玩家1在屏幕左侧
        //player_1.
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W))
        {
            OnClickFangXiangLBt(SSGlobalData.PlayerEnum.PlayerOne, ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.W))
        {
            OnClickFangXiangLBt(SSGlobalData.PlayerEnum.PlayerOne, ButtonState.UP);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S))
        {
            OnClickFangXiangRBt(SSGlobalData.PlayerEnum.PlayerOne, ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S))
        {
            OnClickFangXiangRBt(SSGlobalData.PlayerEnum.PlayerOne, ButtonState.UP);
        }

        //默认玩家2在屏幕右侧
        //player_2.
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnClickFangXiangLBt(SSGlobalData.PlayerEnum.PlayerTwo, ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            OnClickFangXiangLBt(SSGlobalData.PlayerEnum.PlayerTwo, ButtonState.UP);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnClickFangXiangRBt(SSGlobalData.PlayerEnum.PlayerTwo, ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            OnClickFangXiangRBt(SSGlobalData.PlayerEnum.PlayerTwo, ButtonState.UP);
        }

        //setPanel enter button
        if (Input.GetKeyUp(KeyCode.F4)) {
			ClickSetEnterBt( ButtonState.UP );
		}
		
		if (Input.GetKeyDown(KeyCode.F4)) {
			ClickSetEnterBt( ButtonState.DOWN );
		}
		
		//setPanel move button
		if (Input.GetKeyUp(KeyCode.F5)) {
			ClickSetMoveBt( ButtonState.UP );
		}
		
		if (Input.GetKeyDown(KeyCode.F5)) {
			ClickSetMoveBt( ButtonState.DOWN );
		}
    }
}