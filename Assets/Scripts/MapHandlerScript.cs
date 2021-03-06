﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class MapHandlerScript : MonoBehaviour
{
    [SerializeField]
    GameObject centerTileMap;
    [SerializeField]
    GameObject Derecha;
    // [SerializeField]
    // GameObject Izquierda;
    [SerializeField]
    GameObject ESDerecha;
    [SerializeField]
    GameObject ESCentro;


    public static int centerTileX, centerTileY;
    public static int zoom;

    public void Start()
    {
        DownloadMap();
    }
    public void DownloadMap()
    {
        zoom = 12;

        if (Input.location.status == LocationServiceStatus.Running)
        {
            Debug.Log("Status gps map handler: " + Input.location.status);
            WorldToTilePos(Input.location.lastData.longitude, Input.location.lastData.latitude, zoom);
        }
        else
        {
            WorldToTilePos(2.122279f, 41.384616f, zoom);
        }
       

        
        GameObject objectOpenData = GameObject.Find("OpenDataHandler");
        objectOpenData.SendMessage("DownloadOpenData");

        GameObject objectUs = GameObject.Find("User");
        objectUs.SendMessage("MapLocation");

        StartCoroutine(LoadTile(centerTileX, centerTileY, centerTileMap));
        StartCoroutine(LoadTile(centerTileX + 1, centerTileY, Derecha));
        //StartCoroutine(LoadTile(centerTileX - 1, centerTileY, Izquierda));
        StartCoroutine(LoadTile(centerTileX + 1, centerTileY + 1, ESDerecha));
        StartCoroutine(LoadTile(centerTileX, centerTileY + 1, ESCentro));

    }

    //public void DownLoadCenterMapTileGps()
    //{
    //    WorldToTilePos(41.275250f, 1.987500f, zoom);
    //    LoadTile(centerTileX, centerTileY, centerTileMap);
    //}

    public void WorldToTilePos(float lon, float lat, int zoom)
    {
        double tileX, tileY;
        tileX = (double)((lon + 180.0f) / 360.0f * (1 << zoom));
        tileY = (double)((1.0f - Mathf.Log(Mathf.Tan((float)lat * Mathf.PI / 180.0f) + 1.0f / Mathf.Cos((float)lat * Mathf.PI / 180.0f)) / Mathf.PI) / 2.0f * (1 << zoom));
        centerTileX = Mathf.FloorToInt((float)tileX);
        centerTileY = Mathf.FloorToInt((float)tileY);
        Debug.Log("Primer Cop X: " + tileX + " Y: " + tileY);
        Debug.Log("Status gps mapHandler: " + Input.location.status);
    }

    IEnumerator LoadTile(int x, int y, GameObject quadTile)
    {
        Debug.Log("loadTile");
        string uri = "https://a.tile.openstreetmap.org/" + zoom + "/" + x + "/" + y + ".png";
        Debug.Log("https://a.tile.openstreetmap.org/" + zoom + "/" + x + "/" + y + ".png");
        //CustomCertificateHandler certHandler = new CustomCertificateHandler();
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://a.tile.openstreetmap.org/"+zoom+"/"+x+"/"+y+".png");
        //www.certificateHandler = certHandler;
        yield return www.SendWebRequest();

        Debug.Log("server");
        if (www.isNetworkError || www.isHttpError){
            Debug.Log(www.error);
        }else{
            Debug.Log("Received");
            quadTile.GetComponent<MeshRenderer>().material.mainTexture = DownloadHandlerTexture.GetContent(www);
        } 
    }

    public void OnMouseDown()
    {
        zoom--;
        if (zoom < 10) zoom = 10;
        WorldToTilePos(2.122279f, 41.384616f, zoom);
        StartCoroutine(LoadTile(centerTileX, centerTileY, centerTileMap));
        // StartCoroutine(LoadTile(centerTileX, centerTileY, centerTileMap));
        // StartCoroutine(LoadTile(centerTileX + 1, centerTileY, Derecha));
        // StartCoroutine(LoadTile(centerTileX - 1, centerTileY, Izquierda));
        // StartCoroutine(LoadTile(centerTileX + 1, centerTileY + 1, ESDerecha));
        // StartCoroutine(LoadTile(centerTileX, centerTileY + 1, ESCentro));
        

        GameObject[] poiList = GameObject.FindGameObjectsWithTag("poi");
        foreach (GameObject o in poiList)
        {
            o.SendMessage("MapLocation");
        }

    }

    public void ZoomOut()
    {
        zoom++;
        if (zoom > 16) zoom = 16;
        WorldToTilePos(2.122279f, 41.384616f, zoom);
        StartCoroutine(LoadTile(centerTileX, centerTileY, centerTileMap));
        // StartCoroutine(LoadTile(centerTileX + 1, centerTileY, Derecha));
        // StartCoroutine(LoadTile(centerTileX - 1, centerTileY, Izquierda));
        // StartCoroutine(LoadTile(centerTileX + 1, centerTileY + 1, ESDerecha));
        // StartCoroutine(LoadTile(centerTileX, centerTileY + 1, ESCentro));


        GameObject[] poiList = GameObject.FindGameObjectsWithTag("poi");
        foreach (GameObject o in poiList)
        {
            o.SendMessage("MapLocation");
        }
    }
    
}
