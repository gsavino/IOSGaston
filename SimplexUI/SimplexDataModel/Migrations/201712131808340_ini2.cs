namespace SimplexDataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ini2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DBEcuacions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NroEcu = c.Int(nullable: false),
                        Preparada = c.Boolean(nullable: false),
                        Operador = c.String(),
                        ValorDerecho = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VariableBasica = c.String(),
                        DBModelo_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DBModeloes", t => t.DBModelo_Id)
                .Index(t => t.DBModelo_Id);
            
            CreateTable(
                "dbo.DBTerminoes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Variable = c.String(),
                        DBEcuacion_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DBEcuacions", t => t.DBEcuacion_Id)
                .Index(t => t.DBEcuacion_Id);
            
            CreateTable(
                "dbo.DBModeloes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Objetivo = c.String(),
                        Nombre = c.String(),
                        FuncionAOptimizar_Id = c.Guid(),
                        FuncionOriginal_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DBEcuacions", t => t.FuncionAOptimizar_Id)
                .ForeignKey("dbo.DBEcuacions", t => t.FuncionOriginal_Id)
                .Index(t => t.FuncionAOptimizar_Id)
                .Index(t => t.FuncionOriginal_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DBModeloes", "FuncionOriginal_Id", "dbo.DBEcuacions");
            DropForeignKey("dbo.DBModeloes", "FuncionAOptimizar_Id", "dbo.DBEcuacions");
            DropForeignKey("dbo.DBEcuacions", "DBModelo_Id", "dbo.DBModeloes");
            DropForeignKey("dbo.DBTerminoes", "DBEcuacion_Id", "dbo.DBEcuacions");
            DropIndex("dbo.DBModeloes", new[] { "FuncionOriginal_Id" });
            DropIndex("dbo.DBModeloes", new[] { "FuncionAOptimizar_Id" });
            DropIndex("dbo.DBTerminoes", new[] { "DBEcuacion_Id" });
            DropIndex("dbo.DBEcuacions", new[] { "DBModelo_Id" });
            DropTable("dbo.DBModeloes");
            DropTable("dbo.DBTerminoes");
            DropTable("dbo.DBEcuacions");
        }
    }
}
