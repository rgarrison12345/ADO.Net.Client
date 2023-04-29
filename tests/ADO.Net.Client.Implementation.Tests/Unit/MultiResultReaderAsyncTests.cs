using ADO.Net.Client.Tests.Common.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace ADO.Net.Client.Implementation.Tests.Unit
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MultiResultReaderTests
    {   
#if ADVANCE_ASYNC
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("MultiResultReader Async Tests")]
        public async Task WhenDisposeAsync_IsCalled_ShouldCall_ReaderCloseAsync()
        {
            _mockReader.Setup(x => x.CloseAsync());

            MultiResultReader reader = new MultiResultReader(_mockReader.Object, _mockMapper.Object);

            await reader.DisposeAsync();

            _mockReader.Verify(x => x.CloseAsync(), Times.Exactly(1));
            _mockReader.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the close async is called should call reader close asynchronous.
        /// </summary>
        [Test]
        [Category("MultiResultReader Async Tests")]
        public async Task WhenCloseAsync_IsCalled_ShouldCall_ReaderCloseAsync()
        {
            _mockReader.Setup(x => x.CloseAsync());

            MultiResultReader reader = new MultiResultReader(_mockReader.Object, _mockMapper.Object);

            await reader.CloseAsync();

            _mockReader.Verify(x => x.CloseAsync(), Times.Exactly(1));
            _mockReader.VerifyNoOtherCalls();
        }
#endif
        /// <summary>
        /// Whens the read object is called should call reader read asynchronous.
        /// </summary>
        [Test]
        [Category("MultiResultReader Async Tests")]
        public async Task WhenReadObject_IsCalled_ShouldCall_ReaderReadAsyncTrue()
        {
            DateTime dateBirth = _faker.Person.DateOfBirth;
            string firstName = _faker.Person.FirstName;
            string lastName = _faker.Person.LastName;
            PersonModel expectedModel = new PersonModel()
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateBirth
            };
            _mockMapper.Setup(x => x.MapRecord<PersonModel>(It.IsAny<IDataRecord>())).Returns(expectedModel);
            _mockReader.Setup(x => x.ReadAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

            MultiResultReader multiReader = new MultiResultReader(_mockReader.Object, _mockMapper.Object);
            PersonModel returnedModel = await multiReader.ReadObjectAsync<PersonModel>(CancellationToken.None);

            Assert.IsNotNull(returnedModel);
            Assert.AreEqual(returnedModel.DateOfBirth, expectedModel.DateOfBirth);
            Assert.AreEqual(returnedModel.FirstName, expectedModel.FirstName);
            Assert.AreEqual(returnedModel.LastName, expectedModel.LastName);

            //Verify the readers read method was called
            _mockMapper.Verify(x => x.MapRecord<PersonModel>(_mockReader.Object), Times.Exactly(1));
            _mockMapper.VerifyNoOtherCalls();
            _mockReader.Verify(x => x.ReadAsync(CancellationToken.None), Times.Exactly(1));
            _mockReader.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the read object is called should call reader read asynchronous.
        /// </summary>
        [Test]
        [Category("MultiResultReader Async Tests")]
        public async Task WhenReadObjectAsync_IsCalled_ShouldCall_ReaderReadAsyncfalse()
        {
            DateTime dateBirth = _faker.Person.DateOfBirth;
            string firstName = _faker.Person.FirstName;
            string lastName = _faker.Person.LastName;
            PersonModel expectedModel = new PersonModel()
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateBirth
            };
            _mockMapper.Setup(x => x.MapRecord<PersonModel>(It.IsAny<IDataRecord>())).Returns(expectedModel);
            _mockReader.Setup(x => x.ReadAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

            MultiResultReader multiReader = new MultiResultReader(_mockReader.Object, _mockMapper.Object);
            PersonModel returnedModel = await multiReader.ReadObjectAsync<PersonModel>(CancellationToken.None);

            Assert.IsNull(returnedModel);

            //Verify the readers read method was called
            _mockMapper.Verify(x => x.MapRecord<PersonModel>(_mockReader.Object), Times.Never);
            _mockReader.Verify(x => x.ReadAsync(CancellationToken.None), Times.Exactly(1));
            _mockMapper.VerifyNoOtherCalls();
            _mockReader.VerifyNoOtherCalls();
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public void WhenReadObjectAsync_IsCalled_ShouldCall__ItShouldThrowOperationCanceledException()
        {
            //Wrap this in a using to automatically dispose of resources
            using (CancellationTokenSource source = new CancellationTokenSource(0))
            {
                //Make the call
                Assert.ThrowsAsync<OperationCanceledException>(async () => await new MultiResultReader(_mockReader.Object, _mockMapper.Object).ReadObjectAsync<PersonModel>(source.Token));
            }
        }
        /// <summary>
        /// Whens the next result asynchronous is called should call reader next result asynchronous.
        /// </summary>
        [Test]
        [Category("MultiResultReader Async Tests")]
        public async Task WhenNextResultAsync_IsCalled_ShouldCall_ReaderNextResultAsync()
        {
            int delay = _faker.Random.Int(0, 1000);
            bool expected = _faker.Random.Bool();

            _mockReader.Setup(x => x.NextResultAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expected);

            MultiResultReader multiReader = new MultiResultReader(_mockReader.Object, _mockMapper.Object);

            bool returned = await multiReader.MoveToNextResultAsync(CancellationToken.None).ConfigureAwait(false);

            Assert.IsTrue(expected == returned);

            //Verify the readers read method was called
            _mockReader.Verify(x => x.NextResultAsync(CancellationToken.None), Times.Exactly(1));
            _mockReader.VerifyNoOtherCalls();
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("Asynchronous Read Tests")]
        public void WhenNextResultAsync_IsCalled__ItShouldThrowOperationCanceledException()
        {
            //Wrap this in a using to automatically dispose of resources
            using (CancellationTokenSource source = new CancellationTokenSource(0))
            {
                //Make the call
                Assert.ThrowsAsync<OperationCanceledException>(async () => await new MultiResultReader(_mockReader.Object, _mockMapper.Object).MoveToNextResultAsync(source.Token));
            }
        }
    }
}