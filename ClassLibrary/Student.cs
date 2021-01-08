using System;

namespace ClassLibrary
{
    /// <summary>
    /// Class describing the student's test result.
    /// </summary>
    [Serializable]
    public class Student : IComparable<Student>
    {
        /// <summary>
        /// Test score. Private field for <see cref="TestScore"/>
        /// </summary>
        private uint testScore;
        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public Student()
        {
        }
        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="name"><see cref="Student.Name"/></param>
        /// <param name="surname"><see cref="Student.Surname"/></param>
        /// <param name="testName"><see cref="Student.TestName"/></param>
        /// <param name="testScore"><see cref="Student.TestScore"/></param>
        /// <param name="dateOfCompletion"><see cref="Student.DateOfCompletion"/></param>
        public Student(string name, string surname, string testName, uint testScore, DateTime dateOfCompletion)
        {
            Name = name;
            Surname = surname;
            TestName = testName;
            TestScore = testScore;
            DateOfCompletion = dateOfCompletion;
        }
        /// <summary>
        /// Student name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Student surname.
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Test name.
        /// </summary>
        public string TestName { get; set; }
        /// <summary>
        /// Test score.
        /// </summary>
        public uint TestScore
        {
            get => testScore;
            set
            {
                if (value < 101)
                {
                    testScore = value;
                }
                else
                {
                    throw new ArgumentException("The score cannot be higher than 100");
                }

            }
        }
        /// <summary>
        /// Test completion date.
        /// </summary>
        public DateTime DateOfCompletion { get; set; }
        /// <inheritdoc cref="IComparable{T}.CompareTo(T)"/>
        public int CompareTo(Student other)
        {
            return TestScore.CompareTo(other.TestScore);
        }
        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return String.Format("Test score: {3}, Student name: {0} {1}, Test name: {2}, Date of completion: {4}", Name, Surname, TestName, TestScore, DateOfCompletion.ToString("dd.MM.yy HH:mm"));
        }
        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals(object obj)
        {
            return obj is Student student &&
                   Name == student.Name &&
                   Surname == student.Surname &&
                   TestName == student.TestName &&
                   TestScore == student.TestScore &&
                   DateOfCompletion == student.DateOfCompletion;
        }
        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Name, TestName, TestScore, DateOfCompletion);
        }
    }
}
