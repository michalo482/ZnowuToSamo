using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FadeScreen : MonoBehaviour
{

    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void FadeIn() => _animator.SetTrigger("fadeIn");
    public void FadeOut() => _animator.SetTrigger("fadeOut");
}
