using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ChunkRenderer : MonoBehaviour
{
    private const int ChunkWidth = 10;
    private const int ChunkHeight = 128;

    //������ � ���� ������
    public int[,,] Blocks = new int[ChunkWidth, ChunkHeight, ChunkWidth];

    //������� �����������
    //������� ������� ��������(�������)
    private List<Vector3> verticies = new List<Vector3>();
    //������� ������������
    private List<int> triangles = new List<int>();

    private void Start()
    {
        //������� ���� ��� �����
        Mesh chunkMesh = new Mesh();

        Blocks[0, 0, 0] = 1;

        //�������� ������� �� ���� ����� �� ���� 3 �����������
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

        //��� �������� �������������� � ����������
        chunkMesh.RecalculateNormals();
        //��� ����������
        chunkMesh.RecalculateBounds();

        //���������� � ���� ���� ������� � �����������
        chunkMesh.vertices = verticies.ToArray();
        chunkMesh.triangles = triangles.ToArray();

        //���������� ���� � ��������� �����������
        GetComponent<MeshFilter>().mesh = chunkMesh;
    }

    private void GenerateBlock(int x, int y, int z)
    {
        if (Blocks[x, y, z] == 0) return;

        var blockPosition = new Vector3Int(x, y, z);

        GenerateRightSide(blockPosition);
        GenerateLeftSide(blockPosition);
        GenerateFrontSide(blockPosition);
        GenerateBacktSide(blockPosition);
        GenerateTopSide(blockPosition);
        GenerateBottomSide(blockPosition);
    }

    private void GenerateRightSide(Vector3Int blockPosition)
    {
        //�������
        verticies.Add(new Vector3(1, 0, 0) + blockPosition);
        verticies.Add(new Vector3(1, 1, 0) + blockPosition);
        verticies.Add(new Vector3(1, 0, 1) + blockPosition);
        verticies.Add(new Vector3(1, 1, 1) + blockPosition); //2����������� �������

        AddLastVerticiesSquare();
    }

    private void GenerateLeftSide(Vector3Int blockPosition)
    {
        //�������
        verticies.Add(new Vector3(0, 0, 0) + blockPosition);
        verticies.Add(new Vector3(0, 0, 1) + blockPosition);
        verticies.Add(new Vector3(0, 1, 0) + blockPosition);
        verticies.Add(new Vector3(0, 1, 1) + blockPosition); //2����������� �������
               
        AddLastVerticiesSquare();
    }

    private void GenerateFrontSide(Vector3Int blockPosition)
    {
        //�������
        verticies.Add(new Vector3(0, 0, 1) + blockPosition);
        verticies.Add(new Vector3(1, 0, 1) + blockPosition);
        verticies.Add(new Vector3(0, 1, 1) + blockPosition);
        verticies.Add(new Vector3(1, 1, 1) + blockPosition); //2����������� �������

        AddLastVerticiesSquare();
    }

    private void GenerateBacktSide(Vector3Int blockPosition)
    {
        //�������
        verticies.Add(new Vector3(0, 0, 0) + blockPosition);
        verticies.Add(new Vector3(0, 1, 0) + blockPosition);
        verticies.Add(new Vector3(1, 0, 0) + blockPosition);
        verticies.Add(new Vector3(1, 1, 0) + blockPosition); //2����������� �������
               
        AddLastVerticiesSquare();
    }

    private void GenerateTopSide(Vector3Int blockPosition)
    {
        //�������
        verticies.Add(new Vector3(0, 1, 0) + blockPosition);
        verticies.Add(new Vector3(0, 1, 1) + blockPosition);
        verticies.Add(new Vector3(1, 1, 0) + blockPosition);
        verticies.Add(new Vector3(1, 1, 1) + blockPosition); //2����������� �������

        AddLastVerticiesSquare();
    }

    private void GenerateBottomSide(Vector3Int blockPosition)
    {
        //�������
        verticies.Add(new Vector3(0, 0, 0) + blockPosition);
        verticies.Add(new Vector3(0, 0, 1) + blockPosition);
        verticies.Add(new Vector3(1, 0, 0) + blockPosition);
        verticies.Add(new Vector3(1, 0, 1) + blockPosition); //2����������� �������

        AddLastVerticiesSquare();
    }

    private void AddLastVerticiesSquare()
    {
        //����������� �������� �� ������, ��������� ������ ������. ��� �� �������� ��� �� ����������� ������ ������?
        triangles.Add(verticies.Count - 4);
        triangles.Add(verticies.Count - 3);
        triangles.Add(verticies.Count - 2);

        //2 �����������
        triangles.Add(verticies.Count - 3);
        triangles.Add(verticies.Count - 1);
        triangles.Add(verticies.Count - 2);
    }
}
