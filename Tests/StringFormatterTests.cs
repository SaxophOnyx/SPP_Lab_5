using Xunit;
using Core;
using Core.Exceptions;

namespace Tests
{
    public class StringFormatterTests
    {
        [Fact]
        public void Format_Ok_Simple()
        {
            //arrange
            IStringFormatter formatter = new StringFormatter();
            Person person = new Person()
            {
                Name = "Jonh",
                Surname = "Smith",
                Age = 38
            };

            string template = "{Name} {Surname}, {Age}";
            string expected = $"{person.Name} {person.Surname}, {person.Age}";

            //act
            string actual = formatter.Format(template, person);

            //assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Format_Ok_BracesScreening()
        {
            //arrange
            IStringFormatter formatter = new StringFormatter();
            Person person = new Person()
            {
                Name = "Mary",
                Surname = "Watson",
                Age = 21
            };

            string template = "{{{Name}}} {Surname}, {Age}";
            string expected = $"{{{person.Name}}} {person.Surname}, {person.Age}";

            //act
            string actual = formatter.Format(template, person);

            //assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("{Name} {{Surname}, {Age}")]
        [InlineData("{Name}} {Surname}, {Age}")]
        [InlineData("}{Name} {Surname}, {Age}")]
        [InlineData("{}")]
        public void Format_InvalidFormatException(string template)
        {
            //arrange
            IStringFormatter formatter = new StringFormatter();
            Person person = new Person()
            {
                Name = "Jane",
                Surname = "Willson",
                Age = 29
            };

            //act + assert
            Assert.Throws<InvalidFormatException>(() => formatter.Format(template, person));
        }

        [Theory]
        [InlineData("{Name} {WrongSurname}, {Age}")]
        [InlineData("{Na{me}} {Surname}, {Age}")]
        public void Format_UnknownParamException(string template)
        {
            //arrange
            IStringFormatter formatter = new StringFormatter();
            Person person = new Person()
            {
                Name = "Todd",
                Surname = "Hill",
                Age = 54
            };

            //act + assert
            Assert.Throws<UnknownParamException>(() => formatter.Format(template, person));
        }
    }
}
