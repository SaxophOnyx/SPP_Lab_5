namespace Tests
{
    public class Person
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public int Age; //field on purpose


        public Person(string name, string surname, int age)
        {
            Name = name;
            Surname = surname;
            Age = age;
        }

        public Person()
        {
            Name = string.Empty;
            Surname = string.Empty;
            Age = 0;
        }
    }
}
