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
            float WidthToCamera = (position.x - camPos.x) / mapW;

            if (Mathf.Abs(WidthToCamera) <= 0.5f)
                 return position;
                       
            if (WidthToCamera > 0)
                WidthToCamera += 0.5f;
            else
                WidthToCamera -= 0.5f;

            int WidthToCameraToMove = (int)WidthToCamera;
            position.x -= WidthToCameraToMove * mapW;
        }

        if (allowWrapNorthSouth)
        {
            float HeightToCamera = (position.z - camPos.z) / mapH;

            if (Mathf.Abs(HeightToCamera) <= 0.5f)
                return position;

            if (HeightToCamera > 0)
                HeightToCamera += 0.5f;
            else
                HeightToCamera -= 0.5f;

            int HeightToCameraToMove = (int)HeightToCamera;
            position.z -= HeightToCameraToMove * mapH;
        }
        return position;
    }

}