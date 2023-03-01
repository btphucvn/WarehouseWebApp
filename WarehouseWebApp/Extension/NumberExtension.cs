using WarehouseWebApp.Models;
using WarehouseWebApp.Variable;

namespace WarehouseWebApp.Extension
{
    public static class NumberExtension
    {
        public static int RoundUpPage(float number)
        {
            float tmpTotalPage = number / CommonVariables.PAGE_SIZE;

            int totalPage = 0;
            if (tmpTotalPage > (int)tmpTotalPage) totalPage = (int)tmpTotalPage + 1;
            else totalPage = (int)tmpTotalPage;
            return totalPage;
        }
    }
}
