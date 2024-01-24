
using DalApi;

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
        private static Lazy<DalList> instance = null; // Singleton with lazy initialization
        private static readonly object padlock = new object(); // for thread-safe to know which thread is currently use the instance
        private DalList() { } // private CTOR for Singleton
        public static Lazy<DalList> Instance // call to create the instance
        {
            get 
            {
                lock (padlock) // check if the theard have padlock (only one thread can have padlock at a time) if so, allowd to create the instance
                {
                    if (instance == null) // if the instance is null (not created yet) create the instance
                    {
                        instance = new Lazy<DalList>(); // create the instance
                    }
                    return instance; // if the instance is not null (created) return the instance
                }
            }
        }

        public IEngineer Engineer => new EngineerImplementation();

        public ITask Task =>  new TaskImplementation();

        public IDependence Dependence =>  new DependenceImplementation();

   

    }
}
