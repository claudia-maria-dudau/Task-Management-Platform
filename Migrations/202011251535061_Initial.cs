namespace Task_Management_Platform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        DataAdaug = c.DateTime(nullable: false),
                        TaskId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Task1", t => t.TaskId, cascadeDelete: true)
                .Index(t => t.TaskId);
            
            CreateTable(
                "dbo.Task1",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        Status = c.String(),
                        DataStart = c.DateTime(nullable: false),
                        DataFin = c.DateTime(nullable: false),
                        Team_TeamId = c.Int(),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.Teams", t => t.Team_TeamId)
                .Index(t => t.Team_TeamId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        Nume = c.String(),
                        DataInscriere = c.DateTime(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TeamId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        Nume = c.String(),
                        Descriere = c.String(),
                        TeamId = c.Int(nullable: false),
                        Deadline = c.DateTime(nullable: false),
                        DataInceput = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectId)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Task1", "Team_TeamId", "dbo.Teams");
            DropForeignKey("dbo.Projects", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Comments", "TaskId", "dbo.Task1");
            DropIndex("dbo.Projects", new[] { "TeamId" });
            DropIndex("dbo.Task1", new[] { "Team_TeamId" });
            DropIndex("dbo.Comments", new[] { "TaskId" });
            DropTable("dbo.Projects");
            DropTable("dbo.Teams");
            DropTable("dbo.Task1");
            DropTable("dbo.Comments");
        }
    }
}
