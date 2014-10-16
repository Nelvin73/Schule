namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddKlassenarbeiten2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Klassenarbeiten", "Kommentar", c => c.String(maxLength: 4000));
            AddColumn("dbo.Klassenarbeiten", "Datum", c => c.DateTime(nullable: false));
            AddColumn("dbo.KlassenarbeitsNoten", "OhneWertung", c => c.Boolean(nullable: false));
            AlterColumn("dbo.KlassenarbeitsNoten", "Punkte", c => c.Double());
            DropColumn("dbo.KlassenarbeitsNoten", "HatMitgeschrieben");
        }
        
        public override void Down()
        {
            AddColumn("dbo.KlassenarbeitsNoten", "HatMitgeschrieben", c => c.Boolean(nullable: false));
            AlterColumn("dbo.KlassenarbeitsNoten", "Punkte", c => c.Int());
            DropColumn("dbo.KlassenarbeitsNoten", "OhneWertung");
            DropColumn("dbo.Klassenarbeiten", "Datum");
            DropColumn("dbo.Klassenarbeiten", "Kommentar");
        }
    }
}
