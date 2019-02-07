use anime

drop trigger if exists UpdateStudioo
GO

CREATE TRIGGER UpdateStudioo  
ON studio 
FOR Insert  
AS  
EXTERNAL NAME Lab04.Triggers.fifth 
GO
INSERT INTO studio(helper_id, name_studio, year_, center_studio, site_studio) values
					 (1, 'С новым годом', 2019, 'Москва','сайт') 