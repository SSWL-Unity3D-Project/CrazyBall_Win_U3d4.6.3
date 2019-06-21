public class SSGlobalData
{
    public enum GameRaceResult
    {
        /// <summary>
        /// 胜利
        /// </summary>
        Victory = 0,
        /// <summary>
        /// 失败
        /// </summary>
        Failure = 1,
        /// <summary>
        /// 平局
        /// </summary>
        PingJu = 2,
    }

    public enum PlayerEnum
    {
        Null = -1,
        PlayerOne = 0,
        PlayerTwo = 1,
    }
    /// <summary>
    /// 游戏最大玩家数量
    /// </summary>
    public static int MAX_PLAYER = 2;

    #region 玩家数据信息
    /// <summary>
    /// 玩家数据信息
    /// </summary>
    public class PlayerData
    {
        PlayerEnum IndexPlayer = PlayerEnum.Null;
        int _FenShu = 0;
        /// <summary>
        /// 玩家得分
        /// </summary>
        internal int FenShu
        {
            set
            {
                _FenShu = value;
                if (SSGameMange.GetInstance() != null
                    && SSGameMange.GetInstance().m_SSGameUI != null)
                {
                    SSGameMange.GetInstance().m_SSGameUI.ShowPlayerFenShu(IndexPlayer);
                }
            }
            get
            {
                return _FenShu;
            }
        }

        public PlayerData(PlayerEnum indexPlayer)
        {
            IndexPlayer = indexPlayer;
        }

        /// <summary>
        /// 给玩家添加分数
        /// </summary>
        internal void AddPlayerFenShu(PlayerEnum indexPlayer, int val)
        {
            FenShu += val;
        }

        internal void Reset()
        {
            FenShu = 0;
        }
    }
    PlayerData[] m_PlayerDataArray = new PlayerData[MAX_PLAYER];

    PlayerData GetPlayerData(PlayerEnum indexPlayer)
    {
        int index = (int)indexPlayer;
        if (index < 0 || index >= MAX_PLAYER)
        {
            return null;
        }
        return m_PlayerDataArray[index];
    }

    /// <summary>
    /// 给玩家添加分数
    /// </summary>
    internal void AddPlayerFenShu(PlayerEnum indexPlayer, int val)
    {
        PlayerData dt = GetPlayerData(indexPlayer);
        if (dt != null)
        {
            dt.AddPlayerFenShu(indexPlayer, val);
        }
    }

    /// <summary>
    /// 获取玩家分数
    /// </summary>
    internal int GetPlayerFenShu(PlayerEnum indexPlayer)
    {
        PlayerData dt = GetPlayerData(indexPlayer);
        if (dt != null)
        {
            return dt.FenShu;
        }
        return 0;
    } 
    #endregion

    static SSGlobalData _Instance;
    public static SSGlobalData GetInstance()
    {
        if (_Instance == null)
        {
            _Instance = new SSGlobalData();
            _Instance.Init();
        }
        return _Instance;
    }

    void Init()
    {
        for (int i = 0; i < m_PlayerDataArray.Length; i++)
        {
            m_PlayerDataArray[i] = new PlayerData((PlayerEnum)i);
        }
    }

    /// <summary>
    /// 当游戏结束后,重置数据信息
    /// </summary>
    internal void ResetInfo()
    {
        for (int i = 0; i < m_PlayerDataArray.Length; i++)
        {
            if (m_PlayerDataArray[i] != null)
            {
                m_PlayerDataArray[i].Reset();
            }
        }
    }
}
