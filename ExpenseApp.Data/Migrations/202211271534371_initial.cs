namespace ExpenseApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Configs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ConfigKey = c.String(),
                        ConfigValue = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ExpenseHistories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ExpenseId = c.Int(nullable: false),
                        ExpenseStatusId = c.Int(nullable: false),
                        RejectReason = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Expenses", t => t.ExpenseId, cascadeDelete: true)
                .ForeignKey("dbo.ExpenseStatus", t => t.ExpenseStatusId, cascadeDelete: true)
                .Index(t => t.ExpenseId)
                .Index(t => t.ExpenseStatusId);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDate = c.DateTime(nullable: false),
                        LastExpenseActionId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ExpenseItems",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ExpenseId = c.Int(nullable: false),
                        Description = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpenseItemDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Expenses", t => t.ExpenseId, cascadeDelete: true)
                .Index(t => t.ExpenseId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Password = c.String(),
                        FullName = c.String(),
                        UserRoleId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserRoles", t => t.UserRoleId, cascadeDelete: true)
                .Index(t => t.UserRoleId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ExpenseStatus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StatusName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.GetUnApprovedExpenses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ExpenseCreatedDate = c.DateTime(),
                        ExpenseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Level = c.String(),
                        Message = c.String(),
                        Exception = c.String(),
                        LogDate = c.DateTime(nullable: false),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExpenseHistories", "ExpenseStatusId", "dbo.ExpenseStatus");
            DropForeignKey("dbo.Users", "UserRoleId", "dbo.UserRoles");
            DropForeignKey("dbo.Expenses", "UserId", "dbo.Users");
            DropForeignKey("dbo.ExpenseItems", "ExpenseId", "dbo.Expenses");
            DropForeignKey("dbo.ExpenseHistories", "ExpenseId", "dbo.Expenses");
            DropIndex("dbo.Users", new[] { "UserRoleId" });
            DropIndex("dbo.ExpenseItems", new[] { "ExpenseId" });
            DropIndex("dbo.Expenses", new[] { "UserId" });
            DropIndex("dbo.ExpenseHistories", new[] { "ExpenseStatusId" });
            DropIndex("dbo.ExpenseHistories", new[] { "ExpenseId" });
            DropTable("dbo.Logs");
            DropTable("dbo.GetUnApprovedExpenses");
            DropTable("dbo.ExpenseStatus");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Users");
            DropTable("dbo.ExpenseItems");
            DropTable("dbo.Expenses");
            DropTable("dbo.ExpenseHistories");
            DropTable("dbo.Configs");
        }
    }
}
