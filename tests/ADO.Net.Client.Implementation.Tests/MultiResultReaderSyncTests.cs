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
using ADO.Net.Client.Tests.Common;
using ADO.Net.Client.Tests.Common.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace ADO.Net.Client.Implementation.Tests
{
    public partial class MultiResultReaderTests
    {
        #region Tests
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
            _mockMapper.Verify(x => x.MapRecord<PersonModel>(_mockReader.Object), Times.Never);
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
            Assert.IsTrue(returnedModel.DateOfBirth == expectedModel.DateOfBirth);
            Assert.IsTrue(returnedModel.FirstName == expectedModel.FirstName);
            Assert.IsTrue(returnedModel.LastName == expectedModel.LastName);

            _mockReader.Verify(x => x.Read(), Times.Once);
            _mockMapper.Verify(x => x.MapRecord<PersonModel>(_mockReader.Object), Times.Once);
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

            Assert.IsTrue(expected == returned);
            _mockReader.Verify(x => x.NextResult(), Times.Once);
        }
        #endregion
    }
}