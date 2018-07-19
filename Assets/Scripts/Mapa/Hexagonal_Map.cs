using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagonal_Map : MonoBehaviour
{
    public GameObject OptionsManager;

    public GameObject hexagPrefab;

    public Mesh M_Water;
    public Mesh M_Land;
    public Mesh M_Mountain;
    public Mesh M_Hill;

    public Material MatOcean;
    public Material MatMountains;
    public Material MatLand;
    public Material MatWater;
    public Material MatPlans;
    public Material MatDirt;


    public readonly int nRows = 30; //80- 128 mapa enorme en cv - test 66-106
    public readonly int nColumns = 60;

    void Start ()
    {
        GenMap();
    }


    private Hexagon[,] hexagons;
    private Dictionary<Hexagon, GameObject> Hex2GOMap;

    public Hexagon GetHexagonAt(int x, int y)
    {
        if(hexagons == null)
        {
            throw new UnityException("hexgons not instanciated");
            return null;
        }

        if(OptionsManager.GetComponent<OptionsManager>().allowWrapEastWest)
        {
            x = x % nRows;

            if(x < 0)
            {
                x += nRows*2;
            }
            x = x % nRows;
        }
        if(OptionsManager.GetComponent<OptionsManager>().allowWrapNorthSouth)
        {
            y = y % nColumns;

            if (y < 0)
            {
                y += nColumns*2;
            }

            y = y % nColumns;
        }

        try
        {
            return hexagons[x, y];
        }
        catch
        {
            Debug.LogError("hexagon at" + x + "," + y);
            return null;
        }
    }


   virtual public void GenMap()
   {

        hexagons = new Hexagon[nRows, nColumns];
        Hex2GOMap = new Dictionary<Hexagon, GameObject>();

        for (int column = 0; column < nColumns; column++)
        {
            for (int row = 0; row < nRows; row++)
            {
                Hexagon h = new Hexagon(column, row);

                hexagons[row, column] = h;

                h.Altitude = -0.5f;


                Vector3 pos = h.PositionFCamera(
                    Camera.main.transform.position,
                    nRows,
                    nColumns, 
                    OptionsManager.GetComponent<OptionsManager>().allowWrapEastWest, 
                    OptionsManager.GetComponent<OptionsManager>().allowWrapNorthSouth 
                    );

                GameObject GO = (GameObject)Instantiate(
                    hexagPrefab,
                    pos,
                    Quaternion.identity,
                    this.transform
                    );

                Hex2GOMap[h] = GO;

                GO.GetComponent<HexagonComponent>().Hex = h;
                GO.GetComponent<HexagonComponent>().HexMap = this;

                if (OptionsManager.GetComponent<OptionsManager>().activateCoordCells)
                {
                    if (GO.GetComponentInChildren<Renderer>().enabled == true)
                       GO.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);
                   
                }

                  
            }
        }
        UpdateHVisual();

     //   StaticBatchingUtility.Combine(this.gameObject);
    }
    public void UpdateHVisual()
    {
        for (int column = 0; column < nColumns; column++)
        {
            for (int row = 0; row < nRows; row++)
            {

                Hexagon h = hexagons[row, column];
                GameObject GO = Hex2GOMap[h];
                
                MeshRenderer mr = GO.GetComponentInChildren<MeshRenderer>();

                if (h.Altitude >= 0)
                {
                    mr.material = MatLand;

                }
                else
                {
                    mr.material = MatOcean;
                }

                MeshFilter mfr = GO.GetComponentInChildren<MeshFilter>();
                mfr.mesh = M_Water;

            }
        }
    }
    public Hexagon[] GetHexagonsInRange(Hexagon centerH, int range)
    {
        List<Hexagon> ret = new List<Hexagon>();
        for (int dx = -range; dx < range - 1; dx++)
        {
            for (int dy = Mathf.Max(-range + 1, -dx - range); dy < Mathf.Min(range, -dx + range - 1); dy++)
            {
                
                ret.Add(GetHexagonAt( centerH.C + dx, centerH.R + dy));
            }
        }

        return ret.ToArray();

    }
}

