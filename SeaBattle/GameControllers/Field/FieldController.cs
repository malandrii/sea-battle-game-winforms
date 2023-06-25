using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SeaBattle
{
    public static class FieldController
    {
        public const int NextIndex = 1;

        public static HashSet<Point> GetCoordinatesAround(int x, int y)
        {
            Point point = new Point(x, y);
            int[] cartesianSet = { -1, 0, 1 };
            return cartesianSet
                .SelectMany(dx => cartesianSet.Select(dy => new Point(point.X + dx, point.Y + dy)))
                .Where(selectedPoint => !point.Equals(selectedPoint))
                .Where(selectedPoint => Field.CoordinatesInside(selectedPoint.X, selectedPoint.Y))
                .ToHashSet();
        }

        public static Point[] GetCrossCoordiantes()
        {
            const int crossCoordinatesCount = 4;
            Point crossRight = new Point(1, 0),
                     crossUp = new Point(0, 1),
                   crossLeft = new Point(-1, 0),
                   crossDown = new Point(0, -1);
            return new Point[crossCoordinatesCount] { crossRight, crossUp, crossLeft, crossDown };
        }

        public static void ColorShips(Field field, bool humanField)
        {
            foreach (ShipButton shipPart in field)
            {
                if (!shipPart.IsShipPart) continue;
                if (humanField) HumanPlayerShipController.ColorShipPart(shipPart);
                else FormButtonController.SetButtonColorToShipHit(shipPart);
            }
        }

        public static void UnableRegion(HashSet<ShipButton> region)
        {
            foreach (var shipButton in region)
                shipButton.Enabled = false;
        }
    }
}