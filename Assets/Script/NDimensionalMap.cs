using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NDimensionalMap : Graphic
{
    /// <summary>
    /// 绘制方式
    /// regular 正多边形
    /// input 输入边数
    /// </summary>
    public enum DrawTypeEnum
    {
        regular,
        input
    }

    public DrawTypeEnum DrawType;

    /// <summary>
    /// 多边形边数
    /// </summary>
    public int EdgeAmount;

    /// <summary>
    /// 多边形每个角的位置
    /// </summary>
    [SerializeField]
    public List<Vector2> Positions;

    /// <summary>
    /// 多维图的颜色
    /// </summary>
    public Color PolygonColor;

    /// <summary>
    /// 是否绘制外边
    /// </summary>
    public bool DrawLineFlag;

    /// <summary>
    /// 多维图的颜色
    /// </summary>
    public Color LineColor;

    /// <summary>
    /// 多维图变线的宽度
    /// </summary>
    public float LineWidth;

    private RectTransform _rectTransform;
    private LineRenderer _lineRenderer;

    /// <summary>
    /// 刷新图片
    /// </summary>
    public void Refresh()
    {
        if (EdgeAmount < 3)
        {
            Debug.LogError("EdgeAmount error!!");
            return;
        }
        SetAllDirty();
    }

    /// <summary>
    /// 获取linerRenderer
    /// </summary>
    public LineRenderer GetLineRenderer()
    {
        return _lineRenderer;
    }

    protected override void Start()
    {
        Refresh();
    }

    /// <summary>
    /// 更新线段
    /// </summary>
    private void UpdateLine()
    {
        if (_lineRenderer == null && DrawLineFlag)
        {
            CreateLineRenderer();
            UpdateLine();
            return;
        }
        if (!DrawLineFlag)
        {
            _lineRenderer.enabled = false;
            return;
        }
        _lineRenderer.enabled = true;
        _lineRenderer.startWidth = LineWidth;
        _lineRenderer.endWidth = LineWidth;
        _lineRenderer.startColor = LineColor;
        _lineRenderer.endColor = LineColor;
        //绘制点
        Vector2[] posArr = GetPostions(EdgeAmount);
        float polygonWidth = GetPolygonWidth();
        _lineRenderer.positionCount = posArr.Length + 1;
        for (int i = 0; i < posArr.Length; i++)
        {
            _lineRenderer.SetPosition(i, 
                new Vector3(posArr[i].x * polygonWidth, posArr[i].y * polygonWidth, 0));
        }
        //设置一个起点，连成线段
        _lineRenderer.SetPosition(posArr.Length,
                new Vector3(posArr[0].x * polygonWidth, posArr[0].y * polygonWidth, 0));
    }

    private void CreateLineRenderer()
    {
        _lineRenderer = transform.GetComponent<LineRenderer>();
        if (_lineRenderer == null)
        {
            _lineRenderer = gameObject.AddComponent<LineRenderer>();
            _lineRenderer.material = new Material(Shader.Find("UI/Default"));
            //这点很重要
            _lineRenderer.useWorldSpace = false;
            _lineRenderer.loop = true;
        }
    }

    protected override void OnPopulateMesh(VertexHelper vertexHelper)
    {
        Vector2[] posArr = GetPostions(EdgeAmount);
        float polygonWidth = GetPolygonWidth();
        int i;
        vertexHelper.Clear();
        //绘制线段
        for (i = 0; i < posArr.Length; i++)
        {
            vertexHelper.AddVert(new Vector3(posArr[i].x * polygonWidth, posArr[i].y * polygonWidth, 0),
                PolygonColor, Vector2.zero);
        }
        //绘制三角形
        for (i = 1; i < posArr.Length - 1; i++)
        {
            vertexHelper.AddTriangle(0, i, i + 1);
        }
        //绘制线段
        UpdateLine();
    }

    /// <summary>
    /// 获取多边形的尺寸
    /// </summary>
    private float GetPolygonWidth()
    {
        if (_rectTransform == null)
        {
            _rectTransform = transform.GetComponent<RectTransform>();
        }
        if (_rectTransform == null)
        {
            return Mathf.Min(transform.localScale.x, transform.localScale.y) / 2;
        }
        return Mathf.Min(_rectTransform.rect.width, _rectTransform.rect.height) / 2;
    }

    /// <summary>
    /// 获取正多边形每个点的相对坐标
    /// </summary>
    /// <returns></returns>
    private Vector2[] GetPostions(int edgeAmount)
    {
        if (DrawType == DrawTypeEnum.input)
        {
            return Positions.ToArray();
        }
        Vector2[] postions = new Vector2[edgeAmount];
        float oneAngle = 360 / edgeAmount;
        float firstAngle;
        //判断是否单双数
        if (edgeAmount % 2 == 0)
        {
            firstAngle = oneAngle / 2;
            postions[0] = GetPostionByAngle(90 - firstAngle);
        }
        else
        {
            firstAngle = 0;
            postions[0] = GetPostionByAngle(90 - firstAngle);
        }
        //计算剩下的角度
        float angle;
        for (int i = 1; i < edgeAmount; i++)
        {
            angle = firstAngle + oneAngle * i;
            postions[i] = GetPostionByAngle(90 - angle);
        }
        return postions;
    }

    /// <summary>
    /// 根据角度获取相对位置
    /// </summary>
    private Vector2 GetPostionByAngle(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

#if UNITY_EDITOR

    protected override void OnValidate()
    {
        Refresh();
    }

#endif
}
