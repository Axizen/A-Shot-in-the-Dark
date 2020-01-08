using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloatingScore : MonoBehaviour {

   public Animator anim;
    private Text scoreText;

    void OnEnable()
    {
        AnimatorClipInfo[] clipinfo = anim.GetCurrentAnimatorClipInfo(0);
        Destroy(this.gameObject, clipinfo[0].clip.length);
        scoreText = anim.GetComponent<Text>();
    }

    public void SetText(string text)
    {
        scoreText.text = text;
        //scoreText.color = new Color(254.0f / 255.0f, 152.0f / 255.0f, 203.0f / 255.0f);
    }
}
