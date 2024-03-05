using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GraphicLayer
{
    public const string LAYER_OBJECT_NAME_FORMAT = "Layer: {0}";
    public int layerDepth = 0;
    public Transform panel;

    public GraphicObject currentGraphic = null;
    public List<GraphicObject> oldGraphics = new List<GraphicObject>();

    public Coroutine SetTexture(string filePath, float transitionSpeed = 1f)
    {
        Texture tex = Resources.Load<Texture>(filePath);

        if(tex == null)
        {
            Debug.LogError($"Could not load graphic texture from path '{filePath}.' Please ensure it exists within Resources!");
            return null;
        }

        return SetTexture(tex, transitionSpeed, filePath);
    }

    public Coroutine SetTexture(Texture tex, float transitionSpeed = 1f, string filePath = "")
    {
        return CreateGraphic(tex, transitionSpeed, filePath);
    }

    public Coroutine SetVideo(string filePath, float transitionSpeed = 1f, bool useAudio = true)
    {
        VideoClip clip = Resources.Load<VideoClip>(filePath);

        if(clip == null)
        {
            Debug.LogError($"Could not load graphic video from path '{filePath}.' Please ensure it exists within Resources!");
            return null;
        }

        return SetVideo(clip, transitionSpeed, useAudio, filePath);
    }

    public Coroutine SetVideo(VideoClip clip, float transitionSpeed = 1f, bool useAudio = true, string filePath = "")
    {
        return CreateGraphic(clip, transitionSpeed, filePath, useAudio);
    }

    private Coroutine CreateGraphic<T>(T graphicData, float transitionSpeed, string filePath, bool useAudioForVideo = true)
    {
        GraphicObject newGraphic = null;

        if(graphicData is Texture)
            newGraphic = new GraphicObject(this, filePath, graphicData as Texture);
        else if(graphicData is VideoClip)
            newGraphic = new GraphicObject(this, filePath, graphicData as VideoClip, useAudioForVideo);
        
        if(currentGraphic != null && !oldGraphics.Contains(currentGraphic))
            oldGraphics.Add(currentGraphic);

        currentGraphic = newGraphic;

        return currentGraphic.FadeIn(transitionSpeed);
    }

    private void SetTextBox()
    {
        
    }

    public void DestroyOldGraphics()
    {
        foreach(var g in oldGraphics)
            Object.Destroy(g.renderer.gameObject);
        
        oldGraphics.Clear();
    }

    public void Clear(float transitionSpeed = 1)
    {
        if(currentGraphic != null)
            currentGraphic.FadeOut(transitionSpeed);
        
        foreach(var g in oldGraphics)
            g.FadeOut(transitionSpeed);
    }

}
