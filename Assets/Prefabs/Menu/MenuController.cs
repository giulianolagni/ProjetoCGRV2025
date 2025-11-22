using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;

public class MenuController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject menuOpcoes, rawImage;
    private Animator animatorRawImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rawImage.SetActive(false);
        animatorRawImage = rawImage.GetComponent<Animator>();
        menuOpcoes.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!videoPlayer.isPlaying && Input.anyKeyDown)
        {
            videoPlayer.Play();
            rawImage.SetActive(true);
            animatorRawImage.SetTrigger("fadeIn");
            menuOpcoes.SetActive(true);
        }

    }
}
