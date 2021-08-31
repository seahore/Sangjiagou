using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SangjiagouCore;

[RequireComponent(typeof(CanvasRenderer))]
public class ScholarSelection : MonoBehaviour, ITooltipDisplayable
{
    Scholar scholar;

    void Start()
    {
        var playerSchool = Game.CurrentEntities.GetPlayerSchool(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().ID);
        scholar = playerSchool.FindScholarByName(transform.Find("Name").GetComponent<Text>().text);
    }

    public string TooltipContent {
        get {
            string ret = "";

            ret += "��<b>" + scholar.Location.Controller.Name + "</b>��";
            if (scholar.Location.IsCapital) {
                ret += "����";
            }
            ret += "<b>" + scholar.Location.Name;
            if (scholar.Location.IsCapital) {
                ret += "��";
            }
            ret += "</b>��\n";

            var action = scholar.Action;
            ret += action is null ? "Ŀǰ������" : action.ToString();

            return ret;
        }
    }

}