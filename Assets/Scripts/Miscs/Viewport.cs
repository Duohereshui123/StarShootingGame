using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//viewport限制
public class Viewport : Singleton<Viewport>
{
    float minX;
    float maxX;
    float minY;
    float maxY;

    float middleX;

    public float MaxX => maxX;

    void Start()
    {
        Camera mainCamera = Camera.main;
        //因为计算使用的是world坐标，先给camera的左下右上两个（0，0） （1，1）local坐标换成world坐标

        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));

        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).x;

        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
    }

    /// <summary>
    /// 限制玩家移动范围的函数
    /// </summary>
    /// <param name="playerPosition">玩家锚点pivot的位置</param>
    /// <param name="paddingX">超出pivot的横幅</param>
    /// <param name="paddingY">超出pivot的纵幅</param>
    /// <returns>被限制后的位置</returns>
    public Vector3 PlayerMoveablePosition(Vector3 playerPosition, float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Clamp(playerPosition.x, minX + paddingX, maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingY, maxY - paddingY);

        return position;
    }

    //敌人随机在视野外重生
    public Vector3 RandomEnemyRespawnPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = maxX + paddingX;
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;
    }

    //敌人在画面内右半区移动
    public Vector3 RandomRightHalfMovingPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(middleX + paddingX, maxX - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;
    }
}
