using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SangjiagouCore;

public class SelectSchoolPanel : MonoBehaviour
{
    public GameObject MaskPrefab;
    public GameObject SchoolSelection;

    UIHandler handler;

    School selecting;
    int PlayerID;

    void Start()
    {
        handler = GameObject.FindGameObjectWithTag("UIHandler").GetComponent<UIHandler>();
        selecting = null;
        PlayerID = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().ID;

        var mask = Instantiate(MaskPrefab, transform.parent);
        transform.SetParent(mask.transform);

        handler.HideAllPanels();

        var mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCam.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Focused")) {
            mainCam.GetComponent<Animator>().SetTrigger("Unfocus");
        }

        var l = transform.Find("Scroll View/Viewport/Schools List");
        var unitHeight = SchoolSelection.GetComponent<RectTransform>().rect.height;
        l.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ((unitHeight + 8) * Game.CurrentEntities.Schools.Count) + 8);
        float t = -8;
        foreach (var i in Game.CurrentEntities.Schools) {
            var o = Instantiate(SchoolSelection, l.transform);
            o.GetComponent<Toggle>().group = l.GetComponent<ToggleGroup>();
            o.GetComponent<Toggle>().onValueChanged.AddListener(delegate (bool isOn) {
                selecting = i;
                transform.Find("OK Button").GetComponent<Button>().enabled = true;
                if (isOn) UpdateDescriptions();
            });
            o.GetComponent<RectTransform>().offsetMin = new Vector2(8, t - unitHeight);
            o.GetComponent<RectTransform>().offsetMax = new Vector2(-8, t);
            o.transform.Find("Avatar").GetComponent<Image>().sprite = i.Leader.Avatar;
            o.transform.Find("School Name").GetComponent<Text>().text = i.Name;
            o.transform.Find("Leader Name").GetComponent<Text>().text = i.Leader.FullCourtesyName;
            t -= unitHeight + 4;

            if(i.PlayerID == this.PlayerID) {
                selecting = i;
                o.GetComponent<Toggle>().isOn = true;
            }
        }
        
        if(selecting is null) {
            transform.Find("OK Button").GetComponent<Button>().enabled = false;
        }
    }

    void UpdateDescriptions()
    {
        string text = "";
        foreach (var t in selecting.AllowedPropositionTypes) {
            var c = t.GetConstructors()[0];
            var p = new object[c.GetParameters().Length];
            for (int i = 0; i < p.Length; i++) {
                p[i] = null;
            }
            text += (c.Invoke(p) as StateAction).Name + " ";
        }
        transform.Find("Preferred Suggestions Text").GetComponent<Text>().text = text;
        text = ""; 
        foreach (var t in selecting.DisallowedPropositionTypes) {
            var c = t.GetConstructors()[0];
            var p = new object[c.GetParameters().Length];
            for (int i = 0; i < p.Length; i++) {
                p[i] = null;
            }
            text += (c.Invoke(p) as StateAction).Name + " ";
        }
        transform.Find("Not Preferred Suggestions Text").GetComponent<Text>().text = text;
    }

    public void OnOKButtonClick()
    {
        foreach (var i in Game.CurrentEntities.Schools) {
            i.PlayerID = School.AIControl;
        }
        selecting.PlayerID = this.PlayerID;

        var o = GameObject.Find("Player Panel").transform;
        o.Find("Avatar").GetComponent<Image>().sprite = selecting.Leader.Avatar;
        o.Find("Scholar Name").GetComponent<Text>().text = selecting.Leader.FullCourtesyName;
        o.Find("School Name").GetComponent<Text>().text = selecting.Name;

        // 关闭该面板
        if (GameObject.Find("Upper UI Canvas").transform.childCount <= 1) {
            var mainCam = GameObject.FindGameObjectWithTag("MainCamera");
            mainCam.GetComponent<Animator>().SetTrigger("Focus");
        }
        GetComponent<Animator>().SetTrigger("Close");

        handler.ShowPanels(new List<string> { "Player Panel" });
    }

    /// <summary>
    /// 这个函数会在关闭动画的最后调用
    /// </summary>
    public void OnCloseAnimationFinished()
    {
        Destroy(transform.parent.gameObject);
    }
}
