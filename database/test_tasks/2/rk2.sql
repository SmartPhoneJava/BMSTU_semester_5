-- create database RK2
-- drop RK2


create table [������]
(
	ID int IDENTITY(1,1) PRIMARY KEY,
	[��������] nvarchar(100),
	[��������] nvarchar(300)  NOT NULL
)
GO

create table [���������]
(
	ID int IDENTITY(1,1) PRIMARY KEY,
	[��������] nvarchar(100) NOT NULL,
	[��� ���������] date NOT NULL,
	[��������] nvarchar(300)  NOT NULL,
	[������_ID] int,

	foreign key ([������_ID]) references [������]
)
GO

create table [����������]
(
	ID int IDENTITY(1,1) PRIMARY KEY,
	[���] nvarchar(100)  NOT NULL,
	[��� ��������] date  NOT NULL,
	[�����] nvarchar(100)  NOT NULL,
	[E-mail] nvarchar(100)  NOT NULL
)
GO

create table [���������� � ���������]
(
	san_id int NOT NULL,
	otd_id int NOT NULL,
	PRIMARY KEY(san_id, otd_id),
	
	foreign key (san_id) references [���������],
	foreign key (otd_id) references [����������]
)
GO


use RK2 
insert into [������] values
 ('������', '�������� ������')
 insert into [������] values
 ('�����������', '�������� ������������')
 insert into [������] values
 ('����', '�������� ����')
 insert into [������] values
 ('���������', '�������� ����������')
 insert into [������] values
 ('������', '�������� ������')
 insert into [������] values
 ('�����', '�������� ������')
 insert into [������] values
 ('�������', '�������� �������')
 insert into [������] values
 ('������������', '�������� �������������')
 insert into [������] values
 ('�������', '�������� �������')
 insert into [������] values
 ('����', '�������� �����')

 insert into [���������] values
 ('��������',
   CAST('2009-07-12' AS date), '����� �������!', 1)
 insert into [���������] values
 ('���������',
   CAST('2003-03-11' AS date), '����� �����!', 1)
 insert into [���������] values
 ('�������',
   CAST('2009-03-11' AS date), '����� ���������!', 2)
 insert into [���������] values
 ('��������� ���',
   CAST('2007-01-09' AS date), '����� �����!', 3)
 insert into [���������] values
 ('�������',
   CAST('1998-03-09' AS date), '����� �������!', 4)
 insert into [���������] values
 ('������',
   CAST('1995-04-02' AS date), '����� ��� � ��������!', 5)
 insert into [���������] values
 ('�����',
   CAST('1998-03-19' AS date), '����� ��� � �������!', 5)
 insert into [���������] values
 ('������ ��� ������',
   CAST('1994-04-24' AS date), '����� ��� � ��������!', 7)
 insert into [���������] values
 ('����� ��������',
   CAST('2013-02-22' AS date), '��� ��������', 8)
 insert into [���������] values
 ('��� �����',
   CAST('2015-02-22' AS date), '��� ��������', 9)
 insert into [���������] values
 ('���� �����',
   CAST('2016-01-04' AS date), '��� ��������', 9)

 insert into [����������] values
 ('���� ������ ��������',
   CAST('2001-05-20' AS date), '�����', '�����')
 insert into [����������] values
 ('���������� ������������ ��������������',
   CAST('2002-05-20' AS date), '�����', '�����')
 insert into [����������] values
 ('����� ������� ���������',
   CAST('2003-05-20' AS date), '�����', '�����')
 insert into [����������] values
 ('������ ������� ���������',
   CAST('2004-05-20' AS date), '�����', '�����')
 insert into [����������] values
 ('������ �������� ����������',
   CAST('2005-05-20' AS date), '�����', '�����')
 insert into [����������] values
 ('��� ���� �������',
   CAST('2006-05-20' AS date), '�����', '�����')
 insert into [����������] values
 ('���� ����� ����������',
   CAST('2007-05-20' AS date), '�����', '�����')
 insert into [����������] values
 ('������� �������� ����������',
   CAST('2008-05-20' AS date), '�����', '�����')
 insert into [����������] values
 ('������ ����� ����������',
   CAST('2009-05-20' AS date), '�����', '�����')
 insert into [����������] values
 ('����� ������� ����������',
   CAST('2010-05-20' AS date), '�����', '�����')
 insert into [����������] values
 ('���� ������ ����������',
   CAST('2011-05-20' AS date), '�����', '�����')
 insert into [����������] values
 ('���� ������ ����������',
   CAST('2012-05-20' AS date), '�����', '�����')
 insert into [����������] values
 ('��������� ������ �������������',
   CAST('2013-05-20' AS date), '�����', '�����')
 insert into [����������] values
 ('������� ����� ���������',
   CAST('2014-05-20' AS date), '�����', '�����')

insert into [���������� � ���������] values
 (1,1), (1,2), (2,3), 
 (3,4), (4,5), (5,6),
 (6,7), (6,8), (6,9),
 (7,10), (7,11), (9,12),
 (9,13), (9,14)


 select ID, [��������] , case 
	when   [��� ���������] <= CAST('2000-01-01' AS date)
		then '��������� �� ���� ��������'
	when  [��� ���������] <= CAST('2008-01-01' AS date)
		 then '10 ������ ��������'
	else '����������:'+ CAST([��� ���������] AS nvarchar)
	end as '����� ���������'
	from [���������]

-- ��������, ��� ���������� ��� ������� 2, �����
-- ������� ��� � ����� ��������� ��� ������� 3
UPDATE [����������]
SET [���] = 
(
	SELECT [��������]
	FROM [���������]
	where id = 3
)
WHERE ID = 2
SELECT * from [����������]

-- ������� ������ �����, ������� �������� ������, ��� �������
-- ��������� � ������� ��� ��������
select [���], [��� ��������], [��������], [��� ���������]
	from [����������] as O join
	 [���������� � ���������] as C
	ON O.ID = C.otd_id join
	 [���������] as S ON S.ID = C.san_id 
	GROUP BY [��� ��������], [���], [��������], [��� ���������]
	HAVING [��� ��������] < [��� ���������]
go

-- ��� ������� �������� ������

create view sanatoriy
	AS SELECT * 
	FROM [���������]
--drop view sanatoriy
go

create view region
	AS SELECT * 
	FROM [������]
--drop view region
go

create view otdih
	AS SELECT * 
	FROM [����������]
--drop view otdih
go

-- ���������, ��������� ������ � �������������� �� ����������
create procedure destroy @counter INT OUTPUT
AS
BEGIN
	DECLARE @code NVARCHAR(100) = ''
	SET @counter = 0
	SELECT @counter = @counter + 1, @code = @code + 'DROP view ' + name + '; ' 
	FROM sys.objects 
	where type = 'V' 
	group by name
	EXEC (@code)
END
go

DECLARE @count int;
EXEC destroy @counter = @count OUTPUT;
SELECT @count as count;


--drop proc destroy

-- ������������� ��� ��������
 --drop table [������]
 --drop table [���������]
 --drop table [����������]
 --drop table [���������� � ���������]