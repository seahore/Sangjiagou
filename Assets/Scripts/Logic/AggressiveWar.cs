using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SangjiagouCore
{

    public class AggressiveWar : War
    {
        /// <summary>
        /// ����ս����
        /// </summary>
        public new struct Report
        {
            bool _attackerWon;
            public bool AttackerWon => _attackerWon;
            uint _attackerLoss;
            public uint AttackerLoss => _attackerLoss;
            uint _defenderLoss;
            public uint DefenderLoss => _defenderLoss;

            public Report(bool attackerWon, uint attackerLoss, uint defenderLoss)
            {
                _attackerWon = attackerWon;
                _attackerLoss = attackerLoss;
                _defenderLoss = defenderLoss;
            }
        }
        Town _targetTown;
        /// <summary>
        /// ������ս������ĳǹ�
        /// </summary>
        public Town TargetTown { get => _targetTown; }

        /// <summary>
        /// ����һ���µĲ���ս�����Զ�������ս˫���Զ�ѯ�������Ƿ�μ�ս�������Զ���Ը�����ս��ĳһ���Ĺ��Ҽ������
        /// </summary>
        /// <param name="declarer">��ս��</param>
        /// <param name="declaree">����ս��</param>
        /// <param name="targetTown">������ĳǹ�</param>
        public AggressiveWar(State attacker, State defender, Town targetTown) :
            base(attacker, defender)
        {
            _targetTown = targetTown;
            foreach (var s in _attackers) {
                s.Monarch.IsInvader = true;
            }
        }

        /// <summary>
        /// ���в���ս��ģ�⣬���ظô�ս���Ľ��㱨��
        /// </summary>
        /// <returns>�ô�ս���Ľ��㱨��</returns>
        public new AggressiveWar.Report Settle()
        {
            uint attackerArmy = _initialAttackerArmy;
            uint defenderArmy = _initialDefenderArmy;
            bool toMuchLoss() => attackerArmy < (0.75f * _initialAttackerArmy) || defenderArmy < (0.75f * _initialDefenderArmy);
            while (true) {
                defenderArmy -= (uint)(0.02f * attackerArmy * Random.Range(0.5f, 1.5f) * _attackerEnhancement);
                attackerArmy -= (uint)(0.02f * defenderArmy * Random.Range(0.6f, 1.6f) * _defenderEnhancement);
                // ���ĳһ����ʧ̫�࣬��ôÿһ�ֽ���ս�۵Ļ�����ߵ�0.4
                if (toMuchLoss()) {
                    if (Random.Range(0.0f, 1.0f) < 0.4f) break;
                } else {
                    if (Random.Range(0.0f, 1.0f) < 0.1f) break;
                }
            }
            float attackerWonPossibility;
            if (attackerArmy > defenderArmy) {
                attackerWonPossibility = 1.0f - defenderArmy / (2.0f * attackerArmy);
            } else {
                attackerWonPossibility = attackerArmy / (2.0f * defenderArmy);
            }
            bool attackerWon = Random.Range(0.0f, 1.0f) < attackerWonPossibility;
            return new AggressiveWar.Report(attackerWon, _initialAttackerArmy - attackerArmy, _initialDefenderArmy - defenderArmy);
        }
    }

}