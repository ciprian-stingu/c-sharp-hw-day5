namespace DependencyInversionWorkerAfter
{
    public class Manager
    {
        public void Manage(Worker worker)
        {
            worker.Work();
        }
    }
}
