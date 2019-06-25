using UnityEngine;

public class SSGameMono : MonoBehaviour
{
    /// <summary>
    /// 产生预制.
    /// </summary>
    public Object Instantiate(GameObject prefab, Transform parent, Transform trPosRot = null)
    {
        GameObject obj = (GameObject)Instantiate(prefab);
        if (parent != null)
        {
            obj.transform.SetParent(parent);
            if (trPosRot == null)
            {
                obj.transform.localScale = prefab.transform.localScale;
                obj.transform.localPosition = prefab.transform.localPosition;
                obj.transform.localEulerAngles = prefab.transform.localEulerAngles;
            }
            else
            {
                obj.transform.position = trPosRot.position;
                obj.transform.rotation = trPosRot.rotation;
            }
        }
        return obj;
    }

    /// <summary>
    /// 产生预制.
    /// </summary>
    public Object Instantiate(GameObject prefab, Vector3 pos, Transform parent = null)
    {
        GameObject obj = (GameObject)Instantiate(prefab);
        if (parent != null)
        {
            obj.transform.SetParent(parent);
            obj.transform.position = pos;
        }
        return obj;
    }

    bool IsRemoveSelf = false;
    /// <summary>
    /// 销毁自己
    /// </summary>
    public void RemoveSelf()
    {
        if (IsRemoveSelf == false)
        {
            IsRemoveSelf = true;
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 检测是否配置有DestroyThisTimed组件
    /// </summary>
    internal void CheckDestroyThisTimed(GameObject obj)
    {
        if (obj != null)
        {
            DestroyThisTimed com = obj.GetComponent<DestroyThisTimed>();
            if (com == null)
            {
                obj.AddComponent<DestroyThisTimed>();
            }
        }
    }
}
