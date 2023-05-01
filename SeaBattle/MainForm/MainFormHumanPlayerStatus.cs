using System.Drawing;

namespace SeaBattle
{
    public class MainFormHumanPlayerStatus
    {
        private readonly MainForm _mainForm;
        private static readonly Color StandartLabelStatusColor = Color.Black;
        public const string StatusText = "Status: ";

        public MainFormHumanPlayerStatus(MainForm mainForm)
        {
            _mainForm = mainForm;
        }

        public static string GetShipPositionPlacementLabel(bool chosenShipIsHorizontal)
        {
            string shipPositionLabel = "Placement: ";
            shipPositionLabel += chosenShipIsHorizontal ? "—" : "|";
            return shipPositionLabel;
        }

        public void SetStandartLabelStatus()
        {
            const string StandartLabelStatusText = "----";
            _mainForm.SetLabelStatus(StandartLabelStatusText, StandartLabelStatusColor);
        }

        public void SetHitEnemyShipStatus()
        {
            _mainForm.SetLabelStatus("Hit", Color.DarkRed);
        }

        public void SetDeadEnemyShipStatus()
        {
            _mainForm.SetLabelStatus("Dead", Color.Red);
        }

        public void SetFinishGameStatus(bool humanWon)
        {
            string gameFinishTitle = "You ";
            gameFinishTitle += humanWon ? "won" : "lost";
            Color gameFinishTitleColor = humanWon ? Color.Green : Color.Red;
            _mainForm.SetLabelStatus(gameFinishTitle, gameFinishTitleColor);
        }
    }
}
