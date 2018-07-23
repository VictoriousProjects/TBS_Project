using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Hexagon
{
    public readonly int C; //column
    public readonly int R; //row
    public readonly int S;

    public Hexagon()
    {
        HMap = null;
        C = 0;
        R = 0;
        S = 0;
    }

    public Hexagon(Hexagonal_Map map, int c, int r)
    {
        HMap = map;
        C = c;
        R = r;
        S = -(c + r);
    }

    private Hexagonal_Map HMap;
    public float Altitude;
    float rad = 1f;

    public Vector3 Position()
    {
        return new Vector3(
            HexHorizontalSpac() * (this.C + this.R / 2f),
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

    public Vector3 PositionFCamera(Vector3 camPos, float numRows, float numColumns, bool allowWrapEastWest, bool allowWrapNorthSouth)
    {

        float mapH = numRows * HexVerticalSpac();
        float mapW = numColumns * HexHorizontalSpac();

        Vector3 position = this.Position();

        if (allowWrapEastWest)
        {
            float WidthToCamera = Mathf.Round((position.x - camPos.x) / mapW);

            int WidthToCameraToMove = (int)WidthToCamera;
            position.x -= WidthToCameraToMove * mapW;
        }

        if (allowWrapNorthSouth)
        {
            float HeightToCamera = Mathf.Round((position.z - camPos.z) / mapH);

            int HeightToCameraToMove = (int)HeightToCamera;
            position.z -= HeightToCameraToMove * mapH;
        }
        return position;
    }

    public static float Distance(Hexagon a, Hexagon b)
    {
        float ret = 0;


        int dC = Mathf.Abs(a.C - b.C);

        if(a.HMap.allow_Wrap_EW)
        {
            if (dC > a.HMap.nColumns / 2)
            {
                dC = a.HMap.nColumns - dC;
            }
        }
        int dR = Mathf.Abs(a.R - b.R);

        if (a.HMap.allow_Wrap_NS)
        {
            if (dR > a.HMap.nColumns / 2)
            {
                dR = a.HMap.nColumns - dR;
            }
        }

        ret = Mathf.Max(dC, dR, Mathf.Abs(a.S - b.S));

        return (float)ret;
    }

}