namespace Red_Lake_Hospital_Redesign_Team6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FEEDBACKUPDATE : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BlogModels", "ApplicationUserUsername");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BlogModels", "ApplicationUserUsername", c => c.String());
        }
    }
}
