using Microsoft.EntityFrameworkCore.Migrations;

namespace Transactions.API.Migrations
{
    public partial class spInsertOrUpdateTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE InsertOrUpdateTransactions
                            @transactionId int, 
	                        @status nvarchar(max), 
	                        @type nvarchar(max), 
	                        @clientName nvarchar(max),
	                        @amount decimal(18,2)
                    AS
                    BEGIN  
                    SET NOCOUNT ON;  
	                MERGE Transactions AS target  
                    USING (SELECT @transactionId, @status, @type, @clientName, @amount) AS source (TransactionId, Status, Type, ClientName, Amount)  
                    ON (target.TransactionId = source.TransactionId)
                    WHEN MATCHED THEN
                        UPDATE SET Status = source.Status, Type = source.Type, ClientName = source.ClientName  
                    WHEN NOT MATCHED THEN  
                        INSERT (TransactionId, Status, Type, ClientName, Amount )  
                        VALUES (source.TransactionId, source.Status, source.Type, source.ClientName, source.Amount);
					END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
