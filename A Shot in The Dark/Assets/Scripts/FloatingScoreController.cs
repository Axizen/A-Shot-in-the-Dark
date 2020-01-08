using UnityEngine;
using System.Collections;

public class FloatingScoreController : MonoBehaviour {

    private static FloatingScore popupScore;
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
        if (!popupScore)
            popupScore = Resources.Load<FloatingScore>("Prefabs/PopUpScoreParent");
    }

	public static void CreateFloatingScoreText(string text, Transform location)
    {
        FloatingScore instance = Instantiate(popupScore);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + Random.Range(-.5f,.5f), location.position.y + Random.Range(-.5f, .5f)));
        //Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2(location.position.x, location.position.y));
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPosition;
        instance.SetText(text);
    }
}
