using UnityEngine;

public class RayTracing : MonoBehaviour
{
    public ComputeShader shader;
    
    private RenderTexture _target;
    private Camera camera;

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Render(dest);
    }

    private void Render(RenderTexture dest) {
        InitRenderTexture();

        shader.SetTexture(0, "Result", _target);
        int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
        shader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        SetShaderParameters();

        Graphics.Blit(_target, dest);
    }

    private void InitRenderTexture() {
        if (_target == null || _target.width != Screen.width || _target.height != Screen.height) {
            if (_target != null) {
                _target.Release();
            }

            _target = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            _target.enableRandomWrite = true;
            _target.Create();
        }
    }

    void Awake() {
        camera = GetComponent<Camera>();
    }

    private void SetShaderParameters() {
        shader.SetMatrix("_CameraToWorld", camera.cameraToWorldMatrix);
        shader.SetMatrix("_CameraInverseProjection", camera.projectionMatrix.inverse);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
