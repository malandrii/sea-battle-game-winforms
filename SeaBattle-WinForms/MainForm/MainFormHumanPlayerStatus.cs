using System.Drawing;

namespace SeaBattle
{
    public class MainFormHumanPlayerStatus
    {
        private readonly MainForm _mainForm;
        private static readonly Color StandartLabelStatus = Color.Black;
        private static readonly Color HumanPlayerWin = Color.Green;
        private static readonly Color EnemyWin = Color.Red;
        public const string StatusText = "Status: ";

        public MainFormHumanPlayerStatus(MainForm mainForm) => _mainForm = mainForm;

        public static string GetShipPositionPlacementLabel(bool chosenShipIsHorizontal)
        {
            string shipPositionLabel = "Placement: ";
            shipPositionLabel += chosenShipIsHorizontal ? "—" : "|";
            return shipPositionLabel;
        }

        public void SetStandartLabelStatus()
        {
            const string StandartLabelStatusText = "----";
            _mainForm.SetLabelStatus(StandartLabelStatusText, StandartLabelStatus);
        }

        public void SetHitEnemyShipStatus() => _mainForm.SetLabelStatus("Hit", Color.DarkRed);

        public void SetDeadEnemyShipStatus() => _mainForm.SetLabelStatus("Dead", Color.Red);

        public void SetFinishGameStatus(bool humanWon)
        {
            string gameFinishTitle = "You ";
            gameFinishTitle += humanWon ? "won" : "lost";
            Color gameFinishTitleColor = humanWon ? HumanPlayerWin : EnemyWin;
            _mainForm.SetLabelStatus(gameFinishTitle, gameFinishTitleColor);
        }
    }
}
