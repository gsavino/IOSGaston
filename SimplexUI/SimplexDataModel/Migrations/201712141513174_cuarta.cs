namespace SimplexDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cuarta : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DBEcuacions", "DBModelo_Id1", "dbo.DBModeloes");
            DropIndex("dbo.DBEcuacions", new[] { "DBModelo_Id1" });
            DropColumn("dbo.DBEcuacions", "DBModelo_Id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DBEcuacions", "DBModelo_Id1", c => c.Guid());
            CreateIndex("dbo.DBEcuacions", "DBModelo_Id1");
            AddForeignKey("dbo.DBEcuacions", "DBModelo_Id1", "dbo.DBModeloes", "Id");
        }
    }
}
