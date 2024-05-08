using UnityEngine;

[System.Serializable]
public class LootSetting
{
    public GameObject prefab;
    [Range(0f, 100f)] public float dropPercent;

    public void Spawn(Vector3 position)
    {
        if(Random.Range(0f,100f)<=dropPercent)
        {
            PoolManager.Release(prefab,position);
        }
    }
}
