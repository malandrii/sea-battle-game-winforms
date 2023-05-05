using System;
using System.Windows.Forms;

namespace SeaBattle
{
    sealed public class EnemyTimer : Timer
    {
        private readonly Enemy _enemy;
        private readonly MainForm _mainForm;
        private int _timerTicks = 0;

        public EnemyTimer(Enemy enemy, MainForm mainForm)
        {
            const int minimalTimerInterval = 1;
            Interval = minimalTimerInterval;
            Tick += new EventHandler(Timer_Tick);
            _enemy = enemy;
            _mainForm = mainForm;
        }

        public void ResetTicks()
        {
            _timerTicks = 0;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            const int computerMovesSpeedCount = 4;
            int tickAmountFromSpeed = ((computerMovesSpeedCount - FieldController.NextIndex
                - _mainForm.GameController.ComputerMoveSpeed)
                * MainFormButtonController.ButtonSize) + FieldController.NextIndex;
            if (_timerTicks == tickAmountFromSpeed)
            {
                _mainForm.SetLabelComputerMoveVisibility(visible: false);
                _enemy.ContinueAttack();
                _mainForm.SetComputerMovesToolStripsEnables(enable: true);
            }
            _timerTicks++;
        }
    }
}