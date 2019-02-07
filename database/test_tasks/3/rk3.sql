--create database RK3
--CREATE schema RK3

--drop table Filial
--drop table Sotrudniki

Create table Filial (
	f_id int IDENTITY(1,1) PRIMARY KEY,
	name_ nvarchar(100),
	adress nvarchar(100),
	number nvarchar(20),
)

Create table Sotrudniki (
	s_id int IDENTITY(1,1) PRIMARY KEY,
	name_ nvarchar(100),
	date_ date,
	otdel_name nvarchar(100),
	f_id INT NOT NULL REFERENCES Filial (f_id), 
)

INSERT INTO Filial(name_, adress, number)
VALUES ('���������� ������(��)', '�������, 5', '458-62-30')
GO

INSERT INTO Filial(name_, adress, number)
VALUES ('������������� ���. ����', '������������, 8', '457-12-00')
GO

INSERT INTO Filial(name_, adress, number)
VALUES ('����������� ���. ����', '������, 44', '976-96-58')
GO

INSERT INTO Filial(name_, adress, number)
VALUES ('������� ������', '�������, 1', '962-58-45')
GO

INSERT INTO Sotrudniki(name_, date_, otdel_name, f_id)
VALUES ('����� ������� �������������', CAST('1965-01-18' AS DATE), '��', 1)
GO

INSERT INTO Sotrudniki(name_, date_, otdel_name, f_id)
VALUES ('������� �������� �����������', CAST('1971-02-28' AS DATE), '�����������', 1)
GO  

INSERT INTO Sotrudniki(name_, date_, otdel_name, f_id)
VALUES ('������ ����� ��������', CAST('1973-04-23' AS DATE), '��������', 1)
GO 

INSERT INTO Sotrudniki(name_, date_, otdel_name, f_id)
VALUES ('������� ������� ����������', CAST('1969-01-11' AS DATE), '�������', 1)
GO 

INSERT INTO Sotrudniki(name_, date_, otdel_name, f_id)
VALUES ('������� ������ ����������', CAST('1968-04-12' AS DATE), '�������', 2)
GO 

INSERT INTO Sotrudniki(name_, date_, otdel_name, f_id)
VALUES ('������� ���� ���������', CAST('1998-04-12' AS DATE), '��������', 2)
GO 

INSERT INTO Sotrudniki(name_, date_, otdel_name, f_id)
VALUES ('�������� ������ �����������', CAST('2008-04-12' AS DATE), '��', 2)
GO 

INSERT INTO Sotrudniki(name_, date_, otdel_name, f_id)
VALUES ('�������� ���� ������������', CAST('1948-08-21' AS DATE), '��', 3)
GO 

INSERT INTO Sotrudniki(name_, date_, otdel_name, f_id)
VALUES ('����� ���������� ������������', CAST('1969-02-12' AS DATE), '��', 3)
GO

INSERT INTO Sotrudniki(name_, date_, otdel_name, f_id)
VALUES ('����� ������ ��������', CAST('1981-01-12' AS DATE), '��', 3)
GO

select * from Sotrudniki as S join Filial as F On F.f_id = S.f_id where F.adress like '%�������%'

select * from Filial as FS where 
(select COUNT(*) from Sotrudniki as S where S.f_id = FS.f_id) >= 4 and 
(select COUNT(*) from Sotrudniki as S where S.f_id = FS.f_id) <= 20

select DATEDIFF(YEAR, GETDATE(), U2.time_) as diff,
 E.FIO from
	Uchet as U join Uchet
		 as U2 On U.type_= 1 and U2.type_ = 2 and U.id_empl = U2.id_empl and
		 U.sysdate = U2.sysdate
		 join Empl as E On U.id_empl = E.ID 
		 where DAY(U.sysdate) = DAY(GETDATE()) and
		  MONTH(U.sysdate) = MONTH(GETDATE()) and
		  YEAR(U.sysdate) = YEAR(GETDATE()) 

		 ORDER BY (DATEDIFF(HOUR, U.time_, U2.time_))