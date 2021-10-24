using NUnit.Framework;
using Moq;
using BattleShipApi.Managers;
using BattleShipApi.DataProcessing;
using BattleShipApi.Models;
using BattleShipApi.Constants;
using System;
using AutoFixture;

namespace BattleShipApiTests.Managers
{
    public class BoardManagerTests
    {
        Mock<IBoardDataProcessing> mockBoardDataProcessing;
        Mock<IBattleShipManager> mockBattleShipManager;
        BoardManager BoardManager;
        IFixture _fixture;

        [SetUp]
        public void Setup()
        {
            mockBoardDataProcessing = new Mock<IBoardDataProcessing>();
            mockBattleShipManager = new Mock<IBattleShipManager>();
            BoardManager = new BoardManager(mockBoardDataProcessing.Object, mockBattleShipManager.Object);
            _fixture = new Fixture();
        }

        [Test]
        public void AddBoard_Returns_Error_If_Board_Exists_For_Game_And_Player()
        {
            // Arrage
            int GameID = _fixture.Create<int>();
            int MainPlayerID = _fixture.Create<int>();
            int OpponentPlayerID = _fixture.Create<int>();
            const Color MainPlayerColorPreference = Color.Blue;

            mockBoardDataProcessing.Setup(b => b.GetByGameIDAndPlayerID(GameID, MainPlayerID)).Returns(new Board());

            // Act
            var BoardResult = BoardManager.Add(GameID, MainPlayerID, MainPlayerColorPreference);

            // Assert
            Assert.IsTrue(BoardResult.IsError);
            Assert.IsTrue(BoardResult.ErrorMessage.Contains("Board is already created for the player"));
        }

        [Test]
        public void AddBoard_Returns_Error_If_Color_Is_AlreadySelectedByOpponent()
        {
            // Arrage
            int GameID = _fixture.Create<int>();
            int MainPlayerID = _fixture.Create<int>();
            int OpponentPlayerID = _fixture.Create<int>();
            const Color MainPlayerColorPreference = Color.Blue;
            const Color OpponentColorPreference = Color.Blue;

            mockBoardDataProcessing.Setup(b => b.GetByGameIDAndPlayerID(GameID, MainPlayerID)).Returns((Board)null);
            var opponentBoard = new Board(GameID, OpponentPlayerID, DefaultBoardConfig.MaxRows, DefaultBoardConfig.MaxNumberOfShips, DefaultBoardConfig.CanOverlap, OpponentColorPreference);
            mockBoardDataProcessing.Setup(b => b.GetOpponentBoard(GameID, MainPlayerID)).Returns(opponentBoard);

            // Act
            var BoardResult = BoardManager.Add(GameID, MainPlayerID, MainPlayerColorPreference);

            // Assert
            Assert.IsTrue(BoardResult.IsError);
            Assert.IsTrue(BoardResult.ErrorMessage.Contains("Opponent has selected the same color"));
        }

        [Test]
        public void AddBoard_Returns_Success_If_There_Are_No_Errors()
        {
            // Arrage
            int GameID = _fixture.Create<int>();
            int MainPlayerID = _fixture.Create<int>();
            int OpponentPlayerID = _fixture.Create<int>();
            const Color MainPlayerColorPreference = Color.Blue;
            const Color OpponentColorPreference = Color.Red;


            mockBoardDataProcessing.Setup(b => b.GetByGameIDAndPlayerID(GameID, MainPlayerID)).Returns((Board)null);
            var opponentBoard = new Board(GameID, OpponentPlayerID, DefaultBoardConfig.MaxRows, DefaultBoardConfig.MaxNumberOfShips, DefaultBoardConfig.CanOverlap, OpponentColorPreference);
            mockBoardDataProcessing.Setup(b => b.GetOpponentBoard(GameID, MainPlayerID)).Returns(opponentBoard);

            // Act
            var BoardResult = BoardManager.Add(GameID, MainPlayerID, MainPlayerColorPreference);

            // Assert
            Assert.IsTrue(BoardResult.IsSuccess);

            mockBoardDataProcessing.Verify(m => m.Create(It.IsAny<Board>()),
                                                        Times.Once);
        }

    }
}