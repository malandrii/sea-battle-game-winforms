using System.Collections.Generic;
using System.Drawing;

namespace SeaBattle
{
    abstract public class Player
    {
        private int _shipPartsAlive = 20;
        protected MainForm _mainForm;
        protected int _fieldSize;
        protected int _markingOffset;

        public Player(MainForm mainForm)
        {
            _mainForm = mainForm;
            _fieldSize = FieldController.FieldSize;
        }

        public int ShipPartsAlive { get => _shipPartsAlive; protected set { if (value >= 0) _shipPartsAlive = value; } }

        public ShipButton[,] Field { get; set; }

        public virtual void DeclareField()
        {
            FieldController fieldController = new FieldController(_mainForm);
            fieldController.CreateField(Field, _markingOffset);
        }

        public void TakeDamage()
        {
            ShipPartsAlive--;
        }

        public void SpawnRandomShips()
        {
            for (int specificSizeShipSpawnAmount = GameController.ShipSizesAmount;
                specificSizeShipSpawnAmount > 0; specificSizeShipSpawnAmount--)
            {
                for (int shipSize = 1; shipSize <= specificSizeShipSpawnAmount; shipSize++)
                {
                    SpawnRandomShip(shipSize);
                }
            }
        }

        public static void ShiftCoordinates(bool isHorizontal, bool Add, ref int x, ref int y)
        {
            ref int coordinate = ref isHorizontal ? ref x : ref y;
            coordinate = Add ? ++coordinate : --coordinate;
        }

        private void SpawnRandomShip(int size)
        {
            int x = 0, y = 0;
            bool isHorizontal = Randomizer.FiftyFifty;
            Randomizer.MakeCoordinatesRandom(ref x, ref y);
            List<Point> shipCoordinates = GetShipCoordinates(size, x, y, isHorizontal, randomShip: true);
            if (CanMakeShip(shipCoordinates))
                DeclareShip(shipCoordinates);
            else
                SpawnRandomShip(size);
        }

        private bool CanMakeShip(List<Point> shipCoordinates)
        {
            bool shipButtonsFree = true, sameCoordinates = false;
            foreach (Point shipCoordinate in shipCoordinates)
            {
                if (Field[shipCoordinate.X, shipCoordinate.Y].FreeForShipCreation) continue;
                shipButtonsFree = false;
            }
            CheckForSameLocatedShips(ref sameCoordinates, shipCoordinates);
            return shipButtonsFree && !sameCoordinates;
        }

        private void CheckForSameLocatedShips(ref bool sameCoordinates, List<Point> coordinateCopies)
        {
            const int minimumShipSizeToCheck = 2;
            int shipSize = coordinateCopies.Count;
            if (shipSize <= minimumShipSizeToCheck) return;
            for (int specificShipPart = 0; specificShipPart < coordinateCopies.Count - FieldController.NextIndex; specificShipPart++)
            {
                for (int nextShipPart = specificShipPart + FieldController.NextIndex;
                    nextShipPart < coordinateCopies.Count; nextShipPart++)
                {
                    sameCoordinates =
                        coordinateCopies[specificShipPart].X == coordinateCopies[nextShipPart].X
                     && coordinateCopies[specificShipPart].Y == coordinateCopies[nextShipPart].Y;
                    if (sameCoordinates) return;
                }
            }
        }

        protected List<Point> GetShipCoordinates(int size, int x, int y, bool isHorizontal, bool randomShip)
        {
            const int oneSquareShipSize = 1;
            var shipCoordinates = new List<Point> { new Point(x, y) };
            for (int i = oneSquareShipSize; i < size; i++)
            {
                GetShipCoordinate(size, ref x, ref y, isHorizontal, randomShip, oneSquareShipSize);
                shipCoordinates.Add(new Point(x, y));
            }
            return shipCoordinates;
        }

        private void GetShipCoordinate(int size, ref int x, ref int y, bool isHorizontal, bool randomShip, int oneSquareShipSize)
        {
            int sizeToCheck = _fieldSize - size - oneSquareShipSize, coordinateToCheck = isHorizontal ? x : y;
            bool insideField = coordinateToCheck < sizeToCheck;
            ShiftCoordinates(isHorizontal, randomShip ? insideField : _mainForm.PreGameController.ChosenShipIsHorizontal, ref x, ref y);
        }

        protected Ship DeclareShip(List<Point> shipCoordinates)
        {
            var shipParts = new HashSet<ShipButton>();
            foreach (Point shipCoordinate in shipCoordinates)
                shipParts.Add(Field[shipCoordinate.X, shipCoordinate.Y]);
            Ship ship = new Ship(shipParts);
            foreach (Point shipCoordinate in shipCoordinates)
                MakeShipPart(shipCoordinate.X, shipCoordinate.Y, ship);
            return ship;
        }

        public bool CheckDeath()
        {
            bool isDead = ShipPartsAlive == 0;
            if (isDead) _mainForm.GameController.FinishGame();
            return isDead;
        }

        protected void MakeShipPart(int x, int y, Ship ship)
        {
            Field[x, y].IsShipPart = true;
            Field[x, y].ShipFrom = ship;
            OccupyAroundShip(x, y);
        }

        private void OccupyAroundShip(int x, int y)
        {
            foreach (Point point in FieldController.GetCoordinatesAround(x, y))
                Field[point.X, point.Y].Mark(shipFrom: Field[x, y].ShipFrom);
        }
    }
}