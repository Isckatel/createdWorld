using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ChunkRenderer : MonoBehaviour
{
    public const int ChunkWidth = 10;
    public const int ChunkHeight = 128;

    //Данные о всех блоках
    public int[,,] Blocks = new int[ChunkWidth, ChunkHeight, ChunkWidth];

    //Создаем треугольник
    //Вначале создаем вертаксы(вершины)
    private List<Vector3> verticies = new List<Vector3>();
    //Создаем треугольники
    private List<int> triangles = new List<int>();

    private void Start()
    {
        //создаем мешь для блока
        Mesh chunkMesh = new Mesh();

        Blocks = TerrainGenerator.GenerateTerrain((int)transform.position.x, (int)transform.position.z);
        

        //Проходил циклами по всей чанке по всем 3 координатам
        for (int y = 0; y < ChunkHeight; y++)
        {
            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int z = 0; z < ChunkWidth; z++)
                {
                    GenerateBlock(x,y,z);
                }
            }
        }

        //для хорошего взаимодействия с освещением
        chunkMesh.RecalculateNormals();
        //для колайдеров
        chunkMesh.RecalculateBounds();

        //Закидываем в мешь нашы вершины и треугольнии
        chunkMesh.vertices = verticies.ToArray();
        chunkMesh.triangles = triangles.ToArray();

        //Засовываем мешь в компонент мешьфильтер
        GetComponent<MeshFilter>().mesh = chunkMesh;
    }

    private void GenerateBlock(int x, int y, int z)
    {
        var blockPosition = new Vector3Int(x, y, z);

        if (GetBlockAtPosition(blockPosition) == 0) return;


        if (GetBlockAtPosition(blockPosition + Vector3Int.right) == 0) GenerateRightSide(blockPosition);
        if (GetBlockAtPosition(blockPosition + Vector3Int.left) == 0) GenerateLeftSide(blockPosition);
        if (GetBlockAtPosition(blockPosition + Vector3Int.forward) == 0) GenerateFrontSide(blockPosition);
        if (GetBlockAtPosition(blockPosition + Vector3Int.back) == 0) GenerateBacktSide(blockPosition);
        if (GetBlockAtPosition(blockPosition + Vector3Int.up) == 0) GenerateTopSide(blockPosition);
        if (GetBlockAtPosition(blockPosition + Vector3Int.down) == 0) GenerateBottomSide(blockPosition);
    }

    private int GetBlockAtPosition(Vector3Int blockPosition)
    {
        if (blockPosition.x >= 0 && blockPosition.x < ChunkWidth &&
            blockPosition.y >= 0 && blockPosition.y < ChunkHeight &&
            blockPosition.z >= 0 && blockPosition.z < ChunkWidth)
        {
            return Blocks[blockPosition.x, blockPosition.y, blockPosition.z];
        }
        else
        {
            return 0;
        }
    }

    private void GenerateRightSide(Vector3Int blockPosition)
    {
        //Вершины
        verticies.Add(new Vector3(1, 0, 0) + blockPosition);
        verticies.Add(new Vector3(1, 1, 0) + blockPosition);
        verticies.Add(new Vector3(1, 0, 1) + blockPosition);
        verticies.Add(new Vector3(1, 1, 1) + blockPosition); //2треугольник вершина

        AddLastVerticiesSquare();
    }

    private void GenerateLeftSide(Vector3Int blockPosition)
    {
        //Вершины
        verticies.Add(new Vector3(0, 0, 0) + blockPosition);
        verticies.Add(new Vector3(0, 0, 1) + blockPosition);
        verticies.Add(new Vector3(0, 1, 0) + blockPosition);
        verticies.Add(new Vector3(0, 1, 1) + blockPosition); //2треугольник вершина
               
        AddLastVerticiesSquare();
    }

    private void GenerateFrontSide(Vector3Int blockPosition)
    {
        //Вершины
        verticies.Add(new Vector3(0, 0, 1) + blockPosition);
        verticies.Add(new Vector3(1, 0, 1) + blockPosition);
        verticies.Add(new Vector3(0, 1, 1) + blockPosition);
        verticies.Add(new Vector3(1, 1, 1) + blockPosition); //2треугольник вершина

        AddLastVerticiesSquare();
    }

    private void GenerateBacktSide(Vector3Int blockPosition)
    {
        //Вершины
        verticies.Add(new Vector3(0, 0, 0) + blockPosition);
        verticies.Add(new Vector3(0, 1, 0) + blockPosition);
        verticies.Add(new Vector3(1, 0, 0) + blockPosition);
        verticies.Add(new Vector3(1, 1, 0) + blockPosition); //2треугольник вершина
               
        AddLastVerticiesSquare();
    }

    private void GenerateTopSide(Vector3Int blockPosition)
    {
        //Вершины
        verticies.Add(new Vector3(0, 1, 0) + blockPosition);
        verticies.Add(new Vector3(0, 1, 1) + blockPosition);
        verticies.Add(new Vector3(1, 1, 0) + blockPosition);
        verticies.Add(new Vector3(1, 1, 1) + blockPosition); //2треугольник вершина

        AddLastVerticiesSquare();
    }

    private void GenerateBottomSide(Vector3Int blockPosition)
    {
        //Вершины
        verticies.Add(new Vector3(0, 0, 0) + blockPosition);
        verticies.Add(new Vector3(1, 0, 0) + blockPosition);
        verticies.Add(new Vector3(0, 0, 1) + blockPosition);        
        verticies.Add(new Vector3(1, 0, 1) + blockPosition); //2треугольник вершина

        AddLastVerticiesSquare();
    }

    private void AddLastVerticiesSquare()
    {
        //Треугольник строится из вершин, добавляем номера вершин. Как он понимает что мы перечесляем номера вершин?
        triangles.Add(verticies.Count - 4);
        triangles.Add(verticies.Count - 3);
        triangles.Add(verticies.Count - 2);

        //2 треугольник
        triangles.Add(verticies.Count - 3);
        triangles.Add(verticies.Count - 1);
        triangles.Add(verticies.Count - 2);
    }
}
