namespace DependencyInversionDatabaseStaticAfter
{
    using StaticData = Data; //dependency inversion :)

    public class Courses
    {
        public void PrintAll()
        {
            var courses = StaticData.CourseNames();

            // print courses
        }

        public void PrintIds()
        {
            var courses = StaticData.CourseIds();

            // print courses
        }

        public void PrintById(int id)
        {
            var courses = StaticData.GetCourseById(id);

            // print courses
        }

        public void Search(string substring)
        {
            var courses = StaticData.Search(substring);

            // print courses
        }
    }
}
