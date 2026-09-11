using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Netcode;

[RequireComponent(typeof(RectTransform))]
public class LIfeGaugeContener : MonoBehaviour
{

    public static LIfeGaugeContener Instance
    {
        get { return _instance; }


    }

    public static LIfeGaugeContener _instance;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private LifeGauge lifeGaugePrefab;

    private RectTransform rectTransform;
    private readonly Dictionary<MobStatus, LifeGauge> _statusLifeBarMap
        = new Dictionary<MobStatus, LifeGauge>();

    private void Awake()
    {
        if (null != _instance) throw new Exception("LifebarContainer instance already exists.");
        _instance = this;
        rectTransform = GetComponent<RectTransform>();
    }

    public void Add(MobStatus status)
    {
        var lifeGauge = Instantiate(lifeGaugePrefab, transform);
        lifeGauge.Initialize(rectTransform, mainCamera, status);
        _statusLifeBarMap.Add(status, lifeGauge);
    }

    public void Remove(MobStatus status)
    {
        Destroy(_statusLifeBarMap[status].gameObject);
        _statusLifeBarMap.Remove(status);
    }

}
