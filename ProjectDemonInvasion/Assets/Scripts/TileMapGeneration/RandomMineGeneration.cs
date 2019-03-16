using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class RandomMineGeneration : MonoBehaviour {

    [Range(0,100)]
    public int inichance;

    [Range(1,8)]
    public int birthLimit;

    [Range(1,8)]
    public int deathLimit;

    [Range(1, 10)]
    public int numR;
    private int count = 0;

    private int[,] terrainMap;
    public Vector3Int tmapSize;

    public Tilemap topMap;
    public Tilemap botMap;

    public Tile topTile;
    public Tile botTile;

    int width;
    int height;

    public void doSim(int numR)
    {
        clearMap(false);
        width = tmapSize.x;
        height = tmapSize.y;

        if(terrainMap == null)
        {
            terrainMap = new int[width, height];
            initpos();
        }

        for(int i = 0; i<numR;i++)
        {
            terrainMap = genTilePos(terrainMap);
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

               if(terrainMap[x,y] == 1)
                {
                    topMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0),topTile);
                    
                }
                botMap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), botTile);
            }
        }
    }

    public int[,] genTilePos(int[,] oldmap)
    {
        int[,] newMap = new int[width, height];
        int neighbors;
        BoundsInt myB = new BoundsInt(-1, -1, 0, 3, 3, 1);

        for(int x = 0;x<width;x++)
        {
            for (int y = 0; y < height; y++)
            {
                neighbors = 0;

                foreach(var b in myB.allPositionsWithin)
                {
                    if(b.x == 0 && b.y == 0)
                    {
                        continue;
                    }
                    if(x+b.x >=0 && x+b.x <width && y+b.y>=0&&y+b.y<height)
                    {
                        neighbors += oldmap[x + b.x, y + b.y];
                    }
                    else
                    {
                        neighbors++;
                    }
                    if(oldmap[x,y] == 1)
                    {
                        if (neighbors < deathLimit) newMap[x, y] = 0;
                        else
                        {
                            newMap[x, y] = 1;
                        }
                    }
                    if(oldmap[x,y]==0)
                    {
                        if (neighbors > birthLimit) newMap[x, y] = 1;
                        else
                        {
                            newMap[x, y] = 0;
                        }
                    }
                }
            }
        }
        
        return newMap;
    }
    public void initpos()
    {

        for(int x = 0;x<width;x++)
        {
            for (int y = 0; y < height; y++)
            {
                terrainMap[x, y] = Random.Range(1, 100)<inichance?1:0;
            }
        }
        
    }
	// Update is called once per frame
	void Update () {
		
        if(Input.GetMouseButtonDown(0))
        {
            doSim(numR);
        }

        if(Input.GetMouseButtonDown(1))
        {
            clearMap(true);
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            SaveMap();
            count++;
        }
	}

    public void SaveMap()
    {
        string saveName = "Mine" + count.ToString();
        var mf = GameObject.Find("Grid");

        if(mf)
        {
            var savePath = "Assets/Maps/" + saveName + ".prefab";
            if(PrefabUtility.CreatePrefab(savePath,mf))
            {
                EditorUtility.DisplayDialog("Tile Map Saved", "Tile Map was Saved successfully", "Continue");
            }
            else
            {
                EditorUtility.DisplayDialog("Tile Map Was NOT Saved", "Tile Map failed to Save", "Continue");
            }
        }
    }
    public void clearMap(bool complete)
    {
        topMap.ClearAllTiles();
        botMap.ClearAllTiles();

        if(complete)
        {
            terrainMap = null;
        }
    }
}
