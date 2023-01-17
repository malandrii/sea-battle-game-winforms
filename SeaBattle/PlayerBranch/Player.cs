using System;
using System.Drawing;
using System.Collections.Generic;

namespace SeaBattle
{
    public class Player
    {
        private static readonly Random _random = new Random();
        protected MainForm _mainForm;
        protected FieldController _fieldController;
        protected int _fieldSize;
        protected int _markingOffset;
        public int ShipPartsAlive { get; set; } = 20;
        public ShipButton[,] Field { get; set; }

        public Player(MainForm mainForm)
        {
            _mainForm = mainForm;
            _fieldController = new FieldController(_mainForm);
            _fieldSize = FieldController.FieldSize;
        }

        public virtual void DeclareField()
        {
            _fieldController.CreateField(Field, _markingOffset);
        }

        public void MakeCoordinatesRandom(ref int x, ref int y)
        {
            x = _random.Next(0, _fieldSize);
            y = _random.Next(0, _fieldSize);
        }

        public void SpawnRandomShips()
        {
            for (int specificSizeShipSpawnAmount = _mainForm.ShipSizesAmount;
                specificSizeShipSpawnAmount > 0; specificSizeShipSpawnAmount--)
            {
                int shipSize = 1;
                for (int i = 0; i < specificSizeShipSpawnAmount; i++)
                {
                    SpawnRandomShip(shipSize);
                    shipSize++;
                }
            }
        }

        private void CheckForSameLocatedShips(ref bool sameCoordinates, int size,
            List<Point> coordinateCopies)
        {
            const int minimumShipSizeToCheck = 2;
            if (size <= minimumShipSizeToCheck) return;
            for (int specificShipPart = 0; specificShipPart < coordinateCopies.Count - MainForm.NextIndex; specificShipPart++)
            {
                for (int nextShipPart = specificShipPart + MainForm.NextIndex;
                    nextShipPart < coordinateCopies.Count; nextShipPart++)
                {
                    sameCoordinates =
                        coordinateCopies[specificShipPart].X == coordinateCopies[nextShipPart].X
                     && coordinateCopies[specificShipPart].Y == coordinateCopies[nextShipPart].Y;
                    if (sameCoordinates) return;
                }
            }
        }

        public void ShiftCoordinates(bool isHorizontal, bool Add, ref int x, ref int y)
        {
            if (isHorizontal)
                ShiftCoordinate(ref x, Add);
            else
                ShiftCoordinate(ref y, Add);
        }

        private void ShiftCoordinate(ref int coordinate, bool Add)
        {
            coordinate = Add ? ++coordinate : --coordinate;
        }

        private void SpawnRandomShip(int size)
        {
            int x = 0, y = 0;
            bool canMakeShip = true, sameCoordinates = false,
                 fiftyFiftyChance = _random.Next(1, 3) == 1,
                 isHorizontal = fiftyFiftyChance;
            MakeCoordinatesRandom(ref x, ref y);
            List<Point> shipCoordinates = GetShipCoordinates(size, x, y, isHorizontal, randomShip: true);
            for (int i = 0; i < size; i++)
            {
                if (Field[shipCoordinates[i].X, shipCoordinates[i].Y].CanMakeShip()) continue;
                canMakeShip = false;
            }
            CheckForSameLocatedShips(ref sameCoordinates, size, shipCoordinates);
            if (canMakeShip && !sameCoordinates)
                DeclareShip(shipCoordinates);
            else
                SpawnRandomShip(size);
        }

        protected List<Point> GetShipCoordinates(int size, int x, int y, bool isHorizontal, bool randomShip)
        {
            const int oneSquareShipSize = 1;
            List<Point> shipCoordinates = new List<Point> { new Point(x, y) };
            if (size > oneSquareShipSize)
            {
                for (int i = oneSquareShipSize; i < size; i++)
                {
                    int sizeToCheck = _fieldSize - size - oneSquareShipSize,
                        coordinateToCheck = isHorizontal ? x : y;
                    bool insideField = coordinateToCheck < sizeToCheck;
                    ShiftCoordinates(isHorizontal, randomShip ? insideField : _mainForm.ChosenShipIsHorizontal, ref x, ref y);
                    shipCoordinates.Add(new Point(x, y));
                }
            }
            return shipCoordinates;
        }

        protected Ship DeclareShip(List<Point> shipCoordinates)
        {
            int size = shipCoordinates.Count;
            List<ShipButton> shipParts = new List<ShipButton>();
            for (int i = 0; i < size; i++)
                shipParts.Add(Field[shipCoordinates[i].X, shipCoordinates[i].Y]);
            Ship ship = new Ship(shipParts);
            for (int i = 0; i < size; i++)
                MakeShipPart(shipCoordinates[i].X, shipCoordinates[i].Y, ship);
            return ship;
        }

        public bool CheckDeath()
        {
            bool isDead = ShipPartsAlive == 0;
            if (isDead) _mainForm.FinishGame();
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
            foreach (Point point in _fieldController.GetCoordinatesAround(x, y))
                Field[point.X, point.Y].Mark(shipFrom: Field[x, y].ShipFrom);
        }
    }
}