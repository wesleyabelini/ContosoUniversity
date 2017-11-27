namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseStudentGrade1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Enrollment", "Grade", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Enrollment", "Grade");
        }
    }
}
