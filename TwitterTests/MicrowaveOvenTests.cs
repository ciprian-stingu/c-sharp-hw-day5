using Moq;
using System;
using Xunit;

namespace TwitterTests
{
    using _6.Twitter.Interfaces;
    using _6.Twitter.Models;

    public class MicrowaveOvenTests
    {
        [Fact]
        public void Test_SendTweetToServerShouldSendTheMessageToItsServer()
        {
            //prepare
            var writerMock = new Mock<IWriter>();
            var repoMock = new Mock<ITweetRepository>();
            var message = string.Empty;
            repoMock.Setup(o => o.SaveTweet(It.IsAny<string>())).Callback((string mess) => message = mess);
            var classUnderTest = new MicrowaveOven(writerMock.Object, repoMock.Object);

            //act
            classUnderTest.SendTweetToServer("My message!");

            //check
            repoMock.Verify(o => o.SaveTweet("My message!"), Times.Exactly(1));
            Assert.Equal("My message!", message);
        }

        [Fact]
        public void Test_WriteTweetShouldCallItsWriterWithTheTweetsMessage()
        {
            //prepare
            var writerMock = new Mock<IWriter>();
            var message = string.Empty;
            writerMock.Setup(o => o.WriteLine(It.IsAny<string>())).Callback((string mess) => message = mess);

            var repoMock = new Mock<ITweetRepository>();
            var classUnderTest = new MicrowaveOven(writerMock.Object, repoMock.Object);

            //act
            classUnderTest.WriteTweet("My message!");

            //check
            writerMock.Verify(o => o.WriteLine(It.IsAny<string>()), Times.Exactly(1));
            Assert.Equal("My message!", message);
        }
    }
}
