using NUnit.Framework;
using Moq;
using BattleShipApi.Managers;
using BattleShipApi.DataProcessing;
using BattleShipApi.Models;
using BattleShipApi.DTOs;
using BattleShipApi.Constants;
using System;
using AutoFixture;
using System.Collections.Generic;
using System.Linq;
using BattleShipApi.Strategies;

namespace BattleShipApiTests.Strategies.BattleShipAllignment
{
    public class HorizontalAllignmentStrategyTests
    {
        HorizontalAllignmentStrategy HorizontalAllignmentStrategy;
        IFixture _fixture;

        [SetUp]
        public void Setup()
        {
            HorizontalAllignmentStrategy = new HorizontalAllignmentStrategy();
            _fixture = new Fixture();
            _fixture.Customize<Cell>(composer => composer.With(cell => cell.ColumnID, 5).With(Cell => Cell.RowID, 5));

        }

        [Test]
        public void AddBattleShipToCells_Adds_Cells_Horizontally()
        {

            //Arrange 

            var battleShip = _fixture.Build<BattleShip>()
                                      .With(b => b.BattleShipType, BattleShipType.PatrolBat)
                                      .With(b => b.CellsOccupied, new List<Cell>())
                                      .Create();


            var startingCell = new Cell(1, 1);


            var actual = HorizontalAllignmentStrategy.AddBattleShipToCells(battleShip, startingCell);

            var expected = new List<Cell> { new Cell(1, 1), new Cell(1, 2), new Cell(1, 3), new Cell(1, 4), new Cell(1, 5) };

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void CheckIfBoardWillOverFlowWhenShipIsAdded_Returns_True_If_Board_Will_Overflow()
        {

            //Arrange 
            var board = _fixture.Build<Board>()
                                      .With(b => b.MaxRows, 10)
                                     .Create();


            var startingCell = new Cell(1, 6);
            var actual = HorizontalAllignmentStrategy.CheckIfBoardWillOverFlowWhenShipIsAdded(board, BattleShipType.PatrolBat, startingCell);

            Assert.IsTrue(actual);
        }
        [Test]
        public void CheckIfBoardWillOverFlowWhenShipIsAdded_Returns_False_If_Board_WillNot_Overflow()
        {

            //Arrange 
            var board = _fixture.Build<Board>()
                                      .With(b => b.MaxRows, 10)
                                     .Create();


            var startingCell = new Cell(1, 3);
            var actual = HorizontalAllignmentStrategy.CheckIfBoardWillOverFlowWhenShipIsAdded(board, BattleShipType.PatrolBat, startingCell);

            Assert.False(actual);
        }



    }
}