using NUnit.Framework;
using Moq;
using BattleShipApi.Managers;
using BattleShipApi.DataProcessing;
using BattleShipApi.Models;
using BattleShipApi.Constants;
using System;

namespace BattleShipApiTests.Managers
{
    public class GameManagerTests
    {
        Mock<IBoardDataProcessing> mockBoardDataProcessing;
        GameManager GameManager;
        const int randomPlayerID = 111111;
        const int randomGameID = 999999;


        [SetUp]
        public void Setup()
        {
            mockBoardDataProcessing = new Mock<IBoardDataProcessing>();
            GameManager = new GameManager(mockBoardDataProcessing.Object);
        }

        [Test]
        public void AddBoard_Returns_Error_If_Board_Exists_For_Game_And_Player()
        {
            // Arrage
            Color colorPreference = Color.Blue;
            mockBoardDataProcessing.Setup(b => b.GetByGameIDAndPlayerID(randomGameID, randomPlayerID)).Returns(new Board());

            // Act
            var BoardResult = GameManager.AddBoard(randomGameID, randomPlayerID, colorPreference);

            // Assert
            Assert.IsTrue(BoardResult.IsError);
            Assert.IsTrue(BoardResult.ErrorMessage.Contains("Board is already created for the player"));
        }

    }
}