using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    //public LinePoint przeciaganyWezel = null;
    //public Vector2 mousePos;

    [SerializeField] private Transform lineParent;
    [SerializeField] private LinePoint linePointPrefab;

    private LinePoint[] allLinePoints;

    private void Start()
    {
        TworzLine(Vector3.zero, .1f, 45);
    }

    private void Update()
    {
        //if (przeciaganyWezel != null)
        //{
        //    przeciaganyWezel.GetComponent<RectTransform>().anchoredPosition = mousePos;
        //    przeciaganyWezel = null;
        //}

        //if(Input.GetKeyDown(KeyCode.F5))
        PhysicsProcess(0.1f);

    }

    private void TworzLine(Vector3 poczatekLiny, float odstep, int iloscWezlow)
    {
        LinePoint poprzedniPunkt = null;

        Debug.LogError(linePointPrefab.transform.lossyScale.y);
        float linePointHeight = linePointPrefab.transform.lossyScale.y;
        for (int i = 0; i < iloscWezlow; i++)
        {
            var linePoint = Instantiate(linePointPrefab, lineParent);
            linePoint.transform.localPosition = new Vector3(poczatekLiny.x, poczatekLiny.y - (i * (linePointHeight + odstep)));

            if (i > 0)
            {
                linePoint.DodajSasiada(poprzedniPunkt, false);
                poprzedniPunkt.DodajSasiada(linePoint, true);
            }
            else
                linePoint.isStatic = true;

            poprzedniPunkt = linePoint;
        }

        if (allLinePoints == null) allLinePoints = lineParent.GetComponentsInChildren<LinePoint>();
        foreach (var item in allLinePoints) item.SetLineRenderer();
    }

    private void PhysicsProcess(float delta)
    {
        if (allLinePoints == null) allLinePoints = lineParent.GetComponentsInChildren<LinePoint>();

        foreach (var item in allLinePoints)
        {
            if (!item.isStatic && !item.isGrabbed) item.LiczSile();
        }

        foreach (var item in allLinePoints)
        {
            if (!item.isStatic && !item.isGrabbed) item.SymplektEuler(delta);
        }

        foreach (var item in allLinePoints) item.SetLineRenderer();
    }
}
