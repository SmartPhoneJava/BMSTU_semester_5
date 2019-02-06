-- create database RK2
-- drop RK2


create table [Регион]
(
	ID int IDENTITY(1,1) PRIMARY KEY,
	[Название] nvarchar(100),
	[Описание] nvarchar(300)  NOT NULL
)
GO

create table [Санаторий]
(
	ID int IDENTITY(1,1) PRIMARY KEY,
	[Название] nvarchar(100) NOT NULL,
	[Год основания] date NOT NULL,
	[Описание] nvarchar(300)  NOT NULL,
	[Регион_ID] int,

	foreign key ([Регион_ID]) references [Регион]
)
GO

create table [Отдыхающий]
(
	ID int IDENTITY(1,1) PRIMARY KEY,
	[ФИО] nvarchar(100)  NOT NULL,
	[Год рождения] date  NOT NULL,
	[Адрес] nvarchar(100)  NOT NULL,
	[E-mail] nvarchar(100)  NOT NULL
)
GO

create table [Отдыхающий в Санатории]
(
	san_id int NOT NULL,
	otd_id int NOT NULL,
	PRIMARY KEY(san_id, otd_id),
	
	foreign key (san_id) references [Санаторий],
	foreign key (otd_id) references [Отдыхающий]
)
GO


use RK2 
insert into [Регион] values
 ('Москва', 'Описание Москвы')
 insert into [Регион] values
 ('Владивосток', 'Описание Владивостока')
 insert into [Регион] values
 ('Сочи', 'Описание Сочи')
 insert into [Регион] values
 ('Краснодар', 'Описание Краснодара')
 insert into [Регион] values
 ('Казань', 'Описание Казани')
 insert into [Регион] values
 ('Питер', 'Описание Питера')
 insert into [Регион] values
 ('Вологда', 'Описание Вологды')
 insert into [Регион] values
 ('Петрозаводск', 'Описание Петрозаводска')
 insert into [Регион] values
 ('Сызрань', 'Описание Сызраня')
 insert into [Регион] values
 ('Ялта', 'Описание Яльты')

 insert into [Санаторий] values
 ('Ласточка',
   CAST('2009-07-12' AS date), 'Здесь здорово!', 1)
 insert into [Санаторий] values
 ('Канарейка',
   CAST('2003-03-11' AS date), 'Здесь круто!', 1)
 insert into [Санаторий] values
 ('Яблочко',
   CAST('2009-03-11' AS date), 'Здесь прикольно!', 2)
 insert into [Санаторий] values
 ('Фруктовый сад',
   CAST('2007-01-09' AS date), 'Здесь сочно!', 3)
 insert into [Санаторий] values
 ('Паровоз',
   CAST('1998-03-09' AS date), 'Здесь отлично!', 4)
 insert into [Санаторий] values
 ('Совёнок',
   CAST('1995-04-02' AS date), 'Лучше чем в паровозе!', 5)
 insert into [Санаторий] values
 ('Стриж',
   CAST('1998-03-19' AS date), 'Лучше чем в яблочке!', 5)
 insert into [Санаторий] values
 ('Трижды три девять',
   CAST('1994-04-24' AS date), 'Лучше чем в ласточке!', 7)
 insert into [Санаторий] values
 ('Конец фантазии',
   CAST('2013-02-22' AS date), 'Без описания', 8)
 insert into [Санаторий] values
 ('Эль гранд',
   CAST('2015-02-22' AS date), 'Без описания', 9)
 insert into [Санаторий] values
 ('Роял пэлэс',
   CAST('2016-01-04' AS date), 'Без описания', 9)

 insert into [Отдыхающий] values
 ('Иван Иванов Иванович',
   CAST('2001-05-20' AS date), 'Адрес', 'Почта')
 insert into [Отдыхающий] values
 ('Константин Константинов Константинович',
   CAST('2002-05-20' AS date), 'Адрес', 'Почта')
 insert into [Отдыхающий] values
 ('Артем Артемов Артемович',
   CAST('2003-05-20' AS date), 'Адрес', 'Почта')
 insert into [Отдыхающий] values
 ('Данила Данилин Данилович',
   CAST('2004-05-20' AS date), 'Адрес', 'Почта')
 insert into [Отдыхающий] values
 ('Максим Максимин Максимович',
   CAST('2005-05-20' AS date), 'Адрес', 'Почта')
 insert into [Отдыхающий] values
 ('Юра Юрин Юрьевич',
   CAST('2006-05-20' AS date), 'Адрес', 'Почта')
 insert into [Отдыхающий] values
 ('Женя Женин Евгеньевич',
   CAST('2007-05-20' AS date), 'Адрес', 'Почта')
 insert into [Отдыхающий] values
 ('Николай Николаин Николаивич',
   CAST('2008-05-20' AS date), 'Адрес', 'Почта')
 insert into [Отдыхающий] values
 ('Султан Сулин Султанович',
   CAST('2009-05-20' AS date), 'Адрес', 'Почта')
 insert into [Отдыхающий] values
 ('Настя Настина Максимовна',
   CAST('2010-05-20' AS date), 'Адрес', 'Почта')
 insert into [Отдыхающий] values
 ('Маша Машина Максимовна',
   CAST('2011-05-20' AS date), 'Адрес', 'Почта')
 insert into [Отдыхающий] values
 ('Лиза Лизина Максимовна',
   CAST('2012-05-20' AS date), 'Адрес', 'Почта')
 insert into [Отдыхающий] values
 ('Святослав Святин Святославович',
   CAST('2013-05-20' AS date), 'Адрес', 'Почта')
 insert into [Отдыхающий] values
 ('Георгий Герон Георгович',
   CAST('2014-05-20' AS date), 'Адрес', 'Почта')

insert into [Отдыхающий в Санатории] values
 (1,1), (1,2), (2,3), 
 (3,4), (4,5), (5,6),
 (6,7), (6,8), (6,9),
 (7,10), (7,11), (9,12),
 (9,13), (9,14)


 select ID, [Название] , case 
	when   [Год основания] <= CAST('2000-01-01' AS date)
		then 'Построено до двух тысячных'
	when  [Год основания] <= CAST('2008-01-01' AS date)
		 then '10 летней давности'
	else 'Современно:'+ CAST([Год основания] AS nvarchar)
	end as 'Время основания'
	from [Санаторий]

-- Допустим, что отдыхающий под номером 2, решил
-- сменить ФИО в честь санатория под номером 3
UPDATE [Отдыхающий]
SET [ФИО] = 
(
	SELECT [Название]
	FROM [Санаторий]
	where id = 3
)
WHERE ID = 2
SELECT * from [Отдыхающий]

-- Вывести список людей, которые родились раньше, чем основан
-- Саниторий в которым они отдыхали
select [ФИО], [Год рождения], [Название], [Год основания]
	from [Отдыхающий] as O join
	 [Отдыхающий в Санатории] as C
	ON O.ID = C.otd_id join
	 [Санаторий] as S ON S.ID = C.san_id 
	GROUP BY [Год рождения], [ФИО], [Название], [Год основания]
	HAVING [Год рождения] < [Год основания]
go

-- Для отладки удаления вьюшек

create view sanatoriy
	AS SELECT * 
	FROM [Санаторий]
--drop view sanatoriy
go

create view region
	AS SELECT * 
	FROM [Регион]
--drop view region
go

create view otdih
	AS SELECT * 
	FROM [Отдыхающий]
--drop view otdih
go

-- Процедура, удаляющая вьюшки и подсчитывающая их количество
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

-- Расскоментить при удалении
 --drop table [Регион]
 --drop table [Санаторий]
 --drop table [Отдыхающий]
 --drop table [Отдыхающий в Санатории]