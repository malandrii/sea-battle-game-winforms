using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SeaBattle
{
    public class MainFormButtonController
    {
        private readonly MainForm _mainForm;
        private static readonly Color ShipHit = Color.Red;
        private static readonly Color EnemyMarkedHit = Color.DarkRed;
        private static readonly Color HumanPlayerShip = Color.Blue;

        public MainFormButtonController(MainForm mainForm)
        {
            _mainForm = mainForm;
        }

        public const int ButtonSize = 30;

        public static void ButtonColorToStandart(Button button)
        {
            button.BackColor = SystemColors.ButtonFace;
            button.UseVisualStyleBackColor = true;
        }

        public static void ButtonColorShipHit(ShipButton shipButton)
        {
            shipButton.BackColor = ShipHit;
        }

        public static void ButtonColorEnemyMarkedHit(ShipButton shipButton)
        {
            shipButton.BackColor = EnemyMarkedHit;
        }

        public static void ColorHumanPlayerShip(List<ShipButton> shipParts)
        {
            foreach (ShipButton shipPart in shipParts)
                shipPart.BackColor = HumanPlayerShip;
        }

        public static void RefreshShipButtonMarking(ShipButton shipButton, bool mark)
        {
            if (!shipButton.IsShipPart)
            {
                ButtonColorToStandart(shipButton);
                return;
            }
            if (shipButton.IsShot)
            {
                shipButton.BackColor = shipButton.ShipFrom.IsDead ? ShipHit : (mark ? EnemyMarkedHit : HumanPlayerShip);
            }
        }

        internal void CreateNewShipButton(ShipButton[,] field, int markingOffset, int indent, int y, int x)
        {
            int xIndent = indent + markingOffset;
            field[x, y] = new ShipButton(x, y)
            {
                Location = new Point(x * ButtonSize + xIndent, y * ButtonSize + indent),
                Size = new Size(ButtonSize, ButtonSize)
            };
            _mainForm.Controls.Add(field[x, y]);
        }

        public static void UnableRegion(List<ShipButton> region)
        {
            foreach (var shipButton in region)
                shipButton.Enabled = false;
        }

        public static void UnableField(ShipButton[,] Field)
        {
            foreach (ShipButton button in Field)
                button.Enabled = false;
        }
    }
}
