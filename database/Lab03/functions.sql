-- Скалярная функция

IF OBJECT_ID (N'getBlanksAmount') IS NOT NULL
DROP FUNCTION getBlanksAmount
GO

CREATE FUNCTION getBlanksAmount (@s nvarchar(1000))
	RETURNS nvarchar(100) AS
BEGIN
	DECLARE @dif int = len(@s)-len(replace(@s, ' ', ''))
	IF @dif = 0
		RETURN 'Found no blank!'
	RETURN 'I found ' + CAST(@dif AS varchar(5)) + ' blanks'
End

go

DECLARE @something nvarchar(1000)
set @something = (select top 1 title.description_title from title)

SELECT @something, dbo.getBlanksAmount (@something)


-- Табличная функция

IF OBJECT_ID (N'nameLessThen') IS NOT NULL
DROP FUNCTION nameLessThen
GO

CREATE FUNCTION nameLessThen(@length int)
RETURNS TABLE
AS
RETURN (
	SELECT id_title, name_local
    FROM title
    WHERE len(title.name_local) < @length
	)

GO

select * from dbo.nameLessThen(8) as It

IF OBJECT_ID (N'getStudioWithRating') IS NOT NULL
DROP FUNCTION getStudioWithRating
GO

-- Многооператорная функция
-- Функция возвращающая набор значений (вектор) от 1 до N
CREATE FUNCTION getStudioWithRating(@rating float) 
RETURNS @OrderShipperTab TABLE 
    ( 
	studio_name nvarchar(80),
	year_ int,
    titleID int, 
    user_rating float, 
    status_ nvarchar (80)
    ) 
AS 
BEGIN  
    INSERT @OrderShipperTab  
        SELECT S.name_studio, S.year_,  
            A.id_title, A.user_rating, A.status  
        FROM studio AS S INNER JOIN airtime AS A  
            ON S.id_studio = A.id_studio  
        WHERE A.user_rating > @rating
    RETURN 
END
GO

select * from getStudioWithRating(4.0)

-- Рекурсивная функция

IF OBJECT_ID (N'countBlanks') IS NOT NULL
DROP FUNCTION countBlanks
GO

CREATE FUNCTION countBlanks (@String nvarchar(1000)) RETURNS table
AS return
			(
            WITH Numbers (num)
            AS
            (
                  SELECT 0
                  UNION ALL

                  SELECT CASE CHARINDEX(N' ', @String, num)
					WHEN 0 THEN LEN(@String) * 2
				   ELSE CHARINDEX(N' ', @String, num) + 1
				   end
				  
                  FROM Numbers
                  WHERE num < len(@String)
            )
			select num from Numbers where num < len(@String) and num > 1
			);
GO
DECLARE @something nvarchar(1000)
set @something = (select top 1 title.description_title from title)
SELECT @something, num as 'Номер элемента после пробела' from dbo.countBlanks(@something)

-- Хранимая процедура
IF OBJECT_ID (N'titleGetInfo') IS NOT NULL
DROP PROCEDURE titleGetInfo
GO

CREATE PROCEDURE titleGetInfo
@duration int
AS
SELECT id_title, name_local, duration
FROM title
WHERE (duration < @duration)
RETURN

EXEC titleGetInfo 15

-- Хранимая процедура с рекурсивным ОТВ

IF OBJECT_ID ( N'FindFilmsOrSomethingSame') IS NOT NULL
      DROP PROCEDURE FindFilmsOrSomethingSame
GO


CREATE PROCEDURE FindFilmsOrSomethingSame
AS
    WITH Films (helper_id, id_studio, name_studio, Level)
	AS
	(
	-- Определение закрепленного элемента
	 SELECT T.helper_id, T.id_studio, T.name_studio, 0 AS Level
	 FROM studio AS T
	 WHERE T.helper_id IS NULL
	 UNION ALL
	-- Определение рекурсивного элемента
	 SELECT t.helper_id, t.id_studio, t.name_studio, Level + 1
	 FROM studio AS t INNER JOIN Films AS d 
	 ON t.helper_id = d.id_studio
	)
	-- Инструкция, использующая ОТВ
	SELECT helper_id, id_studio, name_studio, Level
	FROM Films ORDER BY Level
	OPTION (MAXRECURSION 5)

Go

EXEC FindFilmsOrSomethingSame


-- Поработаем с курсором

drop table spec_table
CREATE TABLE spec_table
(
	 id_spec int IDENTITY(1,1) PRIMARY KEY,
	 id_title int,
	 name_local NVarChar(60),
	 description_title NVarChar(1000),
	 duration int,
);
Go

IF OBJECT_ID ( N'updateSpecTable') IS NOT NULL
      DROP PROCEDURE updateSpecTable
GO
CREATE PROCEDURE updateSpecTable
AS
DECLARE @id INT
DECLARE @dur INT
DECLARE @name VARCHAR (80)
DECLARE @desc VARCHAR (1000)

DECLARE @CURSOR CURSOR
SET @CURSOR  = CURSOR
FOR
	SELECT  distinct id_title, name_local, description_title, duration  
  FROM  title WHERE  duration > 10

OPEN @CURSOR

FETCH NEXT FROM @CURSOR INTO @ID, @name, @desc, @dur

WHILE @@FETCH_STATUS = 0
BEGIN
        IF EXISTS(SELECT id_title FROM airtime WHERE id_title=@ID and id_studio = 1 and user_rating > 4.9)
        BEGIN
                INSERT INTO spec_table (id_title, name_local, description_title, duration) 
				VALUES(@ID, @name, @desc, @dur)
        END
FETCH NEXT FROM @CURSOR INTO @ID, @name, @desc, @dur
END
CLOSE @CURSOR
DEALLOCATE @CURSOR

EXEC updateSpecTable
select distinct id_title, name_local, description_title, duration from spec_table

-- Доступ к метаданным
IF OBJECT_ID ( N'AmountOfFunctions', 'P' ) IS NOT NULL
      DROP PROCEDURE AmountOfFunctions
GO
CREATE PROCEDURE AmountOfFunctions @Amount INT OUTPUT AS
BEGIN
    DECLARE @funcname varchar(200), @Par varchar(200); SET NOCOUNT ON;
    DECLARE funcName_Cursor CURSOR FOR

    SELECT DISTINCT sys.objects.object_id
    FROM sys.objects JOIN sys.parameters ON sys.objects.object_id = sys.parameters.object_id
    WHERE sys.objects.type = 'FN' or sys.objects.type = 'TF' or sys.objects.type = 'P'
    --ORDER BY sys.objects.name;

    --SELECT *
    --FROM sys.parameters 

    SET @Amount = 0

    OPEN funcName_Cursor;
    FETCH NEXT FROM funcName_Cursor INTO @funcname;
    WHILE @@FETCH_STATUS = 0
    BEGIN

    SET @Par = (SELECT COUNT(name) FROM sys.parameters WHERE sys.parameters.object_id = @funcname)

    IF (@Par != 0)
    BEGIN
        SELECT DISTINCT sys.objects.name AS 'Имя функции', sys.parameters.name AS 'Параметры', sys.objects.type_desc AS 'Тип функции'
        FROM sys.objects JOIN sys.parameters ON sys.objects.object_id = sys.parameters.object_id
        WHERE sys.objects.object_id = @funcname
    END
    SET @Amount = @Amount + 1

    FETCH NEXT FROM funcName_Cursor INTO @funcname; 
    END;

    CLOSE funcName_Cursor;
    DEALLOCATE funcName_Cursor;
END;
GO

DECLARE @OutParm INT
EXECUTE AmountOfFunctions @OutParm OUTPUT;
SELECT @OutParm "Количество функций"
GO

-- Триггер After

/* Логгер рейтинга сериалов*/

drop table TitileRatingLogger
CREATE TABLE TitileRatingLogger (
	id_title int NULL,
    UserName CHAR(16) NULL,
    Date DATETIME NULL,
    ratingOld FLOAT NULL,
    ratingNew FLOAT NULL
);
DROP TRIGGER trigger_ModifyRating
GO
CREATE TRIGGER trigger_ModifyRating
    ON airTime AFTER UPDATE
    AS IF UPDATE(user_rating)
BEGIN
    DECLARE @ratingOld FLOAT
    DECLARE @ratingNew FLOAT
	DECLARE @id int

    SELECT @ratingOld = (SELECT user_rating FROM deleted)
    SELECT @ratingNew = (SELECT user_rating FROM inserted)
    SELECT @id = (SELECT id_title FROM deleted)

    INSERT INTO TitileRatingLogger VALUES
        (@id, USER_NAME(), GETDATE(), @ratingOld, @ratingNew)
END
go
DELETE FROM TitileRatingLogger
UPDATE airtime SET year_rating='r13' WHERE id_title=5
UPDATE airtime SET user_rating=3.4 WHERE id_title=5
UPDATE airtime SET year_rating='r15' WHERE id_title=5
UPDATE airtime SET user_rating=4.5 WHERE id_title=5
UPDATE airtime SET user_rating=3.8 WHERE id_title=5
UPDATE airtime SET year_rating='r18' WHERE id_title=5

-- Автоматическое обновление таблицы сезона(инкремент счетчика сериала или фильмов) в случае добавления нового тайтла

DROP TRIGGER InsertIn_AirTime_And_Season
GO
CREATE TRIGGER InsertIn_AirTime_And_Season 
ON airtime
INSTEAD OF INSERT
AS
BEGIN

	INSERT INTO airtime  SELECT * FROM inserted
	/*
	DECLARE @type nvarchar(20)
	SET @type = (select inserted.id_title from inserted)
	select @type as 'id of title'
	SET @type = (select title.id_type from title join type_anime ON title.id_type = type_anime.id_type where id_title = @type)
	select @type as 'id of type'
	SET @type = (select name_type from type_anime where id_type = @type)
    select @type as 'type is'
	*/
	
	--select * from season join inserted ON inserted.id_season = season.id_season
	--DECLARE @newNumber int
	--if @type = 'фильм'
		--SET @newNumber = (select season.films from season join inserted ON season.id_season = inserted.id_season) + 1
		
		UPDATE season SET serials=(select SUM(1) from airtime join title ON airtime.id_title = title.id_title join type_anime ON type_anime.id_type = title.id_type where type_anime.name_type like 'сериал') from inserted WHERE season.id_season=inserted.id_season
		UPDATE season SET films=(select SUM(1) from airtime join title ON airtime.id_title = title.id_title join type_anime ON type_anime.id_type = title.id_type where type_anime.name_type like 'фильм') from inserted WHERE season.id_season=inserted.id_season
	select season.id_season, serials, films  from season join inserted ON inserted.id_season = season.id_season
	select * from inserted
END;
go
INSERT INTO airtime(id_studio, id_season, id_title, status, begin_date, finish_date, user_rating, year_rating) 
		values (1, 3, 15,'teeest',  CAST('2010-12-31' AS DATE), CAST('2011-12-31' AS DATE), 4.5, 'teeeest') 
INSERT INTO airtime(id_studio, id_season, id_title, status, begin_date, finish_date, user_rating, year_rating) 
		values (1, 3, 16,'teeest',  CAST('2010-12-31' AS DATE), CAST('2011-12-31' AS DATE), 4.5, 'teeeest') 
INSERT INTO airtime(id_studio, id_season, id_title, status, begin_date, finish_date, user_rating, year_rating) 
		values (1, 3, 17,'teeest',  CAST('2010-12-31' AS DATE), CAST('2011-12-31' AS DATE), 4.5, 'teeeest') 

select * from airtime
--delete from airtime where status like 'teeest'

go
CREATE TABLE tableee (
	id_title int NULL,
    UserName CHAR(16) NULL,
    Date DATETIME NULL,
    ratingOld FLOAT NULL,
    ratingNew FLOAT NULL
);


create trigger trDatabse_OnDropTable
on database
for drop_table
as
begin
    set nocount on;
	RAISERROR ('[dbo].[tab2] cannot be dropped.', 16, 1);
    --Get the table schema and table name from EVENTDATA()
    DECLARE @Schema SYSNAME = eventdata().value('(/EVENT_INSTANCE/SchemaName)[1]', 'sysname');
    DECLARE @Table SYSNAME = eventdata().value('(/EVENT_INSTANCE/ObjectName)[1]', 'sysname');

    IF @Schema = 'dbo' AND @Table = 'tableee'
    BEGIN
        PRINT 'DROP TABLE Issued.';

        --Optional: error message for end user.
        RAISERROR ('[dbo].[tab2] cannot be droppe1d.', 16, 1);
    END
    ELSE
    BEGIN
        --Do nothing.  Allow table to be dropped.
        PRINT 'Table dropped: [' + @Schema + '].[' + @Table + ']';
    END
end;
drop table tableee