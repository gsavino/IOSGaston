namespace SimplexDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tercera : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DBEcuacions", "DBModelo_Id1", c => c.Guid());
            CreateIndex("dbo.DBEcuacions", "DBModelo_Id1");
            AddForeignKey("dbo.DBEcuacions", "DBModelo_Id1", "dbo.DBModeloes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DBEcuacions", "DBModelo_Id1", "dbo.DBModeloes");
            DropIndex("dbo.DBEcuacions", new[] { "DBModelo_Id1" });
            DropColumn("dbo.DBEcuacions", "DBModelo_Id1");
        }
    }
}
