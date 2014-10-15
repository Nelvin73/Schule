namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddKlassenarbeiten : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Klassenarbeiten",
                c => new
                    {
                        KlassenarbeitId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        GesamtPunkte = c.Int(nullable: false),
                        Punkteschlüssel = c.String(maxLength: 4000),
                        Fach_FachId = c.Int(),
                        Klasse_KlasseId = c.Int(),
                    })
                .PrimaryKey(t => t.KlassenarbeitId)
                .ForeignKey("dbo.Fächer", t => t.Fach_FachId)
                .ForeignKey("dbo.Klassen", t => t.Klasse_KlasseId)
                .Index(t => t.Fach_FachId)
                .Index(t => t.Klasse_KlasseId);
            
            CreateTable(
                "dbo.KlassenarbeitsNoten",
                c => new
                    {
                        KlassenarbeitId = c.Int(nullable: false),
                        SchülerId = c.Int(nullable: false),
                        HatMitgeschrieben = c.Boolean(nullable: false),
                        Punkte = c.Int(),
                        Note = c.Int(),
                        Kommentar = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => new { t.KlassenarbeitId, t.SchülerId })
                .ForeignKey("dbo.Klassenarbeiten", t => t.KlassenarbeitId, cascadeDelete: true)
                .ForeignKey("dbo.Schüler", t => t.SchülerId, cascadeDelete: true)
                .Index(t => t.KlassenarbeitId)
                .Index(t => t.SchülerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KlassenarbeitsNoten", "SchülerId", "dbo.Schüler");
            DropForeignKey("dbo.KlassenarbeitsNoten", "KlassenarbeitId", "dbo.Klassenarbeiten");
            DropForeignKey("dbo.Klassenarbeiten", "Klasse_KlasseId", "dbo.Klassen");
            DropForeignKey("dbo.Klassenarbeiten", "Fach_FachId", "dbo.Fächer");
            DropIndex("dbo.KlassenarbeitsNoten", new[] { "SchülerId" });
            DropIndex("dbo.KlassenarbeitsNoten", new[] { "KlassenarbeitId" });
            DropIndex("dbo.Klassenarbeiten", new[] { "Klasse_KlasseId" });
            DropIndex("dbo.Klassenarbeiten", new[] { "Fach_FachId" });
            DropTable("dbo.KlassenarbeitsNoten");
            DropTable("dbo.Klassenarbeiten");
        }
    }
}
