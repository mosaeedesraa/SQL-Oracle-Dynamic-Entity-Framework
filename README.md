# SQL-Oracle-Dynamic-Entity-Framework
Call Sql or Oracle Database (Tables and StoredProcedures) Easy 
without write more code and with async functions

This version is update for https://github.com/mosaeedesraa/Dynamic-Entity-Framework

#Example : 
// Oracle or SQl are same code : 
// We have table in database  :Name is  Test and Columns (ID - Name - Price - Date) 
// with the same sort and names
// Table in sql or Oracle : (ID : int , Name : nvarchar(50) , Price : numeric(18,3) , Date : datetime)
// in c# class : (ID : int , Name : string , Price : decimal , Date : DateTime)
// Notice : Table Name = Class Name , Columns Names = Properties Names 
// And Columns sorts = Properities sorts are Neccessary for working fine.

// 1 - Define Entity (table Name is Test - Columns Names and sorts ID-Name-Date-Price)
	BaseEntity<Test> dc = new BaseEntity<Test>("Data Source=MyOracleDB;Integrated Security=yes;"
	,
	Databases.Oracle);
            // Or Sql Database : Databases.Sql
// ----------------------------------------------------------------------------------            
            // 1 -  select * from Test
            List<Test> rows = dc.AllData().Data;  // or
            var rows = dc.AllData().Data;
            // if error is happend you can know by :  dc.AllData().Returened.State = false;
            // Error message : dc.AllData().Returened.ErrorMessage       

            /*=========================================*/
            // 2 - SELECT * FROM Test WHERE Name LIKE '%test%'  
            List<Test> row = dc.Contains(new App.Entities.Models.Func()
            {
                ColumnName = "Name",
                TypeOutParam = App.Entities.Models.DataTypes._string,
                Value = "test"
            }).Data;
            /*=========================================*/
            // 3 - Delete from Test where ID = 1
            // Id you h've errors you can see it by :
            // Deletedrow.Returened.ErrorMessage
            // NO Errors :  Deletedrow.Returened.State == true
            var Deletedrow = dc.Delete(1);
            /*=========================================*/
            // 4 - Execute Stored Procedure : FindTest  this is for SQL
            // Sp has parameter : ID
            // Full Code Sp : 
            /*
                CREATE PROCEDURE [dbo].[FindTest] 
	            @ID int
                AS
                BEGIN
	                SELECT * From Test where ID = @ID		
                END
            */
            var Sp_Row = dc.Select(new App.Entities.Models.StoredProcedure()
            {
                arr = new System.Collections.ArrayList() { 1 },
                Params = new List<string>() { "ID" },
                StoredProcedureName = "FindTest"
            }).Data;
            /*=========================================*/
            // 5 - SELECT * From Test where ID = 1
            Test rowData = dc.Find(1).SingleData;
            /*=========================================*/
            // 6 - select top(1) from Test
            // dc.LastOrDefault() = select top(1) from test order by ID desc
            var FirstOrDefault = dc.FirstOrDefault();
            if (FirstOrDefault.SingleData != null)
            {
                Test record = FirstOrDefault.SingleData;
            }
            /*=========================================*/
            // 7 - select * from Test ==> but Datatable not List<Test>
            DataTable dt = dc.GetListDataTable().dataTable;
            /*=========================================*/
            // 8 - Insert into Test values('test2' , '2016-01-01 11:11:11' , 11);
            var rowInserted = dc.Insert(new Test() { Name = "test2", Date = DateTime.Now, Price = 11 });
            // For update
            var rowID = dc.Find(1).SingleData;
            var rowUpdated = dc.Update(rowID);
            /*=========================================*/
            // 9 - not supported in next version it 'll be supported
            // dc.Trigger(new App.Entities.Models.Trigger() {  })
            //  dc.Transaction(new App.Entities.Models.Trans() {   })
            /*=========================================*/

#Async methods : 
 // You can call async
            // 1 -  select * from Test   async
            var rowsAsync = await dc.AllDataAsync();
            var rows = rowsAsync.Data;

            // 2 - Insert , Update , Delete and call Stored Procedure async
            var Deletedrow = await dc.DeleteAsync(1);
            // Error was exist : DeleteDeletedrow.Returened.State = false
            // Error Message :  DeleteDeletedrow.Returened.ErrorMessage

            // 3 - call Insert or Update
            var rowInserted = await dc.InsertAsync(new Test()
            {
                Name = "test2", Date = DateTime.Now, Price = 11
            });
            var rowID = dc.Find(1).SingleData;
            var rowUpdated = await dc.UpdateAsync(rowID);
            // Or
            var rowUpdated2 = await dc.UpdateAsync(new Test() 
            {
                ID = 1 , Name = "test2", Date = DateTime.Now, Price = 11 
            });

