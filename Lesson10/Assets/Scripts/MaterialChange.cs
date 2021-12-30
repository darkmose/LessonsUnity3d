using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour
{
    private const string ShaderBlendParamName = "_BlendScale";
    private const float BlendMinValue = 0f;
    private const float BlendMaxValue = 1f;
    private Renderer _renderer;
    [SerializeField] [Range(1f, 50f)]
    private float _changeTextureSpeed = 1;
    [SerializeField] [Range(0.001f, 0.3f)]
    private float _changeTextureSteps = 0.01f;
    private bool _isForwardAnimation = true;
    private bool _isAnimated = false;

    private void Awake()
    {
        if (TryGetComponent<Renderer>(out Renderer renderer))
        {
            _renderer = renderer;
        }
        else
        {
            Debug.Log($"Renderer on object [{this.gameObject.name}] doesn't exist!");
        }
    }

    private IEnumerator ChangeTexture() 
    {
        if (!_isAnimated)
        {
            _isAnimated = true;
            if (!_renderer)
            {
                yield return null;
            }
            else
            {

                if (_isForwardAnimation)
                {
                    for (float v = BlendMinValue; v <= BlendMaxValue; v += _changeTextureSteps)
                    {
                        _renderer.sharedMaterial.SetFloat(ShaderBlendParamName, v);
                        yield return new WaitForSeconds(1 / _changeTextureSpeed);
                    }
                    _renderer.sharedMaterial.SetFloat(ShaderBlendParamName, 1);
                    _isAnimated = false;
                    _isForwardAnimation = false;
                }
                else
                {
                    for (float v = BlendMaxValue; v > BlendMinValue; v -= _changeTextureSteps)
                    {
                        _renderer.sharedMaterial.SetFloat(ShaderBlendParamName, v);
                        yield return new WaitForSeconds(1 / _changeTextureSpeed);
                    }
                    _renderer.sharedMaterial.SetFloat(ShaderBlendParamName, 0);
                    _isAnimated = false;
                    _isForwardAnimation = true;
                }
            }
        }        
    }

    private void OnMouseDown()
    {
        StartCoroutine(ChangeTexture());
    }


}
