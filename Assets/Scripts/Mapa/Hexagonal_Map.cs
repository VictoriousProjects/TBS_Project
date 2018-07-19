using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagonal_Map : MonoBehaviour
{
    public GameObject OptionsManager;

    public GameObject hexagPrefab;

    public Material[] HexagMaterials;

    public Vector2 mapNumHex;//w - h - Columns, rows  80- 128 mapa enorme en cv - test 66-106

    void Start ()
    {
        GenMap();
    }
    public void GenMap()
    {
        for (int column = 0; column < mapNumHex.x; column++)
        {
            for (int row = 0; row < mapNumHex.y; row++)
            {
                Hexagon h = new Hexagon(column, row);

                Vector3 pos = h.PositionFCamera(Camera.main.transform.position, mapNumHex.y, mapNumHex.x, OptionsManager.GetComponent<OptionsManager>().allowWrapEastWest, OptionsManager.GetComponent<OptionsManager>().allowWrapNorthSouth );

                GameObject GO = (GameObject)Instantiate(hexagPrefab, pos,Quaternion.identity, this.transform);

                GO.GetComponent<HexagonComponent>().Hex = h;
                GO.GetComponent<HexagonComponent>().HexMap = this;

                MeshRenderer mr = GO.GetComponentInChildren<MeshRenderer>();
                mr.material = HexagMaterials[Random.Range(0, HexagMaterials.Length)];
            }
        }
     //   StaticBatchingUtility.Combine(this.gameObject);
    }
    
}
