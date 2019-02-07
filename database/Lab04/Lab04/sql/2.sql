use anime

drop AGGREGATE if exists getPopularStatus;
GO

CREATE AGGREGATE getPopularStatus (@user_rating float, @status nvarchar(50)) RETURNS nvarchar(50)  
EXTERNAL NAME Lab04.getPopularStatus;  
go  

select dbo.getPopularStatus(user_rating, status) as 'Most popular' from airtime;
go

