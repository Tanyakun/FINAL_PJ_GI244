﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using static SpawnManager;

public class SpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int totalSpawnEnemies;
        public int numberOfRandomSpawnPoint;
        public int delayStart;
        public float spawnInterval;
        public int numberOfPowerUp;
        public int totalSpawnBoss;
    }

    public Wave[] waves; //wavegame
    public Transform[] spawnPoints; //SpawnPoints
    public GameObject enemyPrefab; //Enemy
    public GameObject bossPrefab; //Boss
    public GameObject powerUpPrefab; //GetItem

    public GameObject endPanel;// End Panel

    private int currentWaveIndex = 0;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    /*void Spawn()
    {
        int idx = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[idx].position, Quaternion.identity);
    }*/

    IEnumerator SpawnRoutine()
    {
        //Debug.Log("Hello 1 :" + Time.frameCount);
        //yield return null;
        //yield return new WaitForSeconds(1);
        //Debug.Log("Hello 2 :" + Time.frameCount);

        /*yield return new WaitForSeconds(2f);
        Spawn();

        int spawnCount = 0;
        while (true)
        { 
            yield return new WaitForSeconds(1f);
            Spawn();
            spawnCount++;
            if (spawnCount > 5)
            {
                break;
            }

            while (true)
            {
                yield return new WaitForSeconds(5f);
                Spawn();
            }
        }*/ //Code Prof.

        while (currentWaveIndex < waves.Length)
        {
            Wave wave = waves[currentWaveIndex];

            yield return new WaitForSeconds(wave.delayStart);
            // Cooldown Wave Start

            List<Transform> selectedSpawnPoints = new List<Transform>();
            List<int> availableIndices = new List<int>();
            for (int i = 0; i < spawnPoints.Length; i++) availableIndices.Add(i);

            for (int i = 0; i < wave.numberOfRandomSpawnPoint; i++)
            {
                int randIndex = Random.Range(0, availableIndices.Count);
                selectedSpawnPoints.Add(spawnPoints[availableIndices[randIndex]]);
                availableIndices.RemoveAt(randIndex);
            }// Random Spawn Enemy

            for (int i = 0; i < wave.numberOfPowerUp; i++)
            {
                int randIndex = Random.Range(0, spawnPoints.Length);
                Instantiate(powerUpPrefab, spawnPoints[randIndex].position, Quaternion.identity);
            }// Random Spawn PowerUp
            
            for (int i = 0; i < wave.totalSpawnEnemies; i++)
            {
                Transform spawnPoint = selectedSpawnPoints[Random.Range(0, selectedSpawnPoints.Count)];
                Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                yield return new WaitForSeconds(wave.spawnInterval);
            }// Number of enemies

            for (int i = 0; i < wave.totalSpawnBoss; i++)
            {
                Transform spawnPoint = selectedSpawnPoints[Random.Range(0, selectedSpawnPoints.Count)];
                GameObject boss = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
                Debug.Log("Boss Spawned at Wave " + currentWaveIndex);

                // 🔥 เพิ่มตรงนี้: ให้ Boss ใช้ท่าพิเศษถ้าเป็น Wave 8
                if (currentWaveIndex == 7) // Wave 8 (index เริ่มจาก 0)
                {
                    BossSpecialSkill skill = boss.AddComponent<BossSpecialSkill>();
                    skill.platform = GameObject.Find("Island").transform;
                }

                yield return new WaitForSeconds(wave.spawnInterval);
            }// Number of Boss

            while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                yield return null;
            }// enemy eliminated for next wave

            currentWaveIndex++;

        }

        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            yield return null;
        }// enemy eliminated for next wave

        if (endPanel != null)
        {
            endPanel.SetActive(true);
        }// Game Win Panel On

    } 

}
