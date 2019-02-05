use anime

drop FUNCTION if exists max_rating_OVER_ALL_TIMES;
GO  

CREATE FUNCTION max_rating_OVER_ALL_TIMES() RETURNS float   
AS EXTERNAL NAME Lab04.first.max_rating_OVER_ALL_TIMES;   
GO  

SELECT dbo.max_rating_OVER_ALL_TIMES();  
GO  