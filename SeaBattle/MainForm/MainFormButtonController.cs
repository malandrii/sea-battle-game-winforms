﻿using System.Drawing;
using System.Windows.Forms;

namespace SeaBattle
{
    public class MainFormButtonController
    {
        private readonly MainForm _mainForm;
        private static readonly Color ShipHit = Color.Red;
        private static readonly Color EnemyMarkedHit = Color.DarkRed;
        public const int ButtonSize = 30;

        public MainFormButtonController(MainForm mainForm)
        {
            _mainForm = mainForm;
        }

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

        public static void ButtonColorShipHit(ShipButton shipButton)
        {
            SetShipButtonColor(shipButton, ShipHit);
        }

        public static void ButtonColorEnemyMarkedHit(ShipButton shipButton)
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

        public void CreateNewShipButton(ShipButton[,] field, int markingOffset, int indent, int y, int x)
        {
            int xIndent = indent + markingOffset;
            field[x, y] = new ShipButton(x, y)
            {
                Location = new Point(x * ButtonSize + xIndent, y * ButtonSize + indent),
                Size = new Size(ButtonSize, ButtonSize)
            };
            _mainForm.Controls.Add(field[x, y]);
        }
    }
}