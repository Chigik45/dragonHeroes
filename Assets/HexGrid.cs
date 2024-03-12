using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public int gridSize = 5; // ������ ����� (���������� ���������� � ����� �����������)
    public float hexagonSize = 1f; // ������ ��������� (�������� �� 1.5)
    public GameObject hexagonPrefab; // ������ ���������

    void Start()
    {
        GenerateHexGrid();
    }

    void GenerateHexGrid()
    {
        for (int q = -gridSize; q <= gridSize; q++)
        {
            for (int r = -gridSize + 3; r <= gridSize - 2; r++)
            {
                float xPos = q;
                float yPos = r;

                GameObject hexagon = Instantiate(hexagonPrefab, new Vector3(hexagonSize * xPos * 1 + hexagonSize * (r % 2 == 0 ? 0 : 0.5f), hexagonSize * yPos * (0.793f), 0), Quaternion.identity);
            }
        }
    }
}
