use anime

drop FUNCTION if exists getInfo;
GO

CREATE FUNCTION getInfo(@type int)   
RETURNS TABLE (  
   name nvarchar(1000),  
   description nvarchar(1300),
   time int
)  
AS EXTERNAL NAME Lab04.third.getInfo
go  

SELECT * FROM getInfo(2);  
go  