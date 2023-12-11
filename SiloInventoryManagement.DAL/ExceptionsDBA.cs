using SiloInventoryManagement.Model;

namespace SiloInventoryManagement.DAL
{
    public class ExceptionsDBA
    {
        public void Insert(string message, string stackTrace)
        {
            if (stackTrace == null)
                stackTrace = "";

            using (SiloInventoryEntityDataModel context = new SiloInventoryEntityDataModel())
            {
                context.up_Exceptions_Insert(message, stackTrace);
            }
        }
    }
}
