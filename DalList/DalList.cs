
using DalApi;
using System.Data.SqlTypes;

namespace Dal
{
    sealed internal class DalList : IDal
    {

        /*
         * We implemented the class using thread-safe and lazy initialization.
         * Thread-safe ensures that there will not be a situation where several processes will try to
         * create an instance of the class, and a lack of synchronization will 
         * occur (as mentioned, only one instance will be created in the Singleton class).
         * Lazy initialization will prevent the compiler from compiling the instance creation
         * command unnecessarily, but only at the moment the instance is created.
        */

        // *** we can use this code for DalList and DalXml becouse they will never be used together *** //

        private static Lazy<IDal> lazyInstance = new Lazy<IDal>(() => new DalList(), LazyThreadSafetyMode.ExecutionAndPublication); // Singleton with lazy initialization
        // This line works as the previous code we wrote, it has a lock and a check if the instance is null, but it is more efficient and shorter.
        // We kept the previous code just to show how it works.
        public static IDal Instance => lazyInstance.Value; // call to create the instance. only now the instance is created, and not when the program is compiled.
        private DalList() { } // private CTOR for Singleton


        //private static readonly object padlock = new object(); // for thread-safe to know which thread is currently use the instance
        //public static IDal Instance // call to create the instance
        //{
        //    get
        //    {
        //        lock (padlock) // check if the theard have padlock (only one thread can have padlock at a time) if so, allowd to create the instance
        //        {
        //            if (instance == null) // if the instance is null (not created yet) create the instance
        //            {
        //                instance = new DalXml(); // create the instance
        //            }
        //            return instance; // if the instance is not null (created) return the instance
        //        }
        //    }
        //}




        public IEngineer Engineer => new EngineerImplementation();

        public ITask Task =>  new TaskImplementation();

        public IDependence Dependence =>  new DependenceImplementation();

        public ILooz looz => throw new NotImplementedException();
    }
}
