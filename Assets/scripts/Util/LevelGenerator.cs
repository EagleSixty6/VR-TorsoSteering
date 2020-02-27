using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject m_Building;
    [SerializeField] private GameObject m_Wall;
    [SerializeField] private float m_pathWidth = 5f;
    [SerializeField] private float m_gapWidth = 8f;
    [SerializeField] private int m_LevelLength = 12;
    [SerializeField] private int m_minSementLength = 3;
    [SerializeField] private int m_maxSegemtLength = 7;

    public GameObject GenerateLevel()
    {
        Vector3 start = new Vector3(0, 0, 0);
        List<float> directions = new List<float> { 90f, -90f };
        GameObject level = new GameObject("Level");

        //UnityEngine.Random.seed = 695720989;
        Debug.Log("Random seed: " + UnityEngine.Random.seed);

        List<float> directionsPath = new List<float>();
        directionsPath.Add(0f);


        while (directionsPath.Count < m_LevelLength)
        {
            float newDir = directions[UnityEngine.Random.Range(0, directions.Count)];

            if (directionsPath.Count > 1)
            {
                if (newDir != directionsPath[directions.Count - 1] || newDir != directionsPath[directions.Count - 2])
                {
                    directionsPath.Add(newDir);
                }
            }
            else
            {
                directionsPath.Add(newDir);
            }
        }

        Vector3 size = Vector3.zero;
        float direction = 0f;

        // create starting area
        for (int i = 0; i < Mathf.RoundToInt(m_pathWidth / 3f + 0.5f) + 4; ++i)
        {
            Instantiate(m_Wall, new Vector3(0f, 0f, m_pathWidth + 1f - 3f * i), Quaternion.Euler(-90, 90, 0), level.transform);
        }

        // create path
        for (int j = 0; j < directionsPath.Count; ++j)
        {
            direction += directionsPath[j];

            if (direction == 270f)
            {
                direction = -90f;
            }

            float pathSegmentCount = UnityEngine.Random.Range(m_minSementLength, m_maxSegemtLength + 1);
            Vector3 segmentStart = Vector3.zero;
            GameObject segment = new GameObject("Segment");
            segment.transform.parent = level.transform;

            for (int i = 0; i < pathSegmentCount; ++i)
            {
                GameObject newBuilding = Instantiate(m_Building, segment.transform);

                if (size == Vector3.zero)
                {
                    size = newBuilding.GetComponent<Collider>().bounds.size;
                }

                newBuilding.transform.Rotate(new Vector3(0, 90, 0));
                newBuilding.transform.position = new Vector3(segmentStart.z, 0, (m_pathWidth / 2f));

                newBuilding = Instantiate(m_Building, segment.transform);
                newBuilding.transform.Rotate(new Vector3(0, -90, 0));
                newBuilding.transform.position = new Vector3(segmentStart.z + size.z, 0, -(m_pathWidth / 2f));

                // add walls
                if (i + 1 < pathSegmentCount)
                {
                    GameObject wall = Instantiate(m_Wall, segment.transform);
                    wall.transform.position = new Vector3(segmentStart.z + size.z - 0.5f, 0, -m_pathWidth / 2 - 2 * size.x / 3);
                    wall = Instantiate(m_Wall, segment.transform);
                    wall.transform.position = new Vector3(segmentStart.z + size.z - 0.5f + 3f, 0, -m_pathWidth / 2 - 2 * size.x / 3);
                    wall = Instantiate(m_Wall, segment.transform);
                    wall.transform.position = new Vector3(segmentStart.z + size.z - 0.5f + 6f, 0, -m_pathWidth / 2 - 2 * size.x / 3);

                    wall = Instantiate(m_Wall, segment.transform);
                    wall.transform.position = new Vector3(segmentStart.z + size.z - 0.5f, 0, m_pathWidth / 2 + 2 * size.x / 3);
                    wall = Instantiate(m_Wall, segment.transform);
                    wall.transform.position = new Vector3(segmentStart.z + size.z - 0.5f + 3f, 0, m_pathWidth / 2 + 2 * size.x / 3);
                    wall = Instantiate(m_Wall, segment.transform);
                    wall.transform.position = new Vector3(segmentStart.z + size.z - 0.5f + 6f, 0, m_pathWidth / 2 + 2 * size.x / 3);
                }
                // create corner
                else
                {
                    // end of segment but not of path
                    if (j + 1 < directionsPath.Count)
                    {
                        Vector3 newPos = Vector3.zero;
                        GameObject wall;

                        if (Mathf.Sign(directionsPath[j + 1]) > 0)
                        {
                            for (int k = 0; k < Mathf.RoundToInt(m_pathWidth / 3f + 0.5f) + 6; ++k)
                            {
                                wall = Instantiate(m_Wall, segment.transform);
                                newPos = new Vector3(segmentStart.z + size.z - 0.5f + k * 3f, 0, m_pathWidth / 2 + 2 * size.x / 3);
                                wall.transform.position = newPos;
                            }

                            newPos.x += 3f;
                            newPos.z += 3f;

                            for (int k = 0; k < Mathf.RoundToInt(m_pathWidth / 3f + 0.5f) + 4; ++k)
                            {
                                wall = Instantiate(m_Wall, segment.transform);
                                wall.transform.Rotate(new Vector3(0, 0, 90));
                                newPos.z -= 3f;
                                wall.transform.position = newPos;
                            }
                        }
                        else
                        {
                            for (int k = 0; k < Mathf.RoundToInt(m_pathWidth / 3f + 0.5f) + 6; ++k)
                            {
                                wall = Instantiate(m_Wall, segment.transform);
                                newPos = new Vector3(segmentStart.z + size.z - 0.5f + k * 3f, 0, -m_pathWidth / 2 - 2 * size.x / 3);
                                wall.transform.position = newPos;
                            }

                            newPos.x += 3f;
                            newPos.z -= 3f;

                            for (int k = 0; k < Mathf.RoundToInt(m_pathWidth / 3f + 0.5f) + 4; ++k)
                            {
                                wall = Instantiate(m_Wall, segment.transform);
                                wall.transform.Rotate(new Vector3(0, 0, -90));
                                newPos.z += 3f;
                                wall.transform.position = newPos;
                            }
                        }
                    }
                }
                segmentStart.z += size.z + m_gapWidth;

            }
            segment.transform.rotation = Quaternion.Euler(0, direction, 0);
            segment.transform.position = start;

            if (j + 1 < directionsPath.Count)
            {
                segmentStart.z += size.x + -m_gapWidth + m_pathWidth / 2f;
                segmentStart.x += ((m_pathWidth / 2f) * Mathf.Sign(directionsPath[j + 1]));
                start += Quaternion.AngleAxis(direction + 90, Vector3.up) * segmentStart;
            }
        }

        return level;
    }
}
