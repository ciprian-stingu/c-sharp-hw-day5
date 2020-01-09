namespace DependencyInversionDatabaseAfter
{
    public class Courses
    {
        public void PrintAll(Data database)
        {
            var courses = database.CourseNames();

            // print courses
        }

        public void PrintIds(Data database)
        {
            var courses = database.CourseIds();

            // print courses
        }

        public void PrintById(Data database, int id)
        {
            var courses = database.GetCourseById(id);

            // print courses
        }

        public void Search(Data database, string substring)
        {
            var courses = database.Search(substring);

            // print courses
        }
    }
}
