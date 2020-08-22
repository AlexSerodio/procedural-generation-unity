using UnityEngine;

public class Graph : MonoBehaviour
{

    public Transform pointPrefab;

    [Range(10, 100)]
    public int resolution = 10;
	
    private Transform[] _points;

    void Awake()
    {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position;
        position.z = 0f;

        _points = new Transform[resolution];
        for (int i = 0; i < resolution; i++)
        {
            Transform point = Instantiate(pointPrefab);
            position.x = (i + 0.5f) * step - 1f;
            position.y = position.x * position.x * position.x;
			
            point.localPosition = position;
            point.localScale = scale;
            point.SetParent(transform, false);

            _points[i] = point;
        }
    }

    void Update()
    {
        for (int i = 0; i < _points.Length; i++)
        {
            Transform point = _points[i];
            Vector3 position = point.localPosition;
            //f(x,t)=sin(π(x+t))
            position.y = Mathf.Sin(Mathf.PI * (position.x + Time.time));
            point.localPosition = position;
        }
    }
}
