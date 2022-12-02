namespace Subs.Data
{
    public class Settings
    {
        public static string ConnectionString;
        public static string EMailHostIp;
        public static string ProductFilter;
        public static CustomerDoc2.CustomerDataTable gCustomerTable = new CustomerDoc2.CustomerDataTable();
        public static int CurrentCustomerId = 0;
        public static int Authority = 0;
        //public static int Authority4;
        //public static int Authority3;
        //public static int Authority2;
        public static string DirectoryPath; // The Directory used for batch processing
        public static string Version;
    }

}
