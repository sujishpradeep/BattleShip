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
            _fixture.Customize<Cell>(composer => composer.With(cell => cell.ColumnID, 5).With(Cell => Cell.RowID, 5));
            _fixture.Customize<Board>(composer => composer.With(board => board.MaxRows, 10).With(Board => Board.MaxNumberOfShips, 5));


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
        public void PlaceBattleShip_Returns_Error_If_Board_OverFlows()
        {
            // Arrage
            var boardID = _fixture.Create<string>();
            var battleShipDTO = _fixture.Create<BattleShipDTO>();
            var boardState = _fixture.Create<BoardState>();

            var battleShipType = (BattleShipType)battleShipDTO.BattleShipType;
            var battleShipAllignment = (BattleShipAllignment)battleShipDTO.BattleShipAllignment;
            var startingCell = battleShipDTO.StartingCell;

            mockBoardDataProcessing.Setup(b => b.GetState(boardID)).Returns(boardState);
            mockBattleShipManager.Setup(s => s.WillBoardOverFlowIfShipPlaced(boardState.Board, battleShipType, battleShipAllignment, startingCell)).Returns(true);

            // Act
            var PlaceBattleShipResult = BoardManager.PlaceBattleShip(boardID, battleShipDTO);

            // Assert
            // Assert.IsTrue(PlaceBattleShipResult.IsError);
            Assert.AreEqual("Board cells will overflow if the BattleShip is placed", PlaceBattleShipResult.ErrorMessage);

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
            mockBattleShipManager.Setup(s => s.WillBoardOverFlowIfShipPlaced(boardState.Board, battleShipType, battleShipAllignment, startingCell)).Returns(false);

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

            //Add 1 battleShip to board
            var battleShips = new List<BattleShip>();
            var battleShip = _fixture.Create<BattleShip>();
            battleShips.Add(battleShip);

            //Build board state with 1 battleship
            var boardState = _fixture.Build<BoardState>()
                                      .With(b => b.BattleShips, battleShips)
                                     .Create();


            //set max number of battleships as > number of battleships
            boardState.Board.MaxNumberOfShips = boardState.BattleShips.Count + 1;

            var battleShipType = (BattleShipType)battleShipDTO.BattleShipType;
            var battleShipAllignment = (BattleShipAllignment)battleShipDTO.BattleShipAllignment;
            var startingCell = battleShipDTO.StartingCell;

            mockBoardDataProcessing.Setup(b => b.GetState(boardID)).Returns(boardState);
            mockBattleShipManager.Setup(s => s.WillBoardOverFlowIfShipPlaced(boardState.Board, battleShipType, battleShipAllignment, startingCell)).Returns(false);

            // Act
            var PlaceBattleShipResult = BoardManager.PlaceBattleShip(boardID, battleShipDTO);

            // Assert
            Assert.IsTrue(PlaceBattleShipResult.IsSuccess);
            mockBoardDataProcessing.Verify(m => m.CreateBattleShip(It.IsAny<BattleShip>()),
                                            Times.Once);

        }
        [Test]
        public void Attack_Returns_Error_If_Board_Is_Not_Present()
        {
            // TODO:

        }
        [Test]
        public void Attack_Returns_Error_If_Cell_Is_Not_Valid()
        {
            // TODO:

        }
        [Test]
        public void Attack_Returns_Error_If_CellAlreadyAttacked()
        {
            // TODO:

        }
        [Test]
        public void Attack_Returns_Hit_If_Target_Cell_Has_Ship()
        {
            var boardID = _fixture.Create<string>();
            var targetCell = _fixture.Build<Cell>()
                                     .With(c => c.ColumnID, 1)
                                     .With(c => c.RowID, 1)
                                     .Create();

            var boardState = _fixture.Create<BoardState>();

            //Place a battleShip in target cell
            var battleShip = new BattleShip();
            battleShip.CellsOccupied.Add(targetCell);
            boardState.BattleShips.Add(battleShip);

            // Remove target Cells from already hit and missed cells 
            boardState.HitCells = boardState.HitCells.Where(c => c != targetCell).ToList();
            boardState.MissedCells = boardState.MissedCells.Where(c => c != targetCell).ToList();


            mockBoardDataProcessing.Setup(b => b.GetState(boardID)).Returns(boardState);

            // Act
            var AttackResponseDTO = BoardManager.Attack(boardID, targetCell);

            // Assert
            Assert.AreEqual(AttackResponse.Hit, AttackResponseDTO.Result.AttackResponse);

        }
        [Test]
        public void Attack_Returns_Miss_If_Target_Cell_Does_Not_Have_Ship()
        {
            // TODO:

        }


    }
}