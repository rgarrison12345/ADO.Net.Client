using ADO.Net.Client.Tests.Common.Models;
using Moq;
using NUnit.Framework;
using System;

namespace ADO.Net.Client.Implementation.Tests.Unit
{
    public partial class MultiResultReaderTests
    {
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("MultiResultReader Sync Tests")]
        public void WhenDispose_IsCalled_ShouldCall_ReaderClose()
        {
            _mockReader.Setup(x => x.Close());

            MultiResultReader reader = new MultiResultReader(_mockReader.Object, _mockMapper.Object);

            reader.Dispose();

            _mockReader.Verify(x => x.Close(), Times.Once);
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("MultiResultReader Sync Tests")]
        public void WhenDispose_IsCalledTwice_ShouldCall_ReaderCloseOnce()
        {
            _mockReader.Setup(x => x.Close());

            MultiResultReader reader = new MultiResultReader(_mockReader.Object, _mockMapper.Object);

            reader.Dispose();
            reader.Dispose();

            _mockReader.Verify(x => x.Close(), Times.Once);
            _mockReader.VerifyNoOtherCalls();
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("MultiResultReader Sync Tests")]
        public void WhenClose_IsCalled_ShouldCall_ReaderClose()
        {
            _mockReader.Setup(x => x.Close());

            MultiResultReader reader = new MultiResultReader(_mockReader.Object, _mockMapper.Object);

            reader.Close();

            _mockReader.Verify(x => x.Close(), Times.Once);
            _mockReader.VerifyNoOtherCalls();
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("MultiResultReader Sync Tests")]
        public void WhenReadObject_IsCalled_ShouldCall_ReaderReadFalse()
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
            _mockReader.Setup(x => x.Read()).Returns(false);
            _mockMapper.Setup(x => x.MapRecord<PersonModel>(_mockReader.Object)).Returns(expectedModel);

            MultiResultReader reader = new MultiResultReader(_mockReader.Object, _mockMapper.Object);
            PersonModel returnedModel = reader.ReadObject<PersonModel>();

            Assert.AreEqual(returnedModel, default(PersonModel));

            _mockReader.Verify(x => x.Read(), Times.Once);
            _mockReader.VerifyNoOtherCalls();
            _mockMapper.Verify(x => x.MapRecord<PersonModel>(_mockReader.Object), Times.Never);
            _mockMapper.VerifyNoOtherCalls();
        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("MultiResultReader Sync Tests")]
        public void WhenReadObject_IsCalled_ShouldCall_ReaderReadTrue()
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
            _mockReader.Setup(x => x.Read()).Returns(true);
            _mockMapper.Setup(x => x.MapRecord<PersonModel>(_mockReader.Object)).Returns(expectedModel);

            MultiResultReader reader = new MultiResultReader(_mockReader.Object, _mockMapper.Object);
            PersonModel returnedModel = reader.ReadObject<PersonModel>();

            Assert.IsNotNull(returnedModel);
            Assert.AreEqual(returnedModel.DateOfBirth, expectedModel.DateOfBirth);
            Assert.AreEqual(returnedModel.FirstName, expectedModel.FirstName);
            Assert.AreEqual(returnedModel.LastName, expectedModel.LastName);

            _mockReader.Verify(x => x.Read(), Times.Once);
            _mockReader.VerifyNoOtherCalls();
            _mockMapper.Verify(x => x.MapRecord<PersonModel>(_mockReader.Object), Times.Once);
            _mockMapper.VerifyNoOtherCalls();
        }
        /// <summary>
        /// Whens the next result is called should call reader next result.
        /// </summary>
        [Test]
        [Category("MultiResultReader Sync Tests")]
        public void WhenNextResult_IsCalled_ShouldCall_ReaderNextResult()
        {
            bool expected = _faker.Random.Bool();

            _mockReader.Setup(x => x.NextResult()).Returns(expected);
            
            MultiResultReader reader = new MultiResultReader(_mockReader.Object, _mockMapper.Object);

            bool returned = reader.MoveToNextResult();

            Assert.AreEqual(expected, returned);

            _mockReader.Verify(x => x.NextResult(), Times.Once);
            _mockReader.VerifyNoOtherCalls();
        }
    }
}