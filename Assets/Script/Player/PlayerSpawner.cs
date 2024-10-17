using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] playerSoldier;
    public SplineComputer playerSpline;
    public LayerMask collisionLayers;

    IEnumerator SpawnPlayer(int index)
    {
        if (index < 0 || index >= playerSoldier.Length)
        {
            yield break;
        }
        GameObject spawnedPlayer = Instantiate(playerSoldier[index], playerSpline.GetPointPosition(0), Quaternion.identity);
        UnitMovement movement = spawnedPlayer.AddComponent<UnitMovement>();
        movement.spline = playerSpline;
        movement.collisionLayers = collisionLayers;
        yield return null;
    }

    //IEnumerator MoveAlongSpline(GameObject player)
    //{
    //    int pointCount = playerSpline.pointCount;
    //
    //    for (int i = 0; i < pointCount - 1; i++)
    //    {
    //        Vector3 startPosition = playerSpline.GetPointPosition(i);
    //        Vector3 endPosition = playerSpline.GetPointPosition(i + 1);
    //
    //        float t = 0f;
    //
    //        while (t < 1f)
    //        {
    //            t += Time.deltaTime * moveSpeed / Vector3.Distance(startPosition, endPosition);
    //
    //            player.transform.position = Vector3.Lerp(startPosition, endPosition, t);
    //            yield return null;
    //        }
    //    }
    //}

    public void SoldierButton0()
    {
        StartCoroutine(SpawnPlayer(0));
    }
    public void SoldierButton1()
    {
        StartCoroutine(SpawnPlayer(1));
    }
    public void SoldierButton2()
    {
        StartCoroutine(SpawnPlayer(2));
    }
    public void SoldierButton3()
    {
        StartCoroutine(SpawnPlayer(3));
    }

    public void SoldierButton4()
    {
        StartCoroutine(SpawnPlayer(4));
    }
    public void SoldierButton5()
    {
        StartCoroutine(SpawnPlayer(5));
    }
    public void SoldierButton6()
    {
        StartCoroutine(SpawnPlayer(6));
    }
    public void SoldierButton7()
    {
        StartCoroutine(SpawnPlayer(7));
    }
    public void SoldierButton8()
    {
        StartCoroutine(SpawnPlayer(8));
    }

    public void SoldierButton9()
    {
        StartCoroutine(SpawnPlayer(9));
    }
    public void SoldierButton10()
    {
        StartCoroutine(SpawnPlayer(10));
    }
    public void DefensiveWall()
    {
        HPManager hPManager = GetComponent<HPManager>();   
        hPManager.currentHP += 10;
    }

    
}
