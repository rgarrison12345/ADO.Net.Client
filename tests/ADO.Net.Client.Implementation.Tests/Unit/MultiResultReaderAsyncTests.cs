#region Licenses
/*MIT License
Copyright(c) 2020
Robert Garrison

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/
#endregion
#region Using Statements
using ADO.Net.Client.Tests.Common.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
#endregion;

namespace ADO.Net.Client.Implementation.Tests.Unit
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MultiResultReaderTests
    {
        #region Tests    
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

            _mockReader.Verify(x => x.CloseAsync(), Times.Once);
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

            _mockReader.Verify(x => x.CloseAsync(), Times.Once);
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
            _mockMapper.Verify(x => x.MapRecord<PersonModel>(_mockReader.Object), Times.Once);
            _mockMapper.VerifyNoOtherCalls();
            _mockReader.Verify(x => x.ReadAsync(CancellationToken.None), Times.Once);
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
            _mockReader.Verify(x => x.ReadAsync(CancellationToken.None), Times.Once);
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
            _mockReader.Verify(x => x.NextResultAsync(CancellationToken.None), Times.Once);
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
        #endregion
    }
}