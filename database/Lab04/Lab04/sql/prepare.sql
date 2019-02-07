USE anime
GO
EXEC sp_configure 'clr strict security', 0; 
RECONFIGURE;
GO
sp_configure 'show advanced options', 1
GO
RECONFIGURE
GO

sp_configure 'clr enabled', 1
GO
RECONFIGURE
GO
 
-- Удаляем все созданные объекты, вдруг уже выполнены скрипты 1,2...6
drop FUNCTION if exists max_rating_OVER_ALL_TIMES; -- из файла 1.sql
drop AGGREGATE if exists getPopularStatus;		   -- из файла 2.sql
drop FUNCTION if exists getInfo;				   -- из файла 3.sql
drop procedure if exists updateStudio;			   -- из файла 4.sql
drop trigger if exists UpdateStudioo;			   -- из файла 5.sql
drop TABLE if exists Typity;					   -- из файла 6.sql
drop TYPE if exists Types;						   -- из файла 6.sql

-- Удаляем ключ безопасности, если он уже создан
drop ASSEMBLY if exists Lab04;
GO  

-- Создаём ключ безопасности
CREATE ASSEMBLY Lab04 
AUTHORIZATION dbo
FROM 'C:\msys64\home\BMSTU_semester_5\Lab04\Lab04\bin\Debug\Lab04.dll' 
WITH PERMISSION_SET = SAFE -- EXTERNAL_ACCESS;  
GO   
