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
        [Test]
        public void PlaceBattleShip_Returns_Error_If_Board_Is_Not_Present()
        {
            // Arrage
            var boardID = _fixture.Create<string>();
            var battleShipDTO = _fixture.Create<BattleShipDTO>();

            mockBoardDataProcessing.Setup(b => b.GetState(boardID)).Returns((BoardState)null);

            // Act
            var PlaceBattleShipResult = BoardManager.PlaceBattleShip(boardID, battleShipDTO);

            // Assert
            Assert.IsTrue(PlaceBattleShipResult.IsError);
            Assert.IsTrue(PlaceBattleShipResult.ErrorMessage.Contains("Invalid Board ID"));

        }
        [Test]
        public void PlaceBattleShip_Returns_Error_If_Board_OverFlows_When_Ships_Placed()
        {
            // Arrage
            var boardID = _fixture.Create<string>();
            var battleShipDTO = _fixture.Create<BattleShipDTO>();
            var boardState = _fixture.Create<BoardState>();

            var battleShipType = (BattleShipType)battleShipDTO.BattleShipType;
            var battleShipAllignment = (BattleShipAllignment)battleShipDTO.BattleShipAllignment;
            var startingCell = battleShipDTO.StartingCell;

            mockBoardDataProcessing.Setup(b => b.GetState(boardID)).Returns(boardState);
            mockBattleShipManager.Setup(s => s.CheckIfBoardWillOverFlowWhenShipIsAdded(boardState.Board, battleShipType, battleShipAllignment, startingCell)).Returns(true);

            // Act
            var PlaceBattleShipResult = BoardManager.PlaceBattleShip(boardID, battleShipDTO);

            // Assert
            Assert.IsTrue(PlaceBattleShipResult.IsError);
            Assert.IsTrue(PlaceBattleShipResult.ErrorMessage.Contains("Board cells will overflow if the BattleShip is placed"));

        }
        [Test]
        public void PlaceBattleShip_Returns_Error_If_Max_Allowed_Ships_In_Board_Exeeds()
        {
            // Arrage
            var boardID = _fixture.Create<string>();
            var battleShipDTO = _fixture.Create<BattleShipDTO>();
            var boardState = _fixture.Build<BoardState>()
                                     .Create();

            //Add 1 battleShip to board
            var battleShip = _fixture.Create<BattleShip>();
            boardState.BattleShips.Add(battleShip);

            //set max number of battleships as current number of battleships
            boardState.Board.MaxNumberOfShips = boardState.BattleShips.Count;

            var battleShipType = (BattleShipType)battleShipDTO.BattleShipType;
            var battleShipAllignment = (BattleShipAllignment)battleShipDTO.BattleShipAllignment;
            var startingCell = battleShipDTO.StartingCell;

            mockBoardDataProcessing.Setup(b => b.GetState(boardID)).Returns(boardState);
            mockBattleShipManager.Setup(s => s.CheckIfBoardWillOverFlowWhenShipIsAdded(boardState.Board, battleShipType, battleShipAllignment, startingCell)).Returns(false);

            // Act
            var PlaceBattleShipResult = BoardManager.PlaceBattleShip(boardID, battleShipDTO);

            // Assert
            Assert.IsTrue(PlaceBattleShipResult.IsError);
            Assert.IsTrue(PlaceBattleShipResult.ErrorMessage.Contains("Maximum number of ships placed"));

        }
        [Test]
        public void PlaceBattleShip_Returns_Error_If_BattleShips_WillShipsOverlap()
        {
            // TODO:

            //Set up - boardState.Board.canOverLap as false
            //Set up - _battleShipManager.WillShipsOverlap as true

            //Asssert true, Result.IsError
            //Assert Error Message 
        }
        [Test]
        public void PlaceBattleShip_Returns_Success_If_There_Are_No_Errors()
        {
            // Arrage
            var boardID = _fixture.Create<string>();
            var battleShipDTO = _fixture.Create<BattleShipDTO>();
            var boardState = _fixture.Build<BoardState>()
                                     .Create();

            //Add 1 battleShip to board
            var battleShip = _fixture.Create<BattleShip>();
            boardState.BattleShips.Add(battleShip);

            //set max number of battleships as > number of battleships
            boardState.Board.MaxNumberOfShips = boardState.BattleShips.Count + 1;

            var battleShipType = (BattleShipType)battleShipDTO.BattleShipType;
            var battleShipAllignment = (BattleShipAllignment)battleShipDTO.BattleShipAllignment;
            var startingCell = battleShipDTO.StartingCell;

            mockBoardDataProcessing.Setup(b => b.GetState(boardID)).Returns(boardState);
            mockBattleShipManager.Setup(s => s.CheckIfBoardWillOverFlowWhenShipIsAdded(boardState.Board, battleShipType, battleShipAllignment, startingCell)).Returns(false);

            // Act
            var PlaceBattleShipResult = BoardManager.PlaceBattleShip(boardID, battleShipDTO);

            // Assert
            Assert.IsTrue(PlaceBattleShipResult.IsSuccess);
            mockBoardDataProcessing.Verify(m => m.CreateBattleShip(It.IsAny<BattleShip>()),
                                            Times.Once);

        }


    }
}