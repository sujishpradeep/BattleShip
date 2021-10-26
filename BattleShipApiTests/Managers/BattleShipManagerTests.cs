using BattleShipApi.Managers;

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

namespace BattleShipApiTests.Managers
{
    public class BattleShipManagerTests
    {
        BattleShipManager battleShipManager;
        Mock<IBattleShipAllignmentStrategy> mockShipAllignmentStrategy;
        IFixture _fixture;

        [SetUp]
        public void Setup()
        {
            mockShipAllignmentStrategy = new Mock<IBattleShipAllignmentStrategy>();
            List<IBattleShipAllignmentStrategy> battleShipAllignmentStrategies = new List<IBattleShipAllignmentStrategy> { mockShipAllignmentStrategy.Object };
            battleShipManager = new BattleShipManager(battleShipAllignmentStrategies);
            _fixture = new Fixture();
        }

        [Test]
        public void WillBoardOverFlowIfShipPlaced_Returns_False_When_Strategy_Is_Not_Found()
        {

            //Arrange 
            // BattleShipWith known Strategy - Horizontal
            // Input Strategy - (Unknown) Vertical

            var board = _fixture.Create<Board>();
            var battleShipType = _fixture.Create<BattleShipType>();
            var startingCell = _fixture.Create<Cell>();
            var battleShipAllignmentWithKnownStrategy = BattleShipAllignment.Horizontal;
            mockShipAllignmentStrategy.Setup(s => s.GetBattleShipAllignment()).Returns(battleShipAllignmentWithKnownStrategy);
            var battleShipAllignmentWithUnknownStrategy = BattleShipAllignment.Vertical;

            // Act
            var actual = battleShipManager.WillBoardOverFlowIfShipPlaced(board, battleShipType, battleShipAllignmentWithUnknownStrategy, startingCell);

            // Assert
            // TODO: Should be exception
            Assert.IsTrue(actual);

        }
        [Test]
        public void WillBoardOverFlowIfShipPlaced_Returns_Result_From_Strategy_When_Strategy_Is_Found()
        {
            //Arrange 
            // BattleShipWith known Strategy - Horizontal
            // Input Strategy - Horizontal
            var board = _fixture.Create<Board>();
            var battleShipType = _fixture.Create<BattleShipType>();
            var startingCell = _fixture.Create<Cell>();
            var battleShipAllignmentWithKnownStrategy = BattleShipAllignment.Horizontal;

            mockShipAllignmentStrategy.Setup(s => s.GetBattleShipAllignment()).Returns(battleShipAllignmentWithKnownStrategy);
            var expected = _fixture.Create<bool>();
            mockShipAllignmentStrategy.Setup(s => s.CheckIfBoardWillOverFlowWhenShipIsAdded(board, battleShipType, startingCell)).Returns(expected);

            // Act
            var actual = battleShipManager.WillBoardOverFlowIfShipPlaced(board, battleShipType, battleShipAllignmentWithKnownStrategy, startingCell);

            // Assert
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void WillShipsOverlap_Returns_True_If_Cell_Is_Already_Occupied()
        {

            //Arrange 

            //Add 1 battleShip to board in Row 1, Col 1
            var cells = new List<Cell> { new Cell(1, 1) };
            var battleShips = new List<BattleShip>();
            var battleShip = _fixture.Build<BattleShip>()
                                      .With(b => b.CellsOccupied, cells)
                                    .Create();
            battleShips.Add(battleShip);
            var boardState = _fixture.Build<BoardState>()
                                      .With(b => b.BattleShips, battleShips)
                                     .Create();

            //Create a new battleShip in Row 1, Col 1
            var cellsNew = new List<Cell> { new Cell(1, 1) };
            var battleShipsNew = new List<BattleShip>();
            var battleShipNew = _fixture.Build<BattleShip>()
                                      .With(b => b.CellsOccupied, cellsNew)
                                    .Create();


            // Act 
            var WillShipOverlap = battleShipManager.WillShipsOverlap(boardState, battleShipNew);

            // Assert
            Assert.IsTrue(WillShipOverlap);

        }

        [Test]
        public void WillShipsOverlap_Returns_True_If_Cell_Is_Not_Already_Occupied()
        {

            //Arrange 

            //Add 1 battleShip to board in Row 2, Col 2
            var cells = new List<Cell> { new Cell(2, 2) };
            var battleShips = new List<BattleShip>();
            var battleShip = _fixture.Build<BattleShip>()
                                      .With(b => b.CellsOccupied, cells)
                                    .Create();
            battleShips.Add(battleShip);
            var boardState = _fixture.Build<BoardState>()
                                      .With(b => b.BattleShips, battleShips)
                                     .Create();

            //Create a new battleShip in Row 1, Col 1
            var cellsNew = new List<Cell> { new Cell(1, 1) };
            var battleShipsNew = new List<BattleShip>();
            var battleShipNew = _fixture.Build<BattleShip>()
                                      .With(b => b.CellsOccupied, cellsNew)
                                    .Create();


            // Act 
            var WillShipOverlap = battleShipManager.WillShipsOverlap(boardState, battleShipNew);

            // Assert
            Assert.IsFalse(WillShipOverlap);

        }


    }
}