-- Удаляет записи из таблице графиков для указанного объекта
if exists(select 1 from sysobjects where name = N'ChartsDataDelete' and xtype='P') drop proc ChartsDataDelete
go
create proc ChartsDataDelete (@SubjectId int) as	
begin	

	set nocount on

	begin try 

		delete from ChartsData where SubjectId = @SubjectId
		
		return 0
	end try 
	begin catch 		
		return 1
	end catch 
	
end
go