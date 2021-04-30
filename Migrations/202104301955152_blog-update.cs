namespace Red_Lake_Hospital_Redesign_Team6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class blogupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BlogModels", "ApplicationUserUsername", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BlogModels", "ApplicationUserUsername");
        }
    }
}
