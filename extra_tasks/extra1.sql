/*
CREATE TABLE TTable1
(
	id integer NOT NULL,
	var1 NVarChar(60) NOT NULL,
	valid_from_dttm date NOT NULL,
	valid_to_dttm date NOT NULL,

	constraint id_more_0 check (id > 0),
	constraint date_from_less_to check (valid_from_dttm < valid_to_dttm)
);

CREATE TABLE TTable2
(
	id integer NOT NULL,
	var2 NVarChar(60) NOT NULL,
	valid_from_dttm date NOT NULL,
	valid_to_dttm date NOT NULL,

	constraint id_more_0_t2 check (id > 0),
	constraint date_from_less_to_t2 check (valid_from_dttm < valid_to_dttm)
);
*/

--drop TABLE TTable1, TTable2

DELETE FROM TTable1;
DELETE FROM TTable2;

use anime
INSERT INTO TTable1(id, var1, valid_from_dttm, valid_to_dttm) values
	(1, 'A', CAST('2018-01-01' AS DATE), CAST('2018-01-08' AS DATE))
INSERT INTO TTable1(id, var1, valid_from_dttm, valid_to_dttm) values
	(1, 'B', CAST('2018-01-09' AS DATE), CAST('2018-01-13' AS DATE))
INSERT INTO TTable1(id, var1, valid_from_dttm, valid_to_dttm) values
	(1, 'C', CAST('2018-01-14' AS DATE), CAST('2018-01-17' AS DATE))
INSERT INTO TTable1(id, var1, valid_from_dttm, valid_to_dttm) values
	(1, 'D', CAST('2018-01-18' AS DATE), CAST('5999-01-01' AS DATE))
INSERT INTO TTable1(id, var1, valid_from_dttm, valid_to_dttm) values
	(2, 'A', CAST('2018-01-01' AS DATE), CAST('5999-12-04' AS DATE))

INSERT INTO TTable2(id, var2, valid_from_dttm, valid_to_dttm) values
	(1, 'A', CAST('2018-01-01' AS DATE), CAST('2018-01-05' AS DATE))
INSERT INTO TTable2(id, var2, valid_from_dttm, valid_to_dttm) values
	(1, 'B', CAST('2018-01-06' AS DATE), CAST('2018-01-13' AS DATE))
INSERT INTO TTable2(id, var2, valid_from_dttm, valid_to_dttm) values
	(1, 'C', CAST('2018-01-14' AS DATE), CAST('2018-01-16' AS DATE))
INSERT INTO TTable2(id, var2, valid_from_dttm, valid_to_dttm) values
	(1, 'D', CAST('2018-01-17' AS DATE), CAST('5999-1-17' AS DATE))
INSERT INTO TTable2(id, var2, valid_from_dttm, valid_to_dttm) values
	(2, 'A', CAST('2018-01-01' AS DATE), CAST('5999-12-04' AS DATE))

/*
INSERT INTO TTable2(id, var2, valid_from_dttm, valid_to_dttm) values
	(1, 'À', CAST('2018-09-01' AS DATE), CAST('2018-09-18' AS DATE))
INSERT INTO TTable2(id, var2, valid_from_dttm, valid_to_dttm) values
	(1, 'B', CAST('2018-09-19' AS DATE), CAST('5999-12-31' AS DATE))
INSERT INTO TTable2(id, var2, valid_from_dttm, valid_to_dttm) values
	(2, 'AA', CAST('2018-09-01' AS DATE), CAST('2018-09-08' AS DATE))
INSERT INTO TTable2(id, var2, valid_from_dttm, valid_to_dttm) values
	(2, 'BB', CAST('2018-09-09' AS DATE), CAST('2018-09-10' AS DATE))
INSERT INTO TTable2(id, var2, valid_from_dttm, valid_to_dttm) values
	(2, 'CC', CAST('2018-09-11' AS DATE), CAST('2018-09-13' AS DATE))
INSERT INTO TTable2(id, var2, valid_from_dttm, valid_to_dttm) values
	(2, 'DD', CAST('2018-09-13' AS DATE), CAST('5999-12-04' AS DATE))
*/
select * from TTable1
select * from TTable2

DROP TABLE TTable3
DROP TABLE TTable4

select id as idd, valid_from_dttm as valid into TTable4
FROM (
		SELECT id, valid_from_dttm
		FROM TTable2
			union 
		SELECT id, valid_from_dttm
		FROM TTable1
			union
		SELECT id, valid_to_dttm
		FROM TTable2
			union 
		SELECT id, valid_to_dttm
		FROM TTable1
) AS Itt 

select * from TTable4

CREATE TABLE TTable3
(
	id integer NOT NULL,
	var1 NVarChar(60) NOT NULL,
	var2 NVarChar(60) NOT NULL,
	valid_from_dttm date NOT NULL,
	valid_to_dttm date NOT NULL,

	constraint id_more_0_t3 check (id > 0),
	constraint date_from_less_to_t3 check (valid_from_dttm <= valid_to_dttm)
);

DECLARE @date_min date
DECLARE @date_max date

DECLARE @write1 NVARCHAR(60)
DECLARE @write2 NVARCHAR(60)

DECLARE @old_write1 NVARCHAR(60)
DECLARE @old_write2 NVARCHAR(60)


DECLARE @id integer
SET @id = (SELECT max(idd) From TTable4)

WHILE @id > 0
	BEGIN
		SET @date_min = (SELECT min(valid) from TTable4 as T4 where (idd = @id))

		SET @date_max =  (SELECT min(valid) from TTable4 as T4 where (idd = @id) and (valid > @date_min))

		--select id, var1, var2, valid_from_dttm, valid_to_dttm  from TTable3
		Select @date_min, @date_max

		set @old_write1 = NULL
		set @old_write2 = NULL

		WHILE @date_max is NOT NULL
			BEGIN
				 --Select @date_min, @date_max
				set @old_write1 = @write1
				set @old_write2 = @write2
				set @write1 = (
								select top 1 var1 
							    from TTable1 
								where valid_from_dttm <= @date_min and
									  valid_to_dttm >= @date_max and
									  id = @id
							  )

				set @write2 = (
								select top 1 var2 
							    from TTable2 
								where valid_from_dttm <= @date_min and
									  valid_to_dttm >= @date_max and
									  id = @id
							  )
				if (@write1 is not NULL and @write2 is not NULL)
					if (@write1 = @old_write1 and @write2 = @old_write2)
						begin
							UPDATE TTable3
							SET valid_to_dttm = @date_max
							FROM (
									SELECT TOP 1 valid_to_dttm FROM TTable3
									ORDER BY valid_to_dttm DESC
								 ) AS th  
							WHERE TTable3.valid_to_dttm = th.valid_to_dttm;
						end
					else
						INSERT INTO TTable3(id,  var1,    var2,    valid_from_dttm, valid_to_dttm)
							values (@id, @write1, @write2, @date_min, @date_max)
				
				if (@date_min = @date_max)
					SET @date_max =  (SELECT min(valid) from TTable4 as T4 where (idd = @id) and (valid > @date_min))
				else
					SET @date_min = @date_max;
			
				Select @date_min, @date_max, @write1, @old_write1, @write2, @old_write2	
				--select id, var1, var2, valid_from_dttm, valid_to_dttm  from TTable3 where (var1 is not NULL and var2 is not NULL)
				--select * from TTable4
			END;
		select id, var1, var2, valid_from_dttm, valid_to_dttm  from TTable3
		SET @id = @id - 1
	END;

select id, var1, var2, valid_from_dttm, valid_to_dttm  from TTable3
