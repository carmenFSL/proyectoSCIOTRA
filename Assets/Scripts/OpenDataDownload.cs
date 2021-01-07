using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class OpenDataDownload : MonoBehaviour {

    [SerializeField]
    GameObject prefabPoint;


    public void DownloadOpenData()
    {
        StartCoroutine(OpenDataBcn());
    }

    IEnumerator OpenDataBcn()
    {



        //    https://opendata-ajuntament.barcelona.cat/data/dataset/45884540-b2df-4530-aa0d-a2380e6cf888/resource/2bd27da1-ce9e-41b0-a70a-11b86028282b/download
        //    https://opendata-ajuntament.barcelona.cat/data/api/action/datastore_search?resource_id=2bd27da1-ce9e-41b0-a70a-11b86028282b
        //    https://opendata-ajuntament.barcelona.cat/data/dataset/e9da8fe0-5de1-4ca3-97a8-4a8374ded921/resource/6b186e0f-5e38-4beb-8b1d-39dfa9b31053/download");
        //    https://opendata-ajuntament.barcelona.cat/data/api/action/datastore_search?resource_id=6b186e0f-5e38-4beb-8b1d-39dfa9b31053

        /*
        WWW www = new WWW("https://opendata-ajuntament.barcelona.cat/data/api/action/datastore_search?resource_id=6b186e0f-5e38-4beb-8b1d-39dfa9b31053");  
        yield return www;

        // CSV file
        string[] lines = www.text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
            Debug.Log("Line " + i + ": " + lines[i]);

        */


        //WWW www = new WWW("https://api.bsmsa.eu/ext/api/bsm/chargepoints/v1/chargepoints");
        WWW www = new WWW("https://opendata-ajuntament.barcelona.cat/data/api/action/datastore_search?resource_id=6b186e0f-5e38-4beb-8b1d-39dfa9b31053");
        yield return www;

        JObject obj = JObject.Parse(www.text);
        //JArray chargePoints = (JArray)obj["result"]["chargepoint"];
        JArray chargePoints = (JArray)obj["result"]["records"];

        Debug.Log("Number chargePoints: " + chargePoints.Count);

        List<string> nameList = new List<string>();
        for (var i = 0; i < chargePoints.Count; i++)
        {
            JObject chargePoint = (JObject)chargePoints.GetItem(i);
            //float lat = (float)chargePoint["Lat"];
            //float lon = (float)chargePoint["Lng"];
            //string name = (string)chargePoint["ParkingName"];

            float lat = (float)chargePoint["LATITUD"];
            float lon = (float)chargePoint["LONGITUD"];
            string name = (string)chargePoint["DIRECCIO"];
            //Debug.Log("Charge Point info lat, lon: " + lat.ToString() + "," + lon.ToString());
            if (!nameList.Contains(name))
            {
                nameList.Add(name);
                GameObject o = Instantiate(prefabPoint);
                o.GetComponent<PoiScript>().latObject = lat;
                o.GetComponent<PoiScript>().lonObject = lon;
                o.GetComponent<PoiScript>().textDescription = name;
                o.SendMessage("MapLocation");
            }
        }
        Debug.Log("Points: " + nameList.Count);
        

    }
}
