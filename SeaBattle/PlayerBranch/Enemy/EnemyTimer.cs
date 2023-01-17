using System;
using System.Windows.Forms;

namespace SeaBattle
{
    public class EnemyTimer : Timer
    {
        private readonly Enemy _enemy;
        private readonly FieldController _fieldController;
        private readonly MainForm _mainForm;
        private int _timerTicks = 0;

        public EnemyTimer(Enemy enemy, FieldController fieldController, MainForm mainForm)
        {
            const int minimalTimerInterval = 1;
            Interval = minimalTimerInterval;
            Tick += new EventHandler(Timer_Tick);
            _enemy = enemy;
            _fieldController = fieldController;
            _mainForm = mainForm;
        }

        public void ResetTicks()
        {
            _timerTicks = 0;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            const int computerMovesSpeedCount = 4;
            int tickAmountFromSpeed = ((computerMovesSpeedCount - MainForm.NextIndex
                - _mainForm.GetComputerMoveSpeedSelectedIndex())
                * _fieldController.ButtonSize) + MainForm.NextIndex;
            if (_timerTicks == tickAmountFromSpeed)
            {
                _mainForm.SetLabelComputerMoveVisibility(visible: false);
                _enemy.ContinueTheAttack();
                _mainForm.SetComputerMovesToolStripsEnables(enable: true);
            }
            _timerTicks++;
        }
    }
}