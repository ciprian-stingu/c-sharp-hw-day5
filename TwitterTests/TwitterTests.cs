using Moq;
using System;
using Xunit;


namespace TwitterTests
{
    using _6.Twitter.Interfaces;
    using _6.Twitter.Models;
    public class TwitterTests
    {
        [Fact]
        public void Test_ReceiveMessageShouldInvokeItsClientToWriteTheMessage()
        {
            //prepare
            var clientMock = new Mock<IClient>();
            var message = string.Empty;
            clientMock.Setup(o => o.WriteTweet(It.IsAny<string>())).Callback((string mess) => message = mess);
            //clientMock.Setup(o => o.SendTweetToServer(It.IsAny<string>()))
            var classUnderTest = new Tweet(clientMock.Object);

            //act
            classUnderTest.ReceiveMessage("My other message!");

            //check
            clientMock.Verify(o => o.WriteTweet(It.IsAny<string>()), Times.Exactly(1));
            Assert.Equal("My other message!", message);
        }

        [Fact]
        public void Test_ReceiveMessageShouldInvokeItsClientToSendTheMessageToTheServer()
        {
            //prepare
            var clientMock = new Mock<IClient>();
            var message = string.Empty;
            clientMock.Setup(o => o.SendTweetToServer(It.IsAny<string>())).Callback((string mess) => message = mess);
            var classUnderTest = new Tweet(clientMock.Object);

            //act
            classUnderTest.ReceiveMessage("My other message!");

            //check
            clientMock.Verify(o => o.SendTweetToServer("My other message!"), Times.Exactly(1));
            Assert.Equal("My other message!", message);
        }
    }
}
