using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ViewPort : Singleton<ViewPort>
{
    float minX;
    float minY;
    float maxX;
    float maxY;
    private float middleX;

    private void Start()
    {
        Camera mainCamera = Camera.main;

        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        minX = bottomLeft.x;
        minY = bottomLeft.y;

        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 1));
        maxX = topRight.x;
        maxY = topRight.y;

        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).x;
    }

    public Vector3 PlayerMovealbePosition(Vector3 playerPos, float paddingX, float paddingY)
    {
        Vector3 pos = Vector3.zero;
        pos.x = Mathf.Clamp(playerPos.x, minX + paddingX, maxX - paddingX);
        pos.y = Mathf.Clamp(playerPos.y, minY + paddingY, maxY - paddingY);
        return pos;
    }

    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY) //随机生成敌人的方法
    {
        Vector3 position = Vector3.zero;
        position.x = maxX + paddingX;
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;
    }
/// <summary>
/// 随机生成一个右半屏的位置，供Enemy移动使用
/// </summary>
/// <param name="paddingX"></param>
/// <param name="paddingY"></param>
/// <returns></returns>
    public Vector3 RandomRightHalfPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(middleX, maxX - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;
    }
/// <summary>
/// 随机生成一个全屏的位置，供Enemy移动使用
/// </summary>
/// <param name="paddingX"></param>
/// <param name="paddingY"></param>
/// <returns></returns>
    public Vector3 RandomEnemyMovePosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(minX + paddingX, maxX - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);
        return position;
    }
}