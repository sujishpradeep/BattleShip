using NUnit.Framework;
using Moq;
using BattleShipApi.Managers;
using BattleShipApi.DataProcessing;
using BattleShipApi.Models;
using BattleShipApi.DTOs;
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
            var addBoardDTO = _fixture.Create<AddBoardDTO>();
            var gameID = addBoardDTO.gameID.Value;
            var playerID = addBoardDTO.playerID.Value;


            mockBoardDataProcessing.Setup(b => b.GetByGameIDAndPlayerID(gameID, playerID)).Returns(new Board());

            // Act
            var BoardResult = BoardManager.Add(addBoardDTO);

            // Assert
            Assert.IsTrue(BoardResult.IsError);
            Assert.IsTrue(BoardResult.ErrorMessage.Contains("Board is already created for the player"));
        }

        [Test]
        public void AddBoard_Returns_Error_If_Color_Is_AlreadySelectedByOpponent()
        {
            // Arrage
            var addBoardDTO = _fixture.Build<AddBoardDTO>()
                                      .With(b => b.colorPreference, Color.Blue)
                                      .Create();
            var opponentPlayerID = _fixture.Create<int>();
            var gameID = addBoardDTO.gameID.Value;
            var mainPlayerID = addBoardDTO.playerID.Value;

            const Color opponentColorPreference = Color.Blue;

            mockBoardDataProcessing.Setup(b => b.GetByGameIDAndPlayerID(gameID, mainPlayerID)).Returns((Board)null);
            var opponentBoard = new Board(gameID, opponentPlayerID, DefaultBoardConfig.MaxRows, DefaultBoardConfig.MaxNumberOfShips, DefaultBoardConfig.CanOverlap, opponentColorPreference);
            mockBoardDataProcessing.Setup(b => b.GetOpponentBoard(gameID, mainPlayerID)).Returns(opponentBoard);

            // Act
            var BoardResult = BoardManager.Add(addBoardDTO);

            // Assert
            Assert.IsTrue(BoardResult.IsError);
            Assert.IsTrue(BoardResult.ErrorMessage.Contains("Opponent has selected the same color"));
        }

        [Test]
        public void AddBoard_Returns_Success_If_There_Are_No_Errors()
        {
            // Arrage
            var addBoardDTO = _fixture.Build<AddBoardDTO>()
                                      .With(b => b.colorPreference, Color.Blue)
                                      .Create();
            var opponentPlayerID = _fixture.Create<int>();
            var gameID = addBoardDTO.gameID.Value;
            var mainPlayerID = addBoardDTO.playerID.Value;

            const Color opponentColorPreference = Color.Red;


            mockBoardDataProcessing.Setup(b => b.GetByGameIDAndPlayerID(gameID, mainPlayerID)).Returns((Board)null);
            var opponentBoard = new Board(gameID, opponentPlayerID, DefaultBoardConfig.MaxRows, DefaultBoardConfig.MaxNumberOfShips, DefaultBoardConfig.CanOverlap, opponentColorPreference);
            mockBoardDataProcessing.Setup(b => b.GetOpponentBoard(gameID, mainPlayerID)).Returns(opponentBoard);

            // Act
            var BoardResult = BoardManager.Add(addBoardDTO);

            // Assert
            Assert.IsTrue(BoardResult.IsSuccess);

            mockBoardDataProcessing.Verify(m => m.Create(It.IsAny<Board>()),
                                                        Times.Once);
        }

    }
}