﻿using System.Collections;
using UnityEngine;

public class DrawMeshNow : MonoBehaviour
{
    public Material material;
    public Camera camera;

    private Mesh quadMesh;
    private RenderTexture renderTexture;

    private Coroutine _waitForEndOfFrameCoroutine;

    void Start()
    {
        CreateQuadMesh();
        CreateRenderTexture();
        camera.targetTexture = renderTexture;
        StartCoroutine(WaitForEndOfFrame());
    }

    private void CreateQuadMesh()
    {
        if (quadMesh != null)
        {
            return;
        }

        var mesh = new Mesh();

        var vertices = new[] {
            new Vector3(-1f, -1f, 0f),
            new Vector3(-1f, 1f, 0f),
            new Vector3(1f, 1f, 0f),
            new Vector3(1f, -1f, 0f)
        };

        var uvs = new[] {
            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f)
        };

        var colors = new[]
        {
            new Color(0f, 0f, 1f),
            new Color(0f, 1f, 1f),
            new Color(1f, 1f, 1f),
            new Color(1f, 0f, 1f),
        };

        var triangles = new[] {
            0, 2, 1,
            0, 3, 2
        };

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.colors = colors;

        quadMesh = mesh;
    }

    private void CreateRenderTexture()
    {
        if (renderTexture != null)
        {
            return;
        }

        renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
        renderTexture.antiAliasing = QualitySettings.antiAliasing;
    }

    public IEnumerator WaitForEndOfFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            material.SetPass(0);
        
            material.SetTexture("_MainTex", renderTexture);
            
            GL.PushMatrix();

            var matrix = Matrix4x4.identity;
            Graphics.DrawMeshNow(quadMesh, matrix);
            GL.PopMatrix();
        }
    }
}
