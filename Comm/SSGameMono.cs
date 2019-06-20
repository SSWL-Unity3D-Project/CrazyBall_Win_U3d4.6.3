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
}
