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
        BattleShipManager BattleShipManager;
        Mock<IBattleShipAllignmentStrategy> mockShipAllignmentStrategy;

        public void Setup()
        {
            mockShipAllignmentStrategy = new Mock<IBattleShipAllignmentStrategy>();
            List<IBattleShipAllignmentStrategy> battleShipAllignmentStrategies = new List<IBattleShipAllignmentStrategy> { mockShipAllignmentStrategy.Object };
            BattleShipManager = new BattleShipManager(battleShipAllignmentStrategies);
        }

        [Test]
        public void CheckIfBoardWillOverFlowWhenShipIsAdded()
        {
            //Arrange 

            // Act

            // Assert
        }

    }
}