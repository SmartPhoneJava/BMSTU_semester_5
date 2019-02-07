drop PROCEDURE [CreateTitleWithDescription]

CREATE PROCEDURE [dbo].[CreateTitleWithDescription]
    @name_origin nvarchar(100),
	@id_type int,
    @episodes int,
	@duration int,
    @Id int out
AS
    INSERT INTO title (name_origin, name_local,
	description_title, id_type, episodes,
	duration)
    VALUES (@name_origin, @name_origin,
	'Название:'+ CAST(@name_origin AS VARCHAR(10))+'; Кол-во эпизодов:'+
	CAST(@episodes AS VARCHAR(10)) + '; Продолжительность эпизода:' + 
	CAST(@duration AS VARCHAR(10)),
	@id_type, @episodes, @duration)
  
GO

DECLARE @ret int
exec [CreateTitleWithDescription] 'no', 1, 1, 1, @ret
