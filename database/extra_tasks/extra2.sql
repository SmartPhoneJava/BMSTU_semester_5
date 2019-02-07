create database dopot
CREATE TABLE Tab
(
	 id int IDENTITY(1,1) PRIMARY KEY,
	 date_ date,
	 name_ nvarchar(100)
);
GO 
 
DROP TABLE Tab

INSERT INTO Tab(date_, name_)
VALUES (CAST('2018-12-21' AS Date), 'Колобок')
INSERT INTO Tab(date_, name_)
VALUES (CAST('2019-01-07' AS Date), 'Меч')
INSERT INTO Tab(date_, name_)
VALUES (CAST('2018-12-22' AS Date), 'Колобок')

INSERT INTO Tab(date_, name_)
VALUES (CAST('2019-01-01' AS Date), 'Башня')
INSERT INTO Tab(date_, name_)
VALUES (CAST('2018-12-23' AS Date), 'Колобок')

INSERT INTO Tab(date_, name_)
VALUES (CAST('2019-01-05' AS Date), 'Меч')
INSERT INTO Tab(date_, name_)
VALUES (CAST('2019-01-06' AS Date), 'Меч')

INSERT INTO Tab(date_, name_)
VALUES (CAST('2019-01-02' AS Date), 'Башня')

INSERT INTO Tab(date_, name_)
VALUES (CAST('2019-01-08' AS Date), 'Меч')
INSERT INTO Tab(date_, name_)
VALUES (CAST('2019-01-03' AS Date), 'Башня')
GO 
INSERT INTO Tab(date_, name_)
VALUES (CAST('2019-01-09' AS Date), 'Меч')
GO 

INSERT INTO Tab(date_, name_)
VALUES (CAST('2019-01-15' AS Date), 'Меч')
GO 

INSERT INTO Tab(date_, name_)
VALUES (CAST('2019-01-16' AS Date), 'Меч')
GO 

INSERT INTO Tab(date_, name_)
VALUES (CAST('2019-01-17' AS Date), 'Меч')
GO 

INSERT INTO Tab(date_, name_)
VALUES (CAST('2019-01-19' AS Date), 'Меч')
GO 

INSERT INTO Tab(date_, name_)
VALUES (CAST('2019-01-21' AS Date), 'Меч')
GO 

select * from
	Tab as U join Tab
		 as U2 On U.date_ = (select MIN(T.date_) from Tab as T where T.name_ = U.name_) and 
		 U2.date_ = (select MAX(T.date_) from Tab as T where T.name_ = U2.name_)and U.name_ = U2.name_

INSERT INTO Tablet(date1, date2, name_)
select U.date_, U2.date_, U.name_ from
	Tab as U join Tab
		 as U2 On U.date_ = (select MIN(T.date_) from Tab as T where T.name_ = U.name_) and 
		 U2.date_ = (select MAX(T.date_) from Tab as T where T.name_ = U2.name_)and U.name_ = U2.name_
GO 

CREATE TABLE Tablet
(
	 id int IDENTITY(1,1) PRIMARY KEY,
	 date1 date,
	 date2 date,
	 name_ nvarchar(100)
);
GO 

drop table Tablet

DECLARE @start_var date
DECLARE @end_var date
DECLARE @old_var date
DECLARE @new_var date

DECLARE @new_name NVARCHAR(60)

SELECT * FROM Tab

-- Версия 1 косячная
SELECT T.date_, RANK() OVER(PARTITION BY name_ ORDER BY date_) as 'for kill', * 
FROM Tab as T join (SELECT curr_date.id from (
SELECT *, RANK() OVER(PARTITION BY name_ ORDER BY date_)
 rnk FROM Tab
) curr_date join (SELECT *, RANK() OVER(PARTITION BY name_ ORDER BY date_)
 rnk FROM Tab
) prev_date ON curr_date.date_ = dateadd(DAY, 1, prev_date.date_)--ON Ranked_makers.date_ = Ranked_makers1.date_ --Ranked_makers.date_ = dateadd(DAY, 1, Ranked_makers1.date_)
join (SELECT *, RANK() OVER(PARTITION BY name_ ORDER BY date_)
 rnk FROM Tab
) next_date ON next_date.date_ = dateadd(DAY, 1, curr_date.date_)) kill_me
on 1 = 1 

-- Версия 1.1 Осталась лишь вставка
SELECT T.date_,

(SELECT curr_date.id from (
SELECT *, RANK() OVER(PARTITION BY name_ ORDER BY date_)
 rnk FROM Tab
) curr_date join (SELECT *, RANK() OVER(PARTITION BY name_ ORDER BY date_)
 rnk FROM Tab
) prev_date ON curr_date.date_ = dateadd(DAY, 1, prev_date.date_)
join (SELECT *, RANK() OVER(PARTITION BY name_ ORDER BY date_)
 rnk FROM Tab
) next_date ON next_date.date_ = dateadd(DAY, 1, curr_date.date_) where curr_date.id = T.id)

, ROW_NUMBER() OVER(PARTITION BY name_ ORDER BY date_) as 'for kill', * 
FROM Tab as T 
/*where NOT EXISTS (SELECT curr_date.id from (
SELECT *, RANK() OVER(PARTITION BY name_ ORDER BY date_)
 rnk FROM Tab
) curr_date join (SELECT *, RANK() OVER(PARTITION BY name_ ORDER BY date_)
 rnk FROM Tab
) prev_date ON curr_date.date_ = dateadd(DAY, 1, prev_date.date_)
join (SELECT *, RANK() OVER(PARTITION BY name_ ORDER BY date_)
 rnk FROM Tab
) next_date ON next_date.date_ = dateadd(DAY, 1, curr_date.date_) where curr_date.id = T.id)*/


-- Версия 1.2
select * from (
SELECT T.name_, T.date_ as date1,
(SELECT dateadd(DAY, 1, curr_date.date_) from (
SELECT *, RANK() OVER(PARTITION BY name_ ORDER BY date_)
 rnk FROM Tab
) curr_date join (SELECT *, RANK() OVER(PARTITION BY name_ ORDER BY date_)
 rnk FROM Tab
) prev_date ON curr_date.date_ = dateadd(DAY, 1, prev_date.date_) 
join (SELECT *, RANK() OVER(PARTITION BY name_ ORDER BY date_)
 rnk FROM Tab
) next_date ON next_date.date_ = dateadd(DAY, 1, curr_date.date_) where curr_date.id = T.id) as date2

, ROW_NUMBER() OVER(PARTITION BY T.name_ ORDER BY T.date_) as 'for kill' 
FROM Tab as T) rrr where rrr.date2 is NULL
--join Tab as T1
--where T.name_
--join Tab as T1 ON
--T.date_ < T1.date_ and T1.i
/*where NOT EXISTS (SELECT curr_date.id from (
SELECT *, RANK() OVER(PARTITION BY name_ ORDER BY date_)
 rnk FROM Tab
) curr_date join (SELECT *, RANK() OVER(PARTITION BY name_ ORDER BY date_)
 rnk FROM Tab
) prev_date ON curr_date.date_ = dateadd(DAY, 1, prev_date.date_)
join (SELECT *, RANK() OVER(PARTITION BY name_ ORDER BY date_)
 rnk FROM Tab
) next_date ON next_date.date_ = dateadd(DAY, 1, curr_date.date_) where curr_date.id = T.id)*/


 or Ranked_makers.date_ = dateadd(DAY, -1, Ranked_makers1.date_)

SELECT *, RANK() OVER(PARTITION BY name_ ORDER BY date_) rnk FROM Tab;

set @new_name = 'ddd'

WHILE @new_name is NOT NULL
			BEGIN
				 --Select @date_min, @date_max
				set @new_name = (
								select top 1 T.name_ 
							    from Tab as T
								where @new_name = null or @new_name < T.name_
								order by T.name_
							  )

				set @start_var = CAST('1000-10-10' AS Date)
				set @end_var = CAST('1000-10-10' AS Date)
				set @old_var = CAST('1000-10-10' AS Date)
				set @new_var = CAST('1000-10-10' AS Date)

				select @new_name
				
				WHILE @new_var is NOT NULL
				BEGIN 
				    set @old_var = @new_var
					set @new_var = (
									select top 1 T.date_ 
									from Tab as T
									where (@new_var < T.date_ or @new_var = CAST('1000-10-10' AS Date)) and T.name_ = @new_name
									order by T.name_, T.date_
								  )
					
					
					if (DATEDIFF(DAY, @start_var, CAST('1000-10-10' AS Date) ) != 0)
					BEGIN
					select @start_var, @end_var, @new_name, DATEDIFF(DAY,@old_var, @new_var), 'yes'
						if (DATEDIFF(DAY, @old_var, @new_var) is null or DATEDIFF(DAY, @old_var, @new_var) != 1)
							BEGIN
							select @start_var, @end_var, @new_name, DATEDIFF(DAY,@old_var, @new_var)
							INSERT INTO Tablet(date1, date2, name_)
								select @start_var, @end_var, @new_name
								set @start_var = @new_var
							END
					END
					else 
					BEGIN
						set @start_var = @new_var
					END

					if (@new_var is not null)
						set @end_var = @new_var
					
				END
			
			END;


select * from Tablet

SELECT
    *,
    row_number() OVER ()  AS num
FROM Tablet;