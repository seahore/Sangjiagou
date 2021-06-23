using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SangjiagouCore
{

    public class ProposeAction : ScholarAction
    {
        StateAction _proposition;

        public override void Act()
        {
            _place.Controller.ActionQueue.Add(_proposition);
        }



        public ProposeAction(Scholar actor, Town place, StateAction proposition)
        : base(actor, place)
        {
            if (!actor.BelongTo.AllowedPropositionTypes.Contains(_proposition.GetType()))
                Debug.LogWarning($"{actor.BelongTo.Name}��{actor.FullName}����˲�������ѧ�������ĶԲ�: {proposition.GetType().Name}");
            if (!place.IsCapital)
                Debug.LogWarning($"{actor.BelongTo.Name}��{actor.FullName}��{place.Name}����˶Բߣ���{place.Name}���Ǿ���");

            _proposition = proposition;
        }
    }

}
