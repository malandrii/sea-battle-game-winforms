﻿using System;
using System.Drawing;
using System.Collections.Generic;

namespace SeaBattle
{
    abstract public class Player
    {
        private static readonly Random _random = new Random();
        private int _shipPartsAlive = 20;
        protected MainForm _mainForm;
        protected FieldController _fieldController;
        protected int _fieldSize;
        protected int _markingOffset;

        public Player(MainForm mainForm)
        {
            _mainForm = mainForm;
            _fieldController = new FieldController(_mainForm);
            _fieldSize = FieldController.FieldSize;
        }

        public int ShipPartsAlive { get => _shipPartsAlive; protected set { if (value >= 0) _shipPartsAlive = value; } }

        public ShipButton[,] Field { get; set; }

        public virtual void DeclareField()
        {
            _fieldController.CreateField(Field, _markingOffset);
        }

        public void TakeDamage()
        {
            ShipPartsAlive--;
        }

        public void MakeCoordinatesRandom(ref int x, ref int y)
        {
            x = _random.Next(0, _fieldSize);
            y = _random.Next(0, _fieldSize);
        }

        public void SpawnRandomShips()
        {
            for (int specificSizeShipSpawnAmount = FieldController.ShipSizesAmount;
                specificSizeShipSpawnAmount > 0; specificSizeShipSpawnAmount--)
            {
                for (int shipSize = 1; shipSize <= specificSizeShipSpawnAmount; shipSize++)
                {
                    SpawnRandomShip(shipSize);
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
            ref int coordinate = ref isHorizontal ? ref x : ref y;
            coordinate = Add ? ++coordinate : --coordinate;
        }

        private void SpawnRandomShip(int size)
        {
            int x = 0, y = 0;
            bool fiftyFiftyChance = _random.Next(1, 3) == 1,
                 isHorizontal = fiftyFiftyChance;
            MakeCoordinatesRandom(ref x, ref y);
            List<Point> shipCoordinates = GetShipCoordinates(size, x, y, isHorizontal, randomShip: true);
            if (CanMakeShip(size, shipCoordinates))
                DeclareShip(shipCoordinates);
            else
                SpawnRandomShip(size);
        }

        private bool CanMakeShip(int size, List<Point> shipCoordinates)
        {
            bool shipButtonsFree = true, sameCoordinates = false;
            for (int i = 0; i < size; i++)
            {
                if (Field[shipCoordinates[i].X, shipCoordinates[i].Y].FreeForShipCreation) continue;
                shipButtonsFree = false;
            }
            CheckForSameLocatedShips(ref sameCoordinates, size, shipCoordinates);
            return shipButtonsFree && !sameCoordinates;
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
            ShiftCoordinates(isHorizontal, randomShip ? insideField : _mainForm.ChosenShipIsHorizontal, ref x, ref y);
        }

        protected Ship DeclareShip(List<Point> shipCoordinates)
        {
            int size = shipCoordinates.Count;
            var shipParts = new List<ShipButton>();
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