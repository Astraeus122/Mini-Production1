using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLookAtCamera : MonoBehaviour
{
    public Camera mainCam;
    public Sprite sprite_left;
    public Sprite sprite_right;
    
    bool _ismainCamNotNull;
    private SpriteRenderer _spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        _ismainCamNotNull = mainCam != null;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_ismainCamNotNull)
        {
            gameObject.transform.forward = mainCam.transform.forward;
        }
        else
        {
            Debug.LogWarning("mainCam not assigned.");
        }
        
        float horizontal = Input.GetAxis("Horizontal");
        // float vertical = Input.GetAxis("Vertical");
        
        if (horizontal < 0)
        {
            _spriteRenderer.sprite = sprite_left;
        }
        else if (horizontal > 0)
        {
            _spriteRenderer.sprite = sprite_right;
        }
      
    }
}
