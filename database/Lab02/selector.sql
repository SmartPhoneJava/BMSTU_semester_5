use anime
--  1.Инструкция SELECT, использующая предикат сравнения.

SELECT DISTINCT C1.center_studio, C1.name_studio
FROM Studio C1
WHERE C1.center_studio = 'Kyoto'
ORDER BY C1.center_studio, C1.name_studio


-- 2.Инструкция SELECT, использующая предикат BETWEEN.

SELECT DISTINCT id_title, begin_date, finish_date
FROM airtime
WHERE begin_date BETWEEN '1995-01-01' AND '2000-01-31'  AND finish_date BETWEEN '2000-01-01' AND '2018-01-31'


--3. Инструкция SELECT, использующая предикат LIKE.
/*
SELECT DISTINCT name_local, description_title
FROM airtime JOIN title ON airtime.id_title = title.id_title
WHERE description_title LIKE '%История%'
*/

-- 4. Инструкция SELECT, использующая предикат IN с вложенным подзапросом.
/*
SELECT DISTINCT id_studio, id_title
FROM airtime
WHERE id_studio IN
(
	 SELECT id_studio
	 FROM studio
	 WHERE center_studio = 'Fukuoka'
) AND id_title < 50
ORDER BY id_studio, id_title
*/

--5. Инструкция SELECT, использующая предикат EXISTS с вложенным подзапросом.
SELECT DISTINCT TI.id_title, TI.name_local
FROM title as TI
WHERE EXISTS
(
	SELECT *
	FROM airtime AIR
	WHERE  AIR.id_title = TI.id_title and 
			AIR.user_rating > 4.5
 )

-- 6. Инструкция SELECT, использующая предикат сравнения с квантором.
/*
SELECT id_title, name_local
FROM title
WHERE duration > ALL
	(
	SELECT duration
	FROM title
	where id_title = 11
	)
*/

-- 7. Инструкция SELECT, использующая агрегатные функции в выражениях столбцов.

SELECT AVG(TotalTime) AS 'Actual AVG',
	SUM(TotalTime) / COUNT(id_title) AS 'Calc AVG'
FROM (
	SELECT id_title, SUM(duration * episodes) AS TotalTime
	FROM dbo.title
	WHERE id_type = 1
	GROUP BY id_title
 ) AS TotTime
 

--8. Инструкция SELECT, использующая скалярные подзапросы в выражениях столбцов.
SELECT DISTINCT
	(
		SELECT top 1 name_local
		FROM title
		WHERE title.id_title > airtime.id_title and
			len(title.name_local) > 15
	) AS Name_Of_Title,
	(
		SELECT top 1 title.description_title
		FROM title
		WHERE title.id_title > airtime.id_title and
			len(title.description_title) > 500
	) AS Finish_date
FROM airtime
WHERE id_title < 20

--9. Инструкция SELECT, использующая простое выражение CASE.

SELECT DISTINCT airtime.id_studio, studio.name_studio,
	CASE YEAR(finish_date)
		WHEN YEAR(Getdate()) THEN 'This Year'
		WHEN YEAR(GetDate()) - 1 THEN 'Last year'
		ELSE CAST(DATEDIFF(year, finish_date, Getdate()) AS varchar(5)) + ' years ago'
	END AS 'When'
FROM airtime JOIN studio ON airtime.id_studio = studio.id_studio
ORDER BY studio.name_studio, 'When'


--10. Инструкция SELECT, использующая поисковое выражение CASE.
/*
SELECT name_local, duration as 'Time of one episode',
	CASE
		WHEN duration < 24 THEN 'Short'
		WHEN duration < 60 THEN 'Usual'
		WHEN duration < 100 THEN 'Long'
		ELSE 'Very long'
	END AS Duration
FROM title
*/

-- 11. Создание новой временной локальной таблицы из результирующего
--     набора данных инструкции SELECT.
/*
SELECT episodes,
 SUM(episodes) AS St,
 CAST(SUM(episodes*duration)AS int) AS 'Time'
INTO #Timing
FROM title
WHERE id_title < 50 and id_title > 40
GROUP BY id_title, episodes
*/
--DROP table #Timing;
SELECT * FROM #Timing

-- 12. Инструкция SELECT, использующая вложенные коррелированные 
--     подзапросы в качестве производных таблиц в предложении FROM.

SELECT 'By longest description' AS Criteria, name_local as 'name', description_title as 'description'
FROM airtime AI JOIN
	(
		 SELECT TOP 1 id_title, name_local, description_title, MAX(LEN(description_title)) AS SQ
		 FROM title
		 GROUP BY id_title, name_local, description_title
		 ORDER BY SQ DESC
	) AS TI ON TI.id_title = AI.id_title
UNION
SELECT 'By longest name' AS Criteria, name_local as 'name', description_title as 'description'
FROM airtime AI JOIN
	(
		 SELECT TOP 1 id_title, name_local, description_title, MAX(LEN(name_local)) AS SQ
		 FROM title
		 GROUP BY id_title, name_local, description_title
		 ORDER BY SQ DESC
	) AS TI ON TI.id_title = AI.id_title

-- 13. Инструкция SELECT, использующая вложенные
--     подзапросы с уровнем вложенности 3.

SELECT id_studio
FROM studio as STUD
WHERE (STUD.center_studio like 'Kawasaki' or 
	  STUD.center_studio like 'Sapporo')
	 AND EXISTS
(
	SELECT id_studio
	FROM airtime
	WHERE STUD.id_studio = airtime.id_studio and
		 user_rating > 4 and EXISTS
	(
					SELECT id_title 
					FROM title
					WHERE len(title.description_title) >= 600 and
					airtime.id_title = title.id_title
	)	
)

-- 14. Инструкция SELECT, консолидирующая данные с помощью предложения GROUP BY
/*
SELECT
Air.id_season AS 'Сезон',
AVG(Air.user_rating) AS 'Средняя оценка тайтлов сезона',
MIN(Air.user_rating) AS 'Min',
MAX(Air.user_rating) AS 'Max'
FROM airtime Air 
GROUP BY Air.id_season
*/

-- 15. Инструкция SELECT, консолидирующая данные с
--     помощью предложения GROUP BY и предложения HAVING.

SELECT P.episodes, max(P.duration)
FROM title P
GROUP BY P.episodes
HAVING max(P.duration) < 200 AND max(P.duration) > 12


-- 16. Однострочная инструкция INSERT, выполняющая вставку в таблицу одной строки значений.
/*
INSERT studio(name_studio, year_, center_studio, site_studio)
VALUES ('Something Inc', 2018, 'Moscow', 'www.something.con')
*/

-- 17. Многострочная инструкция INSERT, выполняющая вставку в
--     таблицу результирующего набора данных вложенного подзапроса.

INSERT studio(name_studio, year_, center_studio, site_studio)
SELECT (
	SELECT TOP 1 name_local
	FROM title
	WHERE len(name_local) < 10
), 2018, 'New-York', name_origin
FROM title
WHERE id_title = 15

SELECT * from studio


-- 18. Простая инструкция UPDATE.
/*
UPDATE title
SET description_title = 'описание изменено'
WHERE id_title = 81
*/

-- 19. Инструкция UPDATE со скалярным подзапросом в предложении SET.

UPDATE season
SET serials =
(
	SELECT SUM(1)
	FROM title
	WHERE episodes < 24
)
WHERE id_season = 2
SELECT * from season


-- 20. Простая инструкция DELETE.
/*
DELETE studio
WHERE center_studio LIKE 'New-York'
SELECT * from studio
*/

-- 21. Инструкция DELETE с вложенным коррелированным подзапросом в предложении WHERE.
/*
DELETE FROM studio
WHERE studio.id_studio NOT IN
(
	 SELECT studio.id_studio
	 FROM studio LEFT OUTER JOIN airtime
	 ON studio.id_studio = airtime.id_studio
	 WHERE airtime.user_rating > 3
)
SELECT * from studio
*/

-- 22. Инструкция SELECT, использующая простое 
--     обобщенное табличное выражение

;WITH CTE (serials, films)
AS
(
	 SELECT SUM(serials), COUNT(films)
	 FROM season
	 GROUP BY serials, films
)
SELECT AVG(serials) AS 'Среднее количество сериалов',
		AVG(films) AS 'Среднее количество фильмов'
FROM CTE


-- 23. Инструкция SELECT, использующая рекурсивное обобщенное
-- табличное выражение.

;WITH DirectReports (helper_id, id_studio, name_studio, Level)
AS
(
-- Определение закрепленного элемента
 SELECT T.helper_id, T.id_studio, T.name_studio, 0 AS Level
 FROM studio AS T
 WHERE T.helper_id IS NULL
 UNION ALL
-- Определение рекурсивного элемента
 SELECT t.helper_id, t.id_studio, t.name_studio, Level + 1
 FROM studio AS t INNER JOIN DirectReports AS d 
 ON t.helper_id = d.id_studio
)
-- Инструкция, использующая ОТВ
SELECT helper_id, id_studio, name_studio, Level
FROM DirectReports ORDER BY Level
OPTION (MAXRECURSION 5);

SELECT * from studio


-- 24. Оконные функции. Использование конструкций MIN/MAX/AVG OVER()
-- Для каждой заданной группы продукта вывести среднее значение цены

SELECT  Air.id_title AS 'Тайтл', 
		Air.id_studio AS 'Студия',
		Air.user_rating AS 'Рейтинг',
		AVG(Air.user_rating) OVER(PARTITION BY Air.id_studio) AS 'Средняя оценка тайтлов этой студии',
		MIN(Air.user_rating) OVER(PARTITION BY Air.id_studio) AS 'Min',
		MAX(Air.user_rating) OVER(PARTITION BY Air.id_studio) AS 'Max'
FROM airtime Air 
ORDER BY Air.user_rating DESC



select id_season, id_studio, ROW_NUMBER() over (partition by id_season, id_studio order by id_season) as num

INTO #table
from airtime
go

delete 
from #table
where num > 1

SELECT * FROM dbo.anime.#table
DROP TABLE dbo.anime.#table 


