using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LinePoint : MonoBehaviour
{
    public bool IsGrabbed { get; set; } = false;

    public bool isStatic = false;
    public List<LinePoint> sasiedzi;
    public LinePoint nextSasiad;
    public List<float> odleglosciRownowagowe;

    private Vector3 predkosc = Vector3.zero;
    private Vector3 sila = Vector3.zero;
    private Vector3 grawitacja = new Vector3(0.0f, -1f);
    private float sprezystosc = 15f;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Interactable interactable;

    private void Update()
    {
        if (IsGrabbed)
            this.transform.position = interactable.attachedToHand.transform.position;
    }

    public void DodajSasiada(LinePoint lp, bool isNext)
    {
        if (isNext) nextSasiad = lp;
        sasiedzi.Add(lp);
        odleglosciRownowagowe.Add((lp.transform.localPosition - this.transform.localPosition).magnitude);
    }

    public void LiczSile()
    {
        sila = Vector3.zero;
        for (int i = 0; i < sasiedzi.Count; i++)
        {
            Vector3 wektorRozsuniecia = sasiedzi[i].transform.localPosition - this.transform.localPosition;
            var rozciagniecie = wektorRozsuniecia.magnitude - odleglosciRownowagowe[i];

            if (rozciagniecie > 0)
            {
                var kierunekRozsuniecia = wektorRozsuniecia.normalized;
                sila += sprezystosc * rozciagniecie * kierunekRozsuniecia;
            }
        }

        sila += grawitacja;
        //Tlumienie
        sila -= predkosc;
    }

    public void SymplektEuler(float delta)
    {
        predkosc += sila  * delta;
        transform.localPosition += (predkosc * delta);
    }

    public void SetLineRenderer()
    {
        if (nextSasiad == null)
        {
            lineRenderer.enabled = false;
            return;
        }

        lineRenderer.SetPositions(new Vector3[] { this.transform.position, nextSasiad.transform.position });
    }
}
