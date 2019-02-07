
use anime;
go

select * from title
where episodes < 14
for xml auto

select * from title
where episodes < 14
for xml raw

select A.status, A.user_rating, A.year_rating, T.name_local, T.duration, T.episodes, S.name_studio
from airtime as A join title as T on A.id_title = T.id_title join studio as S on A.id_studio = S.id_studio
where A.user_rating > 3 and T.duration < 25 and S.year_ > 2000
for xml auto

select A.status, A.user_rating, A.year_rating, T.name_local, T.duration, T.episodes, S.name_studio
from airtime as A join title as T on A.id_title = T.id_title join studio as S on A.id_studio = S.id_studio
where A.user_rating > 3 and T.duration < 25 and S.year_ > 2000
for xml raw

select 1    as Tag,  
       null as Parent,  
	   A.id_title as [id!1!id], 
       null as [ratings!2!user_rating], 
	   null as [ratings!2!year_rating],   
       null       as [namings!3!original],  
       null       as [namings!3!local],
	   null       as [id_season!4!id_season],
	   null       as [types!5!films],
	   null       as [types!5!serials],
	   null       as [types!5!anounced]
	   --null		  as [FCost!3!Cost]
from   airtime as A  
join title as T
ON  A.id_title = T.id_title
join season as S
ON  A.id_season = S.id_season
where A.id_title < 20
UNION ALL  
SELECT 2 as Tag,  
       1 as Parent, 
	   A.id_title,
       A.user_rating,
       A.year_rating,
	   null,
	   null,
	   null,
	   null,
	   null,
	   null
from   airtime as A  
join title as T
ON  A.id_title = T.id_title
join season as S
ON  A.id_season = S.id_season
where A.id_title < 20
UNION ALL  
SELECT 3 as Tag,  
       1 as Parent,  
	   A.id_title,
       A.user_rating,
       null,
	   T.name_origin,
	   T.name_local,
	   null,
	   null,
	   null,
	   null
from   airtime as A  
join title as T
ON  A.id_title = T.id_title
join season as S
ON  A.id_season = S.id_season
where A.id_title < 20
UNION ALL  
SELECT 4 as Tag,  
       1 as Parent,  
	   A.id_title,
       A.user_rating,
       null,
	   T.name_origin,
	   null,
	   S.id_season,
	   null,
	   null,
	   null
from   airtime as A  
join title as T
ON  A.id_title = T.id_title
join season as S
ON  A.id_season = S.id_season
where A.id_title < 20
UNION ALL  
SELECT 5 as Tag,  
       4 as Parent,  
	   A.id_title,
       A.user_rating,
       null,
	   T.name_origin,
	   null,
	   S.id_season,
	   S.serials,
	   S.films,
	   S.announce
from   airtime as A  
join title as T
ON  A.id_title = T.id_title
join season as S
ON  A.id_season = S.id_season
where A.id_title < 20
order by [id!1!id], [ratings!2!user_rating], [namings!3!original], [id_season!4!id_season]
for xml explicit;

select A.id_title "id",
	   A.user_rating "id/ratings/user_rating",
       A.year_rating "id/ratings/year_rating",
	   T.name_origin "id/namings/name_origin",
	   T.name_local  "id/namings/name_local",
	   S.id_season  "id/id_season",
	   S.serials  "id/id_season/types/serials",
	   S.films  "id/id_season/types/films",
	   S.announce  "id/id_season/types/announce"
from   airtime as A  
join title as T
ON  A.id_title = T.id_title
join season as S
ON  A.id_season = S.id_season
where A.id_title < 20
for xml path;
go


--n2
