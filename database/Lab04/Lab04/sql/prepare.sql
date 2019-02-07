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
 
-- ������� ��� ��������� �������, ����� ��� ��������� ������� 1,2...6
drop FUNCTION if exists max_rating_OVER_ALL_TIMES; -- �� ����� 1.sql
drop AGGREGATE if exists getPopularStatus;		   -- �� ����� 2.sql
drop FUNCTION if exists getInfo;				   -- �� ����� 3.sql
drop procedure if exists updateStudio;			   -- �� ����� 4.sql
drop trigger if exists UpdateStudioo;			   -- �� ����� 5.sql
drop TABLE if exists Typity;					   -- �� ����� 6.sql
drop TYPE if exists Types;						   -- �� ����� 6.sql

-- ������� ���� ������������, ���� �� ��� ������
drop ASSEMBLY if exists Lab04;
GO  

-- ������ ���� ������������
CREATE ASSEMBLY Lab04 
AUTHORIZATION dbo
FROM 'C:\msys64\home\BMSTU_semester_5\Lab04\Lab04\bin\Debug\Lab04.dll' 
WITH PERMISSION_SET = SAFE -- EXTERNAL_ACCESS;  
GO   
