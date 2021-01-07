using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UserScript : MonoBehaviour {


    double latUser;
    double lonUser;


    public void MapLocation()
    {
        
        StartCoroutine(Relocation());
    }

    IEnumerator Relocation()
    {
        for (; ; ) { 
            int x = MapHandlerScript.centerTileX;
            int y = MapHandlerScript.centerTileY;
            int zoom = MapHandlerScript.zoom;

            if (Input.location.status == LocationServiceStatus.Running)
            {
                lonUser = Input.location.lastData.latitude;
                latUser = Input.location.lastData.latitude;
                
            }
            else
            {
                
                lonUser = 1.987563f;
                latUser = 41.275f;
                
            }
            double a = DrawCubeX(lonUser, TileToWorldPos(x, y, zoom).X, TileToWorldPos(x + 1, y, zoom).X);
            double b = DrawCubeY(latUser, TileToWorldPos(x, y + 1, zoom).Y, TileToWorldPos(x, y, zoom).Y);

            //Debug.Log(" Pos" + a + "/" + b);
            this.transform.position = new Vector3((float)a - 0.5f, (float)b - 0.5f, 0.0f);

            yield return new WaitForSeconds(3);
        }
    }
    public struct Point
    {
        public double X;
        public double Y;
    }


    // p.X -> longitud
    // p.Y -> latitud
    // left upper corner
    public Point TileToWorldPos(double tile_x, double tile_y, int zoom)
    {
        Point p = new Point();
        double n = System.Math.PI - ((2.0 * System.Math.PI * tile_y) / System.Math.Pow(2.0, zoom));

        p.X = ((tile_x / System.Math.Pow(2.0, zoom) * 360.0) - 180.0);
        p.Y = (180.0 / System.Math.PI * System.Math.Atan(System.Math.Sinh(n)));

        return p;
    }

    public double DrawCubeY(double targetLat, double minLat, double maxLat)
    {
        double pixelY = ((targetLat - minLat) / (maxLat - minLat));
        return pixelY;
    }

    public double DrawCubeX(double targetLong, double minLong, double maxLong)
    {
        double pixelX = ((targetLong - minLong) / (maxLong - minLong));
        return pixelX;
    }

    
}
