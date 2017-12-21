using System.Collections;
using UnityEngine;

public class Fractal : MonoBehaviour {

	public Mesh[] meshes;
    public Material material;
    public int maxDepth;
    public float childScale = .5f;
    public float spawnProbability;
    public float maxRotationSpeed;
	public float maxTwist;

	private float _rotationSpeed;
    private int _depth;
    private Material[,] _materials;
    private static Vector3[] _childDirections = {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };
    private static Quaternion[] _childOrientations = {
        Quaternion.identity,
        Quaternion.Euler(0, 0, -90),
        Quaternion.Euler(0, 0, 90),
        Quaternion.Euler(90, 0, 0),
        Quaternion.Euler(-90, 0, 0)
    };

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {
        _rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);

        if(_materials == null)
            InitializeMaterials();

        gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = _materials[_depth, Random.Range(0, 2)];

        if(_depth < maxDepth) 
           StartCoroutine(CreateChildren());       
    }

    void Update () {
		transform.Rotate(0f, 30f * Time.deltaTime, 0f);
	}

    /*interpolates between white and yellow based on the _depth value.
      higher is the _depth, more yellow is the material
    */
    private void InitializeMaterials() {
        _materials = new Material[maxDepth + 1, 2];
        for(int i = 0; i <= maxDepth; i++) {
            float t = i / (maxDepth - 1f);
            t *= t;
            _materials[i, 0] = new Material(material);
            _materials[i, 0].color = Color.Lerp(Color.white, Color.yellow, t);
            _materials[i, 1] = new Material(material);
            _materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, t);
        }
        _materials[maxDepth, 0].color = Color.magenta;
        _materials[maxDepth, 1].color = Color.red;
    }

    private IEnumerator CreateChildren() {
        for(int i = 0; i < _childDirections.Length; i++) {
            if(Random.value < spawnProbability) {
                yield return new WaitForSeconds(Random.Range(.1f, .5f));
                new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
            }
        }
    }

    private void Initialize(Fractal parent, int childIndex) {
        meshes = parent.meshes;
        _materials = parent._materials;
        maxDepth = parent.maxDepth;
        _depth = parent._depth + 1;
        spawnProbability = parent.spawnProbability;
        maxRotationSpeed = parent.maxRotationSpeed;
        childScale = parent.childScale;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = _childDirections[childIndex] * (.5f + .5f * childScale);
        transform.localRotation = _childOrientations[childIndex];
    }

}
