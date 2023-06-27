using System;
using System.Windows.Forms;

namespace SeaBattle
{
    sealed public class EnemyTimer
    {
        private readonly Timer _timer;
        private readonly Enemy _enemy;
        private readonly MainForm _mainForm;
        private int _timerTicks = 0;

        public EnemyTimer(Enemy enemy, MainForm mainForm)
        {
            const int minInterval = 1;
            _timer = new Timer { Interval = minInterval };
            _timer.Tick += new EventHandler(Timer_Tick);
            _enemy = enemy;
            _mainForm = mainForm;
        }

        public void Start() => _timer.Start();

        public void Stop()
        {
            _timer.Stop();
            _mainForm.SetComputerMovesToolStripsEnables(enable: true);
        }

        public void ResetTicks() => _timerTicks = 0;

        private void Timer_Tick(object sender, EventArgs e)
        {
            const int speedTicksMutiplier = 30;
            int moveSpeedIndex = _enemy.MoveSpeedsAmount - FieldController.NextIndex - _enemy.MoveSpeed,
                ticksAmountToStop = moveSpeedIndex * speedTicksMutiplier + FieldController.NextIndex;

            if (_timerTicks == ticksAmountToStop)
            {
                _mainForm.SetLabelComputerMoveVisibility(visible: false);
                _enemy.ContinueAttack();
            }

            _timerTicks++;
        }
    }
}