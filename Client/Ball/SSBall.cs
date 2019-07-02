using UnityEngine;

public class SSBall : MonoBehaviour
{
    [System.Serializable]
    public class BallData
    {
        /// <summary>
        /// 初始化到玩家球拍的z坐标偏差
        /// </summary>
        public float posOffsetZ = 8f;
        /// <summary>
        /// 曲棍球的初始速度
        /// </summary>
        public float ballSpeed = 20f;
        /// <summary>
        /// 曲棍球的碰撞音效
        /// </summary>
        public SSAudioPlayer audioPlayer;
        internal float ballSpeeding
        {
            get
            {
                float speed = ballSpeed;
                if (SSGameMange.GetInstance() != null
                    && SSGameMange.GetInstance().m_SSGameScene != null
                    && SSGameMange.GetInstance().m_SSGameScene.m_BallData != null)
                {
                    speed = SSGameMange.GetInstance().m_SSGameScene.m_BallData.moveSpeed;
                }
                return speed;
            }
        }
        /// <summary>
        /// 碰上范围阻挡几次之后对曲棍球进行折射
        /// </summary>
        public int BadBounceMax = 2;
        /// <summary>
        /// 曲棍球碰到范围阻挡后的离去角度控制信息
        /// </summary>
        public SSGlobalData.MinMaxDataFloat bounceAngle = new SSGlobalData.MinMaxDataFloat(25f, 45f);
        internal int badBounceLayer = 10;
        internal int m_badBounceCount = 0;
        /// <summary>
        /// 是否运动
        /// </summary>
        internal bool isMove = false;
        internal SSGlobalData.PlayerEnum IndexPlayer = SSGlobalData.PlayerEnum.Null;

        internal void PlayAudio()
        {
            if (audioPlayer != null)
            {
                audioPlayer.Play(SSAudioPlayer.Mode.Once);
            }
        }
        internal float GetRandomBounceAngle()
        {
            return bounceAngle.GetRandom();
        }
    }
    public BallData m_BallData;

    // Use this for initialization
    internal void Init()
    {
        InitPosition(SSGlobalData.PlayerEnum.PlayerOne);
    }

    internal void ResetInfo()
    {
        InitPosition(SSGlobalData.PlayerEnum.PlayerOne);
    }

    void InitPosition(SSGlobalData.PlayerEnum indexPlayer)
    {
        SetBallPosition(indexPlayer);
    }

    internal void SetBallPosition(SSGlobalData.PlayerEnum indexPlayer)
    {
        if (SSGameMange.GetInstance() == null || SSGameMange.GetInstance().m_SSGameScene == null)
        {
            return;
        }

        SSPlayerPaddle paddle = SSGameMange.GetInstance().m_SSGameScene.GetPlayerPaddle(indexPlayer);
        if (paddle != null)
        {
            m_BallData.isMove = false;
            rigidbody.isKinematic = true;
            paddle.FillBall(m_BallData.posOffsetZ, this);
        }
        SetBallPlayerIndex(indexPlayer);
    }

    /// <summary>
    /// 发射曲棍球
    /// </summary>
    public void Fire(Vector3 forward)
    {
        transform.SetParent(null);
        rigidbody.isKinematic = false;
        m_BallData.isMove = true;
        //m_BallData.ballSpeeding = m_BallData.ballSpeed;
        rigidbody.velocity = forward * m_BallData.ballSpeeding;
    }

    void FixedUpdate()
    {
        if (m_BallData.isMove == false)
        {
            return;
        }
        moveAtConstantSpeed();
    }

    //move the ball at a constant speed
    void moveAtConstantSpeed()
    {
        //float mod = (m_hasHitPaddle == false) ? m_speedScalarBeforeHittingPaddle : 1f;
        Vector3 vec = rigidbody.velocity;
        vec.y = 0f;
        if (rigidbody.isKinematic == false)
        {
            rigidbody.velocity = vec.normalized * m_BallData.ballSpeeding;
        }
    }

    void OnCollisionExit(Collision col)
    {
        //SSDebug.Log("col == " + col.gameObject.layer);
        if (col.gameObject.layer == m_BallData.badBounceLayer)
        {
            //m_BallData.m_badBounceCount++;
            //if (m_BallData.m_badBounceCount > m_BallData.BadBounceMax)
            //{
            //    //SSDebug.Log("handle bad bounce!");
            //    handleBadBounce();
            //}
            m_BallData.PlayAudio();
        }
        else
        {
            m_BallData.m_badBounceCount = 0;
        }

        SSPlayerPaddle paddle = col.gameObject.GetComponent<SSPlayerPaddle>();
        if (paddle != null)
        {
            m_BallData.PlayAudio();
            SetBallPlayerIndex(paddle.IndexPlayer);
            if (SSGameMange.GetInstance() != null && SSGameMange.GetInstance().m_SSGameScene != null)
            {
                SSGameMange.GetInstance().m_SSGameScene.UpdateBallSpeed();
            }
            //handleHitPaddle();
        }
    }

    void SetBallPlayerIndex(SSGlobalData.PlayerEnum indexPlayer)
    {
        if (m_BallData != null)
        {
            m_BallData.IndexPlayer = indexPlayer;
            //SSDebug.Log("SetBallPlayerIndex -> indexPlayer ============= " + indexPlayer);
        }
    }

    internal SSGlobalData.PlayerEnum GetBallPlayerIndex()
    {
        if (m_BallData != null)
        {
            return m_BallData.IndexPlayer;
        }
        return SSGlobalData.PlayerEnum.Null;
    }

    void handleBadBounce()
    {
        if (rigidbody.isKinematic == true)
        {
            return;
        }

        Vector3 vel = rigidbody.velocity.normalized;
        float angleBounce = m_BallData.GetRandomBounceAngle();
        //SSDebug.Log("handleBadBounce -> vel == " + vel);
        if (vel.z > 0.9f)
        {
            if (vel.x <= 0)
            {
                rigidbody.velocity = Quaternion.AngleAxis(-angleBounce, Vector3.up) * rigidbody.velocity;
            }
            else
            {
                rigidbody.velocity = Quaternion.AngleAxis(angleBounce, Vector3.up) * rigidbody.velocity;
            }
        }
        else if (vel.z < -0.9f)
        {
            if (vel.x <= 0)
            {
                rigidbody.velocity = Quaternion.AngleAxis(angleBounce, Vector3.up) * rigidbody.velocity;
            }
            else
            {
                rigidbody.velocity = Quaternion.AngleAxis(-angleBounce, Vector3.up) * rigidbody.velocity;
            }
        }
    }

    void handleHitPaddle()
    {
        if (rigidbody.isKinematic == true)
        {
            return;
        }

        Vector3 vel = rigidbody.velocity.normalized;
        SSDebug.Log("handleHitPaddle -> vel == " + vel);
    }
}
