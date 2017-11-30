namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaxLengthOnNames : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Student", "LastName", c => c.String(maxLength: 50));
            AlterColumn("dbo.Student", "FirstMidName", c => c.String(maxLength: 50));
            DropColumn("dbo.Student", "Secret");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Student", "Secret", c => c.String());
            AlterColumn("dbo.Student", "FirstMidName", c => c.String());
            AlterColumn("dbo.Student", "LastName", c => c.String());
        }
    }
}
