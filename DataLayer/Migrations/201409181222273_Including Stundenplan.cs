namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncludingStundenplan : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stundenpläne",
                c => new
                    {
                        StundenplanId = c.Int(nullable: false, identity: true),
                        Klasse_KlasseId = c.Int(),
                    })
                .PrimaryKey(t => t.StundenplanId)
                .ForeignKey("dbo.Klassen", t => t.Klasse_KlasseId)
                .Index(t => t.Klasse_KlasseId);
            
            CreateTable(
                "dbo.Unterrichtsstunden",
                c => new
                    {
                        StundenplanId = c.Int(nullable: false),
                        Tag = c.Int(nullable: false),
                        Stunde = c.Int(nullable: false),
                        Fach_FachId = c.Int(),
                        Klasse_KlasseId = c.Int(),
                    })
                .PrimaryKey(t => new { t.StundenplanId, t.Tag, t.Stunde })
                .ForeignKey("dbo.Fächer", t => t.Fach_FachId)
                .ForeignKey("dbo.Klassen", t => t.Klasse_KlasseId)
                .ForeignKey("dbo.Stundenpläne", t => t.StundenplanId, cascadeDelete: true)
                .Index(t => t.StundenplanId)
                .Index(t => t.Fach_FachId)
                .Index(t => t.Klasse_KlasseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Unterrichtsstunden", "StundenplanId", "dbo.Stundenpläne");
            DropForeignKey("dbo.Unterrichtsstunden", "Klasse_KlasseId", "dbo.Klassen");
            DropForeignKey("dbo.Unterrichtsstunden", "Fach_FachId", "dbo.Fächer");
            DropForeignKey("dbo.Stundenpläne", "Klasse_KlasseId", "dbo.Klassen");
            DropIndex("dbo.Unterrichtsstunden", new[] { "Klasse_KlasseId" });
            DropIndex("dbo.Unterrichtsstunden", new[] { "Fach_FachId" });
            DropIndex("dbo.Unterrichtsstunden", new[] { "StundenplanId" });
            DropIndex("dbo.Stundenpläne", new[] { "Klasse_KlasseId" });
            DropTable("dbo.Unterrichtsstunden");
            DropTable("dbo.Stundenpläne");
        }
    }
}
