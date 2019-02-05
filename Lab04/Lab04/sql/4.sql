use anime

drop procedure if exists updateStudio;
go

create procedure updateStudio
	@year int,
	@id int
as external name Lab04.fourth.updateStudio;
go

exec updateStudio 2018, 10;
go