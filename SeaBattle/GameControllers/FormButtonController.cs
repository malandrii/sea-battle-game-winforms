using System.Drawing;
using System.Windows.Forms;

namespace SeaBattle
{
    public class FormButtonController
    {
        private readonly Form _form;
        private static readonly Color ShipHit = Color.Red;
        private static readonly Color EnemyMarkedHit = Color.DarkRed;
        public const int ButtonSize = 30;

        public FormButtonController(Form form) => _form = form;

        public static void ButtonColorToStandart(Button button)
        {
            button.BackColor = SystemColors.ButtonFace;
            button.UseVisualStyleBackColor = true;
        }

        public static void SetShipButtonFontStyle(ShipButton shipButton)
        {
            int fontSize = shipButton.Text == "." ? 15 : 8;
            FontStyle fontStyle = shipButton.Text == "." ? FontStyle.Bold : FontStyle.Regular;

            shipButton.Font = new Font(shipButton.Font.Name, fontSize, fontStyle);
        }

        public static void SetButtonColorToShipHit(ShipButton shipButton)
        {
            SetShipButtonColor(shipButton, ShipHit);
        }

        public static void SetButtonColorToEnemyMarkedHit(ShipButton shipButton)
        {
            SetShipButtonColor(shipButton, EnemyMarkedHit);
        }

        public static void SetShipButtonColor(ShipButton shipButton, Color color)
        {
            shipButton.BackColor = color;
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
                Color shipButtonColor = shipButton.ShipFrom.IsDead ? 
                    ShipHit : (mark ? EnemyMarkedHit : HumanPlayerShipController.HumanPlayerShip);
                SetShipButtonColor(shipButton, shipButtonColor);
            }
        }

        public void CreateNewShipButton(Field field, int markingOffset, int indent, int y, int x)
        {
            int xIndent = indent + markingOffset;
            field[x, y] = new ShipButton(x, y)
            {
                Location = new Point(x * ButtonSize + xIndent, y * ButtonSize + indent),
                Size = new Size(ButtonSize, ButtonSize)
            };
            _form.Controls.Add(field[x, y]);
        }
    }
}
