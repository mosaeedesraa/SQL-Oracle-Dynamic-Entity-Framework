using App.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApplication
{
    public class AsyncCall
    {
        BaseEntity<Test> dc = new BaseEntity<Test>("Data Source=MyOracleDB;Integrated Security=yes;", Databases.Oracle);
        public async Task Call()
        {
            // You can call async
            // 1 -  select * from Test   async
            var rowsAsync = await dc.AllDataAsync();
            var rows = rowsAsync.Data;

            // 2 - Insert , Update , Delete and call Stored Procedure async
            var Deletedrow = await dc.DeleteAsync(1);
            // Error was exist : DeleteDeletedrow.Returened.State = false
            // Error Message :  DeleteDeletedrow.Returened.ErrorMessage

            // 3 - call Insert or Update
            var rowInserted = await dc.InsertAsync(new Test() { Name = "test2", Date = DateTime.Now, Price = 11 });
            var rowID = dc.Find(1).SingleData;
            var rowUpdated = await dc.UpdateAsync(rowID);
            // Or
            var rowUpdated2 = await dc.UpdateAsync(new Test() { ID = 1 , Name = "test2", Date = DateTime.Now, Price = 11 });
        }
    }
}
