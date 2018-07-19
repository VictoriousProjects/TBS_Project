using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon
{
    public Hexagon()
    {
    }
    public Hexagon(int c, int r)
    {
        this.C = c;
        this.R = r;
        this.S = -(c + r);
    }

    public readonly int C; //column
    public readonly int R; //row
    public readonly int S;

    public float Altitude;
    public float Wet;
    public float Cold;
    

    float rad = 1f;

    public Vector3 Position()
    {
        return new Vector3(
            HexHorizontalSpac() * (this.C + this.R /2f),
            0,
            HexVerticalSpac() * this.R
        );
    }

    public float HexHeight()
    {
        return rad * 2;
    }
    public float HexWidth()
    {
        return (Mathf.Sqrt(3) / 2 * HexHeight());
    }

    public float HexVerticalSpac()
    {
        return HexHeight() * 0.75f;
    }
    public float HexHorizontalSpac()
    {
        return HexWidth();
    }

    public Vector3 PositionFCamera(Vector3 camPos,float numRows, float numColumns, bool allowWrapEastWest, bool allowWrapNorthSouth)
    {
       
        float mapH = numRows * HexVerticalSpac();
        float mapW = numColumns * HexHorizontalSpac();

        Vector3 position = this.Position();
  
        if(allowWrapEastWest)
        {
            float WidthToCamera = (float)(position.x - camPos.x) / mapW;

            int WidthToCameraToMove = (int)WidthToCamera;
            position.x -= WidthToCameraToMove * mapW;
        }

        if (allowWrapNorthSouth)
        {
            float HeightToCamera = (position.z - camPos.z) / mapH;

            int HeightToCameraToMove = (int)HeightToCamera;
            position.z -= HeightToCameraToMove * mapH;
        }
        return position;
    }

    public static float Distance(Hexagon a, Hexagon b)
    {
        float ret = 0;

        ret = Mathf.Max(Mathf.Abs(a.C - b.C), Mathf.Abs(a.R - b.R), Mathf.Abs(a.S - b.S));

        return (float)ret;
    }

}