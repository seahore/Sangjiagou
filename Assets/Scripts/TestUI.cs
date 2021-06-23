using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SangjiagouCore;

public class TestUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSaveButtonClick()
    {
        Game.SaveGame("test.sjg", false);
    }

    public void OnLoadButtonClick()
    {
        Game.LoadGame("test.sjg", false);
    }

    public void OnTriggerWarButtonClick()
    {
        War war = new War(Game.CurrentEntities.States[0], Game.CurrentEntities.States[1]);
        War.Report report = war.Settle();
        Debug.Log($"AttackerWon: {report.AttackerWon}\nAttackers: {war.Attackers} Defenders: { war.Defenders}\nAttackerLoss: {report.AttackerLoss} DefenderLoss: {report.DefenderLoss}");
    }

    public void OnNextTurnButtonClick()
    {
        Game.CurrentEntities.NextTurn();
        var states = Game.CurrentEntities.States;
        string str = $"{Game.CurrentEntities.Year}��{Game.CurrentEntities.Month}��\n";
        foreach (var s in states) {
           str += $"{s.Name} - ������{s.Satisfaction} ���֣�{s.Ceremony}�˿ڣ�{s.Population} ��ʿ��{s.Army} ��ʳ��{s.Food }\n";
        }
        Debug.Log(str);
    }



    public void OnAIButton()
    {
        foreach (var s in Game.CurrentEntities.Schools) {
            s.AIControl();
        }
        return;
    }
}
