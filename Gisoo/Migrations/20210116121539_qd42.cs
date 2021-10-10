using Microsoft.EntityFrameworkCore.Migrations;

namespace Gisoo.Migrations
{
    public partial class qd42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp = @"Create PROCEDURE [dbo].[SearchAll]
AS
declare @param1 nvarchar(50)

SELECT a.id ,a.title,a.description,a.image1,a.userId, type = 1 FROM [dbo].[Notices] a
WHERE        (a.title LIKE N'%' + @param1 + '%') OR (a.description LIKE N'%' + @param1 + '%')
UNION All
SELECT b.id ,b.title,b.description,b.image1,b.userId, type = 2 FROM [dbo].[Advertisments] b
WHERE         (b.title LIKE N'%' + @param1 + '%') OR (b.description LIKE N'%' + @param1 + '%')
UNION All
SELECT c.id ,c.title,c.description,f.url,c.userId, type = 3 FROM [dbo].[Lines] c
inner join [dbo].[LineImages] f on f.lineId=c.id 
WHERE         (c.title LIKE N'%' + @param1 + '%') OR (c.description LIKE N'%' + @param1 + '%')
UNION All
SELECT d.id ,d.title,d.description,g.url,d.userId, type = 4 FROM [dbo].[Products] d
inner join [dbo].[ProductImages] g on g.productId=d.id 
WHERE         (d.title LIKE N'%' + @param1 + '%') OR (d.description LIKE N'%' + @param1 + '%')
UNION All
SELECT e.id ,e.title,e.description,i.url,e.userId, type = 5 FROM [dbo].[ClassRooms] e
inner join  [dbo].[ClassRoomImages] i on  i.classRoomId=e.id
WHERE         (e.title LIKE N'%' + @param1 + '%') OR (e.description LIKE N'%' + @param1 + '%')
";
            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp = @"drop PROCEDURE [dbo].[SearchAll]";
            migrationBuilder.Sql(sp);
        }
    }
}
