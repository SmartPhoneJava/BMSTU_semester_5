-- Удаление таблиц, если они уже созданы
drop table if exists airtime, season, title, studio, type_anime

-- Создание таблиц
CREATE TABLE studio
(
	 id_studio int IDENTITY(1,1) PRIMARY KEY,
	 helper_id int NULL,
	 name_studio varchar (30),
	 year_ int,
	 center_studio varchar(30),
	 site_studio varchar(30) 
);

CREATE TABLE type_anime (
    id_type int IDENTITY(1,1) PRIMARY KEY,
	name_type NVarChar(30),
);

CREATE TABLE title
(
	 id_title int IDENTITY(1,1) PRIMARY KEY,
	 name_origin NVarChar(100),
	 name_local NVarChar(100),
	 description_title NVarChar(1000),
	 id_type int references type_anime(id_type),
	 episodes int,
	 duration int,

	constraint e00 check (episodes > -1),
	constraint d0 check (duration > -1)
);

CREATE TABLE season
(
	 id_season int IDENTITY(1,1) PRIMARY KEY,
	 serials int,
	 films int,
	 announce int,
	 begin_date date,
	 finish_date date,

	 constraint check_dates_season check (begin_date < finish_date),
	 constraint f0 check (films > -1),
	 constraint s0 check (serials > -1),
	 constraint a0 check (announce > -1),
	 constraint eee0 check (begin_date > CAST('1900-12-31' AS DATE)),
);

CREATE TABLE airtime (
    id_studio int references studio(id_studio),
	id_season int references season(id_season),
	id_title int references title(id_title),
	status varchar(20),
	begin_date date,
	finish_date date,
	user_rating float,
	year_rating varchar(10),

	constraint check_dates check (begin_date < finish_date),
	constraint more0 check (user_rating > 0.0),
	constraint less10 check (user_rating < 10.0),
	constraint eeee0 check (begin_date > CAST('1900-12-31' AS DATE))
);

-- Ручное добавление строк в одну из таблицу
--INSERT INTO season(serials, films, announce) values (1, 1, 1) 
/*
use anime
INSERT INTO type_anime(name_type) values ('сериал') 
INSERT INTO type_anime(name_type) values ('фильм') 
INSERT INTO type_anime(name_type) values ('особый эпизод') 
INSERT INTO type_anime(name_type) values (' лип') 
*/

--INSERT INTO studio(helper_id, name_studio, year_, center_studio, site_studio) values 
--(NULL, 'Japan Corporation', 1900, 'Tokyo', 'japan.com') 

-- Добавление данных в БД из таблиц, созданных с помощью скрипта create_data.py
use anime
BULK INSERT anime.dbo.studio
FROM 'C:\msys64\home\BMSTU_semester_5\Lab01\studio.csv'
WITH (DATAFILETYPE = 'widechar', FIRSTROW = 2, FIELDTERMINATOR = ',', ROWTERMINATOR = '\n');
GO 

BULK INSERT anime.dbo.season
FROM 'C:\msys64\home\BMSTU_semester_5\Lab01\season.csv'
WITH (DATAFILETYPE = 'widechar', FIRSTROW = 2, FIELDTERMINATOR = ',', ROWTERMINATOR = '\n');
GO 

use anime
BULK INSERT anime.dbo.title
FROM 'C:\msys64\home\BMSTU_semester_5\Lab01\title.csv'
WITH (DATAFILETYPE = 'widechar', FIRSTROW = 2, FIELDTERMINATOR = ',', ROWTERMINATOR = '\n');
GO 

BULK INSERT anime.dbo.type
FROM 'C:\msys64\home\BMSTU_semester_5\Lab01\type.csv'
WITH (DATAFILETYPE = 'char', FIRSTROW = 2, FIELDTERMINATOR = ',', ROWTERMINATOR = '\n');
GO 


BULK INSERT anime.dbo.airtime
FROM 'C:\msys64\home\BMSTU_semester_5\Lab01\airtime.csv'
WITH (DATAFILETYPE = 'widechar', FIRSTROW = 2, FIELDTERMINATOR = ',', ROWTERMINATOR = '\n');
GO 