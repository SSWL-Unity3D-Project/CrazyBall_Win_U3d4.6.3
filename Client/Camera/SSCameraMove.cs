﻿using UnityEngine;
        /// 最大角度
        /// </summary>
                cameraTrTmp.forward = Vector3.MoveTowards(cameraTrTmp.forward, forward.normalized, Time.fixedDeltaTime * RotSpeed);
                //SSDebug.Log("LocalAngle == " + cameraTrTmp.localEulerAngles);

                float angle = cameraTrTmp.localEulerAngles.y;
                {
                    if (angle < 180f)
                    {
                        angle = MaxAngle;
                    }
                    else if (angle > 180f)
                    {
                        if (angle < 360f - MaxAngle)
                        {
                            angle = 360f - MaxAngle;
                        }
                    }
                }