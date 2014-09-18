namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseOhneStundenplan : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Beobachtungen",
                c => new
                    {
                        BeobachtungId = c.Int(nullable: false, identity: true),
                        Datum = c.DateTime(),
                        Text = c.String(maxLength: 4000),
                        SchuljahrId = c.Int(nullable: false),
                        Fach_FachId = c.Int(),
                        Schueler_ID = c.Int(),
                    })
                .PrimaryKey(t => t.BeobachtungId)
                .ForeignKey("dbo.Fächer", t => t.Fach_FachId)
                .ForeignKey("dbo.Schüler", t => t.Schueler_ID)
                .ForeignKey("dbo.Schuljahre", t => t.SchuljahrId, cascadeDelete: true)
                .Index(t => t.SchuljahrId)
                .Index(t => t.Fach_FachId)
                .Index(t => t.Schueler_ID);
            
            CreateTable(
                "dbo.Fächer",
                c => new
                    {
                        FachId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        Inaktiv = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FachId);
            
            CreateTable(
                "dbo.Schüler",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Vorname = c.String(maxLength: 4000),
                        Nachname = c.String(maxLength: 4000),
                        Geburtsdatum = c.DateTime(),
                        Geschlecht = c.Int(nullable: false),
                        Bemerkung = c.String(maxLength: 4000),
                        Adresse_Strasse = c.String(maxLength: 4000),
                        Adresse_PLZ = c.Int(),
                        Adresse_Ort = c.String(maxLength: 4000),
                        Foto = c.Binary(),
                        Inaktiv = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Klassen",
                c => new
                    {
                        KlasseId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        SchuljahrId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.KlasseId)
                .ForeignKey("dbo.Schuljahre", t => t.SchuljahrId, cascadeDelete: true)
                .Index(t => t.SchuljahrId);
            
            CreateTable(
                "dbo.Schuljahre",
                c => new
                    {
                        Startjahr = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Startjahr);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 4000),
                        ValueType = c.String(maxLength: 4000),
                        IntValue = c.Int(nullable: false),
                        StringValue = c.String(maxLength: 4000),
                        BoolValue = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.Textbausteine",
                c => new
                    {
                        TextbausteinId = c.Int(nullable: false, identity: true),
                        UsageCount = c.Int(nullable: false),
                        Name = c.String(maxLength: 4000),
                        Text = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.TextbausteinId);
            
            CreateTable(
                "dbo.Klassen_Schüler",
                c => new
                    {
                        Klasse_KlasseId = c.Int(nullable: false),
                        Schueler_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Klasse_KlasseId, t.Schueler_ID })
                .ForeignKey("dbo.Klassen", t => t.Klasse_KlasseId, cascadeDelete: true)
                .ForeignKey("dbo.Schüler", t => t.Schueler_ID, cascadeDelete: true)
                .Index(t => t.Klasse_KlasseId)
                .Index(t => t.Schueler_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Beobachtungen", "SchuljahrId", "dbo.Schuljahre");
            DropForeignKey("dbo.Beobachtungen", "Schueler_ID", "dbo.Schüler");
            DropForeignKey("dbo.Klassen", "SchuljahrId", "dbo.Schuljahre");
            DropForeignKey("dbo.Klassen_Schüler", "Schueler_ID", "dbo.Schüler");
            DropForeignKey("dbo.Klassen_Schüler", "Klasse_KlasseId", "dbo.Klassen");
            DropForeignKey("dbo.Beobachtungen", "Fach_FachId", "dbo.Fächer");
            DropIndex("dbo.Klassen_Schüler", new[] { "Schueler_ID" });
            DropIndex("dbo.Klassen_Schüler", new[] { "Klasse_KlasseId" });
            DropIndex("dbo.Klassen", new[] { "SchuljahrId" });
            DropIndex("dbo.Beobachtungen", new[] { "Schueler_ID" });
            DropIndex("dbo.Beobachtungen", new[] { "Fach_FachId" });
            DropIndex("dbo.Beobachtungen", new[] { "SchuljahrId" });
            DropTable("dbo.Klassen_Schüler");
            DropTable("dbo.Textbausteine");
            DropTable("dbo.Settings");
            DropTable("dbo.Schuljahre");
            DropTable("dbo.Klassen");
            DropTable("dbo.Schüler");
            DropTable("dbo.Fächer");
            DropTable("dbo.Beobachtungen");
        }
    }
}
