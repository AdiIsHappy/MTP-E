﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]

public class ProgressBarCircle : MonoBehaviour {
    [Header("Title Setting")]
    public string Title;
    public Color TitleColor;
    public Font TitleFont;    

    [Header("Bar Setting")]
    public Color BarColor;
    public Color BarBackGroundColor;
    public Color MaskColor;
    public Sprite BarBackGroundSprite;
    [Range(1f, 100f)]
    public int Alert = 20;
    public Color BarAlertColor;

    [SerializeField]private Image bar, barBackground,Mask;
    private float nextPlay;
    [SerializeField]private Text txtTitle;
    private float barValue;
    public float BarValue
    {
        get { return barValue; }

        set
        {
            value = Mathf.Clamp(value, 0, 100);
            barValue = value;
            UpdateValue(barValue);

        }
    }

    private void Awake()
    {

        txtTitle = transform.Find("Text").GetComponent<Text>();
        barBackground = transform.Find("BarBackgroundCircle").GetComponent<Image>();
        bar = transform.Find("BarCircle").GetComponent<Image>();
        Mask= transform.Find("Mask").GetComponent<Image>();
    }

    private void Start()
    {
        txtTitle.text = Title;
        txtTitle.color = TitleColor;
        txtTitle.font = TitleFont;
       

        bar.color = BarColor;
        Mask.color = MaskColor;
        barBackground.color = BarBackGroundColor;
        barBackground.sprite = BarBackGroundSprite;

        UpdateValue(barValue);


    }

    void UpdateValue(float val)
    {

        bar.fillAmount = -(val / 100) + 1f;

        txtTitle.text = Title + " " + val + "%";

        if (Alert >= val)
        {
            barBackground.color = BarAlertColor;
        }
        else
        {
            barBackground.color = BarBackGroundColor;
        }

    }


    private void Update()
    {
        if (Application.isPlaying) return;
        UpdateValue(50);
        txtTitle.color = TitleColor;
        txtTitle.font = TitleFont;
        Mask.color = MaskColor;
        bar.color = BarColor;
        barBackground.color = BarBackGroundColor;
        barBackground.sprite = BarBackGroundSprite;

    }

}
