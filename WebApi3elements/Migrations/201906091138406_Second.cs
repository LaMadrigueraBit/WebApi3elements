namespace WebApi3elements.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Second : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Breakdowns",
                c => new
                    {
                        breakdownId = c.Int(nullable: false, identity: true),
                        date = c.DateTime(nullable: false),
                        solved = c.Boolean(nullable: false),
                        deviceId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.breakdownId)
                .ForeignKey("dbo.Devices", t => t.deviceId)
                .Index(t => t.deviceId);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        deviceId = c.String(nullable: false, maxLength: 128),
                        name = c.String(),
                        on = c.Boolean(nullable: false),
                        homeId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.deviceId)
                .ForeignKey("dbo.Homes", t => t.homeId)
                .Index(t => t.homeId);
            
            CreateTable(
                "dbo.Homes",
                c => new
                    {
                        homeId = c.String(nullable: false, maxLength: 128),
                        userId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.homeId)
                .ForeignKey("dbo.AspNetUsers", t => t.userId)
                .Index(t => t.userId);
            
            AddColumn("dbo.Measures", "deviceId", c => c.String(maxLength: 128));
            AddColumn("dbo.Measures", "userId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Measures", "deviceId");
            CreateIndex("dbo.Measures", "userId");
            AddForeignKey("dbo.Measures", "userId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Measures", "deviceId", "dbo.Devices", "deviceId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Measures", "deviceId", "dbo.Devices");
            DropForeignKey("dbo.Measures", "userId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Breakdowns", "deviceId", "dbo.Devices");
            DropForeignKey("dbo.Devices", "homeId", "dbo.Homes");
            DropForeignKey("dbo.Homes", "userId", "dbo.AspNetUsers");
            DropIndex("dbo.Measures", new[] { "userId" });
            DropIndex("dbo.Measures", new[] { "deviceId" });
            DropIndex("dbo.Homes", new[] { "userId" });
            DropIndex("dbo.Devices", new[] { "homeId" });
            DropIndex("dbo.Breakdowns", new[] { "deviceId" });
            DropColumn("dbo.Measures", "userId");
            DropColumn("dbo.Measures", "deviceId");
            DropTable("dbo.Homes");
            DropTable("dbo.Devices");
            DropTable("dbo.Breakdowns");
        }
    }
}
