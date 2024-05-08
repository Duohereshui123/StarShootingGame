using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//对象池管理
public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] playerProjectilePools;
    [SerializeField] Pool[] enemyProjectilePools;
    [SerializeField] Pool[] vFXPools;
    [SerializeField] Pool[] enemyPools;
    [SerializeField] Pool[] lootItemPools;

    static Dictionary<GameObject, Pool> dictionary;//对象池字典

    private void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();
        Initialized(playerProjectilePools);
        Initialized(enemyProjectilePools);
        Initialized(vFXPools);
        Initialized(enemyPools);
        Initialized(lootItemPools);
    }
#if UNITY_EDITOR
    private void OnDestroy()
    {
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
        CheckPoolSize(vFXPools);
        CheckPoolSize(enemyPools);
        CheckPoolSize(lootItemPools);
    }
#endif
    void CheckPoolSize(Pool[] pools)//检查对象数量
    {
        foreach (var pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning
                (
                    string.Format
                    (
                        "Pool : {0} has a runtime size {1} bigger than its initial size {2} !",
                        pool.Prefab.name, 
                        pool.RuntimeSize, 
                        pool.Size
                    )
                );
            }
        }
    }


    private void Initialized(Pool[] pools)
    {
        foreach (var pool in pools)
        {
#if UNITY_EDITOR //只在unity editor里编译这段，打包就忽略了。
            if (dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("Same prefab in multiple pools！Prefab: " + pool.Prefab.name);//输出相同的预制体的键值的名字
                continue;//有相同的预制体键值 就跳过
            }
#endif
            dictionary.Add(pool.Prefab, pool);
            Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;
            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }

    /// <summary>
    /// 根据传入的各种参数，返回释放对象池里准备好的对象
    /// </summary>
    /// <param name="prefab">指定的对象预制体名字</param>
    /// <returns>对象池里准备好的对象</returns>
    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could not find prefab ！Prefab: " + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].preparedObject();
    }

    //重载1
    /// <summary>
    /// 根据传入的各种参数，返回释放对象池里准备好的对象
    /// </summary>
    /// <param name="prefab">指定的对象预制体名字</param>
    /// <param name="position">指定的释放位置</param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could not find prefab ！Prefab: " + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].preparedObject(position);
    }

    //重载2
    /// <summary>
    /// 根据传入的各种参数，返回释放对象池里准备好的对象
    /// </summary>
    /// <param name="prefab">指定的对象预制体名字</param>
    /// <param name="position">指定的释放位置</param>
    /// <param name="rotation">指定的旋转值</param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could not find prefab ！Prefab: " + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].preparedObject(position, rotation);
    }

    //重载3
    /// <summary>
    /// 根据传入的各种参数，返回释放对象池里准备好的对象
    /// </summary>
    /// <param name="prefab">指定的对象预制体名字</param>
    /// <param name="position">指定的释放位置</param>
    /// <param name="rotation">指定的旋转值</param>
    /// <param name="localScale">指定的扩缩值</param>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could not find prefab ！Prefab: " + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].preparedObject(position, rotation, localScale);
    }

}
