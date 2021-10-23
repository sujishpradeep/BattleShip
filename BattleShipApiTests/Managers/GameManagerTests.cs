using NUnit.Framework;
using Moq;
using BattleShipApi.Managers;
using BattleShipApi.DataProcessing;

namespace BattleShipApiTests.Managers
{
    public class GameManagerTests
    {
        Mock<IBoardDataProcessing> MockBoardDataProcessing;
        GameManager GameManager;

        [SetUp]
        public void Setup()
        {
            MockBoardDataProcessing = new Mock<IBoardDataProcessing>();
            GameManager = new GameManager(MockBoardDataProcessing.Object);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

    }
}