use anime

drop TABLE if exists Typity;
GO

drop TYPE if exists Types;
GO

CREATE TYPE Types  
EXTERNAL NAME Lab04.Types
GO

CREATE TABLE dbo.Typity
( 
  id INT IDENTITY(1,1) NOT NULL, 
  tpt Types NULL,
);
GO

INSERT INTO dbo.Typity(tpt) VALUES('2, 6, 1'); 
INSERT INTO dbo.Typity(tpt) VALUES('1,0, 15'); 
INSERT INTO dbo.Typity(tpt) VALUES('2, 1,8');  
GO 

SELECT * FROM dbo.Typity;
GO
SELECT id, tpt AS Types 
FROM dbo.Typity;
GO
DECLARE @v dbo.Types
SET @v = CAST('3, 1, 8' AS Types)
SELECT @v.Summ() AS 'summ'
GO
 
DROP TABLE dbo.Typity
GO

DROP TYPE Types
GO

