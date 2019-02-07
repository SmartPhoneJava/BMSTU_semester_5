declare @ixml int
declare @xml xml
select @xml =
(
	select * from openrowset(bulk 'D:\GitHub\basedate\5\titles.xml', 
                                single_blob) as data
)

exec sp_xml_preparedocument @ixml output, @xml

select *
from openxml (@ixml, '/root/row')
with(id_title int, name_origin nvarchar(100), name_local nvarchar(100),
 description_title nvarchar(1000), id_type int, episodes int, duration int)
exec sp_xml_removedocument @ixml

