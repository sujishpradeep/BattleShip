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
    public class VerticalAllignmentStrategyTests
    {
        VerticalAllignmentStrategy VerticalAllignmentStrategy;
        IFixture _fixture;

        [SetUp]
        public void Setup()
        {
            VerticalAllignmentStrategy = new VerticalAllignmentStrategy();
            _fixture = new Fixture();
            _fixture.Customize<Cell>(composer => composer.With(cell => cell.ColumnID, 5).With(Cell => Cell.RowID, 5));

        }

        [Test]
        public void AddBattleShipToCells_Adds_Cells_Vertically()
        {

            //Arrange 

            var battleShip = _fixture.Build<BattleShip>()
                                      .With(b => b.BattleShipType, BattleShipType.PatrolBat)
                                      .With(b => b.CellsOccupied, new List<Cell>())
                                      .Create();


            var startingCell = new Cell(1, 1);


            var actual = VerticalAllignmentStrategy.AddBattleShipToCells(battleShip, startingCell);

            var expected = new List<Cell> { new Cell(1, 1), new Cell(2, 1), new Cell(3, 1), new Cell(4, 1), new Cell(5, 1) };

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void CheckIfBoardWillOverFlowWhenShipIsAdded_Returns_True_If_Board_Will_Overflow()
        {

            //Arrange 
            var board = _fixture.Build<Board>()
                                      .With(b => b.MaxRows, 10)
                                     .Create();


            var startingCell = new Cell(6, 1);
            var actual = VerticalAllignmentStrategy.CheckIfBoardWillOverFlowWhenShipIsAdded(board, BattleShipType.PatrolBat, startingCell);

            Assert.IsTrue(actual);
        }

        [Test]
        public void CheckIfBoardWillOverFlowWhenShipIsAdded_Returns_False_If_Board_WillNot_Overflow()
        {

            //Arrange 
            var board = _fixture.Build<Board>()
                                      .With(b => b.MaxRows, 10)
                                     .Create();


            var startingCell = new Cell(3, 1);
            var actual = VerticalAllignmentStrategy.CheckIfBoardWillOverFlowWhenShipIsAdded(board, BattleShipType.PatrolBat, startingCell);

            Assert.False(actual);
        }




    }
}