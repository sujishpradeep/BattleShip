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
            var board = _fixture.Create<Board>();
            var battleShipType = _fixture.Create<BattleShipType>();
            var startingCell = _fixture.Create<Cell>();
            var battleShipAllignmentWithKnownStrategy = BattleShipAllignment.Horizontal;
            mockShipAllignmentStrategy.Setup(s => s.GetBattleShipAllignment()).Returns(battleShipAllignmentWithKnownStrategy);
            var battleShipAllignmentWithUnknownStrategy = BattleShipAllignment.Vertical;

            // Act
            var actual = battleShipManager.WillBoardOverFlowIfShipPlaced(board, battleShipType, battleShipAllignmentWithUnknownStrategy, startingCell);

            // Assert
            Assert.IsFalse(actual);

        }
        [Test]
        public void WillBoardOverFlowIfShipPlaced_Returns_Result_From_Strategy_When_Strategy_Is_Found()
        {
            //Arrange 
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

    }
}